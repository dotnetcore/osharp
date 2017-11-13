import Vue from "vue";
import Component from "vue-class-component";
import axios, { AxiosResponse } from "axios";
import { osharp } from "../../../../shared/osharp";
import { KendoDataSource } from "../../../../shared/components/kendo-data-source";

@Component({
    template: require("./function.html"),
    components: { 'kendo-data-source': KendoDataSource }
})
export class FunctionComponent extends Vue {

    dataSourceOptions = null;
    dataSource = {};

    constructor() {
        super();

        this.dataSourceOptions = {
            url: {
                read: '/api/admin/function/read',
                create: '/api/admin/function/create',
                update: '/api/admin/function/update',
                destroy: '/api/admin/function/destroy'
            },
            model: {
                id: 'id',
                fields: {
                    name: { type: 'string' },
                    accessType: { type: 'number' }
                }
            },
            pageSize: 15
        };
    }


}