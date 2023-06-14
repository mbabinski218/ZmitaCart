import { UserChatService } from './../../services/user-chat.service';
import { ChangeDetectionStrategy, Component, Input, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { filter, tap, BehaviorSubject, Observable, takeUntil, map, Subject, merge, distinctUntilChanged } from 'rxjs';
import { ChatsStream, MessageOfferInChat, MessageStream } from '../../interfaces/chat.interfaces';
import { MessengerService } from '../messenger/services/messenger.service';
import { ChatItemComponent } from './components/chat-item/chat-item.component';

@Component({
  selector: 'pp-chats',
  standalone: true,
  imports: [CommonModule, ChatItemComponent],
  templateUrl: './chats.component.html',
  styleUrls: ['./chats.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ChatsComponent implements OnInit, OnDestroy {

  @Input() set currentChat(chat: ChatsStream) {
    if (chat) {
      if (chat.id === -10 && !this.added) {
        this.added = true;
        this.previousChats$.next([
          ...this.previousChats$.value,
          chat,
        ]);
      }

      const previousChats = this.previousChats$.value;
      const foundChat = previousChats.find((result) => result.offerId === chat.offerId);

      previousChats.forEach((result) => {
        if (result)
          result.isCurrentChat = false;
      });

      if (foundChat)
        foundChat.isCurrentChat = true;
    }
  }

  onDestroy$ = new Subject<void>();
  previousChats$ = new BehaviorSubject<ChatsStream[]>([]);
  previousMessages$ = new BehaviorSubject<MessageStream[]>([]);
  added = false;
  

  previousChatsWithMessages$ = new BehaviorSubject<MessageOfferInChat[]>([]);


  constructor(
    private userChatService: UserChatService,
  ) { }

  ngOnInit(): void {
    this.userChatService.getPreviousChatsStream().pipe(
      filter((res) => !!res),
      tap((res) => {
        this.previousChats$.next([
          ...this.previousChats$.value,
          res,
        ]);
      }),
      takeUntil(this.onDestroy$),
    ).subscribe();//działa git


    //jakos łączyć oferte z wiadomościa? chyba sie nie da?


    this.userChatService.getMessageStream().pipe(
      filter((res) => !!res),
      // tap((res) => console.log(res)),
      tap((res) => console.log(this.previousChats$.value)),
      map((res) => ({
        ...res,
        date: new Date(res.date),
      })),
      tap((res) => {
        this.previousMessages$.next([
          ...this.previousMessages$.value,
          res,
        ]);
      }),
      takeUntil(this.onDestroy$),
    ).subscribe();
  }

  ngOnDestroy(): void {
    this.onDestroy$.next();
    this.onDestroy$.complete();
  }

  changeCurrentChat(chat: ChatsStream) {
    this.userChatService.setCurrentChat(chat);
  }
}
