export interface Wallet {
  freeTokens: number;
  paidTokens: number;
  lastFreeTokenResetAt: Date;
  transactions: Transaction[];
}

export interface Transaction {
  id: string;
  workflowId: string;
  operationName: string;
  contextInfo: string;
  cost: number;
  timeStamp: Date;
  source: TransactionSource;
}

export enum TransactionSource {
  FREE,
  PAID,
}

export interface DailyTransactionsAggregate {
  date: string;
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

export interface GetTransactionsRequest {
  start: Date;
  end: Date;
  page: number;
  pageSize: number;
}

export interface GetTransactionResponse {
  transactions: PagedTransactions;
}

export interface PagedTransactions {
  items: Transaction[];
  totalCount: number;
}
