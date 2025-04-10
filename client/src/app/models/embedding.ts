export interface EmbeddingRequest {
  file: File;
}

export interface EmbeddingResponse {
  totalChunks: number;
  processedChunks: number;
}

export interface EmbeddingModel {
  label: string;
  value: string;
  speed: number;
  price: number;
  vectorSize: number;
  maxContent: number;
}

export interface EmbeddingModelsResponse {
  openAI: EmbeddingModel[];
  voyage: EmbeddingModel[];
}

export enum EmbeddingProviderEnum {
  OPENAI = 0,
  VOYAGE = 1,
}

export interface EmbeddingProvider {
  provider: EmbeddingProviderEnum;
  apiKey: string;
  vectorSize: number;
  model: string;
}
