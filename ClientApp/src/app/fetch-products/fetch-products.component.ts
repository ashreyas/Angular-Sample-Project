import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
//import { NgxPaginationModule } from 'ngx-pagination';

@Component({
  selector: 'app-fetch-products',
  templateUrl: './fetch-products.component.html'
})
export class FetchProductsComponent {
  public products: Product[];
  //data: Array<any>;
  totalCount: number;
  page: number = 1;
  pageLimit: number = 1000;


  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<any>(baseUrl + 'products/GetFilteredProducts/' + this.page + '/' + this.pageLimit).subscribe(result => {
      this.products = result.products;
      this.totalCount = result.totalCount;
    }, error => console.error(error));
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
