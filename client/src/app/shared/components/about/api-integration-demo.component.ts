import { CommonModule } from '@angular/common';
import { Component, OnInit, OnDestroy } from '@angular/core';

interface FloatingParticle {
  left: string;
  top: string;
  animationDelay: string;
  animationDuration: string;
}

@Component({
  selector: 'app-api-integration-demo',
  templateUrl: './api-integration-demo.component.html',
  imports: [CommonModule],
})
export class ApiIntegrationDemoComponent implements OnInit, OnDestroy {
  step = 0;
  showResponse = false;
  steps = [
    'Writing API request...',
    'Sending to RAG.NET...',
    'Processing document...',
    'Generating response...',
    'Sending callback...',
  ];
  intervalId: any;
  particles: FloatingParticle[] = [];

  ngOnInit(): void {
    this.particles = Array.from({ length: 10 }).map(() => ({
      left: `${20 + Math.random() * 60}%`,
      top: `${20 + Math.random() * 60}%`,
      animationDelay: `${Math.random() * 5}s`,
      animationDuration: `${2 + Math.random()}s`,
    }));

    this.intervalId = setInterval(() => {
      if (this.step < this.steps.length - 1) {
        this.step++;
      } else {
        this.step = 0;
        this.showResponse = false;
        setTimeout(() => (this.showResponse = true), 500);
      }
    }, 2000);
  }

  ngOnDestroy(): void {
    clearInterval(this.intervalId);
  }

  getProgressWidth(): string {
    return `${((this.step + 1) / this.steps.length) * 100}%`;
  }
}
