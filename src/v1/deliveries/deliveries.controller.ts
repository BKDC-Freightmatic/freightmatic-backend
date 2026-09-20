import {
  Controller,
  Get,
  Post,
  Put,
  Body,
  Param,
  Request,
} from '@nestjs/common';
import { ApiTags, ApiOperation, ApiBearerAuth } from '@nestjs/swagger';
import { DeliveriesService } from './deliveries.service';
import { CreateDeliveryDto } from './dto/create-delivery.dto';
import { DeliveryStatusEnum } from '../../common/enums';

@ApiTags('deliveries')
@ApiBearerAuth()
@Controller('v1/deliveries')
export class DeliveriesController {
  constructor(private readonly deliveriesService: DeliveriesService) {}

  @Post()
  @ApiOperation({ summary: 'Create a new delivery request' })
  async create(@Request() req, @Body() dto: CreateDeliveryDto) {
    return this.deliveriesService.create(req.user.userId, dto);
  }

  @Get(':id')
  @ApiOperation({ summary: 'Get delivery details by ID' })
  async findOne(@Param('id') id: string) {
    return this.deliveriesService.findOne(id);
  }

  @Get()
  @ApiOperation({ summary: 'Get all deliveries' })
  async findAll() {
    return this.deliveriesService.findAll();
  }

  @Put(':id/status')
  @ApiOperation({ summary: 'Update delivery status' })
  async updateStatus(
    @Param('id') id: string,
    @Body('status') status: DeliveryStatusEnum,
  ) {
    return this.deliveriesService.updateStatus(id, status);
  }

  @Put(':id/step')
  @ApiOperation({ summary: 'Update delivery step' })
  async updateStep(@Param('id') id: string, @Body() stepData: any) {
    return this.deliveriesService.updateStep(id, stepData);
  }
}
