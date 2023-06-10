import { Injectable } from '@angular/core';
import { SingleChat } from '@components/account/interfaces/account.interface';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserChatService {

  private currentChat$ = new BehaviorSubject<SingleChat>(null);

  setCurrentChat(chat: SingleChat): void {
    this.currentChat$.next(chat);
  }

  getCurrentChat(): Observable<SingleChat> {
    return this.currentChat$.asObservable();
  }
}
