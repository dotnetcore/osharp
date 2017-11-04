export class HelloWorldService {

    msg: string;

    constructor() {
        this.msg = "Hello from osharp + vue + typescript";
    }

    change() {
        if (this.msg.indexOf("HELLO") > -1) {
            this.msg = "Hello from osharp + vue + typescript";
        }
        else {
            this.msg = this.msg.toUpperCase();
        }
    }
}