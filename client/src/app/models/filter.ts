export interface Filter {
  id?: string;
  strategy: FilterStrategyEnum | string;
  isEnabled: boolean;
  maxItems: number;
}

export interface FilterEnableResponse {
  message: string;
  queryResultFilter: Filter;
}

export interface FilterEnableResponse {
  message: string;
  queryResultFilter: Filter;
}

export interface FilterUpdateResponse {
  message: string;
  queryResultFilter: Filter;
}

export enum FilterStrategyEnum {
  RSE = 0,
}
