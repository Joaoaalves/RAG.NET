import { CommonModule } from '@angular/common';
import { VectorStorageProvider } from './../../models/provider';
import { Component, OnInit } from '@angular/core';
import {
  SupportedVectorStorage,
  VectorStoragePolicy,
} from 'src/app/models/vector-storage';
import { VectorStoragesService } from 'src/app/services/vector-storage.service';
import { PineconeFormComponent } from 'src/app/shared/components/vector-storage/pinecone/pinecone-form.component';

@Component({
  imports: [PineconeFormComponent, CommonModule],
  templateUrl: './vector-storages.component.html',
  standalone: true,
})
export class VectorStoragesComponent implements OnInit {
  pineconePolicy?: VectorStoragePolicy;

  constructor(private vectorStorageService: VectorStoragesService) {}

  ngOnInit(): void {
    this.loadVectorStorageServices();
  }

  loadVectorStorageServices() {
    this.vectorStorageService.getVectorStoragesPolicies().subscribe({
      next: (response) => {
        this.pineconePolicy = response.find(
          (policy) => policy.providerId === SupportedVectorStorage.PINECONE
        );
      },
      error: (err) => {
        console.error('Erro ao carregar policies:', err);
      },
    });
  }
}
