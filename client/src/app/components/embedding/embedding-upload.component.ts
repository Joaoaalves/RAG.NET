import { WaveProgressComponent } from './../../shared/components/wave-progress/wave-progress.component';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';

// Icons
import { NgIcon, provideIcons } from '@ng-icons/core';
import { heroArrowUpOnSquare } from '@ng-icons/heroicons/outline';
import { lucideFileText, lucideX } from '@ng-icons/lucide';

// Models
import { Workflow } from 'src/app/models/workflow';

// Services
import { EmbeddingService } from 'src/app/services/embedding.service';
import { WorkflowService } from 'src/app/services/workflow.service';
import { Observable } from 'rxjs';
import { JobItem } from 'src/app/models/job';

// Pipes
import { FileSizePipe } from './file-size.pipe';

@Component({
  imports: [CommonModule, WaveProgressComponent, NgIcon, FileSizePipe],
  providers: [provideIcons({ heroArrowUpOnSquare, lucideFileText })],
  selector: 'app-embedding-upload',
  templateUrl: './embedding-upload.component.html',
  styleUrls: ['./embedding-upload.component.css'],
  standalone: true,
})
export class EmbeddingUploadComponent implements OnInit {
  workflowId!: string;
  workflow!: Workflow;
  selectedFiles: File[] = [];
  jobs$: Observable<JobItem[]>;
  error = '';
  success = '';
  loading = false;
  isDragging = false;

  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;

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
          .subscribe((wf) => (this.workflow = wf));
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
    this.addFiles(event.dataTransfer?.files);
  }

  triggerFileInput(): void {
    this.fileInput.nativeElement.click();
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files) this.addFiles(input.files);
  }

  clearFiles(): void {
    this.selectedFiles = [];
    this.fileInput.nativeElement.value = '';
  }

  private addFiles(fileList?: FileList) {
    if (!fileList) return;
    this.error = '';
    for (let i = 0; i < fileList.length; i++) {
      const file = fileList.item(i)!;
      if (
        file.type === 'application/pdf' ||
        file.type === 'application/epub+zip'
      ) {
        this.selectedFiles.push(file);
      } else {
        this.error = 'We only support PDF and Epub files.';
      }
    }
  }

  onUpload(): void {
    this.error = '';
    if (this.selectedFiles.length === 0) {
      this.error = 'No files selected.';
      return;
    }
    if (!this.workflow?.apiKey) {
      this.error = 'This workflow does not have an API Key.';
      return;
    }

    this.loading = true;
    const apiKey = this.workflow.apiKey;
    for (const file of this.selectedFiles) {
      this.embeddingService.sendFile(file, apiKey);
    }
    // clear selection
    this.selectedFiles = [];
    this.fileInput.nativeElement.value = '';
    this.loading = false;
    this.success = 'Files uploaded successfully.';
  }
}
