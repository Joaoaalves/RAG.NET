import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';

// Icons
import { NgIcon, provideIcons } from '@ng-icons/core';
import { heroArrowUpOnSquare } from '@ng-icons/heroicons/outline';

// Models
import { EmbeddingRequest, EmbeddingResponse } from 'src/app/models/embedding';
import { Workflow } from 'src/app/models/workflow';

// Services
import { EmbeddingService } from 'src/app/services/embedding.service';
import { WorkflowService } from 'src/app/services/workflow.service';
import { Observable } from 'rxjs';
import { JobItem } from 'src/app/models/job';

@Component({
  imports: [CommonModule, NgIcon],
  providers: [provideIcons({ heroArrowUpOnSquare })],
  selector: 'app-embedding-upload',
  templateUrl: './embedding-upload.component.html',
  standalone: true,
})
export class EmbeddingUploadComponent implements OnInit {
  workflowId!: string;
  workflow!: Workflow;
  selectedFile: File | null = null;
  jobs$: Observable<JobItem[]>;

  error: string = '';
  success: string = '';
  loading: boolean = false;
  isDragging = false;

  @ViewChild('fileInput') fileInput!: ElementRef;

  constructor(
    private route: ActivatedRoute,
    private workflowService: WorkflowService,
    private embeddingService: EmbeddingService
  ) {
    this.jobs$ = this.embeddingService.jobs$;
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      this.workflowId = params.get('workflowId') || '';
      if (this.workflowId) {
        this.workflowService
          .getWorkflow(this.workflowId)
          .subscribe((workflow) => (this.workflow = workflow));
      }
    });
  }

  onDragOver(event: DragEvent) {
    event.preventDefault();
    this.isDragging = true;
  }

  onDragLeave(event: DragEvent) {
    event.preventDefault();
    this.isDragging = false;
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    this.isDragging = false;

    if (event.dataTransfer?.files.length) {
      const file = event.dataTransfer.files[0];
      if (
        file.type === 'application/pdf' ||
        file.type === 'application/epub+zip'
      ) {
        this.selectedFile = file;
      } else {
        this.error = 'We only support PDF and Epub files.';
      }
    }
  }

  triggerFileInput(): void {
    this.fileInput.nativeElement.click();
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files?.length) {
      const file = input.files[0];
      if (
        file.type === 'application/pdf' ||
        file.type === 'application/epub+zip'
      ) {
        this.selectedFile = file;
      } else {
        this.error = 'We only support PDF and Epub files.';
      }
    }
  }

  onUpload(): void {
    if (!this.selectedFile) {
      this.error = 'No file selected.';
      return;
    }

    if (!this.workflow || !this.workflow.apiKey) {
      this.error = 'This workflow does not have an API Key.';
      return;
    }

    this.embeddingService.sendFile(this.selectedFile, this.workflow.apiKey);
    this.fileInput.nativeElement.value = '';
    this.selectedFile = null;
  }
}
