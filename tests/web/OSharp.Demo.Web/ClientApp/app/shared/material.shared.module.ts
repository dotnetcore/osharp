import { NgModule } from '@angular/core';

//material
import { MatCommonModule, MatButtonModule, MatButtonToggleModule, MatIconModule, MatSidenavModule, MatAutocompleteModule, MatCheckboxModule } from "@angular/material";

@NgModule({
    declarations: [],
    imports: [
        MatCommonModule,
        MatButtonModule,
        MatButtonToggleModule,
        MatIconModule,
        MatSidenavModule,
        MatAutocompleteModule,
        MatCheckboxModule
    ],
    exports: [
        MatCommonModule,
        MatButtonModule,
        MatButtonToggleModule,
        MatIconModule,
        MatSidenavModule,
        MatAutocompleteModule,
        MatCheckboxModule
    ],
    providers: [],
})
export class MaterialSharedModule { }