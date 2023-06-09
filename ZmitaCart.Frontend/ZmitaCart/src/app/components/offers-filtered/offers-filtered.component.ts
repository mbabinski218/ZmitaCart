import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'pp-offers-filtered',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './offers-filtered.component.html',
  styleUrls: ['./offers-filtered.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OffersFilteredComponent {

}


//po kliknieciu w kategorie, albo po kliknieciu w wyszukiwanie robie routa 
//do tego komponentu i w paramsach dodaje kategorie albo wyszukanie