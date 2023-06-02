import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastMessageService } from '@shared/components/toast-message/services/toast-message.service';
import { Observable, catchError, map, of, tap } from 'rxjs';
import { SubCategories, SuperiorCategories } from '@components/home/components/header/interfaces/header.interface';
import { environment } from '@env/environment';
import { Api } from '@core/enums/api.enum';

@Injectable()
export class HeaderService {

  constructor(
    private http: HttpClient,
    private toastMessageService: ToastMessageService,
  ) { }

  getSuperiorCategories(): Observable<SuperiorCategories[]> {
    return this.http.get<SuperiorCategories[]>(`${environment.httpBackend}${Api.GET_SUPERIOR_CATEGORIES}`).pipe(
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];

        this.toastMessageService.notifyOfError(error[0]);

        return of([]);
      })
    );
  }

  getSubCategories(superiorId: number): Observable<SubCategories> {
    const params = new HttpParams()
      .set('superiorId', superiorId)
      .set('childrenCount', 2);

    return this.http.get<SubCategories[]>(`${environment.httpBackend}${Api.GET_SUB_CATEGORIES_FEW}`, { params }).pipe(
      map((res) => res[0]),
      tap((res) => console.log(res)),
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];

        this.toastMessageService.notifyOfError(error[0]);

        return of();
      })
    );
  }
}
