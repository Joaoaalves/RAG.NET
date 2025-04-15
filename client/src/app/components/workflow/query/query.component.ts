import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { WorkflowService } from './../../../services/workflow.service';
import { Component, OnInit } from '@angular/core';
import { Workflow } from 'src/app/models/workflow';
import { QueryService } from 'src/app/services/query.service';
import { QueryFormComponent } from 'src/app/shared/components/query-form/query-form.component';
import { QueryResultComponent } from 'src/app/shared/components/query-result/query-result.component';

@Component({
  templateUrl: './query.component.html',
  selector: 'app-query',
  imports: [CommonModule, QueryFormComponent, QueryResultComponent],
  standalone: true,
})
export class QueryComponent implements OnInit {
  queryService!: QueryService;
  workflow?: Workflow;

  constructor(
    queryService: QueryService,
    private workflowService: WorkflowService,
    private route: ActivatedRoute
  ) {
    this.queryService = queryService;
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      const id = params.get('workflowId');
      if (id) {
        this.workflowService.getWorkflow(id).subscribe((workflow) => {
          this.workflow = workflow;
        });
      }
    });
  }

  get isDataAvailable(): boolean {
    return !!(
      this.queryService?.chunks?.length &&
      this.queryService?.filteredContent?.length
    );
  }
}
