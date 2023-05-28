import { ChangeDetectionStrategy, Component, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, Validators, ReactiveFormsModule, FormGroup, FormControl } from '@angular/forms';
import { emailPattern, passwordPattern } from '@shared/patterns/valid.pattern';
import { InputComponent } from '@shared/components/input/input.component';
import { MatIconModule } from '@angular/material/icon';
import { RoutesPath } from '@core/enums/routes-path.enum';
import { RouterModule } from '@angular/router';
import { UserLogin } from '@components/authentication/interfaces/authentication-interface';
import { LoginService } from '@components/authentication/api/login.service';

@Component({
  selector: 'pp-login',
  standalone: true,
  imports: [CommonModule, FormsModule, InputComponent, ReactiveFormsModule, MatIconModule, RouterModule],
  providers: [LoginService],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoginComponent {

  readonly REGISTER_LINK = `/${RoutesPath.AUTHENTICATION}/${RoutesPath.REGISTER}`;

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
  ) { }

  togglePasswordVisibility() {
    this.passwordVisible = !this.passwordVisible;
  }

  login(): void {
    if (this.form.valid) {
      const loginForm: UserLogin = {
        email: this.form.value.email,
        password: this.form.value.password,
      };

      this.loginService.login(loginForm).subscribe();
    }
  }
}
