import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { MessengerService } from '@components/account/components/user-chat/components/messenger/services/messenger.service';
import { UserService } from '@core/services/authorization/user.service';

@Component({
  selector: 'pp-app-root',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './app.component.html',
})
export class AppComponent implements OnInit {

  constructor(
    iconRegistry: MatIconRegistry,
    sanitizer: DomSanitizer,
    private messengerService: MessengerService,
    private userService: UserService,
  ) {
    customIcons.forEach(([iconName, icon]) => {
      iconRegistry.addSvgIcon(
        iconName,
        sanitizer.bypassSecurityTrustResourceUrl(`http://localhost:4200/assets/images/${icon}`),
      );
    });
  }

  ngOnInit(): void {
    if (this.userService.isAuthenticated())
      this.messengerService.buildConnection();
  }
}

const customIcons: [string, string][] = [
  ["allegro", "allegro.svg"],
  ["add", "add.svg"],
  ["arrow", "arrow.svg"],
  ["chat", "chat.svg"],
  ["heart", "heart.svg"],
  ["heart-filled", "heart-filled.svg"],
  ["user", "user.svg"],
  ["menu", "menu.svg"],
  ["find", "find.svg"],
  ["my-camera", "camera-add.svg"],
  ["logout", "logout.svg"],
  ["user-basic", "user-basic.svg"],
];
