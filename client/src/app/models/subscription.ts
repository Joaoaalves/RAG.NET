export enum PaymentStatus {
  'PENDING',
  'SUCCESS',
  'CANCELLED',
  'ERROR',
}

export interface PaymentStatusResponse {
  status: PaymentStatus;
}
