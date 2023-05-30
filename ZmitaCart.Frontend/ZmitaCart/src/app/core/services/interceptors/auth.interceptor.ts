import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserService } from '@core/services/authorization/user.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(
    private userService: UserService
  ) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {

    // if (!this.userService.isAuthenticated() && this.userService.getCaptchaToken()) {
    //   request = request.clone({
    //     setHeaders: {
    //       'Content-Type': 'application/x-www-form-urlencoded',
    //       'g-recaptcha-response': this.userService.getCaptchaToken(),
    //     }
    //   });
    // }

    request = request.clone({
      setHeaders: {
        'Authorization': `bearer ${this.userService.getUserToken()}`,
      }
    });

    return next.handle(request);
  }
}
