import { Component, ElementRef, ViewChild, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { interval, switchMap, catchError, of, Subject, takeUntil } from 'rxjs';
import { SubscriptionService } from 'src/app/services/subscription.service';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideLoader, lucideCheck, lucideCircleX } from '@ng-icons/lucide';
import { gsap } from 'gsap';
import {
  PaymentStatus,
  PaymentStatusResponse,
} from 'src/app/models/subscription';
import { UserService } from 'src/app/services/user.service';

@Component({
  standalone: true,
  imports: [CommonModule, NgIcon],
  providers: [
    provideIcons({
      lucideLoader,
      lucideCheck,
      lucideCircleX,
    }),
  ],
  templateUrl: './subscription-pending.component.html',
})
export class SubscriptionPendingComponent implements OnDestroy {
  @ViewChild('loaderIcon', { static: true }) loaderIcon!: ElementRef;

  private destroy$ = new Subject<void>();
  status: PaymentStatus = PaymentStatus.PENDING;

  constructor(
    private subscriptionService: SubscriptionService,
    private userService: UserService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const paymentId = this.route.snapshot.queryParamMap.get('paymentId');
    if (!paymentId) {
      this.router.navigate(['/dashboard/workflows']);
      return;
    }

    interval(1000)
      .pipe(
        switchMap(() =>
          this.subscriptionService.checkPaymentStatus(paymentId).pipe(
            catchError((err) => {
              if (err.status === 404) {
                this.destroy$.next();
                this.router.navigate(['/dashboard/workflows']);
              }
              return of(null);
            })
          )
        ),
        takeUntil(this.destroy$)
      )
      .subscribe((res: PaymentStatusResponse | null) => {
        if (!res) return;

        this.status = res.status;

        if (this.status === PaymentStatus.SUCCESS) {
          this.destroy$.next();
          setTimeout(() => {
            this.userService.clearCache();
            this.userService.getInfo().subscribe(() => {
              this.router.navigate(['/dashboard/subscriptions/success'], {
                queryParams: { paymentId },
              });
            });
          }, 2000);
        }
      });

    gsap.fromTo(
      this.loaderIcon.nativeElement,
      { opacity: 0, scale: 0.5 },
      { opacity: 1, scale: 1, duration: 0.6, ease: 'power2.out' }
    );
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  goToDashboard() {
    this.router.navigate(['/dashboard/workflows']);
  }
}
