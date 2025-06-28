import { CommonModule } from '@angular/common';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { NgIcon } from '@ng-icons/core';
import { CubeComponent } from './cube.component';
import { PlanType } from 'src/app/models/subscription';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-enhanced-subscription',
  imports: [CommonModule, NgIcon, CubeComponent],
  templateUrl: './enhanced-subscription.component.html',
  standalone: true,
})
export class EnhancedSubscriptionComponent {
  @Output() subscribe = new EventEmitter<PlanType>();
  isLoading: boolean = false;
  currentPlan: PlanType = PlanType.CORE;

  constructor(private userService: UserService) {
    this.userService.user$.subscribe((user) => {
      if (user) this.currentPlan = user.subscription.planType;
    });
  }

  get isSubscribed() {
    return this.currentPlan == PlanType.ENHANCED;
  }

  onSubscribe() {
    this.subscribe.emit(PlanType.ENHANCED);
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
