import { UserChatService } from './../../services/user-chat.service';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { filter, tap, BehaviorSubject, Subject, map, takeUntil } from 'rxjs';
import { ChatsStream } from '../../interfaces/chat.interfaces';
import { ChatItemComponent } from './components/chat-item/chat-item.component';
import { ppFixPricePipe } from '@shared/pipes/fix-price.pipe';
import { compareDesc } from 'date-fns';

@Component({
  selector: 'pp-chats',
  standalone: true,
  imports: [CommonModule, ChatItemComponent],
  providers: [ppFixPricePipe],
  templateUrl: './chats.component.html',
  styleUrls: ['./chats.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ChatsComponent implements OnInit, OnDestroy {

  onDestroy$ = new Subject<void>();
  previousChats$ = new BehaviorSubject<ChatsStream[]>([]);
  currentChat$ = new BehaviorSubject<ChatsStream>(null);

  constructor(
    private userChatService: UserChatService,
    private ppFixPricePipe: ppFixPricePipe,
    private ref: ChangeDetectorRef,
  ) { }

  ngOnInit(): void {
    this.userChatService.getCurrentChat().pipe(
      filter((res) => !!res),
      tap((res) => this.currentChat$.next(res)),
      tap((res) => {
        if (this.previousChats$.value.findIndex((result) => result.offerId === res.offerId) === -1)
          this.previousChats$.next([
            res,
            ...this.previousChats$.value,
          ]);
      }),
      tap(() => this.setCurrentChatStyle()),
    ).subscribe();

    this.userChatService.getPreviousChatsStream().pipe(
      filter((res) => !!res),
      map((res) => ({
        ...res,
        offerPrice: this.ppFixPricePipe.transform(Number(res.offerPrice)),
      })),
      tap((res) => {
        const itemIndex = this.previousChats$.value.findIndex((result) => result.offerId === res.offerId);
        if (itemIndex !== -1) {
          if (this.previousChats$.value[itemIndex].isNewChat)
            this.changeCurrentChat(res);

          this.previousChats$.value[itemIndex] = res;

          if (this.currentChat$.value)
            this.setCurrentChatStyle();
            
          return;
        }

        this.previousChats$.next([
          ...this.previousChats$.value,
          res,
        ]);
      }),
      tap(() => this.previousChats$.value.sort((a, b) => compareDesc(new Date(a.date), new Date(b.date)))),
      tap(() => this.ref.detectChanges()),
      takeUntil(this.onDestroy$),
    ).subscribe();
  }

  ngOnDestroy(): void {
    this.previousChats$.next([]);

    this.onDestroy$.next();
    this.onDestroy$.complete();
  }

  changeCurrentChat(chat: ChatsStream) {
    this.userChatService.setCurrentChat(chat);
  }

  private setCurrentChatStyle() {
    const previousChats = this.previousChats$.value;
    const foundChat = previousChats.find((result) => result.offerId === this.currentChat$.value.offerId);

    previousChats.forEach((result) => {
      if (result)
        result.isCurrentChat = false;
    });

    if (foundChat)
      foundChat.isCurrentChat = true;
  }
}
