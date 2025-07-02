import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { toast } from 'ngx-sonner';

@Injectable({
  providedIn: 'root',
})
export class ErrorService {
  handle(error: HttpErrorResponse): void {
    let message = 'An unknown error occurred';

    if (error.error && typeof error.error === 'object') {
      const structured = error.error;
      message = structured.detail || structured.title || message;

      if (structured.errors) {
        const details = structured.errors
          .map((e: any) => `${e.field}: ${e.message}`)
          .join('\n');
        message += `\n${details}`;
      }
    }
    if (typeof error === 'string') message = error;

    toast.error('Error', { description: message });
    console.error('HTTP Error:', error);
  }
}
