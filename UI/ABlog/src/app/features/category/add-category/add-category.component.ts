import { Component, OnDestroy } from '@angular/core';
import { AddCategoryRequest } from '../models/add-category-request.model';
import { CategoryService } from '../services/category.service';
import { Subject, takeUntil } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrl: './add-category.component.scss',
})
export class AddCategoryComponent implements OnDestroy {
  model: AddCategoryRequest;

  private destroy$ = new Subject<void>();

  constructor(
    private categoryService: CategoryService,
    private router: Router
  ) {
    this.model = {
      name: '',
      urlHandle: '',
    };
  }

  onFormSubmit() {
    this.categoryService
      .addCategory(this.model)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (reqponse) => {
          this.router.navigateByUrl('/admin/categories');
        },
        error: () => {},
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
