import {
  ComponentFixture,
  TestBed,
  fakeAsync,
  tick,
} from '@angular/core/testing';
import { EmbeddingUploadComponent } from './embedding-upload.component';
import { ActivatedRoute, convertToParamMap } from '@angular/router';
import { of, Subject, throwError } from 'rxjs';
import { WorkflowService } from 'src/app/services/workflow.service';
import { EmbeddingService } from 'src/app/services/embedding.service';
import { Workflow } from 'src/app/models/workflow';
import { ElementRef } from '@angular/core';
import { EmbeddingResponse } from 'src/app/models/embedding';
import { ChunkerStrategy } from 'src/app/models/chunker';
import { EmbeddingProviderEnum } from 'src/app/models/embedding';
import { HttpClientModule } from '@angular/common/http';

const mockWorkflow: Workflow = {
  id: '123',
  apiKey: 'FAKE_API_KEY',
  name: '',
  description: '',
  documentsCount: 0,
  strategy: ChunkerStrategy.PROPOSITION,
  settings: {
    threshold: 0,
    evaluationPrompt: '',
    maxChunkSize: 0,
  },
  embeddingProvider: {
    provider: EmbeddingProviderEnum.OPENAI,
    apiKey: '',
    vectorSize: 0,
    model: '',
  },
  queryEnhancers: [],
  filter: undefined,
};

describe('EmbeddingUploadComponent', () => {
  let component: EmbeddingUploadComponent;
  let fixture: ComponentFixture<EmbeddingUploadComponent>;
  let mockActivatedRoute: any;
  let workflowServiceSpy: jasmine.SpyObj<WorkflowService>;
  let embeddingServiceSpy: jasmine.SpyObj<EmbeddingService>;

  beforeEach(async () => {
    // Mock andn Spies
    mockActivatedRoute = {
      paramMap: of(convertToParamMap({ workflowId: '123' })),
    };

    workflowServiceSpy = jasmine.createSpyObj('WorkflowService', [
      'getWorkflow',
    ]);
    workflowServiceSpy.getWorkflow.and.returnValue(of(mockWorkflow));

    embeddingServiceSpy = jasmine.createSpyObj('EmbeddingService', ['embedd']);

    await TestBed.configureTestingModule({
      imports: [EmbeddingUploadComponent, HttpClientModule],
      providers: [
        { provide: ActivatedRoute, useValue: mockActivatedRoute },
        { provide: WorkflowService, useValue: workflowServiceSpy },
        { provide: EmbeddingService, useValue: embeddingServiceSpy },
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EmbeddingUploadComponent);
    component = fixture.componentInstance;

    component.fileInput = new ElementRef({
      click: jasmine.createSpy('click'),
    });

    fixture.detectChanges();
  });

  it('should load workflow from route', () => {
    expect(component.workflowId).toBe('123');
    expect(workflowServiceSpy.getWorkflow).toHaveBeenCalledWith('123');
    expect(component.workflow).toEqual(mockWorkflow);
  });

  it('should set isDragging true on dragover and false on dragleave', () => {
    const dragOverEvent = new DragEvent('dragover');
    component.onDragOver(dragOverEvent);
    expect(component.isDragging).toBeTrue();

    const dragLeaveEvent = new DragEvent('dragleave');
    component.onDragLeave(dragLeaveEvent);
    expect(component.isDragging).toBeFalse();
  });

  it('should set selectedFile on drop if PDF', () => {
    const file = new File(['dummy content'], 'test.pdf', {
      type: 'application/pdf',
    });

    const dataTransfer = new DataTransfer();
    dataTransfer.items.add(file);

    const dropEvent = new DragEvent('drop', { dataTransfer });

    component.onDrop(dropEvent);

    expect(component.isDragging).toBeFalse();
    expect(component.selectedFile).toEqual(file);
  });

  it('should set error on drop if file is not PDF', () => {
    const component = fixture.componentInstance;

    const file = new File(['dummy content'], 'test.txt', {
      type: 'text/plain',
    });

    const dataTransfer = new DataTransfer();
    dataTransfer.items.add(file);

    const dropEvent = new DragEvent('drop', {
      dataTransfer: dataTransfer,
    });

    component.onDrop(dropEvent);

    expect(component.error).toBe('We only support PDF files.');
  });
  it('should set error on drop if file is not PDF', () => {
    const file = new File(['dummy content'], 'test.txt', {
      type: 'text/plain',
    });

    const dataTransfer = new DataTransfer();
    dataTransfer.items.add(file);

    const dropEvent = new DragEvent('drop', {
      dataTransfer: dataTransfer,
    });

    component.onDrop(dropEvent);

    expect(component.error).toBe('We only support PDF files.');
  });

  it('should set error on invalid file selected', () => {
    const file = new File(['dummy content'], 'test.txt', {
      type: 'text/plain',
    });
    const event = { target: { files: [file] } } as unknown as Event;

    component.onFileSelected(event);
    expect(component.error).toBe('We only support PDF files.');
  });

  it('should trigger file input click when triggerFileInput is called', () => {
    const component = fixture.componentInstance;

    const fileInputClickSpy = spyOn(
      component.fileInput.nativeElement,
      'click'
    ).and.callThrough();

    component.triggerFileInput();

    expect(fileInputClickSpy).toHaveBeenCalled();
  });

  it('should set error when onUpload is called with no file selected', () => {
    component.selectedFile = null;
    component.onUpload();
    expect(component.error).toBe('No file selected.');
  });
  it('should set error when onUpload is called with workflow missing API Key', () => {
    component.selectedFile = new File(['dummy content'], 'test.pdf', {
      type: 'application/pdf',
    });
    component.workflow = { ...mockWorkflow, apiKey: '' };
    component.onUpload();
    expect(component.error).toBe('This workflow does not have an API Key.');
  });

  it('should update uploadProgress and set success on complete', fakeAsync(() => {
    const progressSubject = new Subject<EmbeddingResponse>();
    embeddingServiceSpy.embedd.and.returnValue(progressSubject.asObservable());
    component.selectedFile = new File(['dummy content'], 'test.pdf', {
      type: 'application/pdf',
    });
    component.workflow = mockWorkflow;

    component.onUpload();
    expect(component.loading).toBeTrue();

    progressSubject.next({ processedChunks: 50, totalChunks: 100 });
    tick();
    expect(component.uploadProgress).toBe(50);

    progressSubject.next({ processedChunks: 40, totalChunks: 100 });
    tick();
    expect(component.uploadProgress).toBe(50);

    progressSubject.complete();
    tick();
    expect(component.loading).toBeFalse();
    expect(component.selectedFile).toBeNull();
    expect(component.uploadProgress).toBe(0);
    expect(component.success).toBe('Document embedded!');
  }));

  it('should set error and stop loading on upload error', fakeAsync(() => {
    const errorMessage = 'Upload failed';
    embeddingServiceSpy.embedd.and.returnValue(throwError(errorMessage));
    component.selectedFile = new File(['dummy content'], 'test.pdf', {
      type: 'application/pdf',
    });
    component.workflow = mockWorkflow;

    component.onUpload();
    tick();
    expect(component.loading).toBeFalse();
    expect(component.error).toBe(errorMessage);
  }));
});
