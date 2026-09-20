import { Controller, Get, Post, Body, Param, Request } from '@nestjs/common';
import { ApiTags, ApiOperation, ApiBearerAuth } from '@nestjs/swagger';
import { SchedulesService } from './schedules.service';
import { CreateScheduleDto } from './dto/create-schedule.dto';

@ApiTags('schedules')
@ApiBearerAuth()
@Controller('v1/schedules')
export class SchedulesController {
  constructor(private readonly schedulesService: SchedulesService) {}

  @Post()
  @ApiOperation({ summary: 'Create a new schedule' })
  async create(@Request() req, @Body() dto: CreateScheduleDto) {
    return this.schedulesService.create(req.user.userId, dto);
  }

  @Get(':id')
  @ApiOperation({ summary: 'Get schedule by ID' })
  async findOne(@Param('id') id: string) {
    return this.schedulesService.findOne(id);
  }

  @Get()
  @ApiOperation({ summary: 'Get all schedules' })
  async findAll() {
    return this.schedulesService.findAll();
  }
}
