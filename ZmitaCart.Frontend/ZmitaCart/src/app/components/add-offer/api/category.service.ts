import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {SuperiorCategory} from "@components/add-offer/interfaces/SuperiorCategory";

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private http: HttpClient) {
  }

  // getAllSupCategories(): Observable<SuperiorCategory[]> {
  //   return this.http.get<SuperiorCategory[]>(`${
  // }

}
