import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TokenData, UserAuthorization, UserLogin } from '@components/authentication/interfaces/authentication-interface';
import { Observable, catchError, of, tap } from 'rxjs';
import { Api } from '@core/enums/api.enum';
import { environment } from '@env/environment';
import jwt_decode from 'jwt-decode';
import { LocalStorageService } from '@core/services/localStorage/local-storage.service';
import { ToastMessageService } from '@shared/components/toast-message/services/toast-message.service';
import { KeyStorage } from '@core/enums/key-storage.enum';
import { UserService } from '@core/services/authorization/user.service';
import { addSeconds } from 'date-fns';

@Injectable()
export class LoginService {

  constructor(
    private http: HttpClient,
    private userService: UserService,
    private localStorageService: LocalStorageService,
    private toastMessageService: ToastMessageService,
  ) { }

  login(userData: UserLogin): Observable<unknown> {
    return this.http.post<UserAuthorization>(`${environment.httpBackend}${Api.LOGIN}`, { email: userData.email, password: userData.password }).pipe(
      tap((token) => {
        const myToken = token;
        this.setUserStorage(myToken);
        this.userService.setUserToken(myToken.token);
      }),
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];

        this.toastMessageService.notifyOfError(error[0]);

        return of(null);
      })
    );
  }

  private setUserStorage(token: UserAuthorization) {
    const decodedToken: TokenData = jwt_decode(token.token);

    const { email, exp, firstName, id, lastName, role } = decodedToken;
    this.localStorageService.setItem<TokenData>(KeyStorage.USER, {
      email, firstName, id, lastName, role, expires_at: addSeconds(new Date(), exp).toISOString()
    });
  }
}
