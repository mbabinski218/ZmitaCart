import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TokenData } from '@components/authentication/interfaces/authentication-interface';
import { MatMenuModule } from '@angular/material/menu';
import { RouterModule } from '@angular/router';
import { RoutesPath } from '@core/enums/routes-path.enum';
import { MatIconModule } from '@angular/material/icon';
import { ClickOutsideDirective } from '@shared/directives/click-outside.directive';
import { UserService } from '@core/services/authorization/user.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';
import { Api } from '@core/enums/api.enum';
import { isEmpty } from 'lodash';

@Component({
  selector: 'pp-login-menu',
  standalone: true,
  imports: [CommonModule, MatMenuModule, RouterModule, MatIconModule, ClickOutsideDirective],
  templateUrl: './login-menu.component.html',
  styleUrls: ['./login-menu.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoginMenuComponent implements OnInit {
  readonly LOGIN_LINK = `/${RoutesPath.AUTHENTICATION}/${RoutesPath.LOGIN}`;

  opened = false;
  userData: TokenData;
  emptyData: boolean;

  constructor(
    private userService: UserService,
    private http: HttpClient,
  ) { }

  ngOnInit(): void {
    this.userData = this.userService.userAuthorization();
    this.emptyData = isEmpty(this.userData);
  }

  logout(): void {
    this.userService.logout();
    //api do wylogowywania
    window.location.reload();
  }

  logoutTest() {
    this.http.post(`${environment.httpBackend}${Api.LOGOUT}`, {}).subscribe((res) => console.log(res));
  }
}
