import { HttpClient } from '@angular/common/http';
import { Feedback } from '../models/feedback';
import { catchError, map, Observable, of } from 'rxjs';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class FeedbackService {
  private apiUrl = environment.apiUrl + '/api/feedback';

  constructor(private httpClient: HttpClient) {}

  sendFeedback(feedback: Feedback): Observable<boolean> {
    return this.httpClient.post<void>(this.apiUrl, feedback).pipe(
      map(() => true),
      catchError(() => of(false))
    );
  }
}
