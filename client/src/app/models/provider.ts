import { ConversationModel } from './chat';
import { EmbeddingModel } from './embedding';

export interface ProviderOption {
  label: string;
  value: number;
}

export interface ProvidersResponse<
  T extends EmbeddingModel | ConversationModel
> {
  [key: string]: T[];
}
