import { ChangeDetectionStrategy, Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, tap } from 'rxjs';
import { isEqual } from 'lodash';
import { SortBy } from '../../interfaces/offers-view.interface';

@Component({
  selector: 'pp-sort-selector',
  standalone: true,
  imports: [CommonModule, MatIconModule, ReactiveFormsModule],
  templateUrl: './sort-selector.component.html',
  styleUrls: ['./sort-selector.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SortSelectorComponent implements OnInit {

  @Output() sortByEmitter = new EventEmitter<SortBy>();

  sortBy: SortBy = 'CreatedAscending';

  form = new FormGroup({
    sortBy: new FormControl('CreatedAscending' as SortBy),
  });

  ngOnInit(): void {
    this.form.valueChanges.pipe(
      debounceTime(300),
      tap(() => this.emitValue(this.form)),
    ).subscribe();
  }

  private emitValue(form: FormGroup): void {
    const sortBy = form.value as SortBy;
    
    if (!isEqual(sortBy, this.sortBy)) {
      this.sortBy = sortBy;
      this.sortByEmitter.emit(sortBy);
    }
  }

}
