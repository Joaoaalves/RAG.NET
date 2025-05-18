import { Component } from '@angular/core';
import { LogoComponent } from './logo.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-supported-providers',
  templateUrl: './supported-providers.component.html',
  imports: [LogoComponent, CommonModule],
  standalone: true,
})
export class SupportedProvidersComponent {
  readonly providers = [
    { title: 'OpenAI', src: '/img/providers/openai.svg', implemented: true },
    { title: 'Qdrant', src: '/img/providers/qdrant.svg', implemented: true },
    { title: 'Gemini', src: '/img/providers/gemini.svg', implemented: true },
    {
      title: 'Anthropic',
      src: '/img/providers/anthropic.svg',
      implemented: true,
    },
    { title: 'Voyage', src: '/img/providers/voyage.svg', implemented: true },
    {
      title: 'Pinecone',
      src: '/img/providers/pinecone.svg',
      implemented: false,
    },
    {
      title: 'Deepseek',
      src: '/img/providers/deepseek.svg',
      implemented: false,
    },
    { title: 'Llama', src: '/img/providers/meta.svg', implemented: false },
    { title: 'CoHere', src: '/img/providers/cohere.svg', implemented: false },
    { title: 'Mistral', src: '/img/providers/mistral.svg', implemented: false },
    { title: 'Manus', src: '/img/providers/manus.svg', implemented: false },
  ];
}
