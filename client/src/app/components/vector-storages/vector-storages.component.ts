import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import {
  SupportedVectorStorage,
  VectorStorage,
} from 'src/app/models/vector-storage';
import { VectorStoragesService } from 'src/app/services/vector-storage.service';
import { toast } from 'ngx-sonner';
import { VectorStorageCardComponent } from 'src/app/shared/components/vector-storage/vector-storage-card.component';

@Component({
  imports: [VectorStorageCardComponent, CommonModule],
  templateUrl: './vector-storages.component.html',
  standalone: true,
})
export class VectorStoragesComponent implements OnInit {
  userVectorStorages: VectorStorage[] = [];
  pinecone!: VectorStorage;
  qdrant!: VectorStorage;

  constructor(private vectorStorageService: VectorStoragesService) {}

  ngOnInit(): void {
    this.loadUserVectorStorages();
  }

  loadUserVectorStorages() {
    this.vectorStorageService.getUserVectorStorages().subscribe({
      next: (response) => {
        this.userVectorStorages = response;

        this.qdrant = this.getProvider(SupportedVectorStorage.QDRANT);
        this.pinecone = this.getProvider(SupportedVectorStorage.PINECONE);
      },
      error: (err) => {
        toast.error('Error while loading vector storages.', {
          description: err.message,
        });
      },
    });
  }

  private getProvider(provider: SupportedVectorStorage): VectorStorage {
    var res = this.userVectorStorages.find((vs) => vs.provider === provider);

    if (!res) {
      return {
        id: '',
        isActive: false,
        provider,
        name: SupportedVectorStorage[provider],
      };
    }

    return res;
  }
}
