import { Injectable } from '@angular/core';
import { UserService } from '@core/services/authorization/user.service';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { Observable, BehaviorSubject } from 'rxjs';
import { UserChatService } from '@components/account/components/user-chat/services/user-chat.service';

@Injectable({
  providedIn: 'root'
})
export class MessengerService {

  private hubConnection: signalR.HubConnection;
  private authorId: string;
  private authorName: string;
  private receiverSet = false;

  private canConnect$ = new BehaviorSubject<boolean>(false);

  constructor(
    private userService: UserService,
    private userChatService: UserChatService,
  ) { }

  buildConnection(): void {
    this.hubConnection = new HubConnectionBuilder().configureLogging(LogLevel.None).withUrl('http://localhost:5102/ChatHub').build();

    this.authorId = this.userService.userAuthorization().id;
    this.authorName = this.userService.userAuthorization().firstName;

    this.startConnection();
  }

  //Wyjście z zakładki czatu
  leaveChatTab() {
    this.hubConnection.invoke("LeaveChat", this.authorId)
      .catch((err) => console.log(err));
  }

  //Wysyłanie wiadomości
  sendMessage(message: string, chatId: number) {
    this.hubConnection.invoke("SendMessage", chatId, this.authorId, this.authorName, message)
      .catch((err) => console.log(err));
  }

  //Historia chatów
  restoreAllConversations() {
    this.hubConnection.invoke("RestoreAllConversations", this.authorId)
      .catch((err) => console.log(err));
  }

  //Przywrócenie wszystkich wiadomości z danego chatu
  restoreMessages(chatId: number) {
    this.hubConnection.invoke("RestoreMessages", chatId, this.authorId)
      .catch((err) => console.log(err));
  }

  getCanConnect(): Observable<boolean> {
    return this.canConnect$.asObservable();
  }

  private startConnection() {
    this.hubConnection.start()
      .then(() => {
        this.hubConnection.invoke("Join", this.authorId).catch((err) => console.log(err));
        this.canConnect$.next(true);
      })
      .catch((err) => console.log(err));

    if (!this.receiverSet)
      this.setReceiver();
  }

  private setReceiver(): void {
    this.receiverSet = true;

    //Otrzymanie wiadomości
    this.hubConnection.on("ReceiveMessage", (chatId: number, authorId: number, authorName: string, date: Date, content: string) => {
      this.userChatService.setMessageStream({ chatId, authorId, authorName, date, content, fromCurrentUser: authorId === Number(this.authorId) });
    });


    //Przywrócenie wszystkich konwersacji pojedynczo
    this.hubConnection.on("ReceiveConversation", (id: number, offerId: number, offerTitle: string, offerPrice: number,
      offerImageUrl: string, withUser: string, date: Date, content: string, isRead: boolean) => {
      this.userChatService.setPreviousChatsStream({ id, offerId, offerTitle, offerPrice: String(offerPrice), offerImageUrl, withUser, date, content, isRead });
    });
    
    
    //Ilość wiadomości niewyświetlonych
    this.hubConnection.on("ReceiveNotificationStatus", (status: number) => {
      this.userChatService.setNotifications(status);
    });
  }
}
