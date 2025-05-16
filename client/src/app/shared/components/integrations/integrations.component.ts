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
    { label: 'Conversation Models', value: 20, icon: 'heroChatBubbleOvalLeft' },
    { label: 'Embedding Models', value: 8, icon: 'heroArrowUpCircle' },
    { label: 'Providers', value: 5, icon: 'heroCodeBracket' },
    {
      label: 'Supported Document Extensions',
      value: 3,
      icon: 'heroCodeBracket',
    },
    { label: 'Chunking Strategies', value: 3, icon: 'heroCodeBracket' },
    { label: 'Query Enhancers', value: 3, icon: 'heroCodeBracket' },
    { label: 'Re-ranking', value: 1, icon: 'heroCodeBracket' },
    { label: 'Result Filters', value: 3, icon: 'heroCodeBracket' },
  ];
}
