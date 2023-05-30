import { AbstractControl, ControlContainer, FormControl, FormGroupDirective, NgForm, ReactiveFormsModule } from '@angular/forms';
import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output, TemplateRef, ViewChild } from '@angular/core';
import { MatFormFieldControl } from '@angular/material/form-field';
import { Nullable } from '@core/types/nullable';
import { ErrorStateMatcher } from '@angular/material/core';
import { ErrorMessage } from '@shared/components/input/interfaces/error-message.interface';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'pp-input',
  standalone: true,
  imports: [CommonModule, MatInputModule, ReactiveFormsModule, MatIconModule],
  templateUrl: './input.component.html',
  styleUrls: ['./input.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  viewProviders: [
    {
      provide: ControlContainer,
      useExisting: FormGroupDirective,
    }
  ]
})
export class InputComponent {

  @ViewChild(MatFormFieldControl, { static: true }) control: MatFormFieldControl<string>;

  @Input() controlForm: AbstractControl;
  @Input() controlName: string;
  @Input() label: string;
  @Input() placeholder: string;
  @Input() type = 'text';
  @Input() maxLength = 524287;
  @Input() min: number;
  @Input() appearance: 'fill' | 'outline' = 'outline';
  @Input() ppErrorMessage: Partial<ErrorMessage>;
  @Input() readonly = false;
  @Input() touchedPlaceholder = false;
  @Input() suffixTemplate: TemplateRef<Nullable<unknown>>;
  @Input() hint: TemplateRef<Nullable<unknown>>;
  @Input() suffixOutsideTemplate: TemplateRef<Nullable<unknown>>;
  @Input() hintAlign: 'end' | 'start' = 'end';
  @Input() optional = false;

  @Output() lzFocus = new EventEmitter<boolean>();

  matcher = new MyErrorStateMatcher();

  constructor(
    private formGroupDirective: FormGroupDirective,
  ) { }

  get getControl(): FormControl {
    if (this.controlForm) {
      return this.controlForm as FormControl;
    }
    return this.formGroupDirective.form.controls[this.controlName] as FormControl;
  }

  get visiblePlaceholder(): string {
    if (this.readonly || this.getControl.touched)
      return;

    if (this.touchedPlaceholder)
      return this.placeholder;

    return this.placeholder;
  }

  get errorMessage(): Nullable<string> {
    if (!this.ppErrorMessage || !this.getControl.errors)
      return null;

    for (const [key] of Object.entries(this.getControl.errors)) {
      if (Object.prototype.hasOwnProperty.call(this.getControl.errors, key))
        return this.ppErrorMessage[key as keyof ErrorMessage];
    }
  }
}

export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;

    return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
  }
}
