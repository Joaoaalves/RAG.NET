import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

export abstract class BaseApiService {
  protected readonly apiUrl: string;

  constructor(protected http: HttpClient) {
    this.apiUrl = environment.apiUrl;
  }

  protected buildUrl(path: string): string {
    return `${this.apiUrl}${path}`;
  }
}
