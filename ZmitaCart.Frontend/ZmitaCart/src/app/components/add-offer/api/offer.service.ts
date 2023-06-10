import { Injectable } from '@angular/core';
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { environment } from "@env/environment";
import { Api } from "@core/enums/api.enum";

@Injectable({
  providedIn: 'root'
})
export class OfferService {
  formData: FormData;

  constructor(private http: HttpClient) { }

  createOffer(title: string, desc: string, price: number, quantity: number, condition: string, categoryId: number, pics: File[]): Observable<number> {
    this.formData = new FormData();
    this.formData.append('title', title);
    this.formData.append('description', desc);
    this.formData.append('price', price.toString());
    this.formData.append('quantity', quantity.toString());
    this.formData.append('condition', condition);
    this.formData.append('categoryId', categoryId.toString());

    Array.from(pics).forEach(f => this.formData.append('pictures', f));

    return this.http.post<number>(`${environment.httpBackend}${Api.OFFER}`, this.formData);
  }
}
