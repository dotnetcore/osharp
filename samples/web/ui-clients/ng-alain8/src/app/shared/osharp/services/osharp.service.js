"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
exports.__esModule = true;
exports.ComponentBase = exports.OsharpService = void 0;
var core_1 = require("@angular/core");
var osharp_model_1 = require("@shared/osharp/osharp.model");
var router_1 = require("@angular/router");
var buffer_1 = require("buffer");
var rxjs_1 = require("rxjs");
var linqts_1 = require("linqts");
var OsharpService = /** @class */ (function () {
    function OsharpService(injector, msgSrv, http, aclSrv) {
        this.injector = injector;
        this.msgSrv = msgSrv;
        this.http = http;
        this.aclSrv = aclSrv;
        // #endregion
        // #region 消息方法
        this.msgOptions = {
            nzDuration: 1000 * 3,
            nzAnimate: true,
            nzPauseOnHover: true
        };
        // #endregion
        // #region 静态数据
        this.data = {
            accessType: [{ id: 0, text: "匿名访问" }, { id: 1, text: "登录访问" }, { id: 2, text: "角色访问" }],
            stringFilterable: {
                operators: {
                    string: {
                        contains: "包含",
                        eq: "等于",
                        neq: "不等于",
                        startswith: "开始于",
                        endswith: "结束于",
                        doesnotcontain: "不包含"
                    }
                }
            },
            dataAuthOperations: [{ id: 0, text: "读取" }, { id: 1, text: "更新" }, { id: 2, text: "删除" }],
            operateType: [{ id: 1, text: "新增" }, { id: 2, text: "更新" }, { id: 3, text: "删除" }],
            ajaxResultType: [
                { id: 200, text: "成功" },
                { id: 203, text: "消息" },
                { id: 401, text: "未登录" },
                { id: 403, text: "无权操作" },
                { id: 404, text: "不存在" },
                { id: 423, text: "锁定" },
                { id: 500, text: "错误" }
            ],
            packLevel: [{ id: 1, text: "Core" }, { id: 10, text: "Framework" }, { id: 20, text: "Application" }, { id: 30, text: "Business" }]
        };
    }
    Object.defineProperty(OsharpService.prototype, "router", {
        get: function () {
            return this.injector.get(router_1.Router);
        },
        enumerable: false,
        configurable: true
    });
    // #region 工具方法
    /**
   * URL编码
   * @param url 待编码的URL
   */
    OsharpService.prototype.urlEncode = function (url) {
        return encodeURIComponent(url);
    };
    /**
   * URL解码
   * @param url 待解码的URL
   */
    OsharpService.prototype.urlDecode = function (url) {
        return decodeURIComponent(url);
    };
    /**
   * Base64字符串解码
   * @param base64 待编码的字符串
   */
    OsharpService.prototype.fromBase64 = function (base64) {
        return new buffer_1.Buffer(base64, "base64").toString();
    };
    /**
   * Base64字符串编码
   * @param str 待解码的Base64字符串
   */
    OsharpService.prototype.toBase64 = function (str) {
        return new buffer_1.Buffer(str).toString("base64");
    };
    /**
   * 获取URL中Hash串中的查询参数值
   * @param url URL字符串
   * @param name 参数名
   */
    OsharpService.prototype.getHashURLSearchParams = function (url, name) {
        if (url.indexOf("#") >= 0) {
            url = this.subStr(url, "#");
        }
        if (url.indexOf("?") >= 0) {
            url = this.subStr(url, "?");
        }
        var params = new URLSearchParams(url);
        return params.get(name);
    };
    /**
   * 提供首尾字符串截取中间的字符串
   * @param str 待截取的字符串
   * @param start 起始的字符串
   * @param end 结束的字符串
   */
    OsharpService.prototype.subStr = function (str, start, end) {
        if (start === void 0) { start = null; }
        if (end === void 0) { end = null; }
        var startIndex = 0;
        var endIndex = str.length;
        if (start) {
            startIndex = str.indexOf(start) + start.length;
        }
        if (end) {
            endIndex = str.indexOf(end);
        }
        return str.substr(startIndex, endIndex - startIndex);
    };
    /**
   * 从集合中删除符合条件的项
   * @param items 集合
   * @param exp 删除项查询表达式
   */
    OsharpService.prototype.remove = function (items, exp) {
        var index = items.findIndex(exp);
        items.splice(index, 1);
        return items;
    };
    /**
   * 值转文字
   * @param id 待转换的值
   * @param array 数据节点集合
   * @param defaultText 转换失败时的默认文字
   */
    OsharpService.prototype.valueToText = function (id, array, defaultText) {
        if (defaultText === void 0) { defaultText = null; }
        var text = defaultText == null ? id.toString() : defaultText;
        array.forEach(function (item) {
            if (item.id === id) {
                text = item.text;
                return false;
            }
            return true;
        });
        return text;
    };
    /**
   * 展开集合拼接字符串
   * @param array 待展开的集合
   * @param separator 分隔符
   */
    OsharpService.prototype.expandAndToString = function (array, separator) {
        if (separator === void 0) { separator = ","; }
        var result = "";
        if (!array || !array.length) {
            return result;
        }
        array.forEach(function (item) {
            result = result + item.toString() + separator;
        });
        return result.substr(0, result.length - separator.length);
    };
    /**
   * 下载数据
   * @param filename 存储的文件名
   * @param content 下载得到的内容
   */
    OsharpService.prototype.download = function (filename, content) {
        var urlObject = window.URL;
        var blob = new Blob([content]);
        var saveLink = document.createElement("a");
        saveLink.href = urlObject.createObjectURL(blob);
        saveLink.download = filename;
        var ev = document.createEvent("MouseEvents");
        ev.initMouseEvent("click", true, false, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
        saveLink.dispatchEvent(ev);
    };
    /**
   * 打开Email的网站
   * @param email Email地址
   */
    OsharpService.prototype.openMailSite = function (email) {
        var host = this.subStr(email, "@");
        var url = "http://mail." + host;
        window.open(url);
    };
    OsharpService.prototype.goto = function (url) {
        var _this = this;
        setTimeout(function () { return _this.router.navigateByUrl(url); });
    };
    /**
   * 处理Ajax结果
   * @param res HTTP响应
   * @param onSuccess 成功后的调用
   * @param onFail 失败后的调用
   */
    OsharpService.prototype.ajaxResult = function (res, onSuccess, onFail) {
        if (!res || !res.Type) {
            return;
        }
        var result = res;
        var type = result.Type;
        var content = result.Content;
        switch (type) {
            case osharp_model_1.AjaxResultType.Info:
                this.info(content);
                break;
            case osharp_model_1.AjaxResultType.NoFound:
                this.router.navigateByUrl("/nofound");
                break;
            case osharp_model_1.AjaxResultType.UnAuth:
                this.warning("用户未登录或登录已失效");
                this.router.navigateByUrl("/passport/login");
                break;
            case osharp_model_1.AjaxResultType.Success:
                this.success(content);
                if (onSuccess && typeof onSuccess === "function") {
                    onSuccess();
                }
                break;
            default:
                this.error(content);
                if (onFail && typeof onFail === "function") {
                    onFail();
                }
                break;
        }
    };
    /**
   * 处理Ajax错误
   * @param xhr 错误响应
   */
    OsharpService.prototype.ajaxError = function (xhr) {
        switch (xhr.status) {
            case 401:
                this.warning("用户未登录或登录已失效");
                this.router.navigateByUrl("/identity/login");
                break;
            case 404:
                this.router.navigateByUrl("/nofound");
                break;
            default:
                this.error("\u53D1\u751F\u9519\u8BEF\uFF1A" + xhr.status + ": " + xhr.statusText);
                break;
        }
    };
    OsharpService.prototype.remoteSFValidator = function (url, error) {
        var _this = this;
        clearTimeout(this.timeout1);
        return new rxjs_1.Observable(function (observer) {
            _this.timeout1 = setTimeout(function () {
                _this.http.get(url).subscribe(function (res) {
                    if (res !== true) {
                        observer.next([]);
                    }
                    else {
                        observer.next([error]);
                    }
                });
            }, 800);
        });
    };
    OsharpService.prototype.remoteInverseSFValidator = function (url, error) {
        var _this = this;
        clearTimeout(this.timeout2);
        return new rxjs_1.Observable(function (observer) {
            _this.timeout2 = setTimeout(function () {
                _this.http.get(url).subscribe(function (res) {
                    if (res !== true) {
                        observer.next([error]);
                    }
                    else {
                        observer.next([]);
                    }
                });
            }, 800);
        });
    };
    //#endregion
    //#region 验证码处理
    /**
   * 获取验证码
   */
    OsharpService.prototype.refreshVerifyCode = function () {
        var _this = this;
        var url = "api/common/verifycode";
        return this.http.get(url, null, { responseType: "text" }).map(function (res) {
            var str = _this.fromBase64(res.toString());
            var strs = str.split("#$#");
            var code = new osharp_model_1.VerifyCode();
            code.image = strs[0];
            code.id = strs[1];
            return code;
        });
    };
    //#endregion
    /**
   * 获取树节点集合
   * @param root 根节点
   * @param array 节点集合
   */
    OsharpService.prototype.getTreeNodes = function (root, array) {
        array.push(root);
        if (root.hasChildren) {
            for (var _i = 0, _a = root.Items; _i < _a.length; _i++) {
                var item = _a[_i];
                this.getTreeNodes(item, array);
            }
        }
    };
    /**
   * 检查URL的功能权限
   * @param url 要检查权限的后端URL
   */
    OsharpService.prototype.checkUrlAuth = function (url) {
        if (!url.startsWith("https:") && !url.startsWith("http") && !url.startsWith("/")) {
            url = "/" + url;
        }
        url = this.urlEncode(url);
        return this.http.get("api/auth/CheckUrlAuth?url=" + url).toPromise();
    };
    /**
   * 获取当前用户的权限点数据(string[])，如本地 ACLServer 中不存在，则从远程获取，并更新到 ACLServer 中
   */
    OsharpService.prototype.getAuthInfo = function (refresh) {
        var _this = this;
        if (!refresh && this.aclSrv.data.abilities && this.aclSrv.data.abilities.length) {
            var authInfo = this.aclSrv.data.abilities;
            return rxjs_1.of(authInfo);
        }
        var url = "api/auth/getauthinfo";
        return this.http.get(url).map(function (auth) {
            _this.aclSrv.setAbility(auth);
            return auth;
        });
    };
    OsharpService.prototype.getOperateEntries = function (operates) {
        return new linqts_1.List(operates).Select(function (m) { return new osharp_model_1.FilterOperateEntry(m); }).ToArray();
    };
    /**
   * 消息加载中
   * @param msg 消息字符串
   */
    OsharpService.prototype.loading = function (msg) {
        return this.msgSrv.loading(msg, this.msgOptions);
    };
    /**
   * 成功的消息
   * @param msg 消息字符串
   */
    OsharpService.prototype.success = function (msg) {
        return this.msgSrv.success(msg, this.msgOptions);
    };
    /**
   * 消息的消息
   * @param msg 消息字符串
   */
    OsharpService.prototype.info = function (msg) {
        return this.msgSrv.info(msg, this.msgOptions);
    };
    /**
   * 警告的消息
   * @param msg 消息字符串
   */
    OsharpService.prototype.warning = function (msg) {
        return this.msgSrv.warning(msg, this.msgOptions);
    };
    /**
   * 错误的消息
   * @param msg 消息字符串
   */
    OsharpService.prototype.error = function (msg) {
        return this.msgSrv.error(msg, {
            nzDuration: 1000 * 6,
            nzAnimate: true,
            nzPauseOnHover: true
        });
    };
    OsharpService = __decorate([
        core_1.Injectable({
            providedIn: "root"
        })
    ], OsharpService);
    return OsharpService;
}());
exports.OsharpService = OsharpService;
//#region 组件基类
/**
 * 组件基类，实现了权限控制
 */
var ComponentBase = /** @class */ (function () {
    function ComponentBase(injector) {
        /**
       * 权限字典，以模块代码为键，是否有权限为值
       */
        this.auth = {};
        this.authConfig = null;
        this.osharp = injector.get(OsharpService);
    }
    /**
   * 初始化并执行权限检查，检查结果存储到 this.auth 中
   */
    ComponentBase.prototype.checkAuth = function () {
        return __awaiter(this, void 0, void 0, function () {
            var position, codes, list, key, path;
            var _this = this;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        if (this.authConfig == null) {
                            this.authConfig = this.AuthConfig();
                            this.authConfig.funcs.forEach(function (key) { return (_this.auth[key] = true); });
                        }
                        position = this.authConfig.position;
                        return [4 /*yield*/, this.osharp.getAuthInfo().toPromise()];
                    case 1:
                        codes = _a.sent();
                        if (!codes) {
                            return [2 /*return*/, this.auth];
                        }
                        list = new linqts_1.List(codes);
                        for (key in this.auth) {
                            if (this.auth.hasOwnProperty(key)) {
                                path = key;
                                if (!path.startsWith("Root.")) {
                                    path = position + "." + path;
                                }
                                this.auth[key] = list.Contains(path);
                            }
                        }
                        return [2 /*return*/, this.auth];
                }
            });
        });
    };
    return ComponentBase;
}());
exports.ComponentBase = ComponentBase;
//#endregion
