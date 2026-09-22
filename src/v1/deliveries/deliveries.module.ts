import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { DeliveryEntity } from './entities/delivery.entity';
import { UserEntity } from '../users/entities/user.entity';
import { DeliveriesService } from './deliveries.service';
import { DeliveriesController } from './deliveries.controller';

@Module({
  imports: [TypeOrmModule.forFeature([DeliveryEntity, UserEntity])],
  controllers: [DeliveriesController],
  providers: [DeliveriesService],
  exports: [DeliveriesService],
})
export class DeliveriesModule {}
