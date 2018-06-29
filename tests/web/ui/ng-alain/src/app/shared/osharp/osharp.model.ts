import { Buffer } from "buffer";

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
  Locked = 423
}

/** 分页数据 */
export class PageData<T> {
  rows: T[];
  total: number;
}
export class ListNode {
  id: number;
  text: string;
}

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
  rules: Rule[] = [];
  operate = 'and';
  groups: Group[] = [];
}
/**
 * 验证码类
 */
export class VerifyCode {
  /** 验证码后台编号 */
  id: string;
  /** 验证码图片的Base64格式 */
  image: string = "data:image/png;base64,null";
  /** 输入的验证码 */
  code: string;
}

//#endregion

//#region Identity Model
export class LoginDto {
  Account: string;
  Password: string;
  VerifyCode: string;
  VerifyCodeId: string;
  Remember = true;
  ReturnUrl: string;
}
export class RegisterDto {
  UserName: string;
  Password: string;
  ConfirmPassword: string;
  NickName: string;
  Email: string;
  VerifyCode: string;
  VerifyCodeId: string;
}
export class ChangePasswordDto {
  UserId: string;
  OldPassword: string;
  NewPassword: string;
  ConfirmPassword: string;
}
export class ConfirmEmailDto {
  UserId: string;
  Code: string;
}
export class SendMailDto {
  Email: string;
  VerifyCode: string;
  VerifyCodeId: string;
}
export class ResetPasswordDto {
  UserId: string;
  Token: string;
  NewPassword: string;
  ConfirmPassword: string;
}
/**权限配置信息 */
export class AuthConfig {
  constructor(
    /**当前模块的位置，即上级模块的路径，如Root,Root.Admin,Root.Admin.Identity */
    public position: string,
    /**要权限控制的功能名称，可以是节点名称或全路径 */
    public funcs: string[]
  ) { }
}

/**
 * 用户信息
 */
export class User {
  Id: number;
  UserName: string;
  NickName: string;
  Email: string;
  SecurityStamp: string;
  IsAdmin: boolean;
  Roles: string[];
  IssuedAt: Date;
  NotBefore: Date;
  Expires: Date;

  constructor(token: string) {
    if (token == null) {
      return;
    }
    const strs: string[] = token.split(".");
    if (strs.length != 3) {
      return;
    }
    const json = this.fromBase64(strs[1]);
    const obj = JSON.parse(json);

    this.Id = obj['nameid'] || 0;
    this.UserName = obj['unique_name'] || null;
    this.NickName = obj['given_name'] || null;
    this.Email = obj['email'] || null;
    this.SecurityStamp = obj['security-stamp'] || null;
    this.IsAdmin = obj['is-admin'] != undefined ? JSON.parse(obj['is-admin']) : false;
    this.Roles = obj['role'] != undefined ? obj['role'].split(',') : [];
    this.IssuedAt = obj['iat'] != undefined ? new Date(obj['iat'] * 1000) : null;
    this.NotBefore = obj['nbf'] != undefined ? new Date(obj['nbf'] * 1000) : null;
    this.Expires = obj['exp'] != undefined ? new Date(obj['exp'] * 1000) : null;
  }

  private fromBase64(base64: string): string {
    return new Buffer(base64, 'base64').toString();
  }
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
  show: boolean = false;
  /**结果类型，可选为： 'success' | 'error' | 'minus-circle-o'*/
  type: 'success' | 'error' | 'minus-circle-o';
  /** 结果标题 */
  title: string;
  /** 结果描述 */
  description: string;
}

//#endregion
