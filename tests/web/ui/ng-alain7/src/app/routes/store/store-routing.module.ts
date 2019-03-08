import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StoreProductComponent } from './product/product.component';
import { StoreGoodsComponent } from './goods/goods.component';
import { StoreOrderComponent } from './order/order.component';
import { StoreOrderEditOrderComponent } from './order/edit/order/order.component';

const routes: Routes = [

  { path: 'product', component: StoreProductComponent },
  { path: 'goods', component: StoreGoodsComponent },
  { path: 'order', component: StoreOrderComponent },
  { path: 'order', component: StoreOrderEditOrderComponent }]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StoreRoutingModule { }
