import { ChangeDetectionStrategy, Component, HostListener, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { RoutesPath } from '@core/enums/routes-path.enum';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'pp-header',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule, RouterModule, MatSelectModule, MatFormFieldModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HeaderComponent {

  @HostListener('window:resize')
  onResize(): void {
    this.isBig = window.innerWidth >= 768;
  }

  readonly RoutesPath = RoutesPath;
  isBig = window.innerWidth >= 768;

  constructor(
    private router: Router,
  ) { }

  goToAddOffer(): void {
    void this.router.navigateByUrl(`${RoutesPath.ADD_OFFER}`);
  }

  goToLogin(): void {
    void this.router.navigateByUrl(`${RoutesPath.ADD_OFFER}`);
  }

}
