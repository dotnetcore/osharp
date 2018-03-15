import { Directive, OnInit, Input, AfterViewInit, OnDestroy, ElementRef, NgZone } from '@angular/core';
declare var $: any;

@Directive({
    selector: '[kendoGrid]',
})
export class KendoGridDirective implements OnInit, AfterViewInit, OnDestroy {

    @Input() options: any;

    constructor(public el: ElementRef, private zone: NgZone) {

    }
    ngOnInit(): void {

    }
    ngAfterViewInit(): void {
        this.zone.runOutsideAngular(() => {
            let $element = $(this.el.nativeElement);
            $element.kendoGrid(this.options);
        });
    } ngOnDestroy(): void {

    }
}