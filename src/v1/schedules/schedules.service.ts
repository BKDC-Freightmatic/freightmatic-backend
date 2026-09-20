import { Injectable, NotFoundException } from '@nestjs/common';
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
    const schedule = this.scheduleRepository.create({
      id: uuidv4(),
      truckerId,
      createdBy: truckerId,
      ...dto,
    });
    return this.scheduleRepository.save(schedule);
  }

  async findOne(id: string): Promise<ScheduleEntity> {
    const schedule = await this.scheduleRepository.findOne({ where: { id } });
    if (!schedule) {
      throw new NotFoundException(`Schedule with ID ${id} not found`);
    }
    return schedule;
  }

  async findAll(): Promise<ScheduleEntity[]> {
    return this.scheduleRepository.find();
  }
}
