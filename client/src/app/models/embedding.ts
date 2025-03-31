export interface EmbeddingRequest {
  file: File;
}

export interface EmbeddingResponse {
  totalChunks: number;
  processedChunks: number;
}
