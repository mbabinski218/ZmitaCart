import { ChatsStream, MessageStream } from '@components/account/components/user-chat/interfaces/chat.interfaces';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserChatService {

  private currentChat$ = new BehaviorSubject<ChatsStream>(null);
  private offerId$ = new BehaviorSubject<string>(null);
  private messageStream$ = new Subject<MessageStream>;
  private previousChatsStream$ = new Subject<ChatsStream>;
  private notifications$ = new Subject<number>;
  
  
  setCurrentChat(chat: ChatsStream): void {
    if (chat !== this.currentChat$.value)
      this.currentChat$.next(chat);
  }

  getCurrentChat(): Observable<ChatsStream> {
    return this.currentChat$.asObservable();
  }


  setMessageStream(chat: MessageStream): void {
    this.messageStream$.next(chat);
  }

  getMessageStream(): Observable<MessageStream> {
    return this.messageStream$.asObservable();
  }


  setPreviousChatsStream(chat: ChatsStream): void {
    this.previousChatsStream$.next(chat);
  }

  getPreviousChatsStream(): Observable<ChatsStream> {
    return this.previousChatsStream$.asObservable();
  }


  setOfferId(id: string): void {
    this.offerId$.next(id);
  }

  getOfferId(): Observable<string> {
    return this.offerId$.asObservable();
  }


  setNotifications(chatsCount: number): void {
    this.notifications$.next(chatsCount);
  }

  getNotifications(): Observable<number> {
    return this.notifications$.asObservable();
  }
}
