export enum PaymentStatus {
  'PENDING',
  'SUCCESS',
  'CANCELLED',
  'ERROR',
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
}
