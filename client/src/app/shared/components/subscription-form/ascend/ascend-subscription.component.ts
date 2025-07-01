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

@Component({
  selector: 'app-ascend-subscription',
  imports: [CommonModule, NgIcon, OrbitalSystemComponent],
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
  isSubscribed: boolean = false;
  isUpgradable: boolean = false;

  constructor(private userService: UserService) {
    this.userService.user$.subscribe((user) => {
      if (user) {
        var subscription = user.subscription;

        this.isActive = subscription.status === SubscriptionStatus.ACTIVE;
        this.isSubscribed = subscription.planType == PlanType.ASCEND;
        this.expiresAt = `Expires at ${new Date(
          subscription.expiresAt
        ).toLocaleDateString()}`;

        this.isUpgradable =
          this.isActive && subscription.planType == PlanType.ENHANCED;
      }
    });
  }

  onSubscribeMonthly() {
    if (!this.isSubscribed) {
      this.subscribe.emit({
        planType: PlanType.ASCEND,
        billingPeriod: BillingPeriod.MONTHLY,
      });
    }
  }

  onSubscribeSemiAnnually() {
    if (!this.isSubscribed) {
      this.subscribe.emit({
        planType: PlanType.ASCEND,
        billingPeriod: BillingPeriod.SEMIANNUALLY,
      });
    }
  }

  onSubscribeYearly() {
    if (!this.isSubscribed) {
      this.subscribe.emit({
        planType: PlanType.ASCEND,
        billingPeriod: BillingPeriod.YEARLY,
      });
    }
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
