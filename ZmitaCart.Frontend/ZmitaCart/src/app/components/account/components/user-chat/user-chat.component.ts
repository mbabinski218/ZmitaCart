import { MessengerService } from '@components/account/components/user-chat/services/messenger.service';
import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountService } from '@components/account/api/account.service';
import { OfferSingleService } from '@components/offer-single/api/offer-single.service';
import { BehaviorSubject, Observable, Subject, filter, map, switchMap, takeUntil, tap } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { ppFixPricePipe } from '@shared/pipes/fix-price.pipe';
import { RoutesPath } from '@core/enums/routes-path.enum';
import { ChatsComponent } from './components/chats/chats.component';
import { MessengerComponent } from './components/messenger/messenger.component';
import { OfferComponent } from './components/offer/offer.component';
import { ChatsStream, MessageStream } from '@components/account/components/user-chat/interfaces/chat.interfaces';
import { UserChatService } from './services/user-chat.service';

@Component({
  selector: 'pp-user-chat',
  standalone: true,
  imports: [CommonModule, ChatsComponent, MessengerComponent, OfferComponent],
  providers: [AccountService, OfferSingleService, ppFixPricePipe],
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

  constructor(
    private offerSingleService: OfferSingleService,
    private route: ActivatedRoute,
    private ppFixPricePipe: ppFixPricePipe,
    private router: Router,
    private userChatService: UserChatService,
    private messengerService: MessengerService,
    private accountService: AccountService,
  ) { }

  ngOnInit(): void {
    this.messengerService.getCanConnect().pipe(
      filter((res) => !!res),
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
      switchMap(() => this.offerSingleService.getOffer(this.offerId)),
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
          // isNewChat: true,
        };
      }),
      // tap((res) => console.log(res)),
      tap((res) => this.userChatService.setCurrentChat(res)),
      takeUntil(this.onDestroy$),
    ).subscribe();

    this.currentChat$ = this.userChatService.getCurrentChat().pipe(
      filter((res) => !!res),
    );
  }

  ngOnDestroy(): void {
    this.userChatService.setCurrentChat(null);
    this.userChatService.setMessageStream(null);
    this.userChatService.setPreviousChatsStream(null);

    this.onDestroy$.next();
    this.onDestroy$.complete();
  }

  details(id: number): void {
    void this.router.navigateByUrl(`${RoutesPath.HOME}/${RoutesPath.OFFER}/${id}`);
  }
}
