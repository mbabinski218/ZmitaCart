import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserRegister } from '@components/authentication/interfaces/authentication-interface';
import { Observable, catchError, of } from 'rxjs';
import { Api } from '@core/enums/api.enum';
import { environment } from '@env/environment';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ToastMessageComponent } from '@shared/components/toast-message/toast-message.component';

@Injectable()
export class RegisterService {

  constructor(
    private http: HttpClient,
    private _snackBar: MatSnackBar,
  ) { }

  register(userData: UserRegister): Observable<unknown> {
    return this.http.post<unknown>(`${environment.httpBackend}${Api.REGISTER}`, userData).pipe(
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];

        this._snackBar.openFromComponent(ToastMessageComponent, {
          duration: 4000,
          data: error[0],
          panelClass: 'error-snackbar',
        });

        return of(null);
      })
    );
  }
}
