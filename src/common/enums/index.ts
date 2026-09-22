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
  NA = 'NA',
  PROCESSING = 'Processing',
  PAYMENT_DONE = 'PaymentDone',
  DONE = 'Done',
  FAILED = 'Failed',
  ALL = 'ALL',
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
  PARTIAL = 'Partial',
  FULL = 'Full',
}
