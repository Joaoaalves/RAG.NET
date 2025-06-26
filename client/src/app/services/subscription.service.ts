import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { StripeService } from 'ngx-stripe';
import { map, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({ providedIn: 'root' })
export class SubscriptionService {
  private apiUrl = environment.apiUrl;
  constructor(private http: HttpClient, private stripeService: StripeService) {}

  startSubscription(): Observable<string> {
    const successUrl = `${window.location.origin}/checkout/success`;
    const cancelUrl = `${window.location.origin}/checkout/cancel`;

    return this.http
      .post<{ url: string }>(`${this.apiUrl}/api/checkout/start`, {
        successUrl,
        cancelUrl,
      })
      .pipe(
        map((response) => {
          if (!response.url) {
            throw new Error('Checkout URL inválida');
          }
          return response.url;
        })
      );
  }
}
