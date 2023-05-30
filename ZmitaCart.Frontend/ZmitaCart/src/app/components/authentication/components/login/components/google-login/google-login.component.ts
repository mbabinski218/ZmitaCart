/* eslint-disable @typescript-eslint/ban-ts-comment */
import { ChangeDetectionStrategy, Component, NgZone, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CredentialResponse, PromptMomentNotification } from 'google-one-tap';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { LoginService } from '@components/authentication/api/login.service';

@Component({
  selector: 'pp-google-login',
  standalone: true,
  imports: [CommonModule, MatCardModule],
  providers: [LoginService],
  templateUrl: './google-login.component.html',
  styleUrls: ['./google-login.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class GoogleLoginComponent implements OnInit {

  constructor(
    private router: Router,
    private _ngZone: NgZone,
    private loginService: LoginService,
  ) { }

  ngOnInit(): void {
    // @ts-ignore
    window.onGoogleLibraryLoad = () => {

      // @ts-ignore
      google.accounts.id.initialize({
        client_id: '',
        callback: this.handleCredentialResponse.bind(this),
        auto_select: false,
        cancel_on_tap_outside: true
      });

      // @ts-ignore
      google.accounts.id.renderButton(
        document.getElementById("googleLoginButton"),
        { theme: "outline", size: "large", text: 'continue_with' }
      );

      // @ts-ignore
      google.accounts.id.prompt((notification: PromptMomentNotification) => { });//czy to musi być?
    };
  }

  handleCredentialResponse(response: CredentialResponse) {
    console.log(response);
    // await this.loginService.googleLogin(response.credential).subscribe(() => {
    //     this._ngZone.run(() => {
    //       void this.router.navigate(['/logout']);
    //     });
    //   }
    // );
  }

}