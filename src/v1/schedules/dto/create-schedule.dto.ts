import { ApiProperty } from '@nestjs/swagger';
import { IsEnum, IsInt, IsNotEmpty, IsOptional, IsString } from 'class-validator';
import { TruckCategoryEnum } from '../../../common/enums';

export class CreateScheduleDto {
  @ApiProperty({ example: 50 })
  @IsInt()
  spaceAvailable: number;

  @ApiProperty({ example: 100 })
  @IsInt()
  width: number;

  @ApiProperty({ example: 200 })
  @IsInt()
  length: number;

  @ApiProperty({ example: 150 })
  @IsInt()
  height: number;

  @ApiProperty({ example: '2026-09-20T10:00:00Z', required: false })
  @IsOptional()
  startTime?: Date;

  @ApiProperty({ example: '2026-09-20T18:00:00Z', required: false })
  @IsOptional()
  endTime?: Date;

  @ApiProperty({ required: false })
  @IsOptional()
  currentLocation?: any;

  @ApiProperty({ required: false })
  @IsOptional()
  startLocation?: any;

  @ApiProperty({ required: false })
  @IsOptional()
  endLocation?: any;

  @ApiProperty({ example: '700000', required: false })
  @IsOptional()
  @IsString()
  postalCode?: string;

  @ApiProperty({ enum: TruckCategoryEnum, default: TruckCategoryEnum.BOX_TRUCK })
  @IsEnum(TruckCategoryEnum)
  truckCategory: TruckCategoryEnum;
}
