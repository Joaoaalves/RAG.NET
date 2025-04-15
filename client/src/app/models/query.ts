export interface QueryRequest {
  query: string;
  topK?: number;
  parentChild?: boolean;
  normalizeScore?: boolean;
  minNormalizedScore?: number;
  minScore?: number;
}

export interface ContentItem {
  id: string;
  text: string;
  score: number;
  pageId?: string;
  chunkId?: string;
}

export interface QueryResponse {
  chunks: ContentItem[];
  filteredContent: string[];
}
