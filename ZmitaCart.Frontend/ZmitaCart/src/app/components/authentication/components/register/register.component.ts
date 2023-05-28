import { ChangeDetectionStrategy, Component, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, Validators, ReactiveFormsModule, FormGroup, FormControl } from '@angular/forms';
import { emailPattern, passwordPattern } from '@shared/patterns/valid.pattern';
import { mustMatch } from '@shared/utils/must-match';
import { InputComponent } from '@shared/components/input/input.component';
import { MatIconModule } from '@angular/material/icon';
import { RegisterService } from '@components/authentication/api/register.service';
import { UserRegister } from '@components/authentication/interfaces/authentication-interface';

@Component({
  selector: 'pp-register',
  standalone: true,
  imports: [CommonModule, FormsModule, InputComponent, ReactiveFormsModule, MatIconModule],
  providers: [RegisterService],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RegisterComponent {

  form = new FormGroup({
    email: new FormControl('', Validators.compose([Validators.required, Validators.pattern(emailPattern)])),
    firstName: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(35)])),
    lastName: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(35)])),
    password: new FormControl('', Validators.compose([Validators.required, Validators.pattern(passwordPattern)])),
    confirmedPassword: new FormControl('', Validators.required),
  },
    {
      updateOn: 'change',
      validators: mustMatch('password', 'confirmedPassword'),
    }
  );

  passwordVisibility = {
    password: false,
    confirmedPassword: false,
  };

  constructor(
    private registerService: RegisterService,
  ) { }

  togglePasswordVisibility(inputName: 'password' | 'confirmedPassword') {
    this.passwordVisibility[inputName] = !this.passwordVisibility[inputName];
  }

  register(): void {
    if (this.form.valid) {
      const registerForm: UserRegister = {
        email: this.form.value.email,
        firstName: this.form.value.firstName,
        lastName: this.form.value.lastName,
        password: this.form.value.password,
        confirmedPassword: this.form.value.confirmedPassword,
      };

      this.registerService.register(registerForm).subscribe();
    }
  }
}

