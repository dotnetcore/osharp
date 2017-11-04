export namespace osharp {
    /** 分页数据 */
    export class PageData<T>{
        rows: T[] = [];
        total: 0;
    }

    export class ListNode {
        id: number;
        text: string;
    }

    export class Tools {
        static urlEncode(url: string) {
            return encodeURIComponent(url);
        }

        static urlDecode(url: string) {
            return decodeURIComponent(url);
        }

        static valueToText(id: number, array: Array<ListNode>, defaultText: string = null) {
            let text = defaultText == null ? id : defaultText;
            array.forEach(item => {
                if (item.id == id) {
                    text = item.text;
                    return false;
                }
                return true;
            });
            return text;
        }

        static expandAndToString(array: Array<any>, separator: string = ',') {
            var result = '';
            array.forEach(item => {
                result = result + item.toString() + separator;
            });
            return result.substr(0, result.length - separator.length);
        }

        static download(filename: string, content: string) {
            var link = document.createElement('a');
            var blob = new Blob([content]);
            var evt = document.createEvent('HTMLEvents');
            evt.initEvent('click', false, false);
            link.download = filename;
            link.href = URL.createObjectURL(blob);
            link.dispatchEvent(evt);
        }
    }
}


export namespace osharp.filter {

    /** 查询条件 */
    export class Rule {
        field: string;
        value: string;
        operate: Operate;

        constructor(field: string, value: string, operate: string = 'equal') {
            this.field = field;
            this.value = value;
            this.operate = Operate[operate];
        }
    }

    export enum Operate {
        And = 'and',
        Or = 'or',
        Equal = 'equal',
        NotEqual = 'notequal',
        Less = 'less',
        LessOrEqual = 'lessorequal',
        Greater = 'greater',
        GreaterOrEqual = 'greaterorequal',
        StartsWith = 'startswith',
        EndsWith = 'endswith',
        Contains = 'contains',
        NotContains = 'notcontains'
    }

    /** 查询条件组 */
    export class Group {
        rules: Rule[] = []
        operate: string = 'and';
        groups: Group[] = [];
    }

}