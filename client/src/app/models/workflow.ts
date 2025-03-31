import { ChunkerSettings, ChunkerStrategy } from './chunker';
import { EmbeddingProviderEnum } from './embedding-provider';

export interface Workflow {
  id: string;
  name: string;
  description: string;
  documents: number;
  strategy: ChunkerStrategy;
  settings: ChunkerSettings;
  apiKey: string;
  embeddingProvider: EmbeddingProvider;
}

export interface EmbeddingProvider {
  provider: EmbeddingProviderEnum;
  apiKey: string;
  vectorSize: number;
}
