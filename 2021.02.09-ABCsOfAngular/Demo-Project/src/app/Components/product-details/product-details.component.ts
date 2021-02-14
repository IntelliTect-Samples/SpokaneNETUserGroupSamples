import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService, IProduct } from 'src/app/Services/data.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss'],
})
export class ProductDetailsComponent implements OnInit {
  product: IProduct;

  constructor(
    private dataService: DataService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const productId = parseInt(this.route.snapshot.paramMap.get('id'));
    this.product = this.dataService.getProduct(productId);

    if (!this.dataService.userLoggedIn) {
      this.router.navigate(['login']);
    }
  }

  handleOutgoing(ev: any) {
    console.log(ev);
  }
}
