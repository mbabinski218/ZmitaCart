import { ChangeDetectionStrategy, Component, HostListener, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { RoutesPath } from '@core/enums/routes-path.enum';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CategoriesMenuComponent } from '@components/home/components/header/components/categories-menu/categories-menu.component';
import { LoginMenuComponent } from '@components/home/components/header/components/login-menu/login-menu.component';
import { OverlayService } from '@core/services/overlay/overlay.service';
import { Observable, shareReplay } from 'rxjs';
import { SuperiorCategories } from '@components/home/components/header/interfaces/header.interface';
import { HeaderService } from '@components/home/components/api/header.service';
import { RoutingService } from '@shared/services/routing.service';

@Component({
  selector: 'pp-header',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule, RouterModule, MatSelectModule, MatFormFieldModule, CategoriesMenuComponent, LoginMenuComponent],
  providers: [HeaderService],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HeaderComponent implements OnInit {

  @HostListener('window:resize')
  onResize(): void {
    this.isBig = window.innerWidth > 768;
  }

  readonly RoutesPath = RoutesPath;

  isBig = window.innerWidth > 768;

  isShowCategories$: Observable<boolean>;
  superiorCategories$: Observable<SuperiorCategories[]>;

  constructor(
    protected overlayService: OverlayService,
    private routingService: RoutingService,
    private headerService: HeaderService,
    private router: Router,
  ) { }

  ngOnInit(): void {
    this.isShowCategories$ = this.overlayService.getState();
    this.superiorCategories$ = this.headerService.getSuperiorCategories().pipe(shareReplay());
  }

  navigateTo(fragment?: string): void {
    this.routingService.navigateTo(`${RoutesPath.HOME}/${RoutesPath.ACCOUNT}`, fragment);
  }

  goToAddOffer(): void {
    void this.router.navigate([`${RoutesPath.HOME}/${RoutesPath.ADD_OFFER}`]).then(() => window.location.reload());
  }
}
