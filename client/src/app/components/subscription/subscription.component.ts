import { UserService } from 'src/app/services/user.service';
import { EnhancedSubscriptionComponent } from 'src/app/shared/components/subscription-form/enhanced/enhanced-subscription.component';
import { AscendSubscriptionComponent } from '../../shared/components/subscription-form/ascend/ascend-subscription.component';
import { Component } from '@angular/core';
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
} from '@ng-icons/lucide';
import { SubscriptionService } from 'src/app/services/subscription.service';
import { PlanType } from 'src/app/models/subscription';

@Component({
  templateUrl: 'subscription.component.html',
  imports: [AscendSubscriptionComponent, EnhancedSubscriptionComponent],
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
      lucideMessageSquare,
    }),
  ],
  standalone: true,
})
export class SubscriptionComponent {
  constructor(
    private subscriptionService: SubscriptionService,
    private userService: UserService
  ) {}

  handleSubscribe(plan: PlanType) {
    this.userService.clearCache();

    this.userService.getInfo().subscribe((user) => {
      if (user.subscription.planType != PlanType.CORE) {
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
}
