import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import {
  PaymentStatus,
  PaymentStatusResponse,
  PlanType,
} from '../models/subscription';

@Injectable({ providedIn: 'root' })
export class SubscriptionService {
  private apiUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  startSubscription(planType: PlanType): Observable<string> {
    const successUrl = `${window.location.origin}/dashboard/subscriptions/pending`;
    const cancelUrl = `${window.location.origin}/dashboard/subscriptions`;
    console.log('Starting Subscription');
    return this.http
      .post<{ url: string }>(`${this.apiUrl}/api/checkout/start`, {
        planType,
        successUrl,
        cancelUrl,
      })
      .pipe(
        map((response) => {
          if (!response.url) {
            throw new Error('Invalid Checkout URL');
          }
          return response.url;
        })
      );
  }

  checkPaymentStatus(paymentId: string): Observable<PaymentStatusResponse> {
    return this.http.get<PaymentStatusResponse>(
      `${this.apiUrl}/api/checkout/status`,
      {
        params: { paymentId },
      }
    );
  }
}
