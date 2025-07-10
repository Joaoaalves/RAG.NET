import { CommonModule } from '@angular/common';
import {
  ChangeDetectorRef,
  Component,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { HlmSwitchComponent } from 'libs/ui/ui-switch-helm/src/lib/hlm-switch.component';
import { AlertComponent } from '../alert/alert.component';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideTrash } from '@ng-icons/lucide';
import { VectorStoragesService } from 'src/app/services/vector-storage.service';
import {
  CreateVectorStorageRequest,
  SupportedVectorStorage,
  VectorStorage,
} from 'src/app/models/vector-storage';
import { toast } from 'ngx-sonner';
import { PineconeFormComponent } from './pinecone/pinecone-form.component';
import { QdrantFormComponent } from './qdrant/qdrant-form.component';

@Component({
  selector: 'app-vector-storage-card',
  templateUrl: './vector-storage-card.component.html',
  imports: [
    CommonModule,
    HlmSwitchComponent,
    AlertComponent,
    NgIcon,
    PineconeFormComponent,
    QdrantFormComponent,
  ],
  providers: [
    provideIcons({
      lucideTrash,
    }),
  ],
  standalone: true,
})
export class VectorStorageCardComponent implements OnInit {
  @Input() src!: string;
  @Input() vectorStorage!: VectorStorage;

  isDeletable: boolean = false;

  constructor(private vsService: VectorStoragesService) {}
  ngOnInit(): void {
    this.isDeletable =
      this.vectorStorage.metas != undefined && !this.vectorStorage.isActive;
  }
  get isPinecone() {
    return this.vectorStorage.provider == SupportedVectorStorage.PINECONE;
  }

  get isQdrant() {
    return this.vectorStorage.provider == SupportedVectorStorage.QDRANT;
  }

  handleSubmit(data: CreateVectorStorageRequest) {
    this.vsService
      .createVectorStorage({
        provider: this.vectorStorage.provider,
        indexType: data.indexType,
        apiKey: data.apiKey,
        host: data.host,
        cloud: data.cloud,
        region: data.region,
        podSize: data.podSize,
        podType: data.podType,
        pods: data.pods,
        environment: data.environment,
      })
      .subscribe((response) => {
        this.vectorStorage = response;
        toast.success('Vector Storage Saved!');
      });
  }

  handleToggleIsActive() {
    var newStatus = !this.vectorStorage.isActive;
    if (this.vectorStorage.metas) {
      this.vsService
        .togleIsActive(this.vectorStorage.id, newStatus)
        .subscribe((response) => {
          this.vectorStorage = response;
          toast.success('Vector Storage Settings updated.');
        });
    }

    this.vectorStorage.isActive = newStatus;
    this.isDeletable = !newStatus;
  }

  handleDelete() {
    if (this.vectorStorage != undefined) {
      if (!this.vectorStorage.isActive) {
        this.vsService
          .deleteVectorStorage(this.vectorStorage.id)
          .subscribe(() => {
            this.vectorStorage.metas = {};
            this.vectorStorage.apiKey = '';

            toast.success('Vector Storage deleted.');
          });

        this.isDeletable = false;
      }
    }
  }
}
