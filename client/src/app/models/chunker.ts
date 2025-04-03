export interface ChunkerSettings {
  threshold: number;
  evaluationPrompt: string;
  maxChunkSize: number;
}

export enum ChunkerStrategy {
  PROPOSITION = 0,
  SEMANTIC = 1,
  PARAGRAPH = 2,
}
