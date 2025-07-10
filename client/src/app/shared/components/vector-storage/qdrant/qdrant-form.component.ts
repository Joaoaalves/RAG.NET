import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import {
  CreateVectorStorageRequest,
  VectorStorage,
} from 'src/app/models/vector-storage';
import { QdrantCloudFormComponent } from './forms/cloud/qdrant-cloud-form.component';

@Component({
  selector: 'app-qdrant-form',
  imports: [CommonModule, QdrantCloudFormComponent],
  templateUrl: './qdrant-form.component.html',
  standalone: true,
})
export class QdrantFormComponent {
  @Input() data!: VectorStorage;
  @Output() formSubmit = new EventEmitter<CreateVectorStorageRequest>();

  types: string[] = ['Cloud'];
  activeType: string = this.types[0];

  setActiveType(type: string) {
    this.activeType = type;
  }

  onSubmit($event: CreateVectorStorageRequest) {
    var data = $event;
    data.indexType = this.types.indexOf(this.activeType);

    this.formSubmit.emit(data);
  }

  getMeta(meta: string) {
    if (this.data?.metas != undefined) {
      return this.data.metas[meta];
    }
    return;
  }
}
