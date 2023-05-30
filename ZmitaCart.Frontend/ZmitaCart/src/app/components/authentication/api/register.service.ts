import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserRegister } from '@components/authentication/interfaces/authentication-interface';
import { Observable, catchError, map, of, tap } from 'rxjs';
import { Api } from '@core/enums/api.enum';
import { environment } from '@env/environment';
import { ToastMessageService } from '@shared/components/toast-message/services/toast-message.service';

@Injectable()
export class RegisterService {

  constructor(
    private http: HttpClient,
    private toastMessageService: ToastMessageService,
  ) { }

  register(userData: Partial<UserRegister>): Observable<unknown> {
    return this.http.post<unknown>(`${environment.httpBackend}${Api.REGISTER}`, userData).pipe(
      map(() => true),
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];

        this.toastMessageService.notifyOfError(error[0]);

        return of(null);
      })
    );
  }
}
