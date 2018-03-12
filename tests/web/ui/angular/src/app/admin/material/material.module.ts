import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AgmCoreModule, GoogleMapsAPIWrapper } from '@agm/core';

import { AngleModule } from '../../shared/angle/angle.module';
import { WidgetsComponent } from './widgets/widgets.component';
import { CardsComponent } from './cards/cards.component';
import { FormsComponent } from './forms/forms.component';
import { InputsComponent } from './inputs/inputs.component';
import { ListsComponent } from './lists/lists.component';
import { WhiteframeComponent } from './whiteframe/whiteframe.component';
import { ColorsComponent } from './colors/colors.component';
import { NgmaterialComponent } from './ngmaterial/ngmaterial.component';
import { DialogResultExampleDialog } from './ngmaterial/dialog.component';
import { PizzaPartyComponent } from './ngmaterial/snackbar.component';

const routes: Routes = [
    { path: 'widgets', component: WidgetsComponent },
    { path: 'cards', component: CardsComponent },
    { path: 'forms', component: FormsComponent },
    { path: 'inputs', component: InputsComponent },
    { path: 'lists', component: ListsComponent },
    { path: 'whiteframe', component: WhiteframeComponent },
    { path: 'colors', component: ColorsComponent },
    { path: 'ngmaterial', component: NgmaterialComponent }
];

@NgModule({
    imports: [
        AngleModule,
        RouterModule.forChild(routes),
        AgmCoreModule.forRoot({
            apiKey: 'AIzaSyBNs42Rt_CyxAqdbIBK0a5Ut83QiauESPA'
        })
    ],
    declarations: [
        WidgetsComponent,
        CardsComponent,
        FormsComponent,
        InputsComponent,
        ListsComponent,
        WhiteframeComponent,
        ColorsComponent,
        NgmaterialComponent,
        DialogResultExampleDialog,
        PizzaPartyComponent
    ],
    exports: [
        RouterModule
    ],
    entryComponents: [
        DialogResultExampleDialog,
        PizzaPartyComponent
    ]
})
export class MaterialModule { }
