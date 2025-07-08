import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';
import {
  CreateVectorStorageRequest,
  GetVectorStoragesResponse,
  SupportedVectorStorage,
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
      .get<GetVectorStoragesResponse>(
        this.buildUrl('/api/vector-storages/user')
      )
      .pipe(
        map((response) => {
          this.vectorStorages = response.vectorStorages;
          return this.vectorStorages;
        })
      );
  }

  getVectorStoragesPolicies(): Observable<VectorStoragePolicy[]> {
    return this.http
      .get<VectorStoragesResponse>(this.buildUrl('/api/vector-storages'))
      .pipe(
        map((response) => {
          return response.vectorStorages;
        })
      );
  }

  createVectorStorage(data: CreateVectorStorageRequest): Observable<any> {
    return this.http.post(this.buildUrl('/api/vector-storages'), data);
  }
}
