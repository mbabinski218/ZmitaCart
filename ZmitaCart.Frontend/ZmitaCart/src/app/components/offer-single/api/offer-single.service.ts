import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Api } from '@core/enums/api.enum';
import { environment } from '@env/environment';
import { ToastMessageService } from '@shared/components/toast-message/services/toast-message.service';
import { Observable, catchError, of } from 'rxjs';
import { SingleOffer } from '@components/offer-single/interfaces/offer-single.interface';

@Injectable()
export class OfferSingleService {

  constructor(
    private http: HttpClient,
    private toastMessageService: ToastMessageService,
  ) { }

  getOffer(id: string): Observable<SingleOffer> {
    return this.http.get<SingleOffer>(`${environment.httpBackend}${Api.OFFER_SINGLE}`.replace(':id', id)).pipe(
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
        return of();
      })
    );
  }
}
