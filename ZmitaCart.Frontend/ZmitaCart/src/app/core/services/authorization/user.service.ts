import { Injectable } from '@angular/core';
import { isEmpty } from 'lodash';
import { LocalStorageService } from '@core/services/localStorage/local-storage.service';
import { KeyStorage } from '@core/enums/key-storage.enum';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';
import { Api } from '@core/enums/api.enum';
import { tap } from 'rxjs/operators';
import { HttpClient, HttpParams } from '@angular/common/http';
import { addSeconds, isAfter } from 'date-fns';
import { TokenData, UserAuthorization } from '@components/authentication/interfaces/authentication-interface';
import jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private userToken: string;
  private captchaToken: string;

  constructor(
    private localStorageService: LocalStorageService,
    private router: Router,
    private http: HttpClient,
  ) { }

  setUserToken(token: string): void {
    console.log(token);
    this.userToken = token;
  }

  getUserToken(): string {
    return this.userToken;
  }

  setCaptchaToken(token: string): void {
    this.captchaToken = token;
  }

  getCaptchaToken(): string {
    return this.captchaToken;
  }

  isAuthenticated(): boolean {
    return !isEmpty(this.user());
  }

  userAuthorization(): TokenData {
    return this.user();
  }

  logout(): void {
    this.clearAll();
  }

  isTokenExpired(): boolean {
    const { exp } = this.user();

    console.log(exp);
    console.log(new Date());
    console.log(new Date(exp));

    return isAfter(new Date(), new Date(exp));
  }

  refreshToken(): Observable<UserAuthorization> {

    if (isEmpty(this.localStorageService.getItem<TokenData>(KeyStorage.USER))) {
      this.clearAll();
    }

    const userData = new HttpParams()
      .set('grant_type', 'refresh_token');
    // .set('client_id', environment.clientId)
    // .set('client_secret', environment.clientSecret)
    // .set('scope', 'webclient')
    // .set('refresh_token', this.localStorageService.getItem<TokenData>(KeyStorage.USER).refresh_token);

    return this.http.post<UserAuthorization>(`${environment.httpBackend}${Api.LOGIN}`, userData)
      .pipe(
        tap((token) => {
          const myToken = token;
          this.setUserStorage(myToken);
          this.setUserToken(myToken.token);
        }),
      );
  }

  private user(): TokenData {
    return this.localStorageService.getItem<TokenData>(KeyStorage.USER);
  }

  private clearAll(): void {
    this.localStorageService.clear();
    void this.router.navigate(['/']);
  }

  private setUserStorage(token: UserAuthorization) {
    const decodedToken: TokenData = jwt_decode(token.token);

    const { email, exp, firstName, id, lastName, role } = decodedToken;
    this.localStorageService.setItem<TokenData>(KeyStorage.USER, {
      email, firstName, id, lastName, role, expires_at: addSeconds(new Date(), exp).toISOString()
    });
  }
}
