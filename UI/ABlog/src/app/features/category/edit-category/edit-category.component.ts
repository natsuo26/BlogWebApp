import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { CategoryService } from '../services/category.service';
import { Category } from '../models/category.model';
import { UpdateCategoryRequest } from '../models/update-category-request.model';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrl: './edit-category.component.scss',
})
export class EditCategoryComponent implements OnInit, OnDestroy {
  id: string | null = null;
  private destroy$ = new Subject<void>();
  category?: Category;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private categoryService: CategoryService
  ) {}
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  ngOnInit(): void {
    this.route.paramMap.pipe(takeUntil(this.destroy$)).subscribe({
      next: (params) => {
        this.id = params.get('id');
        if (this.id) {
          //get data from API Observable
          this.categoryService
            .getCategoryById(this.id)
            .pipe(takeUntil(this.destroy$))
            .subscribe({
              next: (response) => {
                this.category = response;
              },
            });
        }
      },
    });
  }
  onFormSubmit(): void {
    const updateCategoryRequest: UpdateCategoryRequest = {
      name: this.category?.name ?? '',
      urlHandle: this.category?.urlHandle ?? '',
    };
    //pass this object to service with the ID seperately
    if (this.id) {
      this.categoryService
        .updateCategory(this.id, updateCategoryRequest)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (response) => {
            this.router.navigateByUrl('/admin/categories');
          },
        });
    }
  }
  onDelete() {
    if (this.id) {
      this.categoryService
        .deleteCategory(this.id)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (response) => {
            this.router.navigateByUrl('/admin/categories');
          },
        });
    }
  }
}
