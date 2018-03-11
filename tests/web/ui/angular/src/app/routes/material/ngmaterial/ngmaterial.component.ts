import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material';
import { MatSnackBar } from '@angular/material';
import 'rxjs/add/operator/startWith';

import { DialogResultExampleDialog } from './dialog.component';
import { PizzaPartyComponent } from './snackbar.component';

@Component({
    selector: 'app-ngmaterial',
    templateUrl: './ngmaterial.component.html',
    styleUrls: ['./ngmaterial.component.scss']
})
export class NgmaterialComponent implements OnInit {

    // Component - Autocomplete
    stateCtrl: FormControl;
    filteredStates: any;
    states = ['Alabama', 'Alaska', 'Arizona', 'Arkansas', 'California', 'Colorado', 'Connecticut', 'Delaware', 'Florida', 'Georgia', 'Hawaii', 'Idaho', 'Illinois', 'Indiana', 'Iowa', 'Kansas', 'Kentucky', 'Louisiana', 'Maine', 'Maryland', 'Massachusetts', 'Michigan', 'Minnesota', 'Mississippi', 'Missouri', 'Montana', 'Nebraska', 'Nevada', 'New Hampshire', 'New Jersey', 'New Mexico', 'New York', 'North Carolina', 'North Dakota', 'Ohio', 'Oklahoma', 'Oregon', 'Pennsylvania', 'Rhode Island', 'South Carolina', 'South Dakota', 'Tennessee', 'Texas', 'Utah', 'Vermont', 'Virginia', 'Washington', 'West Virginia', 'Wisconsin', 'Wyoming'];
    // Component - Checkbox
    checked = false;
    indeterminate = false;
    align = 'start';
    disabled = false;
    // Component - Radio button
    favoriteSeason: string;
    seasons = [
        'Winter',
        'Spring',
        'Summer',
        'Autumn',
    ];
    // Component - Select
    selectedValue: string;
    foods = [
        { value: 'steak-0', viewValue: 'Steak' },
        { value: 'pizza-1', viewValue: 'Pizza' },
        { value: 'tacos-2', viewValue: 'Tacos' }
    ];
    // Component - Slider
    autoTicks = false;
    disabledSlider = false;
    invert = false;
    max = 100;
    min = 0;
    showTicks = false;
    step = 1;
    thumbLabel = false;
    value = 0;
    vertical = false;
    // Component - Slide toggle
    color = 'accent';
    checkedSliderToggle = false;
    disabledSliderToggle = false;
    // Component - List
    folders = [
        {
            name: 'Photos',
            updated: new Date('1/1/16'),
        },
        {
            name: 'Recipes',
            updated: new Date('1/17/16'),
        },
        {
            name: 'Work',
            updated: new Date('1/28/16'),
        }
    ];
    notes = [
        {
            name: 'Vacation Itinerary',
            updated: new Date('2/20/16'),
        },
        {
            name: 'Kitchen Remodel',
            updated: new Date('1/18/16'),
        }
    ];
    // Component - Grid list
    tiles = [
        { text: 'One', cols: 3, rows: 1, color: 'lightblue' },
        { text: 'Two', cols: 1, rows: 2, color: 'lightgreen' },
        { text: 'Three', cols: 1, rows: 1, color: 'lightpink' },
        { text: 'Four', cols: 2, rows: 1, color: '#DDBDF1' },
    ];
    // Component - Chips
    colorChips: string;
    availableColors = [
        { name: 'none', color: '' },
        { name: 'Primary', color: 'primary' },
        { name: 'Accent', color: 'accent' },
        { name: 'Warn', color: 'warn' }
    ];
    // Component - Progress spinner
    colorProgressSpinner = 'praimry';
    modeProgressSpinner = 'determinate';
    valueProgressSpinner = 50;
    // Component - Progress bar
    colorProgressBar = 'primary';
    modeProgressBar = 'determinate';
    valueProgressBar = 50;
    bufferValue = 75;
    // Component - Dialog
    selectedOption: string;
    // Component - Slider
    private _tickInterval = 1;
    // Component - Tooltip
    position = 'before';

    constructor(public dialog: MatDialog, public snackBar: MatSnackBar) {
        // Component - Autocomplete
        this.stateCtrl = new FormControl();
        this.filteredStates = this.stateCtrl.valueChanges
            .startWith(null)
            .map(name => this.filterStates(name));
    }

    // Component - Autocomplete
    filterStates(val: string) {
        return val ? this.states.filter((s) => new RegExp(val, 'gi').test(s)) : this.states;
    }
    // Component - Slider
    get tickInterval(): number | 'auto' {
        return this.showTicks ? (this.autoTicks ? 'auto' : this._tickInterval) : null;
    }
    set tickInterval(v) {
        this._tickInterval = Number(v);
    }
    // Component - Dialog
    openDialog() {
        let dialogRef = this.dialog.open(DialogResultExampleDialog);
        dialogRef.afterClosed().subscribe(result => {
            this.selectedOption = result;
        });
    }
    // Component - Snackbar
    openSnackBar() {
        this.snackBar.openFromComponent(PizzaPartyComponent, {
            duration: 500,
        });
    }

    ngOnInit() { }
}
