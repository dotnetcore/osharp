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
export class OnlineUser {
  UserId: number;
  UserName: string;
  NickName: string;
  Email: string;
  Roles: string[];
  SessionId: string;
}
