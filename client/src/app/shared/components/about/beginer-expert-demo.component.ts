import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { interval, Subscription } from 'rxjs';
import { NgIcon, provideIcons } from '@ng-icons/core';
import {
  lucideMessageCircle,
  lucideMessageCircleQuestion,
  lucideSearchCheck,
  lucideText,
} from '@ng-icons/lucide';

@Component({
  selector: 'app-beginner-expert-demo',
  imports: [CommonModule, NgIcon],
  providers: [
    provideIcons({
      lucideMessageCircle,
      lucideSearchCheck,
      lucideText,
      lucideMessageCircleQuestion,
    }),
  ],
  templateUrl: './beginer-expert-demo.component.html',
  standalone: true,
})
export class BeginnerExpertDemoComponent implements OnInit, OnDestroy {
  mode: 'beginner' | 'expert' = 'beginner';
  private sub!: Subscription;

  ngOnInit() {
    this.sub = interval(5000).subscribe(() => {
      this.mode = this.mode === 'beginner' ? 'expert' : 'beginner';
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  beginnerTemplates = [
    {
      title: 'Build a Chatbot',
      icon: 'lucideMessageCircle',
      desc: 'Customer support bot',
    },
    {
      title: 'Search Knowledge Base',
      icon: 'lucideSearchCheck',
      desc: 'Document retrieval',
    },
    {
      title: 'Content Summarizer',
      icon: 'lucideText',
      desc: 'Auto-summarization',
    },
    {
      title: 'Q&A Assistant',
      icon: 'lucideMessageCircleQuestion',
      desc: 'FAQ automation',
    },
  ];
}
