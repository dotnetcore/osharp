import { NgModule } from '@angular/core';
import {
  MatSelectModule,
  MatAutocompleteModule,
  MatCardModule,
  MatCheckboxModule,
  MatDatepickerModule,
  MatDialogModule,
  MatGridListModule,
  MatIconModule,
  MatListModule,
  MatMenuModule,
  MatNativeDateModule,
  MatProgressBarModule,
  MatProgressSpinnerModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatSidenavModule,
  MatSliderModule,
  MatSlideToggleModule,
  MatSnackBarModule,
  MatTabsModule,
  MatToolbarModule,
  MatTooltipModule,
  MatRadioModule,
  MatInputModule,
  MatRippleModule,
  MatDividerModule,
} from '@angular/material';

const MODULES = [
  MatSelectModule,
  MatAutocompleteModule,
  MatCardModule,
  MatCheckboxModule,
  MatDatepickerModule,
  MatDialogModule,
  MatGridListModule,
  MatIconModule,
  MatListModule,
  MatMenuModule,
  MatNativeDateModule,
  MatProgressBarModule,
  MatProgressSpinnerModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatSidenavModule,
  MatSliderModule,
  MatSlideToggleModule,
  MatSnackBarModule,
  MatTabsModule,
  MatToolbarModule,
  MatTooltipModule,
  MatRadioModule,
  MatInputModule,
  MatRippleModule,
  MatDividerModule
];

@NgModule({
  exports: [...MODULES]
})
export class MaterialComponentsModule { }
