import { environment } from './../../environments/environment.example';
export interface VectorStoragePolicy {
  id: string;
  providerId: SupportedVectorStorage;
  apiKey: string;
  name: string;
  pattern: string;
  prefix: string;
  url: string;
}

export interface VectorStoragesResponse {
  vectorStorages: VectorStoragePolicy[];
}

export interface VectorStorage {
  id: string;
  providerId: number;
  name: string;
}

export interface GetVectorStoragesResponse {
  vectorStorages: VectorStorage[];
}

export interface CreateVectorStorageRequest {
  apiKey: string;
  provider: SupportedVectorStorage;
  indexType?: number;
  host?: string;
  region?: string;
  cloud?: number;
  environment?: string;
  pods: number;
  podSize: string;
  podType: string;
}

export enum SupportedVectorStorage {
  QDRANT,
  PINECONE,
}
