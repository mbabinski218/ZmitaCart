import { MessengerService } from '@components/account/components/user-chat/services/messenger.service';
import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountService } from '@components/account/api/account.service';
import { BehaviorSubject, Observable, Subject, filter, map, switchMap, takeUntil, tap } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { ppFixPricePipe } from '@shared/pipes/fix-price.pipe';
import { ChatsComponent } from '@components/account/components/user-chat/components/chats/chats.component';
import { MessengerComponent } from '@components/account/components/user-chat/components/messenger/messenger.component';
import { OfferComponent } from '@components/account/components/user-chat/components/offer/offer.component';
import { ChatsStream, MessageStream } from '@components/account/components/user-chat/interfaces/chat.interfaces';
import { UserChatService } from '@components/account/components/user-chat/services/user-chat.service';
import { SharedService } from '@shared/services/shared.service';
import { goToDetails } from '@shared/utils/offer-details';

@Component({
  selector: 'pp-user-chat',
  standalone: true,
  imports: [CommonModule, ChatsComponent, MessengerComponent, OfferComponent],
  providers: [AccountService, ppFixPricePipe],
  templateUrl: './user-chat.component.html',
  styleUrls: ['./user-chat.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserChatComponent implements OnInit, OnDestroy {

  currentChat$: Observable<ChatsStream>;
  newLastMessage$ = new BehaviorSubject<MessageStream>(null);
  offerId: string;
  chatId: number;
  private onDestroy$ = new Subject<void>();
  details = goToDetails;

  constructor(
    protected router: Router,
    private sharedService: SharedService,
    private route: ActivatedRoute,
    private ppFixPricePipe: ppFixPricePipe,
    private userChatService: UserChatService,
    private messengerService: MessengerService,
    private accountService: AccountService,
  ) { }

  ngOnInit(): void {
    this.messengerService.getCanConnect().pipe(
      filter((res) => !!res),
      tap(() => this.messengerService.leaveChatTab()),
      tap(() => this.messengerService.restoreAllConversations()),
      takeUntil(this.onDestroy$),
    ).subscribe();

    this.route.queryParams.pipe(
      map(({ id }) => id as string),
      filter((id) => !!id),
      tap((offerId) => this.offerId = offerId),
      switchMap((offerId) => this.accountService.createNewChat(offerId)),
      tap((chatId) => this.chatId = chatId),
      tap((chatId) => this.messengerService.restoreMessages(chatId)),
      switchMap(() => this.sharedService.getOffer(this.offerId)),
      map((res) => {
        return {
          id: this.chatId,
          offerId: res.id,
          offerTitle: res.title,
          offerImageUrl: res.picturesNames ? res.picturesNames[0] : '',
          offerPrice: this.ppFixPricePipe.transform(res.price),
          withUser: res.user.firstName + ' ' + res.user.lastName,
          date: new Date(),
          content: 'Nowa wiadomość',
          isRead: true,
        };
      }),
      tap((res) => this.userChatService.setCurrentChat(res)),
      takeUntil(this.onDestroy$),
    ).subscribe();

    this.currentChat$ = this.userChatService.getCurrentChat().pipe(
      filter((res) => !!res),
    );
  }

  ngOnDestroy(): void {
    this.messengerService.leaveChatTab();
    this.userChatService.setCurrentChat(null);
    this.userChatService.setMessageStream(null);
    this.userChatService.setPreviousChatsStream(null);

    this.onDestroy$.next();
    this.onDestroy$.complete();
  }
}
