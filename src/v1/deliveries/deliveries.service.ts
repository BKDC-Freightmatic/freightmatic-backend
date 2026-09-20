import { Injectable, NotFoundException } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { v4 as uuidv4 } from 'uuid';
import { DeliveryEntity } from './entities/delivery.entity';
import { CreateDeliveryDto } from './dto/create-delivery.dto';
import { DeliveryStatusEnum } from '../../common/enums';

@Injectable()
export class DeliveriesService {
  constructor(
    @InjectRepository(DeliveryEntity)
    private readonly deliveryRepository: Repository<DeliveryEntity>,
  ) {}

  async create(userId: string, dto: CreateDeliveryDto): Promise<DeliveryEntity> {
    const delivery = this.deliveryRepository.create({
      id: uuidv4(),
      createdBy: userId,
      shippingCode: dto.shippingCode || `SHP-${Date.now().toString().slice(-6)}`,
      ...dto,
    });
    return this.deliveryRepository.save(delivery);
  }

  async findOne(id: string): Promise<DeliveryEntity> {
    const delivery = await this.deliveryRepository.findOne({ where: { id } });
    if (!delivery) {
      throw new NotFoundException(`Delivery with ID ${id} not found`);
    }
    return delivery;
  }

  async findAll(): Promise<DeliveryEntity[]> {
    return this.deliveryRepository.find();
  }

  async updateStatus(id: string, status: DeliveryStatusEnum): Promise<DeliveryEntity> {
    const delivery = await this.findOne(id);
    delivery.status = status;
    return this.deliveryRepository.save(delivery);
  }

  async updateStep(id: string, stepData: any): Promise<DeliveryEntity> {
    const delivery = await this.findOne(id);
    delivery.steps = stepData;
    return this.deliveryRepository.save(delivery);
  }
}
