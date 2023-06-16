import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component, ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
  ViewEncapsulation
} from '@angular/core';
import {CommonModule} from '@angular/common';
import {MatIconModule} from "@angular/material/icon";
import {MatButtonModule} from "@angular/material/button";
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {MatInputModule} from "@angular/material/input";
import {CategoryService} from "@components/add-offer/api/category.service";
import {Category} from "@components/add-offer/interfaces/Category";
import {Condition} from "@core/enums/condition.enum";
import {ConditionWrapperComponent} from "@components/add-offer/condition-wrapper/condition-wrapper.component";
import {ConditionType} from "@components/add-offer/interfaces/ConditionType";
import {CategorySelectorComponent} from "@components/add-offer/category-selector/category-selector.component";
import {OfferService} from "@components/add-offer/api/offer.service";
import {ActivatedRoute, RouterLink} from "@angular/router";
import {RoutingService} from "@shared/services/routing.service";
import {RoutesPath} from "@core/enums/routes-path.enum";
import {Subject, filter, map, switchMap, takeUntil, tap, BehaviorSubject} from 'rxjs';
import {OfferSingleService} from '@components/offer-single/api/offer-single.service';
import {MatSlideToggleModule} from "@angular/material/slide-toggle";


@Component({
  selector: 'pp-add-offer',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule, ReactiveFormsModule, MatInputModule, ConditionWrapperComponent, CategorySelectorComponent, RouterLink, MatSlideToggleModule],
  templateUrl: './add-offer.component.html',
  styleUrls: ['./add-offer.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddOfferComponent implements OnInit, OnDestroy {

  private onDestroy$ = new Subject<void>();

  createOffer: FormGroup;
  characterCount: number;
  // price: number;
  quantity: number;
  condition = new BehaviorSubject<Condition>(null);
  subCategories: Category[] = null;
  children: Category[];
  pickedCategory: Category = null;
  headCategory: Category;
  parentCategory?: Category;
  isInSubCategories: boolean;
  selectedImages: File[] = [];
  previews = new BehaviorSubject<string[]>(null);
  isDragOver: boolean;
  inEdit = false;
  offerId: number;
  isAvailable: boolean;

  @ViewChild('dropZone', {static: true}) dropZone!: ElementRef;

  items: ConditionType[] = [{
    title: "Używany",
    description: "Widoczne ślady używania lub uszkodzenia, które zostały uwzględnione w opisie tego przedmiotu i/lub na zdjęciach.",
    condition: Condition.Używany
  }, {
    title: "Bardzo dobry",
    description: "W bardzo dobrym stanie technicznym i wizualnym. Nie nosi śladów używania lub są one znikome.",
    condition: Condition.Dobry,
  }, {
    title: "Nowy",
    description: "Fabrycznie nowy, nieużywany, nieuszkodzony, zapakowany w oryginalne opakowanie od producenta.",
    condition: Condition.Nowy
  }];

  constructor(
    private categoryService: CategoryService,
    private cdr: ChangeDetectorRef,
    private offerService: OfferService,
    private routerService: RoutingService,
    private route: ActivatedRoute,
  ) {
  }

  readonly imageUrl = 'http://localhost:5102/File?name=';

  ngOnInit(): void {
    this.createOffer = new FormGroup({
      title: new FormControl(null as string, [Validators.required, Validators.minLength(3), Validators.maxLength(50)]),
      description: new FormControl(null as string, [Validators.required]),
      price: new FormControl(null as number, [Validators.required, Validators.pattern("^\\d+(?:\\,\\d{1,2})?$")]),
      quantity: new FormControl(1, [Validators.pattern("^(?=.*[1-9])\\d+$")]),
      availability: new FormControl()
    });

    this.route.queryParams.pipe(
      map(({id}) => id as string),
      filter((id) => !!id),
      tap(() => this.inEdit = true),
      tap(id => this.offerId = id as unknown as number),
      switchMap((id) => this.offerService.goToEditOffer(id)),
      tap((res) => this.createOffer.patchValue(res)),
      tap((res) => this.pickedCategory = {id: res.categoryId, name: ''}),
      tap((res) => this.createOffer.get('price').setValue(res.price.toString().replace('.', ','))),
      tap((res) => this.createOffer.get('availability').setValue(res.isAvailable)),
      tap((res) => this.condition.next(Object.values(Condition).indexOf(res.condition))),
      tap(res => this.previews.next(res.picturesNames?.map((name: string) => name = this.imageUrl + name))),
      switchMap(res => this.offerService.restoreOfferImages(res?.picturesNames as string[])),
      tap(res => this.selectedImages = res as File[]),
    ).subscribe();

    this.createOffer.get('title').valueChanges.subscribe((res: string) => {
      this.characterCount = res.length;
    });

    this.createOffer.get('availability').valueChanges.subscribe((res: boolean) => {
      this.isAvailable = res;
    });

    // this.createOffer.get('price').valueChanges.subscribe((res: string) => {
    //   this.createOffer.get('price').setValue(res.replace('.', ','));
    // });

    const quantityControl = this.createOffer.get('quantity');
    quantityControl.valueChanges.pipe(
      takeUntil(this.onDestroy$),
    ).subscribe((res: string) => {
      if (res.length === 0) {
        quantityControl.setValue(1);
      }
      if (!quantityControl.valid) {
        const val = res.replace(/\D/g, '');
        quantityControl.setValue(val);
      }
    });
  }

  ngOnDestroy(): void {
    this.onDestroy$.next();
    this.onDestroy$.complete();
  }

  loadSuperiorCategories() {
    this.categoryService.getSuperiorsWithChildren(1).pipe(
      takeUntil(this.onDestroy$),
    )
      .subscribe(res => {
        this.subCategories = res;
        this.children = this.subCategories;
        this.cdr.detectChanges();
      });
  }

  public setCondition(event: Condition) {
    this.condition.next(event);
  }

  public getParentCategory(parentId: number) {
    //patrzymy na kategorie nadrzędne
    if (parentId === null) {
      this.parentCategory = null;
      return;
    }

    return this.categoryService.getParentCategory(parentId).pipe(
      takeUntil(this.onDestroy$),
    ).subscribe(
      res => {
        this.parentCategory = res;
      }
    );
  }

  goForward(event: Category) {
    this.isInSubCategories = true;
    if (event.parentId !== null && event.children)
      this.getParentCategory(event.parentId);

    this.categoryService.getFewBySuperiorId(event.id, 2).pipe(
      takeUntil(this.onDestroy$),
    ).subscribe(res => {
      if (res[0].children !== null) {
        this.subCategories = res;
        this.headCategory = this.subCategories[0];
        this.children = this.subCategories[0].children;
      } else {
        this.pickedCategory = res[0];
      }
      this.cdr.detectChanges();
    });
  }

  goBack(event: Category) {
    if (event !== undefined && event !== null) {
      this.getParentCategory(event.parentId);

      this.categoryService.getFewBySuperiorId(event.id, 2).pipe(
        takeUntil(this.onDestroy$),
      ).subscribe(res => {
        this.subCategories = res;
        this.headCategory = this.subCategories[0];
        this.children = this.subCategories[0].children;
        this.cdr.detectChanges();
      });

    } else {
      this.isInSubCategories = false;
      this.categoryService.getSuperiorsWithChildren(1).pipe(
        takeUntil(this.onDestroy$),
      )
        .subscribe(res => {
          this.subCategories = res;
          this.children = this.subCategories;
          this.cdr.detectChanges();
        });
    }
  }

  handleImages() {
    this.previews.next([]);

    for (let i = 0; i < this.selectedImages.length; i++) {
      const reader = new FileReader();

      reader.onload = (e: any) => {
        const url = e.target.result;
        this.previews.getValue().push(url);
        this.cdr.detectChanges();
      };
      reader.readAsDataURL(this.selectedImages[i]);
    }
  }

  onFileSelected(event: any) {
    if (this.selectedImages.length >= 15) {
      //TODO toast z odpowiednią inofmracją
      console.log("more than 15");
      return;
    }

    this.selectedImages = [...this.selectedImages, ...event.target.files];
    console.log(this.selectedImages)
    this.handleImages();
  }

  deleteImage(index: number) {
    this.selectedImages = Array.from(this.selectedImages).filter((file, i) => i !== index);
    this.previews.getValue().splice(index, 1);
    console.log(this.selectedImages)
  }

  addOffer(formValue: any) {
    if (!this.createOffer.valid || !this.validateProps())
      return;

    this.offerService.createOffer(formValue.title, formValue.description, formValue.price, formValue.quantity, Condition[this.condition.value], this.pickedCategory.id, this.selectedImages).pipe(
      takeUntil(this.onDestroy$))
      .subscribe(res => {
          this.routerService.navigateTo(`${RoutesPath.HOME}/${RoutesPath.OFFER}/${res}`);
        }
      );
  }

  updateOffer(formValue: any) {
    if (!this.createOffer.valid || !this.validateProps())
      return;

    this.offerService.updateOffer(this.offerId, formValue.title, formValue.description, formValue.price, formValue.quantity, Condition[this.condition.value], this.isAvailable, this.selectedImages)
      .pipe(
        takeUntil(this.onDestroy$))
      .subscribe(res => {
          this.routerService.navigateTo(`${RoutesPath.HOME}/${RoutesPath.OFFER}/${res}`);
        }
      );
  }

  validateProps(): boolean {
    return !!(this.condition && this.pickedCategory);
  }

  onDrop(event: DragEvent) {
    this.isDragOver = false;
    event.preventDefault();
    const files = event.dataTransfer?.files;
    this.selectedImages = Array.from(files);
    this.handleImages();
  }

  onDragOver(event: DragEvent) {
    this.isDragOver = true;
    event.preventDefault();
    event.stopPropagation();
  }

  onDragLeave(event: DragEvent) {
    this.isDragOver = false;
    event.preventDefault();
    event.stopPropagation();
    // console.log("funkcja onDragLeave");
    // const dropZoneElement = this.dropZone.nativeElement;
    // const isInsideDropZone = dropZoneElement.contains(event.relatedTarget as Node);
    // if (!isInsideDropZone) {
    //   console.log("jestem w if")
    //   this.isDragOver = true;
    // }

  }
}
