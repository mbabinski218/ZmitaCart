/* eslint-disable @typescript-eslint/ban-ts-comment */
import { ChangeDetectionStrategy, Component, NgZone, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CredentialResponse } from 'google-one-tap';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { LoginService } from '@components/authentication/api/login.service';
import { GoogleLoginProvider, SocialAuthService, SocialAuthServiceConfig } from '@abacritt/angularx-social-login';

@Component({
  selector: 'pp-google-login',
  standalone: true,
  imports: [CommonModule, MatCardModule],
  providers: [LoginService, SocialAuthService, {
    provide: 'SocialAuthServiceConfig',
    useValue: {
      autoLogin: false,
      providers: [
        {
          id: GoogleLoginProvider.PROVIDER_ID,
          provider: new GoogleLoginProvider(
            '210267773068-m3nvq2q18bug70vojhiect78nv8i81g5.apps.googleusercontent.com'
          )
        },
      ],
    } as SocialAuthServiceConfig,
  }
  ],
  templateUrl: './google-login.component.html',
  styleUrls: ['./google-login.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class GoogleLoginComponent implements OnInit {

  constructor(
    private router: Router,
    private _ngZone: NgZone,
    private loginService: LoginService,
    private authService: SocialAuthService,//musi zostać, inaczej nie działa xdd
  ) { }

  ngOnInit(): void {
    // @ts-ignore
    window.onGoogleLibraryLoad = () => {
      // @ts-ignore
      google.accounts.id.initialize({
        client_id: '210267773068-m3nvq2q18bug70vojhiect78nv8i81g5.apps.googleusercontent.com',
        callback: this.handleCredentialResponse.bind(this),
        auto_select: false,
        cancel_on_tap_outside: true,
      });

      // @ts-ignore
      google.accounts.id.renderButton(
        document.getElementById("googleLoginButton"),
        { theme: "outline", size: "large", text: 'continue_with' }
      );
    };
  }

  handleCredentialResponse(response: CredentialResponse) {
    this.loginService.googleLogin(response.credential).subscribe(() => {
      this._ngZone.run(() => {
        void this.router.navigate(['/']);
      });
    }
    );
  }

}