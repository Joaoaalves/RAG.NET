import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';
import {
  CreateVectorStorageRequest,
  CreateVectorStorageResponse,
  GetVectorStoragesResponse,
  VectorStorage,
  VectorStoragePolicy,
  VectorStoragesResponse,
} from '../models/vector-storage';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class VectorStoragesService extends BaseApiService {
  vectorStorages: VectorStorage[] = [];

  constructor(http: HttpClient) {
    super(http);
  }

  getUserVectorStorages(): Observable<VectorStorage[]> {
    return this.http
      .get<GetVectorStoragesResponse>(this.buildUrl('/api/vector-storages'))
      .pipe(
        map((response) => {
          this.vectorStorages = response.vectorStorages;
          return this.vectorStorages;
        })
      );
  }

  getVectorStoragesPolicies(): Observable<VectorStoragePolicy[]> {
    return this.http
      .get<VectorStoragesResponse>(
        this.buildUrl('/api/vector-storages/policies')
      )
      .pipe(
        map((response) => {
          return response.vectorStorages;
        })
      );
  }

  createVectorStorage(
    data: CreateVectorStorageRequest
  ): Observable<VectorStorage[]> {
    return this.http
      .post<CreateVectorStorageResponse>(
        this.buildUrl('/api/vector-storages'),
        data
      )
      .pipe(
        map((response) => {
          this.vectorStorages.push(response.vectorStorage);
          return this.vectorStorages;
        })
      );
  }

  deleteVectorStorage(vectorStorageId: string): Observable<VectorStorage[]> {
    return this.http
      .delete(this.buildUrl(`/api/vector-storages/${vectorStorageId}`))
      .pipe(
        map((response) => {
          this.vectorStorages.filter(
            (storage) => storage.id != vectorStorageId
          );
          return this.vectorStorages;
        })
      );
  }

  updateVectorStorage(vectorStorage: VectorStorage): Observable<VectorStorage> {
    return this.http
      .put<CreateVectorStorageResponse>(
        this.buildUrl(`/api/vector-storages/${vectorStorage.id}`),
        vectorStorage
      )
      .pipe(
        map((response) => {
          return response.vectorStorage;
        })
      );
  }
}
