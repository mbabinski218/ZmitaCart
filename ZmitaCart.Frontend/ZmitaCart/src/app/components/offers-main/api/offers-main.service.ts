import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Api } from '@core/enums/api.enum';
import { environment } from '@env/environment';
import { ToastMessageService } from '@shared/components/toast-message/services/toast-message.service';
import { Observable, catchError, map, of, tap } from 'rxjs';
import { MainOffers } from '@components/offers-main/interfaces/offers-main.interface';

@Injectable()
export class OfferMainService {

  constructor(
    private http: HttpClient,
    private toastMessageService: ToastMessageService,
  ) { }

  getOffers(): Observable<MainOffers> {
    const params = new HttpParams()
      .set('categoriesNames', 'Samsung')
      .set('size', 12);

    return this.http.get<MainOffers>(`${environment.httpBackend}${Api.OFFER_MAIN}`, { params }).pipe(
      tap((res) => console.log(res)),
      // map(() => true),
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];

        this.toastMessageService.notifyOfError(error[0]);

        return of();
      })
    );
  }
}
