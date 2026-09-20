import {
  BaseEntity,
  CreateDateColumn,
  UpdateDateColumn,
  Column,
  PrimaryColumn,
} from 'typeorm';

export abstract class BaseGeneralEntity extends BaseEntity {
  @PrimaryColumn('varchar', { length: 36, name: 'id' })
  public id: string;

  @CreateDateColumn({ type: 'datetime', name: 'created_on' })
  public createdOn: Date;

  @UpdateDateColumn({ type: 'datetime', name: 'modified_on' })
  public modifiedOn: Date;

  @Column({ name: 'created_by', type: 'varchar', length: 255, nullable: true })
  public createdBy?: string;

  @Column({ name: 'modified_by', type: 'varchar', length: 255, nullable: true })
  public modifiedBy?: string;

  constructor(args: any = {}) {
    super();
    Object.assign(this, args);
  }
}
