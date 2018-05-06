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
  constructor(public UserId: number,
    public OldPassword: string,
    public NewPassword: string,
    public ConfirmNewPassword: string) { }
}

export class ResetPasswordDto {
  constructor(public UserId: number, public Token: string, public NewPassword: string) { }
}
