import { Component, OnInit } from '@angular/core';
import { Http } from "@angular/http";
import { LoggingService } from "../shared/services/logging.services";
import { ToasterService, ToasterConfig, ToasterContainerComponent } from 'angular2-toaster/angular2-toaster';

@Component({
    selector: 's-home',
    templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {

    title: string = 'osharpns';
    apiValues: string[];

    toasterConfig: any;
    toasterconfig: ToasterConfig = new ToasterConfig({
        positionClass: 'toast-bottom-right',
        showCloseButton: true
    });

    constructor(private http: Http, private logger: LoggingService, public toasterService: ToasterService) {
        logger.info("home ctor call");
    }

    ngOnInit(): void {
        this.http.get('api/values').subscribe(res => {
            this.apiValues = res.json() as string[];
        });
    }

    onclick() {
        this.toasterService.pop("success", "", "成功消息")
    }
}
