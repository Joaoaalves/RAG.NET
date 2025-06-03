import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { BaseApiService } from './base-api.service';
import { catchError, map, Observable, of } from 'rxjs';
import { Provider as AIProvider, Provider } from '../models/provider';

@Injectable({
  providedIn: 'root',
})
export class ProvidersService extends BaseApiService {
  providers: Provider[] = [];
  constructor(http: HttpClient) {
    super(http);
  }

  getUserProviders(): Observable<Provider[]> {
    return this.http.get<Provider[]>(this.buildUrl('/api/provider')).pipe(
      map((response) => {
        this.providers = response;
        return response;
      })
    );
  }

  addProvider(provider: Provider, apiKey: string): Observable<AIProvider> {
    console.log(provider);
    if (this.isValidApiKey(provider.providerId, apiKey)) {
      return this.http
        .post<AIProvider>(this.buildUrl('/api/provider'), {
          provider: provider.providerId,
          apiKey,
        })
        .pipe(map((response) => response));
    }

    throw new Error(`Invalid API Key format for provider ${provider.name}`);
  }

  updateProvider(provider: Provider, apiKey: string): Observable<string> {
    if (this.isValidApiKey(provider.providerId, apiKey)) {
      return this.http
        .put<Provider>(this.buildUrl(`/api/provider/${provider.id}`), {
          apiKey,
        })
        .pipe(map((response) => apiKey));
    }

    throw new Error(`Invalid API Key format for provider ${provider.name}`);
  }

  deleteProvider(providerId: string): Observable<boolean> {
    return this.http
      .delete<void>(this.buildUrl(`/api/provider/${providerId}`))
      .pipe(
        map(() => true),
        catchError(() => of(false))
      );
  }

  private getProviderById(providerId: number) {
    return this.providers.find((prov) => prov.providerId === providerId);
  }

  private isValidApiKey(providerId: number, apiKey: string): boolean {
    const prov = this.getProviderById(providerId);

    if (prov) {
      const regex = new RegExp(prov.pattern);
      return regex.test(apiKey);
    }

    return false;
  }
}
