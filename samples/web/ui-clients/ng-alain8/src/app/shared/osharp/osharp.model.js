"use strict";
exports.__esModule = true;
exports.AdResult = exports.InstallDto = exports.User = exports.AuthConfig = exports.UserLoginInfoEx = exports.ProfileEditDto = exports.ResetPasswordDto = exports.SendMailDto = exports.ConfirmEmailDto = exports.ChangePasswordDto = exports.RegisterDto = exports.LocalTokenModel = exports.JsonWebToken = exports.TokenDto = exports.LoginDto = exports.VerifyCode = exports.EntityProperty = exports.ListSortDirection = exports.SortCondition = exports.PageCondition = exports.PageRequest = exports.FilterOperateEntry = exports.FilterOperate = exports.FilterGroup = exports.FilterRule = exports.ListNode = exports.PageData = exports.AjaxResultType = exports.AjaxResult = void 0;
//#region OSharp Tools
var AjaxResult = /** @class */ (function () {
    function AjaxResult() {
    }
    return AjaxResult;
}());
exports.AjaxResult = AjaxResult;
var AjaxResultType;
(function (AjaxResultType) {
    AjaxResultType[AjaxResultType["Info"] = 203] = "Info";
    AjaxResultType[AjaxResultType["Success"] = 200] = "Success";
    AjaxResultType[AjaxResultType["Error"] = 500] = "Error";
    AjaxResultType[AjaxResultType["UnAuth"] = 401] = "UnAuth";
    AjaxResultType[AjaxResultType["Forbidden"] = 403] = "Forbidden";
    AjaxResultType[AjaxResultType["NoFound"] = 404] = "NoFound";
    AjaxResultType[AjaxResultType["Locked"] = 423] = "Locked";
})(AjaxResultType = exports.AjaxResultType || (exports.AjaxResultType = {}));
/**  分页数据 */
var PageData = /** @class */ (function () {
    function PageData() {
    }
    return PageData;
}());
exports.PageData = PageData;
var ListNode = /** @class */ (function () {
    function ListNode() {
    }
    return ListNode;
}());
exports.ListNode = ListNode;
/** 查询条件 */
var FilterRule = /** @class */ (function () {
    /**
     * 实例化一个条件信息
     * @param field 字段名
     * @param value 属性值
     * @param operate 对比操作
     */
    function FilterRule(field, value, operate) {
        if (operate === void 0) { operate = FilterOperate.Equal; }
        this.Field = field;
        this.Value = value;
        this.Operate = operate;
    }
    return FilterRule;
}());
exports.FilterRule = FilterRule;
/**  查询条件组 */
var FilterGroup = /** @class */ (function () {
    function FilterGroup() {
        /** 条件集合 */
        this.Rules = [];
        /** 条件间操作 */
        this.Operate = FilterOperate.And;
        /** 条件组集合 */
        this.Groups = [];
        this.Level = 1;
    }
    FilterGroup.Init = function (group) {
        if (!group.Level) {
            group.Level = 1;
        }
        group.Groups.forEach(function (subGroup) {
            subGroup.Level = group.Level + 1;
            FilterGroup.Init(subGroup);
        });
    };
    return FilterGroup;
}());
exports.FilterGroup = FilterGroup;
/** 比较操作枚举 */
var FilterOperate;
(function (FilterOperate) {
    FilterOperate[FilterOperate["And"] = 1] = "And";
    FilterOperate[FilterOperate["Or"] = 2] = "Or";
    FilterOperate[FilterOperate["Equal"] = 3] = "Equal";
    FilterOperate[FilterOperate["NotEqual"] = 4] = "NotEqual";
    FilterOperate[FilterOperate["Less"] = 5] = "Less";
    FilterOperate[FilterOperate["LessOrEqual"] = 6] = "LessOrEqual";
    FilterOperate[FilterOperate["Greater"] = 7] = "Greater";
    FilterOperate[FilterOperate["GreaterOrEqual"] = 8] = "GreaterOrEqual";
    FilterOperate[FilterOperate["StartsWith"] = 9] = "StartsWith";
    FilterOperate[FilterOperate["EndsWith"] = 10] = "EndsWith";
    FilterOperate[FilterOperate["Contains"] = 11] = "Contains";
    FilterOperate[FilterOperate["NotContains"] = 12] = "NotContains";
})(FilterOperate = exports.FilterOperate || (exports.FilterOperate = {}));
var FilterOperateEntry = /** @class */ (function () {
    function FilterOperateEntry(operate) {
        this.Operate = operate;
        switch (operate) {
            case FilterOperate.And:
                this.Display = '并且';
                break;
            case FilterOperate.Or:
                this.Display = '或者';
                break;
            case FilterOperate.Equal:
                this.Display = '等于';
                break;
            case FilterOperate.NotEqual:
                this.Display = '不等于';
                break;
            case FilterOperate.Less:
                this.Display = '小于';
                break;
            case FilterOperate.LessOrEqual:
                this.Display = '小于等于';
                break;
            case FilterOperate.Greater:
                this.Display = '大于';
                break;
            case FilterOperate.GreaterOrEqual:
                this.Display = '大于等于';
                break;
            case FilterOperate.StartsWith:
                this.Display = '开始于';
                break;
            case FilterOperate.EndsWith:
                this.Display = '结束于';
                break;
            case FilterOperate.Contains:
                this.Display = '包含';
                break;
            case FilterOperate.NotContains:
                this.Display = '不包含';
                break;
            default:
                this.Display = '未知操作';
                break;
        }
        this.Display = operate + "." + this.Display;
    }
    return FilterOperateEntry;
}());
exports.FilterOperateEntry = FilterOperateEntry;
/**  分页请求 */
var PageRequest = /** @class */ (function () {
    function PageRequest() {
        /**  分页条件信息 */
        this.PageCondition = new PageCondition();
        /**  查询条件组 */
        this.FilterGroup = new FilterGroup();
    }
    return PageRequest;
}());
exports.PageRequest = PageRequest;
/**  分页条件 */
var PageCondition = /** @class */ (function () {
    function PageCondition() {
        /**  页序 */
        this.PageIndex = 1;
        /**  分页大小 */
        this.PageSize = 20;
        /**  排序条件集合 */
        this.SortConditions = [];
    }
    return PageCondition;
}());
exports.PageCondition = PageCondition;
var SortCondition = /** @class */ (function () {
    function SortCondition() {
    }
    return SortCondition;
}());
exports.SortCondition = SortCondition;
var ListSortDirection;
(function (ListSortDirection) {
    ListSortDirection[ListSortDirection["Ascending"] = 0] = "Ascending";
    ListSortDirection[ListSortDirection["Descending"] = 1] = "Descending";
})(ListSortDirection = exports.ListSortDirection || (exports.ListSortDirection = {}));
/**  实体属性信息 */
var EntityProperty = /** @class */ (function () {
    function EntityProperty() {
    }
    return EntityProperty;
}());
exports.EntityProperty = EntityProperty;
/**
 * 验证码类
 */
