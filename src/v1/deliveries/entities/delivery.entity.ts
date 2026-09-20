import { BaseGeneralEntity } from '../../../base/base.entity';
import { Column, Entity } from 'typeorm';
import { DeliveryStatusEnum, PickupCategoryEnum } from '../../../common/enums';

@Entity({ name: 'deliveries' })
export class DeliveryEntity extends BaseGeneralEntity {
  @Column({ name: 'weight', type: 'double', default: 0 })
  public weight: number;

  @Column({ name: 'width', type: 'double', default: 0 })
  public width: number;

  @Column({ name: 'length', type: 'double', default: 0 })
  public length: number;

  @Column({ name: 'height', type: 'double', default: 0 })
  public height: number;

  @Column({ name: 'product_category_id', type: 'varchar', length: 255, nullable: true })
  public productCategoryId?: string;

  @Column({ name: 'pickup_time', type: 'datetime', nullable: true })
  public pickupTime?: Date;

  @Column({ name: 'delivery_insurance_id', type: 'varchar', length: 255, nullable: true })
  public deliveryInsuranceId?: string;

  @Column({ name: 'trucker_id', type: 'varchar', length: 255, nullable: true })
  public truckerId?: string;

  @Column({ name: 'shipping_code', type: 'varchar', length: 255, nullable: true })
  public shippingCode?: string;

  @Column({ name: 'price', type: 'double', default: 0 })
  public price: number;

  @Column({ name: 'description', type: 'text', nullable: true })
  public description?: string;

  @Column({ name: 'progress', type: 'int', default: 0 })
  public progress: number;

  @Column({ name: 'package_description', type: 'text', nullable: true })
  public packageDescription?: string;

  @Column({ name: 'distance', type: 'double', default: 0 })
  public distance: number;

  @Column({ name: 'total_price', type: 'double', default: 0 })
  public totalPrice: number;

  @Column({ name: 'steps', type: 'json', nullable: true })
  public steps?: any;

  @Column({ name: 'start_location', type: 'json', nullable: true })
  public startLocation?: any;

  @Column({ name: 'end_location', type: 'json', nullable: true })
  public endLocation?: any;

  @Column({
    name: 'pickup_category',
    type: 'enum',
    enum: PickupCategoryEnum,
    default: PickupCategoryEnum.NOW,
  })
  public pickupCategory: PickupCategoryEnum;

  @Column({
    name: 'status',
    type: 'enum',
    enum: DeliveryStatusEnum,
    default: DeliveryStatusEnum.PENDING,
  })
  public status: DeliveryStatusEnum;

  @Column({ name: 'product_images', type: 'json', nullable: true })
  public productImages?: string[];

  @Column({ name: 'schedule_id', type: 'varchar', length: 255, nullable: true })
  public scheduleId?: string;
}
