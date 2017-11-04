import Vue from "vue";
import Component from "vue-class-component";
import axios, { AxiosResponse } from "axios";

import { osharp } from "../shared/osharp";

@Component({
    template: require("./function.html")
})
export class FunctionComponent extends Vue {

    private url = "/api/admin/function/read";
    private axios;
    private page: osharp.PageData<any> = null;

    constructor() {
        super();
        this.axios = axios;
    }

    mounted() {
        this.axios.get(this.url).then((response: AxiosResponse) => {
            this.page = response.data;
            console.log(this.page);
        }, (error) => {
            console.error(error);
        });
    }
}