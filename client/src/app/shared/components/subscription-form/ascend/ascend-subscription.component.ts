import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NgIcon } from '@ng-icons/core';
import { OrbitalSystemComponent } from './orbital-system.component';

@Component({
  selector: 'app-ascend-subscription',
  imports: [CommonModule, NgIcon, OrbitalSystemComponent],
  templateUrl: './ascend-subscription.component.html',
  standalone: true,
})
export class AscendSubscriptionComponent {
  @Input() isLoading = false;
  @Output() subscribe = new EventEmitter<'ascend'>();

  onSubscribe() {
    this.subscribe.emit('ascend');
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
