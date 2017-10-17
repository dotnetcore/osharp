import { Injectable } from '@angular/core';

@Injectable()
export class LoggingService {
    debug(msg: string) {
        this.log(msg, "debug");
    }
    info(msg: string) {
        this.log(msg, "info");
    }
    warn(msg: string) {
        this.log(msg, "warn");
    }
    error(msg: string) {
        this.log(msg, "error");
    }
    log(msg: string, stat: string) {
        switch (stat) {
            case "debug":
                console.debug(msg);
                break;
            case "info":
                console.info(msg);
                break;
            case "warn":
                console.warn(msg);
                break;
            case "error":
                console.error(msg);
                break;
            default:
                console.log(msg);
                break;
        }
    }
}