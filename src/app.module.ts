import { Module } from '@nestjs/common';
import { ConfigModule, ConfigService } from '@nestjs/config';
import { TypeOrmModule } from '@nestjs/typeorm';
import { ServeStaticModule } from '@nestjs/serve-static';
import { APP_GUARD } from '@nestjs/core';
import { join } from 'path';

import { UserEntity } from './v1/users/entities/user.entity';
import { DeliveryEntity } from './v1/deliveries/entities/delivery.entity';
import { ScheduleEntity } from './v1/schedules/entities/schedule.entity';
import { FileEntity } from './v1/files/entities/file.entity';

import { AuthModule } from './v1/auth/auth.module';
import { UsersModule } from './v1/users/users.module';
import { DeliveriesModule } from './v1/deliveries/deliveries.module';
import { SchedulesModule } from './v1/schedules/schedules.module';
import { FilesModule } from './v1/files/files.module';
import { JwtAuthGuard } from './v1/auth/jwt-auth.guard';

@Module({
  imports: [
    ConfigModule.forRoot({
      isGlobal: true,
      envFilePath: '.env',
    }),
    TypeOrmModule.forRootAsync({
      imports: [ConfigModule],
      useFactory: (configService: ConfigService) => ({
        type: 'mysql',
        host: configService.get<string>('DB_HOST', 'localhost'),
        port: configService.get<number>('DB_PORT', 3306),
        username: configService.get<string>('DB_USERNAME', 'root'),
        password: configService.get<string>('DB_PASSWORD', 'Pippo321!@'),
        database: configService.get<string>('DB_DATABASE', 'freightmatic'),
        entities: [UserEntity, DeliveryEntity, ScheduleEntity, FileEntity],
        synchronize: configService.get<string>('NODE_ENV') === 'Development',
        logging: configService.get<string>('DB_LOGGING') === 'true',
      }),
      inject: [ConfigService],
    }),
    ServeStaticModule.forRoot({
      rootPath: join(process.cwd(), 'uploads'),
      serveRoot: '/uploads',
      serveStaticOptions: {
        fallthrough: false,
      },
    }),
    AuthModule,
    UsersModule,
    DeliveriesModule,
    SchedulesModule,
    FilesModule,
  ],
  providers: [
    {
      provide: APP_GUARD,
      useClass: JwtAuthGuard,
    },
  ],
})
export class AppModule {}
