import { Component } from '@angular/core';
import { SupportedProvidersComponent } from '../supported-providers/supported-providers.component';
import { AboutItemComponent } from './about-item.component';
import { CommonModule } from '@angular/common';
import { WorkflowBuilderDemoComponent } from './workflow-builder-demo.component';
import { ApiIntegrationDemoComponent } from './api-integration-demo.component';
import { BeginnerExpertDemoComponent } from './beginer-expert-demo.component';
import { ProviderIntegrationDemoComponent } from './provider-integration-demo.component';
import { provideIcons } from '@ng-icons/core';
import {
  lucideBookOpen,
  lucideCable,
  lucideChartCandlestick,
  lucideComponent,
  lucideFastForward,
  lucideLayoutTemplate,
  lucideRadar,
  lucideRefreshCcw,
  lucideSearch,
  lucideSettings,
  lucideTarget,
  lucideWrench,
} from '@ng-icons/lucide';

import { AboutItem } from 'src/app/models/about';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  imports: [SupportedProvidersComponent, AboutItemComponent, CommonModule],
  providers: [
    provideIcons({
      lucideComponent,
      lucideFastForward,
      lucideSearch,
      lucideRefreshCcw,
      lucideChartCandlestick,
      lucideTarget,
      lucideLayoutTemplate,
      lucideSettings,
      lucideWrench,
      lucideCable,
      lucideRadar,
      lucideBookOpen,
    }),
  ],
  standalone: true,
})
export class AboutComponent {
  aboutItems: AboutItem[] = [
    {
      title: 'Create Retrieval Pipelines with a Few Clicks',
      subtitle:
        'Design sophisticated **Retrieval-Augmented Generation pipelines** using a visual, **no-code builder**. Mix and match embedding models, chunking strategies, filters, and LLMs — then deploy your workflow instantly through a single API endpoint. Fine-tune every component with precision and test performance in real-time.',
      imageUrl: '/img/about/dashboard.png',
      component: WorkflowBuilderDemoComponent,
      feats: [
        {
          title: 'Modular Components',
          description: 'Swap LLMs, embeddings, or filters effortlessly',
          icon: 'lucideComponent',
        },
        {
          title: 'Instant Deployments',
          description: 'One-click to production-ready pipelines',
          icon: 'lucideFastForward',
        },
        {
          title: 'Real-Time Insights',
          description: 'Monitor latency, token usage, and model output quality',
          icon: 'lucideSearch',
        },
      ],
    },
    {
      title: 'Seamless Integration with Your Favorite Providers',
      subtitle:
        'Our platform supports **native integration** with leading LLM providers like **OpenAI**, **Gemini**, **Anthropic**, and more. You can switch between models, compare results, or combine providers to suit your specific needs — all through a **single, unified interface** designed for flexibility and scalability.',
      imageUrl: '/img/about/dashboard.png',
      component: ProviderIntegrationDemoComponent,
      feats: [
        {
          title: 'One-Click Provider Switching',
          description:
            'Change from OpenAI to Claude to Gemini without touching your integration',
          icon: 'lucideRefreshCcw',
        },
        {
          title: 'Real-Time Performance Metrics',
          description:
            'See latency, cost, and quality metrics update instantly as you switch',
          icon: 'lucideChartCandlestick',
        },
        {
          title: 'Unified API Interface',
          description:
            'Same endpoints, same response format, regardless of underlying provider',
          icon: 'lucideTarget',
        },
      ],
    },
    {
      title: 'Designed for Beginners and Experts Alike',
      subtitle:
        'Whether you’re exploring retrieval-augmented generation for the first time or building advanced knowledge systems, our platform provides a **beginner-friendly environment** with the depth experts expect. Get started quickly with prebuilt templates or dive deeper with **customizable logic and advanced settings** — the choice is yours.',
      imageUrl: '/img/about/dashboard.png',
      component: BeginnerExpertDemoComponent,
      feats: [
        {
          title: 'Template Gallery',
          description:
            'Pre-built workflows for common use cases - get started in seconds',
          icon: 'lucideLayoutTemplate',
        },
        {
          title: 'Advanced Configuration',
          description:
            'Fine-tune embeddings, chunking strategies, and retrieval parameters',
          icon: 'lucideSettings',
        },
        {
          title: 'Progressive Complexity',
          description:
            "Reveal advanced features as you need them, keep it simple when you don't",
          icon: 'lucideWrench',
        },
      ],
    },
    {
      title: 'External API Integration',
      subtitle:
        'Our platform provides an **easy-to-use API** that allows smooth integration with any external software or service. You can trigger retrieval pipelines, submit documents, and receive results programmatically with minimal setup. We also support **customizable callbacks**, enabling **real-time interaction** between our system and your registered services.',
      imageUrl: '/img/about/dashboard.png',
      component: ApiIntegrationDemoComponent,
      feats: [
        {
          title: 'RESTful API',
          description:
            'Simple HTTP endpoints that integrate with any programming language',
          icon: 'lucideCable',
        },
        {
          title: 'Real-Time Callbacks',
          description:
            'Get notified instantly when processing completes via webhooks',
          icon: 'lucideRadar',
        },
        {
          title: 'Comprehensive Documentation',
          description:
            'Interactive API docs with code examples in multiple languages',
          icon: 'lucideBookOpen',
        },
      ],
    },
  ];
}
