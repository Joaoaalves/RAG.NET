export interface Wallet {
  freeTokens: number;
  paidTokens: number;
  lastFreeTokenResetAt: Date;
  transactions: Transaction[];
}

export interface Transaction {
  id: string;
  operationName: string;
  contextInfo: string;
  cost: number;
  timesTamp: Date;
  source: TransactionSource;
}

export enum TransactionSource {
  FREE,
  PAID,
}

export interface DailyTransactionsAggregate {
  date: Date;
  freeTokensConsumed: number;
  paidTokensConsumed: number;
}

export interface GetWalletResponse {
  wallet: Wallet;
  dailyTransactions: DailyTransactionsAggregate[];
}

export interface GetWalletRequest {
  start: Date;
  end: Date;
}
