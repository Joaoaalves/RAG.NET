export interface VectorStoragePolicy {
  id: string;
  providerId: number;
  apiKey: string;
  name: string;
  pattern: string;
  prefix: string;
  url: string;
  schema: string;
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
export type SupportedVectorStorage = 'qdrant' | 'pinecone';
