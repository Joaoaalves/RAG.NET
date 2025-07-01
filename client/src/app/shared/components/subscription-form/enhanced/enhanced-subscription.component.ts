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
import { BillingPeriodSelectorComponent } from '../billing-period-selector/billing-period-selector.component';

@Component({
  selector: 'app-enhanced-subscription',
  imports: [
    CommonModule,
    NgIcon,
    CubeComponent,
    BillingPeriodSelectorComponent,
  ],
  templateUrl: './enhanced-subscription.component.html',
  standalone: true,
})
export class EnhancedSubscriptionComponent {
  @Input() isLoading: boolean = false;
  @Output() subscribe = new EventEmitter<{
    planType: PlanType;
    billingPeriod: BillingPeriod;
  }>();

  expiresAt = '';
  isActive = true;
  isEnhanced = false;
  isSubscribed = false;

  selectedPeriod: BillingPeriod | null = null;

  billingOptions = [
    {
      period: BillingPeriod.MONTHLY,
      label: 'Monthly',
      price: 10,
      discount: 0,
      description: 'Pay every month. No discount applied.',
    },
    {
      period: BillingPeriod.SEMIANNUALLY,
      label: '6 Months',
      price: 49.99,
      discount: 16,
      description: 'Save ~16% with a semiannual plan.',
    },
    {
      period: BillingPeriod.YEARLY,
      label: 'Yearly',
      price: 89.99,
      discount: 25,
      description: 'Save ~25% by paying once a year.',
    },
  ];

  constructor(private userService: UserService) {
    this.userService.user$.subscribe((user) => {
      if (user) {
        const subscription = user.subscription;
        this.isActive = subscription.status === SubscriptionStatus.ACTIVE;
        this.isEnhanced = subscription.planType === PlanType.ENHANCED;
        this.isSubscribed = subscription.planType !== PlanType.CORE;
        this.expiresAt = `Expires at ${new Date(
          subscription.expiresAt
        ).toLocaleDateString()}`;
      }
    });
  }

  selectPeriod(period: BillingPeriod) {
    this.selectedPeriod = period;
  }

  onSubscribe() {
    if (this.selectedPeriod && !this.isSubscribed) {
      this.subscribe.emit({
        planType: PlanType.ENHANCED,
        billingPeriod: this.selectedPeriod,
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
