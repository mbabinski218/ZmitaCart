import { Injectable } from '@angular/core';
import { UserService } from '@core/services/authorization/user.service';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { RxStomp, RxStompConfig } from '@stomp/rx-stomp';
import { IFrame, StompHeaders } from '@stomp/stompjs';
import { BehaviorSubject, Observable, Subject, Subscription, takeUntil } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MessengerService {

  constructor(private userService: UserService) {}

  rxStomp: RxStomp;
  onDestroy$: Subject<void>;
  isConnected$ = new BehaviorSubject<boolean>(false);
  connectionStatus$ = new BehaviorSubject<number>(0);
  subscription: Subscription;

  hubConnection: signalR.HubConnection;

  startConnection() {
    this.hubConnection = new HubConnectionBuilder().withUrl('http://localhost:5102/ChatHub').build();

    this.hubConnection.start()
      .then(() => {
        const userId = this.userService.userAuthorization().id;
        const chatId = 2;
        const offerId = 40;

        // Do nowego
        //this.hubConnection.invoke("Create", offerId, userId);

        // Do istniejacego
        this.hubConnection.invoke("Join", chatId, userId);
      });
  }

  sendMessage(message: string) {
    const authorName = this.userService.userAuthorization().firstName;
    const authorId = this.userService.userAuthorization().id;
    const chatId = 2;

    this.hubConnection.invoke("SendMessage", chatId, authorId, authorName, message);
  }

  // this.hubConnection.on("ReceiveMessage", (AuthorId : number, AuthorName : string, Date : date, Content : string)


  // connect(): void {
  //   this.rxStomp = new RxStomp();
  //   this.onDestroy$ = new Subject<void>();

  //   const connection = new HubConnectionBuilder()
  //     .withUrl('http://localhost:5102/ChatHub')
  //     .build();

  //   const rxStompConfig: RxStompConfig = {
  //     // heartbeatIncoming: 2000,
  //     // heartbeatOutgoing: 2000,
  //     // reconnectDelay: 2000,
  //     webSocketFactory: () => connection.stream,
  //     beforeConnect: () => {
  //       return new Promise<void>((resolve) => {
  //         connection.start().then(() => {
  //           resolve();
  //         });
  //       });
  //     },
  //   };

  //   this.rxStomp.configure(rxStompConfig);
  //   this.rxStomp.activate();
  // }

}
