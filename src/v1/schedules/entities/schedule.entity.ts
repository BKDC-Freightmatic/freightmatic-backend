import { BaseGeneralEntity } from '../../../base/base.entity';
import { Column, Entity } from 'typeorm';
import { TruckCategoryEnum } from '../../../common/enums';

@Entity({ name: 'schedules' })
export class ScheduleEntity extends BaseGeneralEntity {
  @Column({ name: 'trucker_id', type: 'varchar', length: 255 })
  public truckerId: string;

  @Column({ name: 'space_available', type: 'int', default: 0 })
  public spaceAvailable: number;

  @Column({ name: 'start_time', type: 'datetime', nullable: true })
  public startTime?: Date;

  @Column({ name: 'end_time', type: 'datetime', nullable: true })
  public endTime?: Date;

  @Column({ name: 'current_location', type: 'json', nullable: true })
  public currentLocation?: any;

  @Column({ name: 'start_location', type: 'json', nullable: true })
  public startLocation?: any;

  @Column({ name: 'end_location', type: 'json', nullable: true })
  public endLocation?: any;

  @Column({
    name: 'truck_category',
    type: 'enum',
    enum: TruckCategoryEnum,
    default: TruckCategoryEnum.BOX_TRUCK,
  })
  public truckCategory: TruckCategoryEnum;

  @Column({ name: 'width', type: 'int', default: 0 })
  public width: number;

  @Column({ name: 'length', type: 'int', default: 0 })
  public length: number;

  @Column({ name: 'height', type: 'int', default: 0 })
  public height: number;

  @Column({ name: 'postal_code', type: 'varchar', length: 50, nullable: true })
  public postalCode?: string;
}
