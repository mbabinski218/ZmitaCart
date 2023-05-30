import { ChangeDetectionStrategy, Component, HostListener, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { RoutesPath } from '@core/enums/routes-path.enum';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CategoriesMenuComponent } from '@components/home/components/header/components/categories-menu/categories-menu.component';
import { LoginMenuComponent } from '@components/home/components/header/components/login-menu/login-menu.component';
import { OverlayService } from '@core/services/overlay/overlay.service';
import { Observable } from 'rxjs';
import { UserService } from '@core/services/authorization/user.service';

@Component({
  selector: 'pp-header',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule, RouterModule, MatSelectModule, MatFormFieldModule, CategoriesMenuComponent, LoginMenuComponent],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HeaderComponent implements OnInit {

  @HostListener('window:resize')
  onResize(): void {
    this.isBig = window.innerWidth > 768;
  }

  readonly ADD_OFFER_LINK = `/${RoutesPath.ADD_OFFER}`;

  isBig = window.innerWidth > 768;

  isShowCategories$: Observable<boolean>;

  constructor(
    protected overlayService: OverlayService,
    protected userService: UserService,
  ) { }

  ngOnInit(): void {
    this.isShowCategories$ = this.overlayService.getState();
  }

}
