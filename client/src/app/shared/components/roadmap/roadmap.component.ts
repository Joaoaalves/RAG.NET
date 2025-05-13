import { TextAreaComponent } from 'src/app/shared/components/text-area/text-area.component';
import { InputComponent } from 'src/app/shared/components/input/input.component';
// roadmap.component.ts
import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import gsap from 'gsap';
import ScrollTrigger from 'gsap/ScrollTrigger';
import { TimelineItemComponent } from './timeline-item.component';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';

gsap.registerPlugin(ScrollTrigger);

interface RoadmapItem {
  title: string;
  description: string;
}

interface RoadmapCategory {
  label: string;
  color: 'sky' | 'indigo' | 'rose';
  items: RoadmapItem[];
}

@Component({
  selector: 'app-roadmap',
  standalone: true,
  imports: [
    TimelineItemComponent,
    InputComponent,
    TextAreaComponent,
    ReactiveFormsModule,
    CommonModule,
  ],
  templateUrl: './roadmap.component.html',
})
export class RoadmapComponent implements OnInit {
  form!: FormGroup;
  error?: string;

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      name: [''],
      email: [''],
      feedback: [''],
    });
  }

  colorMap = {
    sky: '#0ea5e9', // sky-500
    indigo: '#6366f1', // indigo-500
    rose: '#f43f5e', // rose-500
  };

  categories: RoadmapCategory[] = [
    {
      label: 'Released',
      color: 'sky',
      items: [
        {
          title: 'User Authentication',
          description: 'JWT-based secure login system implemented.',
        },
        {
          title: 'User Authentication',
          description: 'JWT-based secure login system implemented.',
        },
        {
          title: 'User Authentication',
          description: 'JWT-based secure login system implemented.',
        },
        {
          title: 'User Authentication',
          description: 'JWT-based secure login system implemented.',
        },
        {
          title: 'User Authentication',
          description: 'JWT-based secure login system implemented.',
        },
      ],
    },
    {
      label: 'In Development',
      color: 'indigo',
      items: [
        {
          title: 'AI-Generated Video',
          description:
            'Integrating OpenAI & QDrant to create automatic videos.',
        },
        {
          title: 'AI-Generated Video',
          description:
            'Integrating OpenAI & QDrant to create automatic videos.',
        },
        {
          title: 'AI-Generated Video',
          description:
            'Integrating OpenAI & QDrant to create automatic videos.',
        },
        {
          title: 'AI-Generated Video',
          description:
            'Integrating OpenAI & QDrant to create automatic videos.',
        },
      ],
    },
    {
      label: 'Planned',
      color: 'rose',
      items: [
        {
          title: 'Mobile App',
          description: 'Cross-platform app for managing workflow pipelines.',
        },
      ],
    },
  ];
}
