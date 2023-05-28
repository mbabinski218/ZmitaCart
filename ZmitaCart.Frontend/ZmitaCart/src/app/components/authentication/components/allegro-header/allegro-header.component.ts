import { ChangeDetectionStrategy, Component, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'pp-allegro-header',
  standalone: true,
  imports: [CommonModule, MatSnackBarModule, RouterModule],
  templateUrl: './allegro-header.component.html',
  styleUrls: ['./allegro-header.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AllegroHeaderComponent {

  constructor(
    private _snackBar: MatSnackBar,
  ) { }

  showPrank(): void {
    this._snackBar.open("xdddd nie", '', {
      duration: 4000,
      panelClass: ['prank-snackbar']
    });
  }
}
