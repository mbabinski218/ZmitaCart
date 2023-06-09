import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Api } from '@core/enums/api.enum';
import { environment } from '@env/environment';
import { ToastMessageService } from '@shared/components/toast-message/services/toast-message.service';
import { Chats, CredentialsForm, FavouriteOffers, UserCredentials } from '@components/account/interfaces/account.interface';
import { Observable, catchError, map, of } from 'rxjs';

@Injectable()
export class AccountService {

  constructor(
    private http: HttpClient,
    private toastMessageService: ToastMessageService,
  ) { }

  sendForm(userData: Partial<CredentialsForm>): Observable<boolean> {
    return this.http.put<unknown>(`${environment.httpBackend}${Api.USER_CREDENTIALS_UPDATE}`, userData).pipe(
      map(() => true),
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];

        this.toastMessageService.notifyOfError(error[0]);

        return of(false);
      })
    );
  }

  getUserData(): Observable<UserCredentials> {
    return this.http.get<UserCredentials>(`${environment.httpBackend}${Api.USER_CREDENTIALS}`).pipe(
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];

        this.toastMessageService.notifyOfError(error[0]);

        return of();
      })
    );
  }

  getUserOffers(): Observable<any> {
    return this.http.get<any>(`${environment.httpBackend}${Api.USER_OFFERS}`).pipe(
      //   catchError((err: HttpErrorResponse) => {
      //     const error = err.error as string[];

      //     this.toastMessageService.notifyOfError(error[0]);

      //     return of();
      //   })
    );
  }

  getAllChats(pageNumber: number): Observable<Chats> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', 4);

    return this.http.get<Chats>(`${environment.httpBackend}${Api.CONVERSATION}`, { params }).pipe(
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];

        this.toastMessageService.notifyOfError(error[0]);

        return of();
      })
    );
  }

  getFavourites(pageNumber: number): Observable<FavouriteOffers> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', 10);

    return this.http.get<FavouriteOffers>(`${environment.httpBackend}${Api.OFFER_FAVOURITES}`, { params }).pipe(
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];

        this.toastMessageService.notifyOfError(error[0]);

        return of();
      })
    );
  }
}
