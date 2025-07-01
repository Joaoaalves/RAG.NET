import { UserService } from 'src/app/services/user.service';
import { EnhancedSubscriptionComponent } from 'src/app/shared/components/subscription-form/enhanced/enhanced-subscription.component';
import { AscendSubscriptionComponent } from '../../shared/components/subscription-form/ascend/ascend-subscription.component';
import { Component, OnInit } from '@angular/core';
import { NgIcon, provideIcons } from '@ng-icons/core';
import {
  lucideArrowRight,
  lucideCode,
  lucideCrown,
  lucideDatabase,
  lucideInfinity,
  lucideUpload,
  lucideZap,
  lucideShield,
  lucideMessageSquare,
  lucideCheck,
  lucideTrash,
  lucideStar,
  lucideCalendar,
  lucideCreditCard,
} from '@ng-icons/lucide';
import { SubscriptionService } from 'src/app/services/subscription.service';
import {
  PlanType,
  Subscription,
  SubscriptionStatus,
} from 'src/app/models/subscription';
import { CommonModule } from '@angular/common';

import { HlmToasterComponent } from 'libs/ui/ui-sonner-helm/src/lib/hlm-toaster.component';
import { AlertComponent } from 'src/app/shared/components/alert/alert.component';

@Component({
  templateUrl: 'subscription.component.html',
  imports: [
    AscendSubscriptionComponent,
    EnhancedSubscriptionComponent,
    HlmToasterComponent,
    NgIcon,
    AlertComponent,
    CommonModule,
  ],
  providers: [
    provideIcons({
      lucideInfinity,
      lucideDatabase,
      lucideCode,
      lucideZap,
      lucideUpload,
      lucideCrown,
      lucideArrowRight,
      lucideShield,
      lucideCheck,
      lucideTrash,
      lucideMessageSquare,
      lucideCalendar,
      lucideCreditCard,
      lucideStar,
    }),
  ],
  standalone: true,
})
export class SubscriptionComponent implements OnInit {
  subscription?: Subscription;
  subscribedOn: string = '';
  isSubscribed: boolean = false;
  renewsOn: string = '';

  constructor(
    private subscriptionService: SubscriptionService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.userService.user$.subscribe((user) => {
      this.subscription = user?.subscription;
      if (this.subscription) {
        this.isSubscribed =
          this.subscription.status == SubscriptionStatus.ACTIVE;

        this.subscribedOn = this.getFormatedDate(
          this.subscription.subscribedAt
        );

        this.renewsOn = this.getFormatedDate(this.subscription.expiresAt);
      }
    });
  }

  getFormatedDate(date: string) {
    return new Date(date).toLocaleDateString('en-US', {
      month: 'short',
      day: 'numeric',
      year: 'numeric',
    });
  }

  handleCancel() {
    this.subscriptionService.cancelSubscription().subscribe({
      next: () => {
        this.userService.clearCache();
        window.location.reload();
      },
    });
  }

  handleSubscribe(plan: PlanType) {
    this.userService.clearCache();

    this.userService.getInfo().subscribe((user) => {
      if (
        user.subscription.planType != PlanType.CORE &&
        user.subscription.status
      ) {
        this.subscriptionService.changePlan(plan).subscribe({
          next: () => {
            this.userService.clearCache();
            window.location.href = '/dashboard/workflows';
          },
          error: (err) => console.error('Failed to change subscription plan'),
        });
      } else {
        this.subscriptionService.startSubscription(plan).subscribe({
          next: (url) => (window.location.href = url),
          error: (err) => console.error('Failed to start subscription', err),
        });
      }
    });
  }

  get subscriptionStatus() {
    if (this.subscription?.status !== undefined) {
      return this.subscription.status ? 'Active' : 'Canceled';
    }

    return '';
  }
}
