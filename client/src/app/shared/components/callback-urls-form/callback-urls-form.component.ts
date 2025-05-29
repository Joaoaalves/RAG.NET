// callback-urls-form.component.ts
import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { toast } from 'ngx-sonner';

// Models
import { CallbackUrl } from 'src/app/models/callback-url';

// Components
import { AlertComponent } from '../alert/alert.component';

// Services
import { CallbackUrlService } from 'src/app/services/callback-url.service';
import { NgIcon, provideIcons } from '@ng-icons/core';
import {
  lucideCheck,
  lucidePencil,
  lucideExternalLink,
  lucideTrash,
} from '@ng-icons/lucide';

@Component({
  selector: 'app-callback-urls-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, AlertComponent, NgIcon],
  providers: [
    provideIcons({
      lucideCheck,
      lucidePencil,
      lucideExternalLink,
      lucideTrash,
    }),
  ],
  templateUrl: './callback-urls-form.component.html',
})
export class CallbackUrlsFormComponent implements OnInit {
  @Input() workflowId!: string;
  @Input() urls: CallbackUrl[] = [];

  // Parent form wrapping the FormArray
  parentForm!: FormGroup;
  urlsFormArray!: FormArray;
  newUrlForm!: FormGroup;
  editing: boolean[] = [];

  private urlPattern =
    /^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$/;

  constructor(
    private fb: FormBuilder,
    private callbackUrlService: CallbackUrlService
  ) {}

  ngOnInit(): void {
    this.initializeForms();
  }

  private initializeForms(): void {
    // build a FormArray of FormGroups for existing URLs
    this.urlsFormArray = this.fb.array(
      this.urls.map((u) =>
        this.fb.group({
          url: [
            { value: u.url, disabled: true },
            [Validators.required, Validators.pattern(this.urlPattern)],
          ],
        })
      )
    );

    // wrap it in a parent FormGroup
    this.parentForm = this.fb.group({
      urls: this.urlsFormArray,
    });

    // form for adding a new URL
    this.newUrlForm = this.fb.group({
      url: ['', [Validators.required, Validators.pattern(this.urlPattern)]],
    });

    this.editing = new Array(this.urls.length).fill(false);
  }

  startEditing(index: number): void {
    this.editing[index] = true;
    this.urlsFormArray.at(index).get('url')?.enable();
  }

  saveUrl(index: number): void {
    const ctrl = this.urlsFormArray.at(index).get('url');
    if (!ctrl || ctrl.invalid) {
      toast.error('Invalid URL');
      return;
    }
    const updated: CallbackUrl = {
      ...this.urls[index],
      url: ctrl.value,
    };
    this.callbackUrlService
      .updateCallbackUrl(this.workflowId, updated)
      .subscribe(
        (cb) => {
          this.urls[index] = cb;
          ctrl.setValue(cb.url, { emitEvent: false });
          ctrl.disable();
          this.editing[index] = false;
          toast.success('URL updated successfully');
        },
        () => {
          toast.error('Failed to update URL');
        }
      );
  }

  addUrl(): void {
    if (this.newUrlForm.invalid) {
      toast.error('Invalid URL');
      return;
    }
    const newUrl = this.newUrlForm.value.url;
    this.callbackUrlService.addCallbackUrl(this.workflowId, newUrl).subscribe(
      (cb) => {
        this.urls.push(cb);

        // push into FormArray as disabled FormGroup
        this.urlsFormArray.push(
          this.fb.group({
            url: [
              { value: cb.url, disabled: true },
              [Validators.required, Validators.pattern(this.urlPattern)],
            ],
          })
        );
        this.editing.push(false);
        this.newUrlForm.reset();
        toast.success('URL added successfully');
      },
      () => {
        toast.error('Failed to add URL');
      }
    );
  }

  deleteUrl(index: number): void {
    const id = this.urls[index].id;
    this.callbackUrlService.deleteCallbackUrl(this.workflowId, id).subscribe(
      () => {
        this.urls.splice(index, 1);
        this.urlsFormArray.removeAt(index);
        this.editing.splice(index, 1);
        toast.success('URL deleted successfully');
      },
      () => {
        toast.error('Failed to delete URL');
      }
    );
  }
}
