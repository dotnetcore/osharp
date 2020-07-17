/**
 * @Name osharp
 * @Description 封装OSharp框架需要的前端工具
 * @Homepage //www.osharp.org
 * @Author 郭明锋
 * @License apache 2.0 
 */

!function (win) {
  "use strict";
  var OSharp = function () {
    this.version = "3.1.5";
  };

  /**类型定义 */
  OSharp.prototype.FilterOperate = {
    And: 1,
    Or: 2,
    Equal: 3,
    NotEqual: 4,
    Less: 5,
    LessOrEqual: 6,
    Greater: 7,
    GreaterOrEqual: 8,
    StartsWith: 9,
    EndsWith: 10,
    Contains: 11,
    NotContains: 12
  };
  OSharp.prototype.FilterOperateEntry = function (operate) {
    this.Operate = operate;
    switch (operate) {
      case OSharp.prototype.FilterOperate.And:
        this.Display = "并且";
        break;
      case OSharp.prototype.FilterOperate.Or:
        this.Display = "或者";
        break;
      case OSharp.prototype.FilterOperate.Equal:
        this.Display = "等于";
        break;
      case OSharp.prototype.FilterOperate.NotEqual:
        this.Display = "不等于";
        break;
      case OSharp.prototype.FilterOperate.Less:
        this.Display = "小于";
        break;
      case OSharp.prototype.FilterOperate.LessOrEqual:
        this.Display = "小于等于";
        break;
      case OSharp.prototype.FilterOperate.Greater:
        this.Display = "大于";
        break;
      case OSharp.prototype.FilterOperate.GreaterOrEqual:
        this.Display = "大于等于";
        break;
      case OSharp.prototype.FilterOperate.StartsWith:
        this.Display = "开始于";
        break;
      case OSharp.prototype.FilterOperate.EndsWith:
        this.Display = "结束于";
        break;
      case OSharp.prototype.FilterOperate.Contains:
        this.Display = "包含";
        break;
      case OSharp.prototype.FilterOperate.NotContains:
        this.Display = "不包含";
        break;
      default:
        this.Display = "未知操作";
        break;
    }
    this.Display = operate + "." + this.Display;
  };
  OSharp.prototype.FilterRule = function (field, value, operate) {
    if (operate === void 0) {
      operate = FilterOperate.Equal;
    }
    this.Field = field;
    this.Value = value;
    this.Operate = operate;
  };
  OSharp.prototype.FilterGroup = function () {
    this.Rules = [];
    this.Operate = OSharp.prototype.FilterOperate.And;
    this.Groups = [];
    this.Level = 1;
  };
  OSharp.prototype.FilterGroup.prototype.Init = function (group) {
    if (!group.Level) {
      group.Level = 1;
    }
    group.Groups.forEach(function (subGroup) {
      subGroup.Level = group.Level + 1;
      OSharp.prototype.FilterGroup.prototype.Init(subGroup);
    });
  };
  OSharp.prototype.ListSortDirection = {
    Ascending: 0,
    Descending: 1
  };
  OSharp.prototype.SortCondition = {
    SortField: null,
    ListSortDirection: OSharp.prototype.ListSortDirection.Ascending
  };
  OSharp.prototype.PageCondition = function () {
    this.PageIndex = 1;
    this.PageSize = 20;
    this.SortConditions = [];
  };
  OSharp.prototype.PageRequest = function () {
    this.PageCondition = new OSharp.prototype.PageCondition();
    this.FilterGroup = new OSharp.prototype.FilterGroup();
  };

  /**工具类 */
  OSharp.prototype.tools = {
    /**
     * Cookie操作
     */
    cookies: {
      get: function (name) {
        var value = "";
        var search = name + "=";
        if (document.cookie.length === 0) {
          return value;
        }
        var offset = document.cookie.indexOf(search);
        if (offset === -1) {
          return value;
        }
        offset += search.length;
        var end = document.cookie.indexOf(";", offset);
        if (end === -1) {
          end = document.cookie.length;
        }
        value = decodeURIComponent(document.cookie.substring(offset, end));
        return value;
      },
      set: function (name, value, expireDays) {
        var expires;
        if (!expireDays) {
          expireDays = 1;
        }
        expires = new Date((new Date()).getTime() + expireDays * 86400000);
        expires = ";expires=" + expires.toGMTString();
        document.cookie = name + "=" + encodeURIComponent(value) + ";path=/" + expires;
      },
      remove: function (name) {
        var expires;
        expires = new Date(new Date().getTime() - 1);
        expires = ";expires=" + expires.toGMTString();
        document.cookie = name + "=" + escape("") + ";path=/" + expires;
      }
    },
    /**
     * URL编码与解码
     */
    url: {
      encode: function (url) {
        return encodeURIComponent(url);
      },
      decode: function (url) {
        return decodeURIComponent(url);
      }
    },
    /**
     * 集合操作
     */
    array: {
      /**
       * 将集合展开并连接为字符串
       * @param {any[]} array 要展开的集合
       * @param {string} separator 分隔符
       */
      expandAndToString: function (array, separator) {
        var result = "";
        if (!separator) {
          separator = ";";
        }
        $.each(array,
          function (index, item) {
            result = result + item.toString() + separator;
          });
        return result.substring(0, result.length - separator.length);
      },
      /**
       * 从集合中删除符合条件的项
       * @param items 集合
       * @param exp 删除项查询表达式
       */
      remove: function (items, exp) {
        var index = items.findIndex(exp);
        items.splice(index, 1);
        return items;
      }
    },
    string: {
      /**
       * 提供首尾字符串截取中间的字符串
       * @param {any} str 待截取的字符串
       * @param {any} start 起始的字符串
       * @param {any} end 结束的字符串
       */
      subStr: function (str, start, end) {
        var startIndex = 0;
        var endIndex = str.length;
        if (start) {
          startIndex = str.indexOf(start) + start.length;
        }
        if (end) {
          endIndex = str.indexOf(end);
        }
        return str.substr(startIndex, endIndex - startIndex);
      }
    },
    dateFormat: function (date, format) {
      if (!date) {
        return "";
      }
      if (!format) {
        format = "YYYY/MM/DD hh:mm";
      }
      return moment(date).format(format);
    },
    post: function(jQuery, url, data, success) {
      jQuery.ajax({
        type: "POST",
        url: url,
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(data),
        dataType: "json",
        success: success
      });
    },
    goto: function(url, timeout) {
      if (!timeout) timeout = 500;
      setTimeout(function() {
        location.href = url;
      }, timeout);
    },
    success: function(layer, msg) {
      layer.msg(msg, { icon: 1 });
    },
    error: function(layer, msg) {
      layer.alert(msg, { icon: 2 });
    },
    ajaxResult: function(res, layer, onSuccess, onFail) {
      if (res.Type === 200) {
        this.success(layer, res.Content);
        if (onSuccess && typeof onSuccess === 'function') {
          onSuccess();
        } 
      } else {
        this.error(layer, res.Content);
        if (onFail && typeof onFail === "function") {
          onFail();
        }
      }
    }
  };

  OSharp.prototype.post = function(jQuery, url, data, success) {
    jQuery.ajax({
      type: "POST",
      url: url,
      contentType: "application/json;charset=utf-8",
      data: JSON.stringify(data),
      dataType: "json",
      success: success
    });
  };

  win.osharp = new OSharp();
}(window);
