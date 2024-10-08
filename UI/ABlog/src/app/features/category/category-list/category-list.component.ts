import { Component, OnDestroy, OnInit } from '@angular/core';
import { CategoryService } from '../services/category.service';
import { Observable, Subject, takeUntil } from 'rxjs';
import { Category } from '../models/category.model';
import { MatTableDataSource } from '@angular/material/table';
import { CategoryDataSource } from './category-table-datasource';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrl: './category-list.component.scss',
})
export class CategoryListComponent implements OnInit, OnDestroy {
  dataSource!: MatTableDataSource<Category>;
  private destroy$ = new Subject<void>();
  displayedColumns: string[] = ['id', 'name', 'urlHandle', 'edit'];
  categories$?: Observable<Category[]>;
  constructor(private categoryService: CategoryService) {}
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  ngOnInit(): void {
    this.categories$ = this.categoryService.getAllCategories();
    this.categories$.pipe(takeUntil(this.destroy$)).subscribe((categories) => {
      this.dataSource = new CategoryDataSource(categories);
    });
  }
}
