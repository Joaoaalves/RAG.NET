import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { interval, Subscription } from 'rxjs';

interface Step {
  title: string;
  description: string;
}

@Component({
  selector: 'app-workflow-builder-demo',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './workflow-builder-demo.component.html',
})
export class WorkflowBuilderDemoComponent implements OnInit, OnDestroy {
  steps: Step[] = [
    {
      title: 'Select Chunking Strategy',
      description: 'Choose Semantic Chunking',
    },
    {
      title: 'Configure your Providers',
      description: 'Choose OpenAI Text Embedding 3',
    },
    {
      title: 'Configure Query Enhancers',
      description: 'Enable Auto Query with custom prompt',
    },
    { title: 'Activate Filter', description: 'Set max segments to 5' },
  ];

  currentStep = 0;
  private sub!: Subscription;

  ngOnInit() {
    this.sub = interval(2000).subscribe(() => {
      if (this.currentStep < this.steps.length) {
        this.currentStep++;
      }
      if (this.currentStep >= this.steps.length) {
        this.sub.unsubscribe();
      }
    });
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }
}
