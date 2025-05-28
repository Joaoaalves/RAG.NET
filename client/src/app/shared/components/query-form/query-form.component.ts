import { Component, Input, OnInit } from '@angular/core';
import { InputComponent } from '../input/input.component';
import { TextAreaComponent } from '../text-area/text-area.component';
import { BehaviorSubject, Observable } from 'rxjs';
import { QueryRequest, QueryResponse } from 'src/app/models/query';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { HlmSwitchComponent } from 'libs/ui/ui-switch-helm/src/lib/hlm-switch.component';
import { CommonModule } from '@angular/common';
import { lucideChevronDown, lucideLoaderCircle } from '@ng-icons/lucide';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { HlmToasterComponent } from 'libs/ui/ui-sonner-helm/src/lib/hlm-toaster.component';
import { toast } from 'ngx-sonner';
import { UsageTooltipComponent } from '../usage-tooltip/usage-tooltip.component';
import { SliderInputComponent } from '../slider-input/slider-input.component';

@Component({
  templateUrl: './query-form.component.html',
  selector: 'app-query-form',
  imports: [
    CommonModule,
    TextAreaComponent,
    InputComponent,
    HlmSwitchComponent,
    ReactiveFormsModule,
    HlmToasterComponent,
    SliderInputComponent,
    UsageTooltipComponent,
    NgIcon,
  ],
  providers: [provideIcons({ lucideLoaderCircle, lucideChevronDown })],
  standalone: true,
  styles: `
  :host {
    display: contents;
  }
  `,
})
export class QueryFormComponent implements OnInit {
  queryForm!: FormGroup;
  private isLoadingSubject = new BehaviorSubject<boolean>(false);
  isLoading$ = this.isLoadingSubject.asObservable();
  isRefineOpen = false;

  @Input() apiKey!: string;
  @Input() onQuery!: (
    data: QueryRequest,
    apiKey: string
  ) => Observable<QueryResponse>;

  constructor(private readonly fb: FormBuilder) {}

  ngOnInit(): void {
    this.queryForm = this.fb.group({
      query: ['', [Validators.required]],
      enableTopK: [false, [Validators.required]],
      topK: [5, [Validators.min(1), Validators.max(10)]],
      parentChild: [false, [Validators.required]],
      normalizeScore: [true, [Validators.required]],
      minNormalizedScore: [0.5, [Validators.min(0.5), Validators.max(1)]],
      minScore: [0.3, [Validators.min(0.3), Validators.max(1)]],
    });
  }

  onSubmit(event: Event) {
    event.preventDefault();
    this.isLoadingSubject.next(true);

    if (this.queryForm.valid) {
      const data: QueryRequest = {
        query: this.queryForm.value.query,
        parentChild: this.queryForm.value.parentChild,
        normalizeScore: this.queryForm.value.normalizeScore,
      };

      if (this.queryForm.value.enableTopK) {
        data.topK = this.queryForm.value.topK;
      }

      if (this.queryForm.value.normalizeScore) {
        data.minNormalizedScore = this.queryForm.value.minNormalizedScore;
      } else {
        data.minScore = this.queryForm.value.minScore;
      }

      this.onQuery(data, this.apiKey).subscribe({
        next: (response) => {
          if (response.chunks.length === 0) {
            toast.error('No results found for your query.');
          } else {
            toast.success('Query executed successfully!');
          }
          this.queryForm.value.query = '';
        },
        error: (err) => {
          toast.error('Error on request:', err);
          this.isLoadingSubject.next(false);
        },
        complete: () => {
          this.isLoadingSubject.next(false);
        },
      });
    } else {
      toast.error('Please fill in all required fields.');
      this.isLoadingSubject.next(false);
    }
  }

  toggleNormalize() {
    const curr = this.queryForm.get('normalizeScore')?.value;
    this.queryForm.patchValue({
      normalizeScore: !curr,
    });
  }

  toggleRefine() {
    this.isRefineOpen = !this.isRefineOpen;
  }
}
