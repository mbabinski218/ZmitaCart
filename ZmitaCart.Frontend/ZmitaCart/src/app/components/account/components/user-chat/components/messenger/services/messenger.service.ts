import { Injectable } from '@angular/core';
import { UserService } from '@core/services/authorization/user.service';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { MessageStream } from '@components/account/components/user-chat/interfaces/chat.interfaces';

@Injectable()
export class MessengerService {

  private hubConnection: signalR.HubConnection;
  private chatId: number;
  private authorId: string;
  private authorName: string;
  private receiverSet = false;

  private messageStream$ = new Subject<MessageStream>;

  constructor(private userService: UserService) { }

  buildConnection(chatId: number): void {
    if (!this.hubConnection) {
      this.authorId = this.userService.userAuthorization().id;
      this.authorName = this.userService.userAuthorization().firstName;
      this.hubConnection = new HubConnectionBuilder().configureLogging(LogLevel.None).withUrl('http://localhost:5102/ChatHub').build();
    }

    this.hubConnection.stop()
      .finally(() => {
        this.messageStream$.next(null);
        this.startConnection(chatId);
      });
  }

  sendMessage(message: string) {
    this.hubConnection.invoke("SendMessage", this.chatId, this.authorId, this.authorName, message)
      .catch((err) => console.log(err));
  }

  getMessageStream(): Observable<MessageStream> {
    return this.messageStream$.asObservable();
  }

  private startConnection(chatId: number) {
    this.chatId = chatId;

    this.hubConnection.start()
      .then(() => {
        this.hubConnection.invoke("Join", this.chatId, this.authorId)
          .catch((err) => console.log(err));
      })
      .catch((err) => console.log(err));

    if (!this.receiverSet)
      this.setReceiver();
  }

  private setReceiver(): void {
    this.receiverSet = true;
    this.hubConnection.on("ReceiveMessage", (authorId: number, authorName: string, date: Date, content: string) => {
      this.messageStream$.next({ authorId, authorName, date, content, fromCurrentUser: authorId === Number(this.authorId) });
  });
}
}