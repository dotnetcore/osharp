import { ToasterService } from "angular2-toaster";
import { isFunction } from "util";
import { Rule, Group, ListNode, AjaxResult, AjaxResultType, User } from "./osharp/osharp.model";
declare var layer: any;

export namespace osharp {
  /**辅助操作工具类 */
  export class Tools {
    /**URL编码 */
    static urlEncode(url: string): string {
      return encodeURIComponent(url);
    }
    /**URL解码 */
    static urlDecode(url: string): string {
      return decodeURIComponent(url);
    }
    /**Base64字符串解码 */
    static fromBase64(base64: string): string {
      return new Buffer(base64, 'base64').toString();
    }
    /**Base64字符串编码 */
    static toBase64(str: string): string {
      return new Buffer(str).toString('base64');
    }
    /**获取URL中Hash串中的查询参数值 */
    static getHashURLSearchParams(url: string, name: string): string {
      if (url.indexOf("#") >= 0) {
        url = osharp.Tools.subStr(url, "#");
      }
      if (url.indexOf("?") >= 0) {
        url = osharp.Tools.subStr(url, "?");
      }
      let params = new URLSearchParams(url);
      return params.get(name);
    }
    /**提供首尾字符串截取中间的字符串 */
    static subStr(str: string, start: string = null, end: string = null): string {
      let startIndex = 0, endIndex = str.length;
      if (start) {
        startIndex = str.indexOf(start) + start.length;
      }
      if (end) {
        endIndex = str.indexOf(end);
      }
      return str.substr(startIndex, endIndex - startIndex);
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
      if (!array || !array.length) {
        return result;
      }
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
    static ajaxResult(res, onSuccess?, onFail?) {
      if (!res || !res.Type) {
        return;
      }
      var result = <AjaxResult>res;
      var type = result.Type;
      var content = result.Content;
      switch (type) {
        case AjaxResultType.Info:
          osharp.Tip.warning(content);
          return;
        case AjaxResultType.NoFound:
          window.location.href = "/#/nofound";
          return;
        case AjaxResultType.UnAuth:
          osharp.Tip.warning("用户未登录或登录已失效");
          window.location.href = "/#/identity/login";
          return;
        case AjaxResultType.Success:
          osharp.Tip.success(content);
          if (onSuccess && typeof onSuccess === "function") {
            onSuccess();
          }
          return;
        default:
          osharp.Tip.error(content);
          if (onFail && typeof onFail === "function") {
            onFail();
          }
          return;
      }
    }
    /**处理Ajax错误 */
    static ajaxError(xhr) {
      switch (xhr.status) {
        case 401:
          osharp.Tip.warning("用户未登录或登录已失效");
          window.location.href = "/#/identity/login";
          break;
        case 404:
          window.location.href = "/#/nofound";
          break;
        default:
          osharp.Tip.error(`发生错误：${xhr.status}: ${xhr.statusText}`);
          break;
      }
    }

    /**全屏指定元素 */
    static fullscreen(el) {
      if (el.requestFullscreen) {
        el.requestFullscreen();
      } else if (el.mozRequestFullScreen) {
        el.mozRequestFullScreen();
      } else if (el.webkitRequestFullscreen) {
        el.webkitRequestFullscreen();
      } else if (el.msRequestFullscreen) {
        el.msRequestFullscreen();
      }
    }
    /**退出全屏 */
    static exitFullscreen() {
      var doc: any = document;
      if (doc.exitFullscreen) {
        doc.exitFullscreen();
      } else if (doc.mozCancelFullScreen) {
        doc.mozCancelFullScreen();
      } else if (doc.webkitExitFullscreen) {
        doc.webkitExitFullscreen();
      }
    }
    /**获取树节点集合 */
    static getTreeNodes(root: any, array: Array<any>) {
      array.push(root);
      if (root.hasChildren) {
        for (let i = 0; i < root.Items.length; i++) {
          const item = root.Items[i];
          this.getTreeNodes(item, array);
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
      if (type == "error") {
        layer.alert(msg, { icon: 2, title: "" });
        return;
      }
      let icon = type == "warning" ? 0 : type == "success" ? 1 : 6;
      layer.msg(msg, { time: 3000, icon: icon, offset: 't' });
    }
  }


  export class Data {
    static AccessTypes = [{ id: 0, text: "匿名访问" }, { id: 1, text: "登录访问" }, { id: 2, text: "角色访问" }]
    static stringFilterable = { operators: { string: { contains: "包含", eq: "等于", neq: "不等于", startswith: "开始于", endswith: "结束于", doesnotcontain: "不包含" } } }
  }

  export class Auth {

    static token(): string {
      return localStorage.getItem('id_token');
    }

    static user(): User {
      let token = this.token();
      if (!token) {
        return null;
      }
      return new User(token);
    }
  }
}

/** kendo ui */
export namespace osharp {
  /** kendo 操作类 */
  export class Kendo {
    /** 获取osharp查询条件组 */
    static getFilterGroup(filter, funcFieldReplace): Group {
      if (!funcFieldReplace) {
        funcFieldReplace = field => field;
      }
      if (!filter || !filter.filters || !filter.filters.length) {
        return null;
      }
      var group = new Group();
      filter.filters.forEach(item => {
        if (item.filters && item.filters.length) {
          group.groups.push(Kendo.getFilterGroup(item, funcFieldReplace));
        } else {
          group.rules.push(Kendo.getFilterRule(item, funcFieldReplace));
        }
      });
      group.operate = filter.logic;
      return group;
    }
    /** 获取osharp查询条件 */
    static getFilterRule(filter, funcFieldReplace = null): Rule {
      if (!funcFieldReplace || !isFunction(funcFieldReplace)) {
        throw "funcFieldReplace muse be function";
      }
      var field = funcFieldReplace(filter.field);
      var operate = Kendo.renderRuleOperate(filter.operator);
      var rule = new Rule(field, filter.value, operate);
      return rule;
    }
    /** 转换查询操作 */
    static renderRuleOperate(operate) {
      if (operate === "eq") return "equal";
      if (operate === "neq") return "notequal";
      if (operate === "gt") return "greater";
      if (operate === "gte") return "greaterorequal";
      if (operate === "lt") return "less";
      if (operate === "lte") return "lessorequal";
      if (operate === "doesnotcontain") return "notcontains";
      return operate;
    }

    /**给每个请求头设置 JWT-Token */
    static setAuthToken(dataSource: kendo.data.DataSource) {
      let authToken = localStorage.getItem('id_token');
      if (!authToken) {
        return;
      }
      if (dataSource && dataSource.options && dataSource.options.transport) {
        var trans = dataSource.options.transport;
        if (trans.read) {
          (<any>trans.read).beforeSend = this.setAuthHeaderToken;
        }
        if (trans.create) {
          (<any>trans.create).beforeSend = this.setAuthHeaderToken;
        }
        if (trans.update) {
          (<any>trans.update).beforeSend = this.setAuthHeaderToken;
        }
        if (trans.destroy) {
          (<any>trans.destroy).beforeSend = this.setAuthHeaderToken;
        }
      }
    }
    private static setAuthHeaderToken(xhr, opts) {
      let authToken = localStorage.getItem('id_token');
      xhr.setRequestHeader("Authorization", `Bearer ${authToken}`);
    }
  }

  export namespace kendoui {
    export class Grid {
      /** 处理kendoui到osharp框架的查询参数 */
      static readParameterMap(options, funcFieldReplace) {
        if (!funcFieldReplace) {
          funcFieldReplace = field => field;
        }
        var paramter = {
          pageIndex: options.page,
          pageSize: options.pageSize || 100000,
          sortField: null,
          sortOrder: null,
          filter_group: null
        };
        if (options.sort && options.sort.length) {
          var fields = [], orders = [];
          options.sort.forEach(item => {
            fields.push(funcFieldReplace(item.field));
            orders.push(item.dir);
          });
          paramter.sortField = Tools.expandAndToString(fields);
          paramter.sortOrder = Tools.expandAndToString(orders);
        }
        if (options.filter && options.filter.filters.length) {
          var group = Kendo.getFilterGroup(options.filter, funcFieldReplace);
          paramter.filter_group = JSON.stringify(group);
        }
        return paramter;
      }
    }
  }
}
