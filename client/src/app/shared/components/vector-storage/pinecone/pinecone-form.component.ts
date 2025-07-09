import { VectorStoragesService } from 'src/app/services/vector-storage.service';
import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import {
  SupportedVectorStorage,
  VectorStorage,
  VectorStoragePolicy,
} from 'src/app/models/vector-storage';
import { ServerlessFormComponent } from './forms/serverless/serverless-form.component';
import { PodsFormComponent } from './forms/pods/pods-form.component';
import { BYOCFormComponent } from './forms/byoc/byoc-form.component';
import { toast } from 'ngx-sonner';
import { VectorStorageCard } from '../vector-storage-card.component';

@Component({
  selector: 'app-pinecone-form',
  imports: [
    CommonModule,
    VectorStorageCard,
    ServerlessFormComponent,
    PodsFormComponent,
    BYOCFormComponent,
  ],
  templateUrl: './pinecone-form.component.html',
  standalone: true,
})
export class PineconeFormComponent {
  @Input() policy!: VectorStoragePolicy;
  @Input() providerData: VectorStorage | undefined;

  types: string[] = ['Serverless', 'Pods', 'BYOC'];
  activeType: string = this.types[0];

  constructor(private vsService: VectorStoragesService) {}

  setActiveType(type: string) {
    this.activeType = type;
  }

  get apiKey() {
    return this.providerData?.apiKey;
  }

  get cloud() {
    if (this.providerData) return parseInt(this.providerData.metas['cloud']);
    return;
  }

  get region() {
    return this.providerData?.metas['region'];
  }

  get pods() {
    if (this.providerData) return parseInt(this.providerData.metas['pods']);
    return;
  }

  get podSize() {
    return this.providerData?.metas['podSize'];
  }

  get podType() {
    return this.providerData?.metas['podType'];
  }

  get environment() {
    return this.providerData?.metas['environment'];
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
        this.providerData = response;
        toast.success('Vector Storage Saved!');
      });
  }

  handleToggleIsActive() {
    if (this.providerData != undefined) {
      this.vsService
        .togleIsActive(this.providerData.id, !this.providerData.isActive)
        .subscribe((response) => {
          this.providerData = response;
          toast.success('Pinecone Settings was updated.');
        });
    }
  }

  handleDelete() {
    if (this.providerData != undefined) {
      if (!this.providerData.isActive) {
        this.vsService
          .deleteVectorStorage(this.providerData.id)
          .subscribe((response) => {
            this.providerData = undefined;
            toast.success('Pinecone provider was deleted.');
          });
      }
    }
  }
}
