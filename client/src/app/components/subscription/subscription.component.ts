import { UserService } from 'src/app/services/user.service';
import { EnhancedSubscriptionComponent } from 'src/app/shared/components/subscription-form/enhanced/enhanced-subscription.component';
import { AscendSubscriptionComponent } from '../../shared/components/subscription-form/ascend/ascend-subscription.component';
import { Component, OnInit } from '@angular/core';
import { provideIcons } from '@ng-icons/core';
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
} from '@ng-icons/lucide';
import { SubscriptionService } from 'src/app/services/subscription.service';
import { PlanType, Subscription } from 'src/app/models/subscription';
import { CommonModule } from '@angular/common';

import { HlmToasterComponent } from 'libs/ui/ui-sonner-helm/src/lib/hlm-toaster.component';

@Component({
  templateUrl: 'subscription.component.html',
  imports: [
    AscendSubscriptionComponent,
    EnhancedSubscriptionComponent,
    HlmToasterComponent,
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
    }),
  ],
  standalone: true,
})
export class SubscriptionComponent implements OnInit {
  subscription?: Subscription;

  constructor(
    private subscriptionService: SubscriptionService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.userService.user$.subscribe((user) => {
      this.subscription = user?.subscription;
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
