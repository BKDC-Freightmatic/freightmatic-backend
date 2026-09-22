import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { v4 as uuidv4 } from 'uuid';
import { ScheduleEntity } from './entities/schedule.entity';
import { CreateScheduleDto } from './dto/create-schedule.dto';

@Injectable()
export class SchedulesService {
  constructor(
    @InjectRepository(ScheduleEntity)
    private readonly scheduleRepository: Repository<ScheduleEntity>,
  ) {}

  async create(truckerId: string, dto: CreateScheduleDto): Promise<ScheduleEntity> {
    const targetTruckerId = dto['truckerId'] || truckerId;
    const schedule = this.scheduleRepository.create({
      id: uuidv4(),
      truckerId: targetTruckerId,
      createdBy: truckerId,
      ...dto,
    });
    return this.scheduleRepository.save(schedule);
  }

  async findByTruckerOrId(identifier: string): Promise<ScheduleEntity[]> {
    const schedulesByTrucker = await this.scheduleRepository.find({
      where: { truckerId: identifier },
      order: { createdOn: 'DESC' },
    });

    if (schedulesByTrucker && schedulesByTrucker.length > 0) {
      return schedulesByTrucker;
    }

    const scheduleById = await this.scheduleRepository.findOne({
      where: { id: identifier },
    });

    if (scheduleById) {
      return [scheduleById];
    }

    return [];
  }

  async findAll(): Promise<ScheduleEntity[]> {
    return this.scheduleRepository.find({
      order: { createdOn: 'DESC' },
    });
  }
}
