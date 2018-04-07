import { isFunction } from "util";

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


/** kendo ui */
export namespace osharp {
  /** kendo 操作类 */
  export class Kendo {
    /** 获取osharp查询条件组 */
    static getFilterGroup(filter, funcFieldReplace): filter.Group {
      if (!funcFieldReplace) {
        funcFieldReplace = field => field;
      }
      if (!filter || !filter.filters || !filter.filters.length) {
        return null;
      }
      var group = new osharp.filter.Group();
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
    static getFilterRule(filter, funcFieldReplace = null): filter.Rule {
      if (!funcFieldReplace || !isFunction(funcFieldReplace)) {
        throw "funcFieldReplace muse be function";
      }
      var field = funcFieldReplace(filter.field);
      var operate = Kendo.renderRuleOperate(filter.operator);
      var rule = new osharp.filter.Rule(field, filter.value, operate);
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
  }

  export namespace kendo {
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
