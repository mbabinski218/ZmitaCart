import { ChatsStream, MessageStream } from './../interfaces/chat.interfaces';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserChatService {

  private currentChat$ = new BehaviorSubject<ChatsStream>(null);
  private messageStream$ = new Subject<MessageStream>;
  private previousChatsStream$ = new Subject<ChatsStream>;
  
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
}
