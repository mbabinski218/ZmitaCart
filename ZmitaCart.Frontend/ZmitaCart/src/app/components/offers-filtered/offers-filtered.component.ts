import { OffersFilteredService } from './api/offers-filtered.service';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { map, switchMap, tap } from 'rxjs';
import { OffersViewComponent } from '@shared/components/offers-view/offers-view.component';

@Component({
  selector: 'pp-offers-filtered',
  standalone: true,
  imports: [CommonModule, OffersViewComponent],
  templateUrl: './offers-filtered.component.html',
  styleUrls: ['./offers-filtered.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OffersFilteredComponent {

}
