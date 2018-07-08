import { NgModule, } from '@angular/core';
import { HomeComponent } from './home.component';
import { HomeRoutingModule, } from './home.routing';
import { SharedModule } from '@shared/shared.module';
import { TestComponent } from './test.component';

@NgModule({
  declarations: [
    HomeComponent,
    TestComponent
  ],
  imports: [
    HomeRoutingModule,
    SharedModule
  ],
  providers: [
  ]
})
export class HomeModule { }
