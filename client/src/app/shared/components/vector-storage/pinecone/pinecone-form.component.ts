import { VectorStoragesService } from 'src/app/services/vector-storage.service';
import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import {
  SupportedVectorStorage,
  VectorStoragePolicy,
} from 'src/app/models/vector-storage';
import { ServerlessFormComponent } from './forms/serverless/serverless-form.component';
import { PodsFormComponent } from './forms/pods/pods-form.component';
import { BYOCFormComponent } from './forms/byoc/byoc-form.component';
import { toast } from 'ngx-sonner';

@Component({
  selector: 'app-pinecone-form',
  imports: [
    CommonModule,
    ServerlessFormComponent,
    PodsFormComponent,
    BYOCFormComponent,
  ],
  templateUrl: './pinecone-form.component.html',
  standalone: true,
})
export class PineconeFormComponent {
  @Input() policy!: VectorStoragePolicy;
  types: string[] = ['Serverless', 'Pods', 'BYOC'];
  activeType: string = this.types[0];

  constructor(private vsService: VectorStoragesService) {}

  setActiveType(type: string) {
    this.activeType = type;
  }

  handleSubmit(data: any) {
    this.vsService
      .createVectorStorage({
        provider: SupportedVectorStorage.PINECONE,
        indexType: this.types.indexOf(this.activeType),
        apiKey: data.apiKey,
        cloud: data.cloud,
        region: data.region,
        podSize: data.podSize,
        podType: data.podType,
        pods: data.pods,
        environment: data.environment,
      })
      .subscribe((response) => {
        toast.success('Vector Storage Saved!');
      });
  }
}
