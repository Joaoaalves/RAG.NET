import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EmbeddingService } from 'src/app/services/embedding.service';
import { WorkflowService } from 'src/app/services/workflow.service';
import { Workflow } from 'src/app/models/workflow';
import { EmbeddingRequest, EmbeddingResponse } from 'src/app/models/embedding';
import { SidebarComponent } from 'src/app/shared/components/sidebar/sidebar.component';
import { CommonModule } from '@angular/common';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { heroArrowUpOnSquare } from '@ng-icons/heroicons/outline';

@Component({
  imports: [SidebarComponent, CommonModule, NgIcon],
  providers: [provideIcons({ heroArrowUpOnSquare })],
  selector: 'app-embedding-upload',
  templateUrl: './embedding-upload.component.html',
  standalone: true,
})
export class EmbeddingUploadComponent implements OnInit {
  workflowId!: string;
  workflow!: Workflow;
  selectedFile: File | null = null;
  uploadProgress = 0;
  error: string = '';
  success: string = '';
  loading: boolean = false;
  isDragging = false;
  @ViewChild('fileInput') fileInput!: ElementRef;

  constructor(
    private route: ActivatedRoute,
    private workflowService: WorkflowService,
    private embeddingService: EmbeddingService
  ) {}

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
      if (file.type === 'application/pdf') {
        this.selectedFile = file;
      } else {
        this.error = 'We only support PDF files.';
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
      if (file.type === 'application/pdf') {
        this.selectedFile = file;
      } else {
        this.error = 'We only support PDF files.';
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

    const requestData: EmbeddingRequest = { file: this.selectedFile };
    this.loading = true;
    this.embeddingService.embedd(requestData, this.workflow.apiKey).subscribe({
      next: (progress: EmbeddingResponse) => {
        if (progress.totalChunks > 0) {
          this.uploadProgress = Math.max(
            Math.round((progress.processedChunks / progress.totalChunks) * 100),
            this.uploadProgress
          );
        }
      },
      error: (error) => {
        this.loading = false;
        this.error = error;
      },
      complete: () => {
        this.error = '';
        this.loading = false;
        this.selectedFile = null;
        this.uploadProgress = 0;
        this.success = 'Document embedded!';
      },
    });
  }
}
