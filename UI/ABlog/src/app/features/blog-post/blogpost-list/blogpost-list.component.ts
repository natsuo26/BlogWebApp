import { Component, OnDestroy, OnInit } from '@angular/core';
import { BlogPost } from '../models/blog-post.model';
import { BlogPostService } from '../services/blog-post.service';
import { Observable, Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-blogpost-list',
  templateUrl: './blogpost-list.component.html',
  styleUrl: './blogpost-list.component.scss'
})
export class BlogpostListComponent implements OnInit, OnDestroy{
  destroy$=new Subject<void>();
  blogPosts$?:Observable<BlogPost[]>;
  constructor(private blogPostService:BlogPostService){}

  ngOnInit(): void {
    //getAllblogposts
    this.blogPosts$=this.blogPostService.getAllBlogPosts();
  }
  
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete()
  }

}
