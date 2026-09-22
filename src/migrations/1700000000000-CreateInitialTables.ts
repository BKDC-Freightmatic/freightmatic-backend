import { MigrationInterface, QueryRunner } from 'typeorm';

export class CreateInitialTables1700000000000 implements MigrationInterface {
  name = 'CreateInitialTables1700000000000';

  public async up(queryRunner: QueryRunner): Promise<void> {
    // 1. Create users table
    await queryRunner.query(`
      CREATE TABLE IF NOT EXISTS \`users\` (
        \`id\` varchar(36) NOT NULL,
        \`created_on\` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
        \`modified_on\` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
        \`created_by\` varchar(255) NULL,
        \`modified_by\` varchar(255) NULL,
        \`name\` varchar(255) NULL,
        \`avatar\` varchar(500) NULL,
        \`user_name\` varchar(255) NOT NULL,
        \`email\` varchar(255) NOT NULL,
        \`phone_number\` varchar(50) NULL,
        \`password\` varchar(255) NOT NULL,
        \`refresh_token\` varchar(500) NULL,
        \`address\` text NULL,
        \`other_address\` text NULL,
        \`uci_number\` varchar(255) NULL,
        \`drive_history\` text NULL,
        \`most_active_region\` varchar(255) NULL,
        \`truck\` json NULL,
        \`identity_card\` json NULL,
        \`driver_license\` json NULL,
        \`delivery_category\` json NULL,
        \`working_time\` json NULL,
        \`user_type\` enum('SHIPPER','TRUCKER','ADMIN') NOT NULL DEFAULT 'SHIPPER',
        \`images_selfie\` json NULL,
        \`images_uci\` json NULL,
        \`rating\` double NOT NULL DEFAULT 0,
        \`delivery_price\` double NOT NULL DEFAULT 0,
        UNIQUE INDEX \`IDX_users_user_name\` (\`user_name\`),
        UNIQUE INDEX \`IDX_users_email\` (\`email\`),
        PRIMARY KEY (\`id\`)
      ) ENGINE=InnoDB;
    `);

    // 2. Create deliveries table
    await queryRunner.query(`
      CREATE TABLE IF NOT EXISTS \`deliveries\` (
        \`id\` varchar(36) NOT NULL,
        \`created_on\` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
        \`modified_on\` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
        \`created_by\` varchar(255) NULL,
        \`modified_by\` varchar(255) NULL,
        \`weight\` double NOT NULL DEFAULT 0,
        \`width\` double NOT NULL DEFAULT 0,
        \`length\` double NOT NULL DEFAULT 0,
        \`height\` double NOT NULL DEFAULT 0,
        \`product_category_id\` varchar(255) NULL,
        \`pickup_time\` datetime NULL,
        \`delivery_insurance_id\` varchar(255) NULL,
        \`trucker_id\` varchar(255) NULL,
        \`shipping_code\` varchar(255) NULL,
        \`price\` double NOT NULL DEFAULT 0,
        \`description\` text NULL,
        \`progress\` int NOT NULL DEFAULT 0,
        \`package_description\` text NULL,
        \`distance\` double NOT NULL DEFAULT 0,
        \`total_price\` double NOT NULL DEFAULT 0,
        \`steps\` json NULL,
        \`start_location\` json NULL,
        \`end_location\` json NULL,
        \`pickup_category\` enum('NOW','SCHEDULE') NOT NULL DEFAULT 'NOW',
        \`status\` enum('PENDING','ACCEPTED','DELIVERING','COMPLETED','CANCELLED') NOT NULL DEFAULT 'PENDING',
        \`product_images\` json NULL,
        \`schedule_id\` varchar(255) NULL,
        PRIMARY KEY (\`id\`)
      ) ENGINE=InnoDB;
    `);

    // 3. Create schedules table
    await queryRunner.query(`
      CREATE TABLE IF NOT EXISTS \`schedules\` (
        \`id\` varchar(36) NOT NULL,
        \`created_on\` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
        \`modified_on\` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
        \`created_by\` varchar(255) NULL,
        \`modified_by\` varchar(255) NULL,
        \`trucker_id\` varchar(255) NOT NULL,
        \`space_available\` int NOT NULL DEFAULT 0,
        \`start_time\` datetime NULL,
        \`end_time\` datetime NULL,
        \`current_location\` json NULL,
        \`start_location\` json NULL,
        \`end_location\` json NULL,
        \`truck_category\` enum('CONTAINER','BOX_TRUCK','FLATBED','VAN','REFRIGERATED') NOT NULL DEFAULT 'BOX_TRUCK',
        \`width\` int NOT NULL DEFAULT 0,
        \`length\` int NOT NULL DEFAULT 0,
        \`height\` int NOT NULL DEFAULT 0,
        \`postal_code\` varchar(50) NULL,
        PRIMARY KEY (\`id\`)
      ) ENGINE=InnoDB;
    `);
  }

  public async down(queryRunner: QueryRunner): Promise<void> {
    await queryRunner.query(`DROP TABLE IF EXISTS \`schedules\`;`);
    await queryRunner.query(`DROP TABLE IF EXISTS \`deliveries\`;`);
    await queryRunner.query(`DROP TABLE IF EXISTS \`users\`;`);
  }
}
