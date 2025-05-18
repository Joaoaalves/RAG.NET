import { Component } from '@angular/core';
import { StatsCardComponent } from './stats-card.component';
import { CommonModule } from '@angular/common';
import { GlowBackgroundComponent } from './glow-background.component';

@Component({
  selector: 'app-integrations',
  templateUrl: './integrations.component.html',
  standalone: true,
  imports: [StatsCardComponent, CommonModule, GlowBackgroundComponent],
})
export class IntegrationsComponent {
  stats = [
    {
      label: 'Conversation Models',
      text: 'We’ve integrated a wide range of state-of-the-art conversation models from the most trusted LLM providers in the industry. This allows you to experiment and deploy powerful conversational agents tailored to your specific use cases, without needing to worry about complex configurations.',
      value: 20,
      icon: 'heroChatBubbleOvalLeft',
    },
    {
      label: 'Embedding Models',
      text: 'Our platform supports multiple high-quality embedding models from top LLM providers. These models are essential for turning your data into meaningful vector representations, powering efficient and accurate information retrieval across your documents.',
      value: 8,
      icon: 'heroArrowUpCircle',
    },
    {
      label: 'Providers',
      text: 'We currently support five integrations, including OpenAI, Gemini, Anthropic, Voyage, and QDrant. This gives you the freedom to choose the best-performing models for your needs, with full flexibility and no vendor lock-in.',
      value: 5,
      icon: 'heroCodeBracket',
    },
    {
      label: 'Supported Document Extensions',
      text: 'You can upload and process documents in .pdf, .epub, and .txt formats out of the box. Our goal is to support nearly every widely used text-based file format, making it easy for you to work with your existing content without extra conversions.',
      value: 3,
      icon: 'heroCodeBracket',
    },
    {
      label: 'Chunking Strategies',
      text: 'We provide three modern chunking strategies to intelligently split your documents into semantically meaningful segments. These strategies are key to generating better embeddings and ensuring high-quality retrieval results when working with large volumes of content.',
      value: 3,
      icon: 'heroCodeBracket',
    },
    {
      label: 'Query Enhancers',
      text: 'Our platform includes advanced query enhancement techniques that help improve the quality and accuracy of search queries. These enhancements ensure that users retrieve the most relevant content, even when queries are short, vague, or ambiguous.',
      value: 3,
      icon: 'heroCodeBracket',
    },
    {
      label: 'Re-ranking',
      text: 'We use intelligent re-ranking techniques to reorder retrieved results based on relevance and context. This step adds an extra layer of precision, helping to surface the most meaningful content at the top of the results list.',
      value: 1,
      icon: 'heroCodeBracket',
    },
    {
      label: 'Result Filters',
      text: 'Our result filtering capabilities allow users to refine and customize the results they receive. Whether filtering by file type, metadata, or other custom attributes, it’s easy to drill down and find exactly what you need.',
      value: 3,
      icon: 'heroCodeBracket',
    },
  ];
}
