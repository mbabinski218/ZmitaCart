import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';

@Component({
    selector: 'pp-app-root',
    standalone: true,
    imports: [CommonModule, RouterModule],
    templateUrl: './app.component.html',
})
export class AppComponent {
    constructor(iconRegistry: MatIconRegistry, sanitizer: DomSanitizer) {
        customIcons.forEach(([iconName, icon]) => {
            iconRegistry.addSvgIcon(
                iconName,
                sanitizer.bypassSecurityTrustResourceUrl(`http://localhost:4200/assets/images/${icon}`),
            );
        });
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
    ["logout", "logout.svg"],
    ["user-basic", "user-basic.svg"],
];
