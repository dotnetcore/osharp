import { Component, OnInit } from '@angular/core';
import { ColorsService } from '../../../shared/angle/colors/colors.service';

@Component({
    selector: 'app-widgets',
    templateUrl: './widgets.component.html',
    styleUrls: ['./widgets.component.scss']
})
export class WidgetsComponent implements OnInit {

    dt: Date = new Date();
    minDate: Date = void 0;

    lat: number = 33.790807;
    lng: number = -117.835734;
    zoom: number = 14;
    scrollwheel = false;

    sparkOption1 = {
        type: 'line',
        width: '100%',
        height: '140px',
        tooltipOffsetX: -20,
        tooltipOffsetY: 20,
        spotColor: 'rgba(0,0,0,.26)',
        minSpotColor: 'rgba(0,0,0,.26)',
        maxSpotColor: 'rgba(0,0,0,.26)',
        highlightSpotColor: 'rgba(0,0,0,.26)',
        highlightLineColor: 'rgba(0,0,0,.26)',
        spotRadius: 2,
        tooltipPrefix: '',
        tooltipSuffix: ' Visits',
        tooltipFormat: '{{prefix}}{{y}}{{suffix}}',
        chartRangeMin: 0,
        resize: true,
        lineColor: this.colors.byName('success'),
        fillColor: this.colors.byName('success')
    };

    sparkOption2 = {
        type: 'line',
        width: '100%',
        height: '140px',
        tooltipOffsetX: -20,
        tooltipOffsetY: 20,
        spotColor: 'rgba(0,0,0,.26)',
        minSpotColor: 'rgba(0,0,0,.26)',
        maxSpotColor: 'rgba(0,0,0,.26)',
        highlightSpotColor: 'rgba(0,0,0,.26)',
        highlightLineColor: 'rgba(0,0,0,.26)',
        spotRadius: 2,
        tooltipPrefix: '',
        tooltipSuffix: ' Visits',
        tooltipFormat: '{{prefix}}{{y}}{{suffix}}',
        chartRangeMin: 0,
        resize: true,
        lineColor: this.colors.byName('info'),
        fillColor: this.colors.byName('info')
    };

    sparkOption3 = {
        type: 'line',
        width: '100%',
        height: '140px',
        tooltipOffsetX: -20,
        tooltipOffsetY: 20,
        spotColor: 'rgba(0,0,0,.26)',
        minSpotColor: 'rgba(0,0,0,.26)',
        maxSpotColor: 'rgba(0,0,0,.26)',
        highlightSpotColor: 'rgba(0,0,0,.26)',
        highlightLineColor: 'rgba(0,0,0,.26)',
        spotRadius: 2,
        tooltipPrefix: '',
        tooltipSuffix: ' Visits',
        tooltipFormat: '{{prefix}}{{y}}{{suffix}}',
        chartRangeMin: 0,
        resize: true,
        lineColor: "#fff",
        fillColor: "#fff"
    };

    sparkOptionPie = {
        type: 'pie',
        width: '2em',
        height: '2em',
        sliceColors: [this.colors.byName('success'), this.colors.byName('gray-light')]
    };

    constructor(private colors: ColorsService) { }

    ngOnInit() {
    }

}
