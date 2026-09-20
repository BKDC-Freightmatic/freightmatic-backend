export enum UserTypeEnum {
  SHIPPER = 'SHIPPER',
  TRUCKER = 'TRUCKER',
  ADMIN = 'ADMIN',
}

export enum PickupCategoryEnum {
  NOW = 'NOW',
  SCHEDULE = 'SCHEDULE',
}

export enum DeliveryStatusEnum {
  PENDING = 'PENDING',
  ACCEPTED = 'ACCEPTED',
  DELIVERING = 'DELIVERING',
  COMPLETED = 'COMPLETED',
  CANCELLED = 'CANCELLED',
}

export enum TruckCategoryEnum {
  CONTAINER = 'CONTAINER',
  BOX_TRUCK = 'BOX_TRUCK',
  FLATBED = 'FLATBED',
  VAN = 'VAN',
  REFRIGERATED = 'REFRIGERATED',
}
