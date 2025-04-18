import { Injectable, Provider } from '@angular/core';
import { HttpClient } from '@angular/common/http';

// Types and Constants
import { PROVIDERS_DATA } from '../core/constants/providers.constant';

import { BaseApiService } from './base-api.service';
import { catchError, map, Observable, of } from 'rxjs';
import {
  ProviderCreateResponse,
  ProviderData,
  SupportedProvider,
} from '../models/provider';

@Injectable({
  providedIn: 'root',
})
export class ProvidersService extends BaseApiService {
  constructor(http: HttpClient) {
    super(http);
  }

  getProviderData(provider: SupportedProvider): ProviderData {
    return PROVIDERS_DATA[provider];
  }

  getAllProviders() {
    return Object.values(PROVIDERS_DATA);
  }

  addProvider(
    provider: SupportedProvider,
    apiKey: string
  ): Observable<ProviderCreateResponse> {
    const providerData = this.getProviderData(provider);
    if (!providerData) {
      throw new Error(`Provider ${provider} not found`);
    }

    return this.http
      .post<ProviderCreateResponse>(this.buildUrl('/api/providers'), {
        provider,
        apiKey,
      })
      .pipe(map((response) => response));
  }

  updateProvider(providerId: string, apiKey: string): Observable<string> {
    return this.http
      .put<ProviderCreateResponse>(
        this.buildUrl(`/api/providers/${providerId}`),
        {
          apiKey,
        }
      )
      .pipe(map((response) => apiKey));
  }

  deleteProvider(providerId: string): Observable<boolean> {
    return this.http
      .delete<void>(this.buildUrl(`/api/providers/${providerId}`))
      .pipe(
        map(() => true),
        catchError(() => of(false))
      );
  }
}
