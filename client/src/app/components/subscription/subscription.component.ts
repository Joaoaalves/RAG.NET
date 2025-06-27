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
export class SubscriptionComponent {}
