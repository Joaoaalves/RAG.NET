import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { interval, Subscription } from 'rxjs';

interface Provider {
  name: string;
  logo: string;
  latency: string;
  cost: string;
  quality: string;
}

@Component({
  selector: 'app-provider-integration-demo',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './provider-integration-demo.component.html',
})
export class ProviderIntegrationDemoComponent implements OnInit, OnDestroy {
  providers: Provider[] = [
    {
      name: 'OpenAI',
      logo: '/img/providers/openai.svg',
      latency: '120ms',
      cost: '$0.03',
      quality: '94%',
    },
    {
      name: 'Anthropic',
      logo: '/img/providers/anthropic.svg',
      latency: '95ms',
      cost: '$0.025',
      quality: '96%',
    },
    {
      name: 'Gemini',
      logo: '/img/providers/gemini.svg',
      latency: '110ms',
      cost: '$0.02',
      quality: '92%',
    },
  ];

  selectedProvider = 0;
  showMetrics = false;
  private sub!: Subscription;

  ngOnInit() {
    this.cycleProviders();
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  private cycleProviders() {
    this.sub = interval(4000).subscribe(() => {
      this.selectedProvider =
        (this.selectedProvider + 1) % this.providers.length;
      this.showMetrics = false;

      setTimeout(() => {
        this.showMetrics = true;
      }, 500);
    });
  }

  get performanceValues() {
    return [85, 92, 94];
  }

  get performanceLabels() {
    return ['Latency', 'Cost Efficiency', 'Quality'];
  }
}
