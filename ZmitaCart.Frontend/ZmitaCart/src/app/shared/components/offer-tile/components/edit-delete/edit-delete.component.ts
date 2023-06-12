import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OfferItem } from '@components/account/interfaces/account.interface';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AreYouSureDialogComponent } from '../../../are-you-sure-dialog/are-you-sure-dialog.component';
import { NoopScrollStrategy } from '@angular/cdk/overlay';
import { RoutingService } from '@shared/services/routing.service';
import { RoutesPath } from '@core/enums/routes-path.enum';

@Component({
  selector: 'pp-edit-delete',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule, MatDialogModule],
  templateUrl: './edit-delete.component.html',
  styleUrls: ['./edit-delete.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class EditDeleteComponent {
  @Input() item: OfferItem;

  @Output() deleteOffer = new EventEmitter<boolean>();

  constructor(
    private dialog: MatDialog,
    private routingService: RoutingService,
  ) { }

  openDialog(): void {
    const dialogRef = this.dialog.open(AreYouSureDialogComponent, {
      disableClose: true,
      scrollStrategy: new NoopScrollStrategy(),
    });

    dialogRef.afterClosed().subscribe((res) => {
      if (res === 'yes')
        this.deleteOffer.emit(true);
    });
  }

  goToEdit(): void {
    this.routingService.navigateTo(`${RoutesPath.HOME}/${RoutesPath.ADD_OFFER}`, null, { paramName: 'id', value: this.item.id });
  }
}
