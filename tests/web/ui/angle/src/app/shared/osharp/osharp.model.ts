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
export class PageData<T>{
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
  rules: Rule[] = []
  operate: string = 'and';
  groups: Group[] = [];
}


//Identity Model
export class LoginDto {
  Account: string;
  Password: string;
  VerifyCode: string;
  Remember: boolean;
  ReturnUrl: string;
}
export class RegisterDto {
  UserName: string;
  Password: string;
  ConfirmPassword: string;
  NickName: string;
  Email: string;
  VerifyCode: string;
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
}
export class ResetPasswordDto {
  UserId: string;
  Token: string;
  NewPassword: string;
  ConfirmPassword: string;
}

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
    let strs: string[] = token.split(".");
    if (strs.length != 3) {
      return;
    }
    let json = this.fromBase64(strs[1]);
    let obj = JSON.parse(json);

    this.Id = obj['nameid'] || 0;
    this.UserName = obj['unique_name'] || null;
    this.NickName = obj['given_name'] || null;
    this.Email = obj['email'] || null;
    this.SecurityStamp = obj['security-stamp'] || null;
    this.IsAdmin = obj['is-admin'] != undefined ? obj['is-admin'] : false;
    this.Roles = obj['role'] != undefined ? obj['role'].split(',') : [];
    this.IssuedAt = obj['iat'] != undefined ? new Date(obj['iat'] * 1000) : null;
    this.NotBefore = obj['nbf'] != undefined ? new Date(obj['nbf'] * 1000) : null;
    this.Expires = obj['exp'] != undefined ? new Date(obj['exp'] * 1000) : null;
  }

  private fromBase64(base64: string): string {
    return new Buffer(base64, 'base64').toString();
  }
}
