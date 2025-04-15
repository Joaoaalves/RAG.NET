import { Component, Input } from '@angular/core';
import { InputComponent } from '../input/input.component';
import { TextAreaComponent } from '../text-area/text-area.component';
import { Observable } from 'rxjs';
import { QueryRequest, QueryResponse } from 'src/app/models/query';

@Component({
  templateUrl: './query-form.component.html',
  selector: 'app-query-form',
  imports: [TextAreaComponent],
  standalone: true,
})
export class QueryFormComponent {
  @Input() onQuery!: (
    data: QueryRequest,
    apiKey: string
  ) => Observable<QueryResponse>;
  constructor() {}

  onSubmit() {}
}
