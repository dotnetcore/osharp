import Vue from "vue";
import Component from "vue-class-component";

@Component({
    template: require("./test01.html")
})
export class Test01Component extends Vue {
    msg: string;
    count: number = 0;
    constructor() {
        super();
        this.msg = "msg from test01";
    }

    btnClick(): void {
        this.count++;
    }
    mounted() { }
} 