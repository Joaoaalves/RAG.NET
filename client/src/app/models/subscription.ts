export enum PaymentStatus {
  'PENDING',
  'SUCCESS',
  'CANCELED',
  'ERROR',
}

export enum SubscriptionStatus {
  CANCELED = 0,
  ACTIVE = 1,
}

export interface PaymentStatusResponse {
  status: PaymentStatus;
}

export enum PlanType {
  CORE,
  ENHANCED,
  ASCEND,
}

export interface Subscription {
  planType: PlanType;
  expiresAt: Date;
  subscribedAt: Date;
  status?: SubscriptionStatus;
}
