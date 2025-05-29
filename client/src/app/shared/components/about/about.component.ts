import { Component } from '@angular/core';
import { SupportedProvidersComponent } from '../supported-providers/supported-providers.component';
import { AboutItemComponent } from './about-item.component';
import { CommonModule } from '@angular/common';
import { WorkflowBuilderDemoComponent } from './workflow-builder-demo.component';
import { ApiIntegrationDemoComponent } from './api-integration-demo.component';
import { BeginnerExpertDemoComponent } from './beginer-expert-demo.component';
import { ProviderIntegrationDemoComponent } from './provider-integration-demo.component';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  imports: [
    SupportedProvidersComponent,
    AboutItemComponent,
    CommonModule,
  ],
  standalone: true,
})
export class AboutComponent {
  aboutItems = [
    {
      title: 'Create Retrieval Pipelines with a Few Clicks',
      subtitle:
        'Design sophisticated **Retrieval-Augmented Generation pipelines** using a visual, **no-code builder**. Mix and match embedding models, chunking strategies, filters, and LLMs — then deploy your workflow instantly through a single API endpoint. Fine-tune every component with precision and test performance in real-time.',
      imageUrl: '/img/about/dashboard.png',
      component: WorkflowBuilderDemoComponent,
    },
    {
      title: 'Seamless Integration with Your Favorite Providers',
      subtitle:
        'Our platform supports **native integration** with leading LLM providers like **OpenAI**, **Gemini**, **Anthropic**, and more. You can switch between models, compare results, or combine providers to suit your specific needs — all through a **single, unified interface** designed for flexibility and scalability.',
      imageUrl: '/img/about/dashboard.png',
      component: ProviderIntegrationDemoComponent,
    },
    {
      title: 'Designed for Beginners and Experts Alike',
      subtitle:
        'Whether you’re exploring retrieval-augmented generation for the first time or building advanced knowledge systems, our platform provides a **beginner-friendly environment** with the depth experts expect. Get started quickly with prebuilt templates or dive deeper with **customizable logic and advanced settings** — the choice is yours.',
      imageUrl: '/img/about/dashboard.png',
      component: BeginnerExpertDemoComponent,
    },
    {
      title: 'External API Integration',
      subtitle:
        'Our platform provides an **easy-to-use API** that allows smooth integration with any external software or service. You can trigger retrieval pipelines, submit documents, and receive results programmatically with minimal setup. We also support **customizable callbacks**, enabling **real-time interaction** between our system and your registered services.',
      imageUrl: '/img/about/dashboard.png',
      component: ApiIntegrationDemoComponent,
    },
  ];
}
