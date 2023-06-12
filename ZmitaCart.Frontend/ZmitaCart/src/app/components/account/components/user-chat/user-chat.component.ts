import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountService } from '@components/account/api/account.service';
import { OfferSingleService } from '@components/offer-single/api/offer-single.service';
import { BehaviorSubject, Observable, filter, map, of, shareReplay, switchMap, tap } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { SingleChat } from '@components/account/interfaces/account.interface';
import { ppFixPricePipe } from '@shared/pipes/fix-price.pipe';
import { RoutesPath } from '@core/enums/routes-path.enum';
import { ChatsComponent } from './components/chats/chats.component';
import { MessengerComponent } from './components/messenger/messenger.component';
import { OfferComponent } from './components/offer/offer.component';
import { MessageStream } from '@components/account/components/user-chat/interfaces/chat.interfaces';

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

  currentChat$: Observable<SingleChat>;
  newLastMessage$ = new BehaviorSubject<MessageStream>(null);
  offerId: string;

  constructor(
    private offerSingleService: OfferSingleService,
    private route: ActivatedRoute,
    private ppFixPricePipe: ppFixPricePipe,
    private router: Router,
  ) { }

  ngOnInit(): void {
    this.currentChat$ = this.route.queryParams.pipe(
      map(({ id }) => id as string),
      filter((id) => !!id),
      tap((res) => this.offerId = res),
      switchMap((id) => this.offerSingleService.getOffer(id)),
      shareReplay(),
      map((res) => {
        return {
          withUser: res.user.firstName + ' ' + res.user.lastName,
          offerId: res.id,
          offerTitle: res.title,
          offerImageUrl: res.picturesNames ? res.picturesNames[0] : '',
          offerPrice: this.ppFixPricePipe.transform(res.price),
          forcedToHistory: true,
        };
      }),
    );
  }

  details(id: number): void {
    void this.router.navigateByUrl(`${RoutesPath.HOME}/${RoutesPath.OFFER}/${id}`);
  }

  newCurrentChat(currentChat: SingleChat): void {
    this.currentChat$ = of(currentChat);
  }

  newMessageEmitter(message: MessageStream): void {
    this.newLastMessage$.next(message);
  }
}
