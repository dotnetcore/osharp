import { Component, OnInit, NgZone, ElementRef } from '@angular/core';
import { kendoui } from '../../../shared/kendoui';

@Component({
    selector: 'admin-module-function',
    template: `<div id="grid-box"><ng-content></ng-content></div>`
})
export class ModuleFunctionComponent extends kendoui.GridComponentBase implements OnInit {
    constructor(protected zone: NgZone, protected element: ElementRef) {
        super(zone, element);
    }

    ngOnInit() { }

    protected GetModel() {

    }
    protected GetGridColumns(): kendo.ui.GridColumn[] {
        return [];
    }

}
