import { MessengerService } from '@components/account/components/user-chat/components/messenger/services/messenger.service';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountService } from '@components/account/api/account.service';
import { OfferSingleService } from '@components/offer-single/api/offer-single.service';
import { BehaviorSubject, Observable, filter, map, switchMap, tap } from 'rxjs';
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
export class UserChatComponent implements OnInit {

  currentChat$: Observable<ChatsStream>;
  newLastMessage$ = new BehaviorSubject<MessageStream>(null);
  offerId: string;

  constructor(
    private offerSingleService: OfferSingleService,
    private route: ActivatedRoute,
    private ppFixPricePipe: ppFixPricePipe,
    private router: Router,
    private userChatService: UserChatService,
    private messengerService: MessengerService,
  ) { }

  ngOnInit(): void {
    this.messengerService.getCanConnect().pipe(
      filter((res) => !!res),
      tap(() => this.messengerService.restoreAllConversations())
    ).subscribe();

    this.route.queryParams.pipe(
      map(({ id }) => id as string),
      filter((id) => !!id),
      tap((res) => this.offerId = res),
      switchMap((id) => this.offerSingleService.getOffer(id)),
      map((res) => {
        return {
          id: -10,
          offerId: res.id,
          offerTitle: res.title,
          offerImageUrl: res.picturesNames ? res.picturesNames[0] : '',
          offerPrice: this.ppFixPricePipe.transform(res.price),
          withUser: res.user.firstName + ' ' + res.user.lastName,
        };
      }),
      tap((res) => this.userChatService.setCurrentChat(res))
    ).subscribe();

    this.currentChat$ = this.userChatService.getCurrentChat().pipe(
      filter((res) => !!res),
    );
  }

  details(id: number): void {
    void this.router.navigateByUrl(`${RoutesPath.HOME}/${RoutesPath.OFFER}/${id}`);
  }
}
