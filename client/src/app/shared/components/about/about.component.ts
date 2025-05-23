import { Component } from '@angular/core';
import { SupportedProvidersComponent } from '../supported-providers/supported-providers.component';
import { AboutItemComponent } from './about-item.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  imports: [SupportedProvidersComponent, AboutItemComponent, CommonModule],
  standalone: true,
})
export class AboutComponent {
  aboutItems = [
    {
      title: 'Create Retrieval Pipelines with a Few Clicks',
      subtitle:
        'With our **intuitive no-code interface**, you can create, customize, and deploy retrieval pipelines in just a few steps. Whether you’re connecting to vector databases, embedding new documents, or testing your pipeline, everything is designed to be **streamlined and user-friendly**, removing the complexity often associated with LLM-powered systems.',
      imageUrl: 'assets/images/about.jpg',
    },
    {
      title: 'Seamless Integration with Your Favorite Providers',
      subtitle:
        'Our platform supports **native integration** with leading LLM providers like **OpenAI**, **Gemini**, **Anthropic**, and more. You can switch between models, compare results, or combine providers to suit your specific needs — all through a **single, unified interface** designed for flexibility and scalability.',
      imageUrl: 'assets/images/mission.jpg',
    },
    {
      title: 'Designed for Beginners and Experts Alike',
      subtitle:
        'Whether you’re exploring retrieval-augmented generation for the first time or building advanced knowledge systems, our platform provides a **beginner-friendly environment** with the depth experts expect. Get started quickly with prebuilt templates or dive deeper with **customizable logic and advanced settings** — the choice is yours.',
      imageUrl: 'assets/images/vision.jpg',
    },
    {
      title: 'External API Integration',
      subtitle:
        'Our platform provides an **easy-to-use API** that allows smooth integration with any external software or service. You can trigger retrieval pipelines, submit documents, and receive results programmatically with minimal setup. We also support **customizable callbacks**, enabling **real-time interaction** between our system and your registered services.',
      imageUrl: 'assets/images/vision.jpg',
    },
  ];
}
