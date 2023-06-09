import { ChangeDetectionStrategy, Component, ElementRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MessengerService } from './services/messenger.service';

@Component({
  selector: 'pp-messenger',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './messenger.component.html',
  styleUrls: ['./messenger.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MessengerComponent {
  @ViewChild('myTextarea', { static: false }) myTextarea: ElementRef<HTMLTextAreaElement>;

  constructor(private messengerService: MessengerService) {}

  start() {
    this.messengerService.startConnection()
    // this.messengerService.connect()
  }
  
  getValueFromTextarea() {
    const textareaValue = this.myTextarea.nativeElement.value;
    this.messengerService.sendMessage(textareaValue);
  }
}
