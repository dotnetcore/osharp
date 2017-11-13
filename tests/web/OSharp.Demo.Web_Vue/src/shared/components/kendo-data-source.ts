import Vue from "vue";
import Component from "vue-class-component";
import { List } from "linqts";
import { osharp } from "../../shared/osharp";

@Component({
    template: '<div></div>',
    name: 'kendo-data-source',
    props: ['options'],
    created() {
        var options = this.options;
        this.dataSource = new kendo.data.DataSource({
            transport: {
                read: { url: options.url.read, type: 'post' },
                create: { url: options.url.create, type: 'post' },
                update: { url: options.url.update, type: 'post' },
                destroy: { url: options.url.destroy, type: 'post' },
                parameterMap: (opts, operation) => {
                    if (operation == 'read') {
                        return osharp.kendo.Grid.readParameterMap(opts, options.funcFieldReplace);
                    }
                    if (operation == 'create' || operation == 'update') {
                        return { dtos: opts.models };
                    }
                    if (operation == 'destroy' && opts.models.length) {
                        var ids = new List(opts.models).Select(m => m.id).ToArray();
                        return { ids: ids };
                    }
                    return {};
                }
            },
            group: options.group || [],
            schema: {
                model: options.model || {},
                data: d => d.rows,
                total: d => d.total
            },
            aggregate: options.aggregate || [],
            batch: options.batch != undefined ? options.batch : true,
            pageSize: options.pageSize || 20,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            requestEnd: requestEnd,
            change: options.change || function () { }
        });

        this.$emit('DataSourceCreated', this.dataSource);

        function requestEnd(e) {
            if (!e.response) {
                return;
            }
            var data = e.response;
            if (!data.type) {
                return;
            }
            if (data.type == 'error') {
                console.error(data.content);
                return;
            }
            if (data.type == 'info') {
                console.info(data.content);
            }
            if (data.type == 'success') {
                console.info(data.content);
            }
            this.dataSource.read();
        }
    },
    model: {
        event: 'DataSourceCreated'
    }
})
export class KendoDataSource extends Vue {
    constructor() {
        super();
    }
}