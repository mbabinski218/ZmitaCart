import { Injectable } from '@angular/core';
import { NavigationEnd, Router, Event } from '@angular/router';
import { Observable, filter } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoutingService {

  constructor(private router: Router,) { }

  getUrlChanges(): Observable<Event> {
    return this.router.events.pipe(
      filter((res) => res instanceof NavigationEnd),
    );
  }

  getCurrentUrl(): string {
    return window.location.pathname;
  }
}
