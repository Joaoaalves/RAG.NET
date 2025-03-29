import { ChunkerSettings } from './chunker';
export interface WorkflowsResponse {
  workflows: [];
}
export interface Workflow {
  id: string;
  name: string;
  strategy: string;
  settings: ChunkerSettings;
  apiKey: string;
  embeddingProvider: EmbeddingProvider;
}

export interface EmbeddingProvider {
  provider: string;
  apiKey: string;
  vectorSize: number;
}
