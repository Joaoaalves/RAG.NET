import { Subscription } from './subscription';

export interface User {
  email: string;
  firstName: string;
  lastName: string;
  tokenWallet?: number;
  subscription: Subscription;
}
