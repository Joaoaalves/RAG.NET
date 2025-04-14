export interface Filter {
  id?: string;
  strategy: FilterStrategy | string;
  isEnabled: boolean;
  maxItems: number;
}

export interface FilterEnableResponse {
  message: string;
  filter: Filter;
}

export interface FilterEnableResponse {
  message: string;
  filter: Filter;
}

export interface FilterUpdateResponse {
  message: string;
  filter: Filter;
}

export enum FilterStrategy {
  RSE = 0,
}
