import { ChangeDetectionStrategy, Component, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, Validators, ReactiveFormsModule, FormGroup, FormControl } from '@angular/forms';
import { emailPattern, passwordPattern } from '@shared/patterns/valid.pattern';
import { InputComponent } from '@shared/components/input/input.component';
import { MatIconModule } from '@angular/material/icon';
import { RoutesPath } from '@core/enums/routes-path.enum';
import { Router, RouterModule } from '@angular/router';
import { LoginService } from '@components/authentication/api/login.service';
import { Subject, filter, takeUntil, tap } from 'rxjs';
import { ToastMessageService } from '@shared/components/toast-message/services/toast-message.service';

@Component({
  selector: 'pp-login',
  standalone: true,
  imports: [CommonModule, FormsModule, InputComponent, ReactiveFormsModule, MatIconModule, RouterModule],
  providers: [LoginService],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoginComponent implements OnDestroy {

  readonly REGISTER_LINK = `/${RoutesPath.AUTHENTICATION}/${RoutesPath.REGISTER}`;

  private onDestroy$ = new Subject<void>();

  form = new FormGroup({
    email: new FormControl('', Validators.compose([Validators.required, Validators.pattern(emailPattern)])),
    password: new FormControl('', Validators.compose([Validators.required, Validators.pattern(passwordPattern)])),
  },
    {
      updateOn: 'change',
    });

  passwordVisible = false;

  constructor(
    private loginService: LoginService,
    private toastMessageService: ToastMessageService,
    private router: Router,
  ) { }

  ngOnDestroy(): void {
    this.onDestroy$.next();
    this.onDestroy$.complete();
  }

  togglePasswordVisibility() {
    this.passwordVisible = !this.passwordVisible;
  }

  login(): void {
    if (this.form.valid)
      this.loginService.login(this.form.value).pipe(
        filter((res) => !!res),
        tap(() => this.router.navigate(['/'])),
        // tap(() => this.toastMessageService.notifyOfSuccess('Zalogowano')),
        takeUntil(this.onDestroy$),
      ).subscribe();
  }
}