var VerifyCode = /** @class */ (function () {
    function VerifyCode() {
        /**  验证码图片的Base64格式 */
        this.image = 'data:image/png;base64,null';
    }
    return VerifyCode;
}());
exports.VerifyCode = VerifyCode;
//#endregion
//#region Identity Model
var LoginDto = /** @class */ (function () {
    function LoginDto() {
        this.Remember = true;
    }
    return LoginDto;
}());
exports.LoginDto = LoginDto;
var TokenDto = /** @class */ (function () {
    function TokenDto() {
    }
    return TokenDto;
}());
exports.TokenDto = TokenDto;
var JsonWebToken = /** @class */ (function () {
    function JsonWebToken() {
    }
    return JsonWebToken;
}());
exports.JsonWebToken = JsonWebToken;
var LocalTokenModel = /** @class */ (function () {
    function LocalTokenModel() {
    }
    return LocalTokenModel;
}());
exports.LocalTokenModel = LocalTokenModel;
var RegisterDto = /** @class */ (function () {
    function RegisterDto() {
    }
    return RegisterDto;
}());
exports.RegisterDto = RegisterDto;
var ChangePasswordDto = /** @class */ (function () {
    function ChangePasswordDto() {
    }
    return ChangePasswordDto;
}());
exports.ChangePasswordDto = ChangePasswordDto;
var ConfirmEmailDto = /** @class */ (function () {
    function ConfirmEmailDto() {
    }
    return ConfirmEmailDto;
}());
exports.ConfirmEmailDto = ConfirmEmailDto;
var SendMailDto = /** @class */ (function () {
    function SendMailDto() {
    }
    return SendMailDto;
}());
exports.SendMailDto = SendMailDto;
var ResetPasswordDto = /** @class */ (function () {
    function ResetPasswordDto() {
    }
    return ResetPasswordDto;
}());
exports.ResetPasswordDto = ResetPasswordDto;
var ProfileEditDto = /** @class */ (function () {
    function ProfileEditDto() {
    }
    return ProfileEditDto;
}());
exports.ProfileEditDto = ProfileEditDto;
var UserLoginInfoEx = /** @class */ (function () {
    function UserLoginInfoEx(key) {
        this.ProviderKey = key;
    }
    return UserLoginInfoEx;
}());
exports.UserLoginInfoEx = UserLoginInfoEx;
/**  权限配置信息 */
var AuthConfig = /** @class */ (function () {
    function AuthConfig(
    /**  当前模块的位置，即上级模块的路径，如Root,Root.Admin,Root.Admin.Identity */
    position, 
    /**  要权限控制的功能名称，可以是节点名称或全路径 */
    funcs) {
        this.position = position;
        this.funcs = funcs;
    }
    return AuthConfig;
}());
exports.AuthConfig = AuthConfig;
/**  用户信息 */
var User = /** @class */ (function () {
    function User() {
        this.roles = [];
    }
    return User;
}());
exports.User = User;
//#endregion
//#region system
/**
 * 系统初始化安装DTO
 */
var InstallDto = /** @class */ (function () {
    function InstallDto() {
    }
    return InstallDto;
}());
exports.InstallDto = InstallDto;
//#endregion
//#region delon
var AdResult = /** @class */ (function () {
    function AdResult() {
        /**
         * 是否显示结果框
         */
        this.show = false;
    }
    return AdResult;
}());
exports.AdResult = AdResult;
//#endregion
