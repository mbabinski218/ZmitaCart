import { ChangeDetectionStrategy, Component, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { InputComponent } from '@shared/components/input/input.component';
import { CredentialsForm } from '@components/account/interfaces/account.interface';
import { AccountService } from '@components/account/api/account.service';
import { Subject, filter, takeUntil, tap } from 'rxjs';
import { ToastMessageService } from '@shared/components/toast-message/services/toast-message.service';

@Component({
  selector: 'pp-address-form',
  standalone: true,
  imports: [CommonModule, InputComponent, FormsModule, ReactiveFormsModule],
  providers: [AccountService],
  templateUrl: './address-form.component.html',
  styleUrls: ['./address-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddressFormComponent implements OnDestroy {

  private onDestroy$ = new Subject<void>();

  form = new FormGroup({
    phoneNumber: new FormControl('', Validators.required),
    country: new FormControl('', Validators.required),
    city: new FormControl('', Validators.required),
    street: new FormControl('', Validators.required),
    postalCode: new FormControl(null as number, Validators.required),
    houseNumber: new FormControl(null as number, Validators.required),
    apartmentNumber: new FormControl(null as number),
  });

  constructor(
    private accountService: AccountService,
    private toastMessageService: ToastMessageService,
  ) { }

  ngOnDestroy(): void {
    this.onDestroy$.next();
    this.onDestroy$.complete();
  }

  sendForm(): void {
    const formData: CredentialsForm = {
      phoneNumber: this.form.value.phoneNumber,
      address: {
        country: this.form.value.country,
        city: this.form.value.city,
        street: this.form.value.street,
        postalCode: this.form.value.postalCode,
        houseNumber: this.form.value.houseNumber,
        apartmentNumber: this.form.value.apartmentNumber,
      }
    };

    this.accountService.sendForm(formData).pipe(
      filter((res) => !!res),
      tap(() => this.toastMessageService.notifyOfSuccess('Zaktualizowano dane')),
      tap(() => this.form.reset()),
      takeUntil(this.onDestroy$),
    ).subscribe();
  }
}