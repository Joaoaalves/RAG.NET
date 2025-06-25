import {
  DailyTransactionsAggregate,
  GetWalletRequest,
  GetWalletResponse,
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

  getInfo({ start, end }: GetWalletRequest): Observable<GetWalletResponse> {
    let params = new HttpParams();

    params = params.append('start', start.toISOString());
    params = params.append('end', end.toISOString());

    return this.httpClient
      .get<GetWalletResponse>(`${this.apiUrl}/api/wallet`, {
        params: params,
      })
      .pipe(map((response) => response));
  }
}
