export enum ConversationProviderEnum {
  OPENAI = 0,
  ANTHROPIC = 1,
}

export interface ConversationModel {
  label: string;
  value: string;
  speed: number;
  inputPrice: number;
  outputPrice: number;
  maxOutput: number;
  contextWindow: number;
}

export interface ConversationProvider {
  provider: ConversationProviderEnum;
  model: string;
}
