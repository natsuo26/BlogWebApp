import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subject, take, takeUntil } from 'rxjs';
import { BlogPostService } from '../services/blog-post.service';
import { BlogPost } from '../models/blog-post.model';
import { CategoryService } from '../../category/services/category.service';
import { Category } from '../../category/models/category.model';
import { UpdateBlogPost } from '../models/update-blog-post.model';

@Component({
  selector: 'app-edit-blogpost',
  templateUrl: './edit-blogpost.component.html',
  styleUrl: './edit-blogpost.component.scss',
})
export class EditBlogpostComponent implements OnInit, OnDestroy {
  Id: string | null = null;
  destroy$ = new Subject<void>();
  categories$?: Observable<Category[]>;
  selectedCategories?:string[];
  model?: BlogPost;
  constructor(
    private route: ActivatedRoute,
    private blogPostService: BlogPostService,
    private categoryService: CategoryService,
    private router:Router
  ) {}

  ngOnInit(): void {
    this.categories$ = this.categoryService.getAllCategories();
    this.route.paramMap.pipe(takeUntil(this.destroy$)).subscribe({
      next: (params) => {
        this.Id = params.get('id');
        if (this.Id !== null) {
          this.blogPostService
            .getBlogPostById(this.Id)
            .pipe(takeUntil(this.destroy$))
            .subscribe({
              next: (blogPost) => {
                this.model = blogPost;
                this.selectedCategories=blogPost.categories.map(x=>x.id);
              },
            });
        }
      },
    });
  }

  onFormSubmit() {
    if(this.model && this.Id){
      var updateBlogPost:UpdateBlogPost={
        title: this.model.title,
        shortDescription: this.model.shortDescription,
        urlHandle: this.model.urlHandle,
        content: this.model.content,
        featureImageUrl: this.model.featureImageUrl,
        author: this.model.author,
        isVisible: this.model.isVisible,
        publishedDate: this.model.publishedDate,
        categories:this.selectedCategories??[]
      }
      this.blogPostService.updateBlogPost(this.Id,updateBlogPost).pipe(takeUntil(this.destroy$)).subscribe({
        next:(response)=>{
          this.router.navigateByUrl('/admin/blogposts');
        }
      });
    }
    
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
