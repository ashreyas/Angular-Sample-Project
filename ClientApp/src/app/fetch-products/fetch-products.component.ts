import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NgxPaginationModule } from 'ngx-pagination';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-fetch-products',
  templateUrl: './fetch-products.component.html'
})
export class FetchProductsComponent {
  public products: Product[];
  //data: Array<any>;
  public totalCount: any;
  public page: number;
  public pageLimit = 6;
  public loading: boolean;


  constructor(private http: HttpClient, @Inject('BASE_URL')private baseUrl: string) { }

  ngOnInit() {
    this.getPage(1);
  }

  getPage(page: any): Observable<Product[]> {
    this.loading = true;
    this.http.get<any>(this.baseUrl + 'products/GetFilteredProducts/' + page + '/' + this.pageLimit).subscribe(result => {
      this.products = result.products;
      this.totalCount = result.totalCount;
      this.page = page;
      this.loading = false;
      return Observable.apply(this.products);
    }, error => console.error(error));
    return Observable.apply([]);
  }
}

interface Product {
  id: number;
  name: string;
  description: string;
  image: string;
  price: number;
  discount_amount: number;
  status: string;
  categories: Array<Category>;
}

interface Category {
  id: number;
  name: string;
}
