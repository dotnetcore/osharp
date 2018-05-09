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

export class SendMailDto{
  Email:string;
  VerifyCode:string;
}

export class ResetPasswordDto {
  UserId: string;
  Token: string;
  NewPassword: string;
  ConfirmPassword: string;
}
