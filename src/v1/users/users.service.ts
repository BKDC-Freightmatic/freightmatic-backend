import { Injectable, NotFoundException, OnModuleInit } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import * as bcrypt from 'bcryptjs';
import { UserEntity } from './entities/user.entity';
import { ScheduleEntity } from '../schedules/entities/schedule.entity';
import { UserTypeEnum } from '../../common/enums';

@Injectable()
export class UsersService implements OnModuleInit {
  constructor(
    @InjectRepository(UserEntity)
    private readonly userRepository: Repository<UserEntity>,
    @InjectRepository(ScheduleEntity)
    private readonly scheduleRepository: Repository<ScheduleEntity>,
  ) {}

  async onModuleInit() {
    const count = await this.userRepository.count();
    if (count === 0) {
      const defaultPassword = await bcrypt.hash('password123', 10);
      const truckerPassword = await bcrypt.hash('Aa123456', 10);
      await this.userRepository.save([
        {
          id: '80b66ac1-65a4-476d-91a4-41eb6ed01fb7',
          userName: 'mvfurniture',
          email: 'mvfurniture.official@gmail.com',
          name: 'Mv Furniture',
          phoneNumber: '04385557989',
          password: defaultPassword,
          userType: UserTypeEnum.SHIPPER,
          rating: 5,
        },
        {
          id: 'abe1a58d-7273-46bd-ae88-d54288f620fc',
          userName: 'Chris.lee',
          email: 'chrislee.offical@gmail.com',
          name: 'Chris Lee',
          phoneNumber: '04385557123',
          password: truckerPassword,
          userType: UserTypeEnum.TRUCKER,
          rating: 5,
          truck: { maximumWeight: 5000, category: 'BOX_TRUCK' },
        },
      ]);
    }
  }

  async findAll(): Promise<UserEntity[]> {
    return this.userRepository.find();
  }

  async findOne(id: string): Promise<UserEntity> {
    const user = await this.userRepository.findOne({ where: { id } });
    if (!user) {
      throw new NotFoundException(`User with ID ${id} not found`);
    }
    return user;
  }

  private formatIsoDate(date: any): string | null {
    if (!date) return null;
    const d = new Date(date);
    if (isNaN(d.getTime())) return typeof date === 'string' ? date : null;
    return d.toISOString().split('.')[0]; // "2026-09-22T21:22:00"
  }

  private filterTruckers(users: UserEntity[]): UserEntity[] {
    return users.filter(
      (u) =>
        u.truck != null ||
        u.userType === UserTypeEnum.TRUCKER ||
        (u.userType as any) === 'TRUCKER' ||
        (u.userType as any) === 'Trucker',
    );
  }

  private async populateUserSchedules(users: UserEntity[]): Promise<any[]> {
    const schedules = await this.scheduleRepository.find({
      order: { createdOn: 'DESC' },
    });

    return users.map((user) => {
      const userSchedule = schedules.find((s) => s.truckerId === user.id);
      const userObj: any = { ...user };

      if (userSchedule) {
        const maxWeight = user.truck?.maximumWeight || 1000;
        const available = userSchedule.spaceAvailable || 0;
        const percent = Math.min(100, Math.max(0, Math.round((available / maxWeight) * 100)));

        userObj.startLocation = userSchedule.startLocation;
        userObj.endLocation = userSchedule.endLocation;
        userObj.currentLocation = userSchedule.currentLocation;
        userObj.startTime = this.formatIsoDate(userSchedule.startTime);
        userObj.endTime = this.formatIsoDate(userSchedule.endTime);
        userObj.spaceAvailable = available;
        userObj.percentAvailable = percent;
      }

      return userObj;
    });
  }

  async getPartialTrucks(): Promise<any[]> {
    const users = await this.userRepository.find();
    return this.populateUserSchedules(this.filterTruckers(users));
  }

  async getFullTrucks(): Promise<any[]> {
    const users = await this.userRepository.find();
    return this.populateUserSchedules(this.filterTruckers(users));
  }

  async getTopTruckers(): Promise<any[]> {
    const users = await this.userRepository.find({
      order: { rating: 'DESC' },
    });
    return this.populateUserSchedules(this.filterTruckers(users));
  }

  async getSuitableTruckersAI(dto: any): Promise<any[]> {
    const users = await this.userRepository.find({
      order: { rating: 'DESC' },
    });
    return this.populateUserSchedules(this.filterTruckers(users));
  }

  async update(id: string, updateData: Partial<UserEntity>): Promise<UserEntity> {
    await this.userRepository.update(id, updateData);
    return this.findOne(id);
  }
}
