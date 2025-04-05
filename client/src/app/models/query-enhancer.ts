export interface QueryEnhancer {
  id: string;
  type: QueryEnhancerStrategy | string;
  maxQueries: number;
  isEnabled: boolean;
  guidance?: string;
}

export enum QueryEnhancerStrategy {
  AUTO_QUERY = 0,
  HYDE = 1,
}

export interface QueryEnhancerEnableResponse {
  message: string;
  queryEnhancer: QueryEnhancer;
}
