import {
  DailyTransactionsAggregate,
  GetTransactionResponse,
  GetTransactionsRequest,
  GetWalletRequest,
  GetWalletResponse,
  PagedTransactions,
  Wallet,
} from './../models/wallet';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class WalletService {
  private apiUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}

  getWallet(): Observable<Wallet> {
    return this.httpClient
      .get<{ wallet: Wallet }>(`${this.apiUrl}/api/wallet`)
      .pipe(map((res) => res.wallet));
  }

  getTransactions({
    start,
    end,
    page,
    pageSize,
  }: GetTransactionsRequest): Observable<PagedTransactions> {
    let params = new HttpParams()
      .set('start', start.toISOString())
      .set('end', end.toISOString())
      .set('page', page)
      .set('pageSize', pageSize);

    return this.httpClient
      .get<GetTransactionResponse>(`${this.apiUrl}/api/wallet/transactions`, {
        params,
      })
      .pipe(map((res) => res.transactions));
  }

  getDailyTransactions({
    start,
    end,
  }: GetWalletRequest): Observable<DailyTransactionsAggregate[]> {
    let params = new HttpParams()
      .set('start', start.toISOString())
      .set('end', end.toISOString());

    return this.httpClient
      .get<{ dailyTransactions: DailyTransactionsAggregate[] }>(
        `${this.apiUrl}/api/wallet/transactions/daily`,
        { params }
      )
      .pipe(map((res) => res.dailyTransactions));
  }
}
