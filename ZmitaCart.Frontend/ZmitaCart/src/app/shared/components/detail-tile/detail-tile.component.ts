import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { DetailDataType } from '@shared/components/detail-tile/types/detailDataType';

@Component({
  selector: 'pp-detail-tile',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './detail-tile.component.html',
  styleUrls: ['./detail-tile.component.scss'],
})
export class DetailTileComponent {

  @Input() data: DetailDataType;
  @Input() propertyName: string;
  @Input() icon: string;
  @Input() type: string;
}
