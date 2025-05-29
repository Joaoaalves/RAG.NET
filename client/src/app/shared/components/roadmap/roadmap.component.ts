import { Component, OnInit } from '@angular/core';
import { TimelineItemComponent } from './timeline-item.component';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { FloatingParticlesComponent } from './floating-particles.component';
import { FeedbackFormComponent } from './feedback-form.component';

export interface RoadmapItem {
  id: number;
  title: string;
  description: string;
  status: 'completed' | 'in-progress' | 'upcoming';
  icon: string;
}

@Component({
  selector: 'app-roadmap',
  standalone: true,
  imports: [
    TimelineItemComponent,
    ReactiveFormsModule,
    FloatingParticlesComponent,
    FeedbackFormComponent,
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
  activeSection = 0;
  roadmapItems: RoadmapItem[] = [
    {
      id: 1,
      icon: '🔐',
      title: 'User Authentication',
      description:
        'Implemented secure JWT‑based authentication, password hashing, and session management.',
      status: 'completed',
    },
    {
      id: 2,
      icon: '🗝️',
      title: 'Per‑Workflow API Key Management',
      description:
        'Introduced scoped API keys for each workflow, enabling isolated access control and auditing.',
      status: 'completed',
    },
    {
      id: 3,
      icon: '🗄️',
      title: 'Qdrant Integration',
      description:
        'Integrated Qdrant vector database for high‑performance nearest‑neighbor searches on embeddings.',
      status: 'completed',
    },
    {
      id: 4,
      icon: '🧠',
      title: 'OpenAI Embedding & Chat',
      description:
        'Orchestrated OpenAI text‑embedding and chat‑completion APIs within the ingestion pipeline.',
      status: 'completed',
    },
    {
      id: 5,
      icon: '🤖',
      title: 'Anthropic Model Support',
      description:
        'Added Anthropic Claude for conversational AI, unifying prompt handling across providers.',
      status: 'completed',
    },
    {
      id: 6,
      icon: '🚢',
      title: 'Voyage Embedding Adapter',
      description:
        'Wrapped Voyage embedding endpoints behind a common interface in the embedding service.',
      status: 'completed',
    },
    {
      id: 7,
      icon: '📝',
      title: 'Document Chunking Strategies',
      description:
        'Implemented Proposition, Semantic and Paragraph chunkers to segment large texts for retrieval.',
      status: 'completed',
    },
    {
      id: 8,
      icon: '⚙️',
      title: 'Multithreaded Embedding',
      description:
        'Enabled multithreaded embedding jobs to parallelize requests and boost throughput.',
      status: 'completed',
    },
    {
      id: 9,
      icon: '🔍',
      title: 'Query Enhancement Techniques',
      description:
        'Added Hypothetical Document Embedding and Auto‑Query to expand and refine search queries.',
      status: 'completed',
    },
    {
      id: 10,
      icon: '🧹',
      title: 'Content Filtering Pipelines',
      description:
        'Built Relevant Segment Extraction and Multiple Score Filter to improve result precision.',
      status: 'completed',
    },
    {
      id: 11,
      icon: '🌌',
      title: 'Gemini Model Support',
      description:
        'Integrated Google Gemini for both conversation and embedding workflows.',
      status: 'completed',
    },
    {
      id: 12,
      icon: '🔔',
      title: 'Embedding Callback Mechanism',
      description:
        'Implemented callbacks to notify external URLs upon completion of embedding jobs.',
      status: 'completed',
    },
    {
      id: 13,
      icon: '🐇',
      title: 'RabbitMQ Streaming Queue',
      description:
        'Migrated to RabbitMQ for reliable job queuing and real‑time streaming of progress events.',
      status: 'completed',
    },

    // — IN‑PROGRESS —
    {
      id: 14,
      icon: '🌲',
      title: 'Pinecone Vector Store',
      description:
        'Developing a Pinecone adapter to provide scalable vector storage and similarity search.',
      status: 'in-progress',
    },
    {
      id: 15,
      icon: '🔎',
      title: 'DeepSeek Integration',
      description:
        'Adding DeepSeek as an alternative vector database for high‑volume embedding queries.',
      status: 'in-progress',
    },
    {
      id: 16,
      icon: '🎯',
      title: 'Cohere Re‑Ranking API',
      description:
        'Integrating Cohere’s re‑ranking endpoint to reorder retrieval results by relevance.',
      status: 'in-progress',
    },
    {
      id: 17,
      icon: '⏱️',
      title: 'Robust Timeout Handling',
      description:
        'Enhancing retry and timeout logic for LLM API requests to improve system resilience.',
      status: 'in-progress',
    },

    // — UPCOMING —
    {
      id: 18,
      icon: '🦙',
      title: 'Llama, Mistral & Manus',
      description:
        'Planning on‑premise support for open‑source Llama, Mistral and Manus language models.',
      status: 'upcoming',
    },
    {
      id: 19,
      icon: '💡',
      title: 'Self‑Query Retrieval',
      description:
        'Designing dynamic query generation from user inputs to drive semantic retrieval.',
      status: 'upcoming',
    },
    {
      id: 20,
      icon: '🛠️',
      title: 'One‑Click Workflow Templates',
      description:
        'Creating reusable templates for rapid RAG workflow instantiation across any provider.',
      status: 'upcoming',
    },
    {
      id: 21,
      icon: '💬',
      title: 'Embedded Conversational SDKs',
      description:
        'Building native in‑app chat UIs using each provider’s SDK for seamless developer experience.',
      status: 'upcoming',
    },
  ];

  getColor(status: RoadmapItem['status']) {
    switch (status) {
      case 'completed':
        return '#f43f5e'; // rose-500
      case 'in-progress':
        return '#d946ef'; // fuchsia-500
      case 'upcoming':
        return '#0ea5e9'; // sky-500
    }
  }
}
