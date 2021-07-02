import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-products',
  templateUrl: './fetch-products.component.html'
})
export class FetchProductsComponent {
  public products: Product[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Product[]>(baseUrl + 'products/GetFilteredProducts').subscribe(result => {
      this.products = result;
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
