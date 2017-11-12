import Vue from "vue";
import Component from "vue-class-component";
import axios, { AxiosResponse } from "axios";

@Component({
    template: require("./function.html")
})
export class FunctionComponent extends Vue {

    private url = "/api/admin/function/read";
    items: any = null;

    constructor() {
        super();
    }

    mounted() {
        axios.get(this.url).then((response: AxiosResponse) => {
            this.items = response.data;
        }, (error) => {
            console.error(error);
        });
    }
}