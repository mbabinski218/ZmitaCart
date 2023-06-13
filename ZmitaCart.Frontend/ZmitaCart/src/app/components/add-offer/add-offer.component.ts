import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component, ElementRef,
  OnInit,
  ViewChild,
  ViewEncapsulation
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from "@angular/material/icon";
import { MatButtonModule } from "@angular/material/button";
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatInputModule } from "@angular/material/input";
import { CategoryService } from "@components/add-offer/api/category.service";
import { Category } from "@components/add-offer/interfaces/Category";
import { Condition } from "@core/enums/condition.enum";
import { ConditionWrapperComponent } from "@components/add-offer/condition-wrapper/condition-wrapper.component";
import { ConditionType } from "@components/add-offer/interfaces/ConditionType";
import { CategorySelectorComponent } from "@components/add-offer/category-selector/category-selector.component";
import { OfferService } from "@components/add-offer/api/offer.service";
import { ActivatedRoute, RouterLink } from "@angular/router";
import { RoutingService } from "@shared/services/routing.service";
import { RoutesPath } from "@core/enums/routes-path.enum";
import { filter, map, switchMap, tap } from 'rxjs';
import { OfferSingleService } from '@components/offer-single/api/offer-single.service';


@Component({
  selector: 'pp-add-offer',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule, ReactiveFormsModule, MatInputModule, ConditionWrapperComponent, CategorySelectorComponent, RouterLink],
  templateUrl: './add-offer.component.html',
  styleUrls: ['./add-offer.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddOfferComponent implements OnInit {

  createOffer: FormGroup;
  characterCount: number;
  price: number;
  quantity: number;
  condition: Condition;
  subCategories: Category[] = null;
  children: Category[];
  pickedCategory: Category = null;
  headCategory: Category;
  parentCategory?: Category;
  isInSubCategories: boolean;
  selectedImages: File[];
  previews: string[] = [];
  isDragOver: boolean;
  inEdit = false;

  @ViewChild('dropZone', { static: true }) dropZone!: ElementRef;

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

  ngOnInit(): void {
    this.createOffer = new FormGroup({
      title: new FormControl(null as string, [Validators.required, Validators.minLength(3), Validators.maxLength(50)]),
      description: new FormControl(null as string, [Validators.required]),
      price: new FormControl(null as number, [Validators.required, Validators.pattern("^\\d+(?:\\,\\d{2})?$")]),
      quantity: new FormControl(1, [Validators.pattern("^(?=.*[1-9])\\d+$")]),
    });

    this.route.queryParams.pipe(
      map(({ id }) => id as string),
      filter((id) => !!id),
      tap(() => this.inEdit = true),
      switchMap((id) => this.offerService.getForEditOffer(id)),
      tap((res) => this.createOffer.patchValue(res)),
      tap((res) => this.condition = res.condition as unknown as Condition),
    ).subscribe();

    this.createOffer.get('title').valueChanges.subscribe((res: string) => {
      this.characterCount = res.length;
    });

    const qControl = this.createOffer.get('quantity');

    qControl.valueChanges.subscribe((res: string) => {
      if (res.length === 0) {
        qControl.setValue(1);
      }
      if (!qControl.valid) {
        const val = res.replace(/\D/g, '');
        qControl.setValue(val);
      }
    });
  }

  loadSuperiorCategories() {
    this.categoryService.getSuperiorsWithChildren(1)
      .subscribe(res => {
        this.subCategories = res;
        this.children = this.subCategories;
        this.cdr.detectChanges();
      });
  }

  public SetCondition(event: Condition) {
    this.condition = event;
  }

  public getParentCategory(parentId: number) {
    //patrzymy na kategorie nadrzędne
    if (parentId === null) {
      this.parentCategory = null;
      return;
    }

    return this.categoryService.getParentCategory(parentId).subscribe(
      res => {
        this.parentCategory = res;
      }
    );
  }

  goForward(event: Category) {
    this.isInSubCategories = true;
    if (event.parentId !== null && event.children)
      this.getParentCategory(event.parentId);

    this.categoryService.getFewBySuperiorId(event.id, 2).subscribe(res => {
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

      this.categoryService.getFewBySuperiorId(event.id, 2).subscribe(res => {
        this.subCategories = res;
        this.headCategory = this.subCategories[0];
        this.children = this.subCategories[0].children;
        this.cdr.detectChanges();
      });

    } else {
      this.isInSubCategories = false;
      this.categoryService.getSuperiorsWithChildren(1)
        .subscribe(res => {
          this.subCategories = res;
          this.children = this.subCategories;
          this.cdr.detectChanges();
        });
    }
  }

  handleImages() {
    this.previews = [];

    for (let i = 0; i < this.selectedImages.length; i++) {
      const reader = new FileReader();

      reader.onload = (e: any) => {
        const url = e.target.result;
        this.previews.push(url);
        this.cdr.detectChanges();
      };
      reader.readAsDataURL(this.selectedImages[i]);
    }
  }

  onFileSelected(event: any) {
    this.selectedImages = event.target.files;
    this.handleImages();
  }

  deleteImage(index: number) {
    this.selectedImages = Array.from(this.selectedImages).filter((file, i) => i !== index);
    this.previews.splice(index, 1);
  }

  addOffer() {
    if (!this.createOffer.valid || !this.validateProps())
      return;

    const title = this.createOffer.value.title;
    const desc: string = this.createOffer.value.description;
    const price: number = this.createOffer.value.price;
    const quantity: number = this.createOffer.value.quantity;

    this.offerService.createOffer(title, desc, price, quantity, Condition[this.condition], this.pickedCategory.id, this.selectedImages).subscribe(res => {
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
