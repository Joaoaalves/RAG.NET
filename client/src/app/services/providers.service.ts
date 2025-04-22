import { Injectable, Provider } from '@angular/core';
import { HttpClient } from '@angular/common/http';

// Types and Constants
import { PROVIDERS_DATA } from '../core/constants/providers.constant';

import { BaseApiService } from './base-api.service';
import { catchError, map, Observable, of } from 'rxjs';
import {
  Provider as AIProvider,
  GetProvidersResponse,
  ProviderData,
  ProvidersResponse,
  SupportedProvider,
} from '../models/provider';

@Injectable({
  providedIn: 'root',
})
export class ProvidersService extends BaseApiService {
  constructor(http: HttpClient) {
    super(http);
  }

  mapProviderData(provider: SupportedProvider): ProviderData {
    return PROVIDERS_DATA[provider];
  }

  getAllProviders() {
    return Object.values(PROVIDERS_DATA);
  }

  getUserProviders(): Observable<GetProvidersResponse> {
    return this.http
      .get<GetProvidersResponse>(this.buildUrl('/api/provider'))
      .pipe(map((response) => response));
  }

  addProvider(
    provider: SupportedProvider,
    apiKey: string
  ): Observable<AIProvider> {
    const providerData = this.mapProviderData(provider);
    if (!providerData) {
      throw new Error(`Provider ${provider} not found`);
    }

    return this.http
      .post<AIProvider>(this.buildUrl('/api/provider'), {
        provider: providerData.id,
        apiKey,
      })
      .pipe(map((response) => response));
  }

  updateProvider(providerId: string, apiKey: string): Observable<string> {
    return this.http
      .put<Provider>(this.buildUrl(`/api/provider/${providerId}`), {
        apiKey,
      })
      .pipe(map((response) => apiKey));
  }

  deleteProvider(providerId: string): Observable<boolean> {
    return this.http
      .delete<void>(this.buildUrl(`/api/provider/${providerId}`))
      .pipe(
        map(() => true),
        catchError(() => of(false))
      );
  }
}
