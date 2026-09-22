import { Injectable, NotFoundException } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { v4 as uuidv4 } from 'uuid';
import { DeliveryEntity } from './entities/delivery.entity';
import { UserEntity } from '../users/entities/user.entity';
import { CreateDeliveryDto } from './dto/create-delivery.dto';
import { DeliveryStatusEnum } from '../../common/enums';

@Injectable()
export class DeliveriesService {
  constructor(
    @InjectRepository(DeliveryEntity)
    private readonly deliveryRepository: Repository<DeliveryEntity>,
    @InjectRepository(UserEntity)
    private readonly userRepository: Repository<UserEntity>,
  ) {}

  private getDefaultSteps() {
    return [
      { step: 1, title: 'Pick up package', deadline: null, status: 'Waiting', files: [], description: '' },
      { step: 2, title: 'Drop off package', deadline: null, status: 'Waiting', files: [], description: '' },
    ];
  }

  private calculateProgress(status?: string, steps?: any[], truckerId?: string): number {
    const s = (status || 'NA').toUpperCase();
    if (s === 'DONE' || s === 'COMPLETED') {
      return 100;
    }
    if (s === 'FAILED' || s === 'CANCELLED') {
      return 0;
    }
    if (s === 'PROCESSING' || s === 'DELIVERING') {
      const step1Done = Array.isArray(steps) && steps[0]?.status?.toLowerCase() === 'done';
      const step2Done = Array.isArray(steps) && steps[1]?.status?.toLowerCase() === 'done';
      if (step1Done && step2Done) {
        return 85;
      }
      if (step1Done) {
        return 70;
      }
      return 50;
    }
    if (s === 'PAYMENTDONE' || s === 'PAYMENT_DONE') {
      return 35;
    }
    if (truckerId) {
      return 20;
    }
    return 0;
  }

  private async populateTrucker(deliveries: DeliveryEntity[]): Promise<any[]> {
    const truckerIds = Array.from(
      new Set(deliveries.map((d) => d.truckerId).filter(Boolean)),
    ) as string[];

    let truckers: UserEntity[] = [];
    if (truckerIds.length > 0) {
      truckers = await this.userRepository.find({
        where: truckerIds.map((id) => ({ id })),
      });
    }

    return deliveries.map((delivery) => {
      const deliveryObj: any = { ...delivery };

      if (!deliveryObj.status || deliveryObj.status === 'PENDING') {
        deliveryObj.status = DeliveryStatusEnum.NA;
      }

      if (!deliveryObj.steps || !Array.isArray(deliveryObj.steps) || deliveryObj.steps.length === 0) {
        deliveryObj.steps = this.getDefaultSteps();
      }

      deliveryObj.progress = this.calculateProgress(
        deliveryObj.status,
        deliveryObj.steps,
        deliveryObj.truckerId,
      );

      const trucker = truckers.find((t) => t.id === delivery.truckerId);
      if (trucker) {
        deliveryObj.trucker = {
          id: trucker.id,
          name: trucker.name,
          userName: trucker.userName,
          email: trucker.email,
          phoneNumber: trucker.phoneNumber,
          avatar: trucker.avatar,
          rating: trucker.rating || 5,
        };
      } else {
        deliveryObj.trucker = null;
      }
      return deliveryObj;
    });
  }

  async create(userId: string, dto: CreateDeliveryDto): Promise<DeliveryEntity> {
    let parsedPickupTime: Date | null = null;
    if (dto.pickupTime) {
      const d = new Date(dto.pickupTime);
      if (!isNaN(d.getTime())) {
        parsedPickupTime = d;
      }
    }

    const status = (dto as any).status || DeliveryStatusEnum.NA;
    const steps = (dto as any).steps || this.getDefaultSteps();
    const progress = this.calculateProgress(status, steps, dto.truckerId);

    const delivery = this.deliveryRepository.create({
      id: uuidv4(),
      createdBy: userId,
      shippingCode: dto.shippingCode || `SHP-${Date.now().toString().slice(-6)}`,
      price: dto.price ?? dto.totalPrice ?? 0,
      totalPrice: dto.totalPrice ?? dto.price ?? 0,
      pickupTime: parsedPickupTime,
      status,
      steps,
      progress,
      ...dto,
    });
    return this.deliveryRepository.save(delivery);
  }

  async findOne(id: string): Promise<any> {
    const delivery = await this.deliveryRepository.findOne({ where: { id } });
    if (!delivery) {
      throw new NotFoundException(`Delivery with ID ${id} not found`);
    }
    const [populated] = await this.populateTrucker([delivery]);
    return populated;
  }

  async findAll(searchFieldName?: string, searchFieldValue?: string): Promise<any[]> {
    let whereClause: any = {};
    if (searchFieldName && searchFieldValue) {
      whereClause[searchFieldName] = searchFieldValue;
    }

    const deliveries = await this.deliveryRepository.find({
      where: whereClause,
      order: { createdOn: 'DESC' },
    });

    return this.populateTrucker(deliveries);
  }

  async updateStatus(id: string, status: DeliveryStatusEnum): Promise<DeliveryEntity> {
    const delivery = await this.deliveryRepository.findOne({ where: { id } });
    if (!delivery) {
      throw new NotFoundException(`Delivery with ID ${id} not found`);
    }
    delivery.status = status;
    delivery.progress = this.calculateProgress(status, delivery.steps, delivery.truckerId);
    return this.deliveryRepository.save(delivery);
  }

  async updateStep(id: string, stepData: any): Promise<DeliveryEntity> {
    const delivery = await this.deliveryRepository.findOne({ where: { id } });
    if (!delivery) {
      throw new NotFoundException(`Delivery with ID ${id} not found`);
    }

    let currentSteps = delivery.steps;
    if (!Array.isArray(currentSteps) || currentSteps.length === 0) {
      currentSteps = this.getDefaultSteps();
    }

    if (stepData && typeof stepData.step === 'number') {
      const index = currentSteps.findIndex((s: any) => s.step === stepData.step);
      if (index !== -1) {
        currentSteps[index] = {
          ...currentSteps[index],
          ...stepData,
        };
      } else {
        currentSteps.push(stepData);
      }
    } else {
      currentSteps = stepData;
    }

    delivery.steps = currentSteps;
    delivery.progress = this.calculateProgress(delivery.status, delivery.steps, delivery.truckerId);
    return this.deliveryRepository.save(delivery);
  }
}
