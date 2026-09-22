import { ApiProperty } from '@nestjs/swagger';
import { IsEnum, IsNumber, IsOptional, IsString } from 'class-validator';
import { DeliveryStatusEnum, PickupCategoryEnum } from '../../../common/enums';

export class CreateDeliveryDto {
  @ApiProperty({ example: 10.5, required: false })
  @IsOptional()
  @IsNumber()
  weight?: number;

  @ApiProperty({ example: 50.0, required: false })
  @IsOptional()
  @IsNumber()
  width?: number;

  @ApiProperty({ example: 80.0, required: false })
  @IsOptional()
  @IsNumber()
  length?: number;

  @ApiProperty({ example: 40.0, required: false })
  @IsOptional()
  @IsNumber()
  height?: number;

  @ApiProperty({ example: 'cat_electronics', required: false })
  @IsOptional()
  @IsString()
  productCategoryId?: string;

  @ApiProperty({ example: 'ins_12345', required: false })
  @IsOptional()
  @IsString()
  deliveryInsuranceId?: string;

  @ApiProperty({ example: 'trucker_123', required: false })
  @IsOptional()
  @IsString()
  truckerId?: string;

  @ApiProperty({ example: 'schedule_123', required: false })
  @IsOptional()
  @IsString()
  scheduleId?: string;

  @ApiProperty({ example: 'SHP-99281', required: false })
  @IsOptional()
  @IsString()
  shippingCode?: string;

  @ApiProperty({ example: 150000, required: false })
  @IsOptional()
  @IsNumber()
  price?: number;

  @ApiProperty({ example: 'Handle with care', required: false })
  @IsOptional()
  @IsString()
  description?: string;

  @ApiProperty({ example: 'Electronic components package', required: false })
  @IsOptional()
  @IsString()
  packageDescription?: string;

  @ApiProperty({ example: 15.5, required: false })
  @IsOptional()
  @IsNumber()
  distance?: number;

  @ApiProperty({ example: 165000, required: false })
  @IsOptional()
  @IsNumber()
  totalPrice?: number;

  @ApiProperty({ required: false })
  @IsOptional()
  pickupTime?: any;

  @ApiProperty({ required: false })
  @IsOptional()
  startLocation?: any;

  @ApiProperty({ required: false })
  @IsOptional()
  endLocation?: any;

  @ApiProperty({ enum: PickupCategoryEnum, default: PickupCategoryEnum.NOW, required: false })
  @IsOptional()
  @IsEnum(PickupCategoryEnum)
  pickupCategory?: PickupCategoryEnum;

  @ApiProperty({ required: false, type: [String] })
  @IsOptional()
  productImages?: string[];
}
