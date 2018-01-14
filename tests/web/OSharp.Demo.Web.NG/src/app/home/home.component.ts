import { Component, OnInit } from '@angular/core';
import { Http } from "@angular/http";
import { LoggingService } from "../shared/services/logging.services";

@Component({
    selector: 's-home',
    templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {

    title: string = 'osharpns';
    apiValues: string[];
    constructor(private http: Http, private logger: LoggingService) {
        logger.info("home ctor call");
    }

    ngOnInit(): void {
        this.http.get('api/values').subscribe(res => {
            this.apiValues = res.json() as string[];
        });
    }
}
