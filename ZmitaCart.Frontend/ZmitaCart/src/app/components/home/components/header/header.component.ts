import { ChangeDetectionStrategy, Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router, ActivatedRoute } from '@angular/router';
import { RoutesPath } from '@core/enums/routes-path.enum';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { CategoriesMenuComponent } from '@components/home/components/header/components/categories-menu/categories-menu.component';
import { LoginMenuComponent } from '@components/home/components/header/components/login-menu/login-menu.component';
import { OverlayService } from '@core/services/overlay/overlay.service';
import { Observable, Subject, map, shareReplay, takeUntil, tap } from 'rxjs';
import { SuperiorCategories } from '@components/home/components/header/interfaces/header.interface';
import { HeaderService } from '@components/home/components/api/header.service';
import { RoutingService } from '@shared/services/routing.service';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatBadgeModule } from '@angular/material/badge';
import { SharedService } from '@shared/services/shared.service';
import { UserChatService } from '@components/account/components/user-chat/services/user-chat.service';

@Component({
  selector: 'pp-header',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule, MatButtonModule, MatIconModule, MatBadgeModule,
    RouterModule, MatSelectModule, CategoriesMenuComponent, LoginMenuComponent],
  providers: [HeaderService],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HeaderComponent implements OnInit, OnDestroy {

  @HostListener('window:resize')
  onResize(): void {
    this.isBig = window.innerWidth > 768;
  }

  private onDestroy$ = new Subject<void>();

  form = new FormGroup({
    input: new FormControl(null as string),
    category: new FormControl('-1'),
  });

  readonly RoutesPath = RoutesPath;

  isBig = window.innerWidth > 768;
  likedCount$: Observable<number>;
  unreadChatsCount$: Observable<number>;

  isShowCategories$: Observable<boolean>;
  superiorCategories$: Observable<SuperiorCategories[]>;
  superiorCategoriesContainer: SuperiorCategories[];

  constructor(
    protected overlayService: OverlayService,
    private routingService: RoutingService,
    private headerService: HeaderService,
    private router: Router,
    private route: ActivatedRoute,
    private sharedService: SharedService,
    private userChatService: UserChatService,
  ) { }

  ngOnInit(): void {
    this.isShowCategories$ = this.overlayService.getState();
    this.superiorCategories$ = this.headerService.getSuperiorCategories().pipe(
      tap((res) => this.superiorCategoriesContainer = res),
      shareReplay()
    );

    this.unreadChatsCount$ = this.userChatService.getNotifications().pipe(
      takeUntil(this.onDestroy$),
    );

    this.likedCount$ = this.sharedService.getFavouritesCount();

    this.route.queryParams.pipe(
      map(({ c, i }) => ({ c, i })),
      tap((res) => {
        const input = res.i as string;
        const category = res.c as string;

        this.form.patchValue({ input, category: category || '-1' });
      }),
      takeUntil(this.onDestroy$),
    ).subscribe();
  }

  ngOnDestroy(): void {
    this.onDestroy$.next();
    this.onDestroy$.complete();
  }

  navigateTo(fragment?: string): void {
    this.routingService.navigateTo(`${RoutesPath.HOME}/${RoutesPath.ACCOUNT}`, fragment);
  }

  goToAddOffer(): void {
    void this.router.navigate([`${RoutesPath.HOME}/${RoutesPath.ADD_OFFER}`]).then(() => window.location.reload());
  }

  find(): void {
    const category = (this.form.value.category === '-1') ? null : this.form.value.category;
    const input = this.form.value.input || null;

    void this.router.navigate([`${RoutesPath.HOME}/${RoutesPath.OFFERS_FILTERED}`], {
      queryParams: { c: category, i: input },
    });
  }
}
