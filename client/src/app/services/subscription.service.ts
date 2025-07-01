import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import {
  BillingPeriod,
  PaymentStatusResponse,
  PlanType,
} from '../models/subscription';

@Injectable({ providedIn: 'root' })
export class SubscriptionService {
  private apiUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  startSubscription(
    planType: PlanType,
    billingPeriod: BillingPeriod
  ): Observable<string> {
    const successUrl = `${window.location.origin}/dashboard/subscriptions/pending`;
    const cancelUrl = `${window.location.origin}/dashboard/subscriptions`;

    return this.http
      .post<{ url: string }>(`${this.apiUrl}/api/checkout/start`, {
        planType,
        billingPeriod,
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

  changePlan(
    newPlan: PlanType,
    billingPeriod: BillingPeriod
  ): Observable<boolean> {
    return this.http
      .post<{ message: string }>(`${this.apiUrl}/api/checkout/change-plan`, {
        newPlan,
        billingPeriod,
      })
      .pipe(
        map(() => {
          return true;
        })
      );
  }

  cancelSubscription(password: string): Observable<boolean> {
    return this.http
      .post<{ message: string }>(`${this.apiUrl}/api/checkout/cancel`, {
        password,
      })
      .pipe(
        map(() => {
          return true;
        }),
        catchError((err) => of(false))
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
