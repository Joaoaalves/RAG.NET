export interface EmbeddingRequest {
  file: File;
}
export interface EmbeddingResponse {
  message: string;
  jobId: string;
}

export interface EmbeddingModel {
  label: string;
  value: string;
  speed: number;
  price: number;
  vectorSize: number;
  maxContext: number;
}

export enum EmbeddingProviderEnum {
  OPENAI = 0,
  VOYAGE = 1,
}

export interface EmbeddingProvider {
  provider: 'OPENAI' | 'VOYAGE';
  model: string;
  apiKey: string;
  vectorSize: number;
}
