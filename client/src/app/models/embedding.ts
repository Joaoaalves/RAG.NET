export interface EmbeddingRequest {
  file: File;
}
export interface EmbeddingResponse {
  message: string;
  jobId: string;
}

export interface EmbeddingProvider {
  providerName: string;
  providerId: number;
  model: string;
  apiKey: string;
  vectorSize: number;
}
