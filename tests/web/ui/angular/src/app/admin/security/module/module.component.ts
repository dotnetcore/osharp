import { Component, OnInit, OnDestroy, ElementRef, AfterViewInit, NgZone } from '@angular/core';
declare var $: any;

import { ModuleService, } from './module.service';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

@Component({
    selector: 'security-module',
    templateUrl: './module.component.html',
    styleUrls: ['./module.component.css']
})
export class ModuleComponent implements OnInit, AfterViewInit, OnDestroy {
    ngAfterViewInit(): void {
        let $el = $(this.element.nativeElement);
        console.log($el);
        $el = $el.find("#grid-module");
        console.log($($el));
    }
    moduleName = "module";
    constructor(private _service: ModuleService, private zone: NgZone, private element: ElementRef) {

    }

    ngOnInit() {
    }

    ngOnDestroy() {
    }
}
