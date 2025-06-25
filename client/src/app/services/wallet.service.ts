import { GetWalletResponse, Wallet } from './../models/wallet';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class WalletService {
  private apiUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}

  getInfo(): Observable<Wallet> {
    return this.httpClient
      .get<GetWalletResponse>(`${this.apiUrl}/api/wallet`)
      .pipe(map((response) => response.wallet));
  }
}
