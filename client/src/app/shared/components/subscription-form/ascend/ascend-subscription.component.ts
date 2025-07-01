import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NgIcon } from '@ng-icons/core';
import { OrbitalSystemComponent } from './orbital-system.component';
import {
  BillingPeriod,
  PlanType,
  SubscriptionStatus,
} from 'src/app/models/subscription';
import { UserService } from 'src/app/services/user.service';
import { BillingPeriodSelectorComponent } from '../billing-period-selector/billing-period-selector.component';

@Component({
  selector: 'app-ascend-subscription',
  imports: [
    CommonModule,
    NgIcon,
    OrbitalSystemComponent,
    BillingPeriodSelectorComponent,
  ],
  templateUrl: './ascend-subscription.component.html',
  standalone: true,
})
export class AscendSubscriptionComponent {
  @Input() isLoading: boolean = false;
  @Output() subscribe = new EventEmitter<{
    planType: PlanType;
    billingPeriod: BillingPeriod;
  }>();

  expiresAt: string = '';
  isActive: boolean = true;
  isAscend: boolean = true;
  isSubscribed: boolean = false;

  selectedPeriod: BillingPeriod | null = null;

  billingOptions = [
    {
      period: BillingPeriod.MONTHLY,
      label: 'Monthly',
      price: 30,
      discount: 0,
      description: 'Pay every month. No discount applied.',
    },
    {
      period: BillingPeriod.SEMIANNUALLY,
      label: '6 Months',
      price: 149.99,
      discount: 16,
      description: 'Save ~16% with a semiannual plan.',
    },
    {
      period: BillingPeriod.YEARLY,
      label: 'Yearly',
      price: 269.99,
      discount: 25,
      description: 'Save ~25% by paying once a year.',
    },
  ];

  constructor(private userService: UserService) {
    this.userService.user$.subscribe((user) => {
      if (user) {
        var subscription = user.subscription;

        this.isActive = subscription.status === SubscriptionStatus.ACTIVE;
        this.isAscend = subscription.planType == PlanType.ASCEND;
        this.isSubscribed = subscription.planType !== PlanType.CORE;
        this.expiresAt = `Expires at ${new Date(
          subscription.expiresAt
        ).toLocaleDateString()}`;
      }
    });
  }

  onSubscribe() {
    if (this.selectedPeriod && !this.isSubscribed) {
      this.subscribe.emit({
        planType: PlanType.ASCEND,
        billingPeriod: this.selectedPeriod,
      });
    }
  }

  selectPeriod(period: BillingPeriod) {
    this.selectedPeriod = period;
  }

  get features() {
    return [
      { icon: 'lucideInfinity', text: '10,500 Monthly Tokens' },
      { icon: 'lucideDatabase', text: 'All Embedding Providers' },
      { icon: 'lucideMessageSquare', text: 'All Conversation Providers' },
      { icon: 'lucideCode', text: 'Semantic Chunker' },
      { icon: 'lucideZap', text: 'SERP Context Enricher' },
      { icon: 'lucideUpload', text: '1GB/File Uploads' },
      { icon: 'lucideInfinity', text: 'Unlimited Workflows' },
      {
        icon: 'lucideDatabase',
        text: 'Access to Ready-to-Use Knowledge Bases',
      },
      { icon: 'lucideCrown', text: 'Top-tier Support' },
      { icon: 'lucideCode', text: 'API Access + Webhooks' },
    ];
  }
}
