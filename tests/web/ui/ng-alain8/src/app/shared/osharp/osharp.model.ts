import { User as NzUser } from '@delon/theme';
import { JWTTokenModel } from '@delon/auth';

//#region OSharp Tools
export class AjaxResult {
  Type: AjaxResultType;
  Content?: string;
  Data?: any;
}
export enum AjaxResultType {
  Info = 203,
  Success = 200,
  Error = 500,
  UnAuth = 401,
  Forbidden = 403,
  NoFound = 404,
  Locked = 423,
}

/**  分页数据 */
export class PageData<T> {
  /**  数据行 */
  Rows: T[];
  /**  总数据量 */
  Total: number;
}
export class ListNode {
  id: number;
  text: string;
}

/** 查询条件 */
export class FilterRule {
  /** 属性名 */
  Field: string;
  /** 属性值 */
  Value: any;
  /** 对比操作 */
  Operate: FilterOperate;

  /**
   * 实例化一个条件信息
   * @param field 字段名
   * @param value 属性值
   * @param operate 对比操作
   */
  constructor(
    field: string,
    value: string,
    operate: FilterOperate = FilterOperate.Equal,
  ) {
    this.Field = field;
    this.Value = value;
    this.Operate = operate;
  }
  [key: string]: any;
}
/**  查询条件组 */
export class FilterGroup {
  /** 条件集合 */
  Rules: FilterRule[] = [];
  /** 条件间操作 */
  Operate: FilterOperate = FilterOperate.And;
  /** 条件组集合 */
  Groups: FilterGroup[] = [];
  Level = 1;

  static Init(group: FilterGroup) {
    if (!group.Level) {
      group.Level = 1;
    }
    group.Groups.forEach(subGroup => {
      subGroup.Level = group.Level + 1;
      FilterGroup.Init(subGroup);
    });
  }
}
/** 比较操作枚举 */
export enum FilterOperate {
  And = 1,
  Or = 2,
  Equal = 3,
  NotEqual = 4,
  Less = 5,
  LessOrEqual = 6,
  Greater = 7,
  GreaterOrEqual = 8,
  StartsWith = 9,
  EndsWith = 10,
  Contains = 11,
  NotContains = 12,
}
export class FilterOperateEntry {
  Operate: FilterOperate;
  Display: string;

  constructor(operate: FilterOperate) {
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
    this.Display = `${operate as number}.${this.Display}`;
  }
}
/**  分页请求 */
export class PageRequest {
  /**  分页条件信息 */
  PageCondition: PageCondition = new PageCondition();
  /**  查询条件组 */
  FilterGroup: FilterGroup = new FilterGroup();
}
/**  分页条件 */
export class PageCondition {
  /**  页序 */
  PageIndex = 1;
  /**  分页大小 */
  PageSize = 20;
  /**  排序条件集合 */
  SortConditions: SortCondition[] = [];
}
export class SortCondition {
  SortField: string;
  ListSortDirection: ListSortDirection;
}
export enum ListSortDirection {
  Ascending,
  Descending,
}

/**  实体属性信息 */
export class EntityProperty {
  Name: string;
  Display: string;
  TypeName: string;
  IsUserFlag: boolean;
  ValueRange: any[];
}

/**
 * 验证码类
 */
export class VerifyCode {
  /**  验证码后台编号 */
  id: string;
  /**  验证码图片的Base64格式 */
  image = 'data:image/png;base64,null';
  /**  输入的验证码 */
  code: string;
}

//#endregion

//#region Identity Model
export class LoginDto {
  Type: number;
  Account: string;
  Password: string;
  VerifyCode: string;
  VerifyCodeId: string;
  Remember = true;
  ReturnUrl: string;
}
export class TokenDto {
  GrantType: 'password' | 'refresh_token';
  Account?: string;
  Password?: string;
  VerifyCode?: string;
  RefreshToken?: string;
}
export class JsonWebToken {
  AccessToken: string;
  RefreshToken: string;
  RefreshUctExpires: number;
}
export class LocalTokenModel {
  AccessToken: JWTTokenModel;
  RefreshToken: string;
}
export class RegisterDto {
  Email?: string;
  Password?: string;
  ConfirmPassword?: string;
  VerifyCode?: string;
  VerifyCodeId?: string;
}
export class ChangePasswordDto {
  UserId?: string;
  OldPassword?: string;
  NewPassword?: string;
  ConfirmNewPassword?: string;
}
export class ConfirmEmailDto {
  UserId: string;
  Code: string;
}
export class SendMailDto {
  Email?: string;
  VerifyCode?: string;
  VerifyCodeId?: string;
}
export class ResetPasswordDto {
  UserId?: string;
  Token?: string;
  NewPassword?: string;
  ConfirmPassword?: string;
}
export class ProfileEditDto {
  Id: number;
  UserName: string;
  NickName: string;
  Email?: string;
  HeadImg?: string;
}
export class UserLoginInfoEx {
  constructor(key: string) {
    this.ProviderKey = key;
  }
  ProviderKey?: string;
  Email?: string;
  Password?: string;
}
/**  权限配置信息 */
export class AuthConfig {
  constructor(
    /**  当前模块的位置，即上级模块的路径，如Root,Root.Admin,Root.Admin.Identity */
    public position: string,
    /**  要权限控制的功能名称，可以是节点名称或全路径 */
    public funcs: string[],
  ) { }
}

/**  用户信息 */
export class User implements NzUser {
  constructor() {
    this.roles = [];
  }
  id?: number;
  name?: string;
  avatar?: string;
  email?: string;
  [key: string]: any;
  nickName?: string;
  roles?: string[];
  isAdmin?: boolean;
}

//#endregion

//#region system

/**
 * 系统初始化安装DTO
 */
export class InstallDto {
  SiteName: string;
  SiteDescription: string;
  AdminUserName: string;
  AdminPassword: string;
  ConfirmPassword: string;
  AdminEmail: string;
  AdminNickName: string;
}

//#endregion

//#region delon

export class AdResult {
  /**
   * 是否显示结果框
   */
  show = false;
  /**  结果类型，可选为： 'success' | 'error' | 'minus-circle-o' */
  type: 'success' | 'error' | 'minus-circle-o';
  /**  结果标题 */
  title: string;
  /**  结果描述 */
  description: string;
}

//#endregion
