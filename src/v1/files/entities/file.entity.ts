import { BaseEntity, Column, CreateDateColumn, Entity, PrimaryGeneratedColumn } from 'typeorm';

@Entity({ name: 'files' })
export class FileEntity extends BaseEntity {
  @PrimaryGeneratedColumn('uuid')
  public id: string;

  @Column({ name: 'filename', type: 'varchar', length: 255 })
  public filename: string;

  @Column({ name: 'original_name', type: 'varchar', length: 255 })
  public originalName: string;

  @Column({ name: 'path', type: 'varchar', length: 500 })
  public path: string;

  @Column({ name: 'url', type: 'varchar', length: 500 })
  public url: string;

  @CreateDateColumn({ name: 'created_at' })
  public createdAt: Date;
}
