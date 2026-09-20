import { BaseGeneralEntity } from '../../../base/base.entity';
import { Column, Entity } from 'typeorm';
import { Exclude } from 'class-transformer';
import { UserTypeEnum } from '../../../common/enums';

@Entity({ name: 'users' })
export class UserEntity extends BaseGeneralEntity {
  @Column({ name: 'name', type: 'varchar', length: 255, nullable: true })
  public name?: string;

  @Column({ name: 'avatar', type: 'varchar', length: 500, nullable: true })
  public avatar?: string;

  @Column({ name: 'user_name', type: 'varchar', length: 255, unique: true })
  public userName: string;

  @Column({ name: 'email', type: 'varchar', length: 255, unique: true })
  public email: string;

  @Column({ name: 'phone_number', type: 'varchar', length: 50, nullable: true })
  public phoneNumber?: string;

  @Exclude()
  @Column({ name: 'password', type: 'varchar', length: 255 })
  public password: string;

  @Exclude()
  @Column({ name: 'refresh_token', type: 'varchar', length: 500, nullable: true })
  public refreshToken?: string;

  @Column({ name: 'address', type: 'text', nullable: true })
  public address?: string;

  @Column({ name: 'other_address', type: 'text', nullable: true })
  public otherAddress?: string;

  @Column({ name: 'uci_number', type: 'varchar', length: 255, nullable: true })
  public uciNumber?: string;

  @Column({ name: 'drive_history', type: 'text', nullable: true })
  public driveHistory?: string;

  @Column({ name: 'most_active_region', type: 'varchar', length: 255, nullable: true })
  public mostActiveRegion?: string;

  @Column({ name: 'truck', type: 'json', nullable: true })
  public truck?: any;

  @Column({ name: 'identity_card', type: 'json', nullable: true })
  public identityCard?: any;

  @Column({ name: 'driver_license', type: 'json', nullable: true })
  public driverLicense?: any;

  @Column({ name: 'delivery_category', type: 'json', nullable: true })
  public deliveryCategory?: any;

  @Column({ name: 'working_time', type: 'json', nullable: true })
  public workingTime?: any;

  @Column({
    name: 'user_type',
    type: 'enum',
    enum: UserTypeEnum,
    default: UserTypeEnum.SHIPPER,
  })
  public userType: UserTypeEnum;

  @Column({ name: 'images_selfie', type: 'json', nullable: true })
  public imagesSelfie?: string[];

  @Column({ name: 'images_uci', type: 'json', nullable: true })
  public imagesUCI?: string[];

  @Column({ name: 'rating', type: 'double', default: 0 })
  public rating: number;

  @Column({ name: 'delivery_price', type: 'double', default: 0 })
  public deliveryPrice: number;
}
