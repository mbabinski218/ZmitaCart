import { ChangeDetectionStrategy, Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { tap } from 'rxjs';
import { isEqual } from 'lodash';
import { Prices } from '../../interfaces/offers-view.interface';

@Component({
  selector: 'pp-price-input',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './price-input.component.html',
  styleUrls: ['./price-input.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PriceInputComponent implements OnInit {

  @Output() pricesEmitter = new EventEmitter<Prices>();
  @Output() hasErrors = new EventEmitter<boolean>();

  prices: Prices = {
    minPrice: null,
    maxPrice: null,
  };

  form = new FormGroup({
    minPrice: new FormControl(null),
    maxPrice: new FormControl(null),
  });

  ngOnInit(): void {
    this.form.valueChanges.pipe(
      tap((res) => {
        if (res.maxPrice && res.minPrice) {
          if (res.maxPrice < res.minPrice)
            this.form.setErrors({ 'incorrect': true });
        } else {
          this.form.setErrors(null);
        }
      }),
      tap(() => this.emitValue(this.form)),
    ).subscribe();
  }

  private emitValue(form: FormGroup): void {
    const formPrices = form.value as Prices;

    if (!isEqual(form.value, this.prices)) {
      this.prices = formPrices;
      this.pricesEmitter.emit(formPrices);
      this.hasErrors.emit(!!this.form.errors);
    }
  }

}
