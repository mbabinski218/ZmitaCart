import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Chats, SingleChat } from '@components/account/interfaces/account.interface';
import { Observable } from 'rxjs';
import { AccountService } from '@components/account/api/account.service';

@Component({
  selector: 'pp-chats',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './chats.component.html',
  styleUrls: ['./chats.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ChatsComponent implements OnInit {

  @Input() currentChat: SingleChat;
  
  pageIndex = 1;

  previousChats$: Observable<Chats>;

  constructor(
    private accountService: AccountService,
  ) { }

  ngOnInit(): void {
    this.previousChats$ = this.accountService.getAllChats(this.pageIndex);
  }
}
