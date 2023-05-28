import { ChangeDetectionStrategy, Component, HostListener, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { RoutesPath } from '@core/enums/routes-path.enum';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatMenuModule } from '@angular/material/menu';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'pp-header',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule, RouterModule, MatSelectModule, MatFormFieldModule, MatMenuModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HeaderComponent {

  readonly LOGIN_LINK = `/${RoutesPath.AUTHENTICATION}/${RoutesPath.LOGIN}`;
  readonly ADD_OFFER_LINK = `/${RoutesPath.ADD_OFFER}`;

  @HostListener('window:resize')
  onResize(): void {
    this.isBig = window.innerWidth >= 768;
  }

  isBig = window.innerWidth >= 768;
}
