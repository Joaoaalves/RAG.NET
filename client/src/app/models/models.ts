interface Model {
  label: string;
  value: string;
  speed: number;
}

export interface ConversationModel extends Model {
  inputPrice: number;
  outputPrice: number;
  maxOutput: number;
  contextWindow: number;
}

export interface EmbeddingModel extends Model {
  price: number;
  vectorSize: number;
  maxContext: number;
}
