import { Component } from '@angular/core';
import { QueryService } from 'src/app/services/query.service';
import { QueryFormComponent } from 'src/app/shared/components/query-form/query-form.component';

@Component({
  templateUrl: './query.component.html',
  selector: 'app-query',
  imports: [QueryFormComponent],
  standalone: true,
})
export class QueryComponent {
  queryService: QueryService;

  constructor(queryService: QueryService) {
    this.queryService = queryService;
  }
}
