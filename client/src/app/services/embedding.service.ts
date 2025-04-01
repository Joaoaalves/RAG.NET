import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EmbeddingRequest, EmbeddingResponse } from '../models/embedding';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class EmbeddingService {
  private apiUrl = environment.apiUrl;

  embedd(
    requestData: EmbeddingRequest,
    apiKey: string
  ): Observable<EmbeddingResponse> {
    return new Observable<EmbeddingResponse>((observer) => {
      const xhr = new XMLHttpRequest();
      xhr.open('POST', `${this.apiUrl}/api/workflows/embedding?stream=true`);
      xhr.setRequestHeader('x-api-key', apiKey);

      const formData = new FormData();
      formData.append('file', requestData.file);

      let lastIndex = 0;

      xhr.onprogress = () => {
        const responseText = xhr.responseText;

        const newData = responseText.substring(lastIndex);
        lastIndex = responseText.length;

        const events = newData.split('\n\n');
        events.forEach((event) => {
          if (event.startsWith('data: ')) {
            const jsonStr = event.substring(6).trim();
            if (jsonStr) {
              try {
                const data: EmbeddingResponse = JSON.parse(jsonStr);
                observer.next(data);
              } catch (e) {
                console.error('Erro ao parsear JSON do SSE:', e);
              }
            }
          }
        });
      };

      xhr.onload = () => {
        observer.complete();
      };

      xhr.onerror = (error) => {
        observer.error(error);
      };

      xhr.send(formData);
    });
  }
}
