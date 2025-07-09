import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import {
  SupportedVectorStorage,
  VectorStorage,
  VectorStoragePolicy,
} from 'src/app/models/vector-storage';
import { VectorStoragesService } from 'src/app/services/vector-storage.service';
import { PineconeFormComponent } from 'src/app/shared/components/vector-storage/pinecone/pinecone-form.component';
import { toast } from 'ngx-sonner';

@Component({
  imports: [PineconeFormComponent, CommonModule],
  templateUrl: './vector-storages.component.html',
  standalone: true,
})
export class VectorStoragesComponent implements OnInit {
  pineconePolicy?: VectorStoragePolicy;
  userVectorStorages: VectorStorage[] = [];

  constructor(private vectorStorageService: VectorStoragesService) {}

  ngOnInit(): void {
    this.loadUserVectorStorages();
    this.loadVectorStoragePolicies();
  }

  get pinecone(): VectorStorage | undefined {
    var res = this.userVectorStorages.find(
      (v) => v.provider == SupportedVectorStorage.PINECONE
    );

    return res;
  }

  loadVectorStoragePolicies() {
    this.vectorStorageService.getVectorStoragesPolicies().subscribe({
      next: (response) => {
        this.pineconePolicy = response.find(
          (policy) => policy.providerId === SupportedVectorStorage.PINECONE
        );
      },
      error: (err) => {
        toast.error('Error while loading policies:', {
          description: err.message,
        });
      },
    });
  }

  loadUserVectorStorages() {
    this.vectorStorageService.getUserVectorStorages().subscribe({
      next: (response) => {
        this.userVectorStorages = response;
      },
      error: (err) => {
        toast.error('Error while loading vector storages.', {
          description: err.message,
        });
      },
    });
  }
}
