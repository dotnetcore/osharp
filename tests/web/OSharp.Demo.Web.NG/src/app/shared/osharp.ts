import toastr from 'toastr'

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

    /**辅助操作工具类 */
    export class Tools {
        /**URL编码 */
        static urlEncode(url: string) {
            return encodeURIComponent(url);
        }
        /**URL解码 */
        static urlDecode(url: string) {
            return decodeURIComponent(url);
        }
        /**值转文字 */
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
        /**展开集合拼接字符串 */
        static expandAndToString(array: Array<any>, separator: string = ',') {
            var result = '';
            array.forEach(item => {
                result = result + item.toString() + separator;
            });
            return result.substr(0, result.length - separator.length);
        }
        /** 下载数据 */
        static download(filename: string, content: string) {
            var urlObject = window.URL;
            var blob = new Blob([content]);
            var saveLink = document.createElement("a");
            saveLink.href = urlObject.createObjectURL(blob);
            saveLink.download = filename;
            var ev = document.createEvent("MouseEvents");
            ev.initMouseEvent(
                "click", true, false, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null
            );
            saveLink.dispatchEvent(ev);
        }
        /**处理AjaxResult */
        static ajaxResult(result, onSuccess?, onFail?) {
            if (!result || !result.Type) {
                return;
            }
            var type = result.Type;
            var content = result.Content;
            if (type === "Error") {
                osharp.Tip.error(content);
                if (onFail && typeof onFail === "function") {
                    onFail();
                }
                return;
            }
            if (type === "Warning") {
                osharp.Tip.warning(content);
                return;
            }
            if (type === "Info") {
                osharp.Tip.info(content);
                return;
            }
            if (type === "Success") {
                osharp.Tip.success(content);
                if (onSuccess && typeof onSuccess === "function") {
                    onSuccess();
                }
            }
        }
    }


    export class Tip {
        static success(msg) {
            Tip.msg(msg, "success");
        }
        static info(msg) {
            Tip.msg(msg, "info");
        }
        static warning(msg) {
            Tip.msg(msg, "warning");
        }
        static error(msg) {
            Tip.msg(msg, "error");
        }
        static msg(msg, type) {
            type = type || 'info';
            toastr.options = {
                timeOut: type == 'error' ? '6000' : '3000',
                positionClass: "toast-top-center",
                closeButton: true,
                newestOnTop: false
            }
            toastr[type](msg, "")
        }
    }


    export class Data {
        static AccessTypes = [{ id: 0, text: "匿名访问" }, { id: 1, text: "登录访问" }, { id: 2, text: "角色访问" }]
        static stringFilterable = { operators: { string: { contains: "包含", eq: "等于", neq: "不等于", startswith: "开始于", endswith: "结束于", doesnotcontain: "不包含" } } }
    }
}

export namespace osharp.filter {
    /** 查询条件 */
    export class Rule {
        field: string;
        value: string;
        operate: string;

        constructor(field: string, value: string, operate: string = 'equal') {
            this.field = field;
            this.value = value;
            this.operate = operate;
        }
    }
    /** 查询条件组 */
    export class Group {
        rules: Rule[] = []
        operate: string = 'and';
        groups: Group[] = [];
    }

}