import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NgIcon } from '@ng-icons/core';
import { CubeComponent } from './cube.component';
import {
  BillingPeriod,
  PlanType,
  SubscriptionStatus,
} from 'src/app/models/subscription';
import { UserService } from 'src/app/services/user.service';
import { AlertComponent } from '../../alert/alert.component';

@Component({
  selector: 'app-enhanced-subscription',
  imports: [CommonModule, NgIcon, CubeComponent],
  templateUrl: './enhanced-subscription.component.html',
  standalone: true,
})
export class EnhancedSubscriptionComponent {
  @Input() isLoading: boolean = false;
  @Output() subscribe = new EventEmitter<{
    planType: PlanType;
    billingPeriod: BillingPeriod;
  }>();

  expiresAt: string = '';
  isActive: boolean = true;
  isSubscribed: boolean = false;

  constructor(private userService: UserService) {
    this.userService.user$.subscribe((user) => {
      if (user) {
        var subscription = user.subscription;

        this.isActive = subscription.status === SubscriptionStatus.ACTIVE;
        this.isSubscribed = subscription.planType == PlanType.ENHANCED;
        this.expiresAt = `Expires at ${new Date(
          subscription.expiresAt
        ).toLocaleDateString()}`;
      }
    });
  }

  onSubscribeMonthly() {
    if (!this.isSubscribed) {
      this.subscribe.emit({
        planType: PlanType.ENHANCED,
        billingPeriod: BillingPeriod.MONTHLY,
      });
    }
  }

  onSubscribeSemiAnnually() {
    if (!this.isSubscribed) {
      this.subscribe.emit({
        planType: PlanType.ENHANCED,
        billingPeriod: BillingPeriod.SEMIANNUALLY,
      });
    }
  }

  onSubscribeYearly() {
    if (!this.isSubscribed) {
      this.subscribe.emit({
        planType: PlanType.ENHANCED,
        billingPeriod: BillingPeriod.YEARLY,
      });
    }
  }

  get features() {
    return [
      { icon: 'lucideZap', text: '3,500 Monthly Tokens' },
      { icon: 'lucideDatabase', text: 'All Embedding Providers' },
      { icon: 'lucideMessageSquare', text: 'All Conversation Providers' },
      { icon: 'lucideCode', text: 'Access to Proposition Chunker' },
      { icon: 'lucideShield', text: 'Auto Query Enhancer' },
      { icon: 'lucideCheck', text: 'Relevant Segment Extraction' },
      { icon: 'lucideUpload', text: 'Upload up to 100MB per file' },
      { icon: 'lucideCrown', text: 'Up to 50 Workflows' },
      { icon: 'lucideShield', text: 'Priority Support' },
      { icon: 'lucideCode', text: 'API Access' },
    ];
  }
}
