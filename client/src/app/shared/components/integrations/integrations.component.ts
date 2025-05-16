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
  ];
}
