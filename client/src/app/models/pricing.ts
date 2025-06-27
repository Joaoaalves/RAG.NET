export interface Plan {
  name: string;
  description: string;
  icon: string;
  price: string;
  tokens: number;
  features: string[];
  cta: string;
}

export enum Intent {
  CORE = 'core',
  ENHANCED = 'enhanced',
  ASCEND = 'ascend',
}
