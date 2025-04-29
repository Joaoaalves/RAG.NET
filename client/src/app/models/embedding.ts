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
  maxContent: number;
}

export enum EmbeddingProviderEnum {
  OPENAI = 0,
  VOYAGE = 1,
}

export interface EmbeddingProvider {
  provider: EmbeddingProviderEnum;
  model: string;
}
