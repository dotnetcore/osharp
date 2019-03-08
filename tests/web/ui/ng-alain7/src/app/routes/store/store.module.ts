import { NgModule } from '@angular/core';
import { SharedModule } from '@shared';
import { StoreRoutingModule } from './store-routing.module';
import { StoreProductComponent } from './product/product.component';
import { StoreProductEditComponent } from './product/edit/edit.component';
import { StoreGoodsComponent } from './goods/goods.component';
import { StoreGoodsEditComponent } from './goods/edit/edit.component';
import { StoreGoodsViewComponent } from './goods/view/view.component';
import { StoreOrderComponent } from './order/order.component';
import { StoreOrderViewComponent } from './order/view/view.component';
import { StoreOrderEditOrderComponent } from './order/edit/order/order.component';

const COMPONENTS = [
  StoreProductComponent,
  StoreGoodsComponent,
  StoreOrderComponent,
  StoreOrderEditOrderComponent];
const COMPONENTS_NOROUNT = [
  StoreProductEditComponent,
  StoreGoodsEditComponent,
  StoreGoodsViewComponent,
  StoreOrderViewComponent];

@NgModule({
  imports: [
    SharedModule,
    StoreRoutingModule
  ],
  declarations: [
    ...COMPONENTS,
    ...COMPONENTS_NOROUNT
  ],
  entryComponents: COMPONENTS_NOROUNT
})
export class StoreModule { }
