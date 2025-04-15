import { Component, Input, OnInit } from '@angular/core';
import { InputComponent } from '../input/input.component';
import { TextAreaComponent } from '../text-area/text-area.component';
import { Observable } from 'rxjs';
import { QueryRequest, QueryResponse } from 'src/app/models/query';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { HlmSwitchComponent } from 'libs/ui/ui-switch-helm/src/lib/hlm-switch.component';
import { CommonModule } from '@angular/common';

@Component({
  templateUrl: './query-form.component.html',
  selector: 'app-query-form',
  imports: [
    CommonModule,
    TextAreaComponent,
    InputComponent,
    HlmSwitchComponent,
    ReactiveFormsModule,
  ],
  standalone: true,
})
export class QueryFormComponent implements OnInit {
  queryForm!: FormGroup;

  @Input() apiKey!: string;
  @Input() onQuery!: (
    data: QueryRequest,
    apiKey: string
  ) => Observable<QueryResponse>;

  constructor(private readonly fb: FormBuilder) {}

  ngOnInit(): void {
    this.queryForm = this.fb.group({
      query: ['', [Validators.required]],
      enableTopK: [true, [Validators.required]],
      topK: [5, [Validators.required, Validators.min(1), Validators.max(10)]],
      parentChild: [false, [Validators.required]],
      normalizeScore: [false, [Validators.required]],
      minNormalizedScore: [0.5, [Validators.min(0), Validators.max(1)]],
      scoreThreshold: [false, [Validators.required]],
      minScore: [0.3, [Validators.min(0), Validators.max(1)]],
    });
  }

  onSubmit(event: Event) {
    event.preventDefault();

    if (this.queryForm.valid) {
      const data: QueryRequest = {
        query: this.queryForm.value.query,
        parentChild: this.queryForm.value.parentChild,
      };

      if (this.queryForm.value.enableTopK) {
        data.topK = this.queryForm.value.topK;
      }

      if (this.queryForm.value.normalizeScore) {
        data.minNormalizedScore = this.queryForm.value.minNormalizedScore;
      }

      if (this.queryForm.value.scoreThreshold) {
        data.minScore = this.queryForm.value.minScore;
      }

      this.onQuery(data, this.apiKey).subscribe((response) => {
        console.log('Query response:', response);
      });
    } else {
      console.error('Invalid form submission:', this.queryForm.errors);
    }
  }
}
