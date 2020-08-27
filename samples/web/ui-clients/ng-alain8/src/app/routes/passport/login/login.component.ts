import { SettingsService, _HttpClient } from "@delon/theme";
import { Component, OnDestroy, Inject, Optional, Injector } from "@angular/core";
import { Router } from "@angular/router";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { NzMessageService, NzModalService } from "ng-zorro-antd";
import { SocialService, SocialOpenType, ITokenService, DA_SERVICE_TOKEN } from "@delon/auth";
import { ReuseTabService } from "@delon/abc";
import { StartupService } from "@core";
import { ComponentBase } from "@shared/osharp/services/osharp.service";
import { AuthConfig, LoginDto, AjaxResultType, TokenDto } from "@shared/osharp/osharp.model";
import { IdentityService } from "@shared/osharp/services/identity.service";

@Component({
	selector: "passport-login",
	templateUrl: "./login.component.html",
	styleUrls: [ "./login.component.less" ],
	providers: [ SocialService ]
})
export class LoginComponent extends ComponentBase implements OnDestroy {
	constructor(
		fb: FormBuilder,
		modalSrv: NzModalService,
		private router: Router,
		private settingsService: SettingsService,
		private socialService: SocialService,
		@Optional()
		@Inject(ReuseTabService)
		@Inject(DA_SERVICE_TOKEN)
		private tokenService: ITokenService,
		private startupSrv: StartupService,
		public msg: NzMessageService,
		private identity: IdentityService,
		injector: Injector
	) {
		super(injector);
		super.checkAuth();
		this.form = fb.group({
			userName: [ null, [ Validators.required, Validators.minLength(4) ] ],
			password: [ null, Validators.required ],
			mobile: [ null, [ Validators.required, Validators.pattern(/^1\d{10}$/) ] ],
			captcha: [ null, [ Validators.required ] ],
			remember: [ true ]
		});
		modalSrv.closeAll();
	}

	// #region fields

	get http() {
		return this.identity.http;
	}

	get userName() {
		return this.form.controls.userName;
	}
	get password() {
		return this.form.controls.password;
	}
	get mobile() {
		return this.form.controls.mobile;
	}
	get captcha() {
		return this.form.controls.captcha;
	}

	form: FormGroup;
	error = "";
	type = 0;
	resendConfirmMail = false;

	// #region get captcha

	count = 0;
	interval$: any;

	protected AuthConfig(): AuthConfig {
		return new AuthConfig("Root.Site.Identity", [ "Login", "Jwtoken", "Register", "SendResetPasswordMail", "SendConfirmMail" ]);
	}

	// #endregion

	switch(ret: any) {
		this.type = ret.index;
	}

	getCaptcha() {
		if (this.mobile.invalid) {
			this.mobile.markAsDirty({ onlySelf: true });
			this.mobile.updateValueAndValidity({ onlySelf: true });
			return;
		}
		this.count = 59;
		this.interval$ = setInterval(() => {
			this.count -= 1;
			if (this.count <= 0) clearInterval(this.interval$);
		}, 1000);
	}

	// #endregion

	submit() {
		this.error = "";

		const dto: LoginDto = new LoginDto();
		dto.Type = this.type;
		if (this.type === 0) {
			this.userName.markAsDirty();
			this.userName.updateValueAndValidity();
			this.password.markAsDirty();
			this.password.updateValueAndValidity();
			if (this.userName.invalid || this.password.invalid) return;
			dto.Account = this.userName.value;
			dto.Password = this.password.value;
		} else {
			this.mobile.markAsDirty();
			this.mobile.updateValueAndValidity();
			this.captcha.markAsDirty();
			this.captcha.updateValueAndValidity();
			if (this.mobile.invalid || this.captcha.invalid) return;
			dto.Account = this.mobile.value;
			dto.Password = this.captcha.value;
		}

		const tokenDto: TokenDto = { GrantType: "password", Account: dto.Account, Password: dto.Password };
		this.identity
			.token(tokenDto)
			.then((result) => {
				if (result.Type === AjaxResultType.Success) {
					this.msg.success("用户登录成功");
					// 重新获取 StartupService 内容，我们始终认为应用信息一般都会受当前用户授权范围而影响
					this.startupSrv.load().then(() => {
						let url = (this.tokenService.referrer && this.tokenService.referrer.url) || "/";
						if (url.includes("/passport")) url = "/";
						this.router.navigateByUrl(url);
					});
					return;
				}
				this.error = `登录失败：${result.Content}`;
				this.resendConfirmMail = result.Content.indexOf("邮箱未验证") > -1;
			})
			.catch((e) => {
				this.error = `发生错误：${e.statusText}`;
			});

		/*
        // 默认配置中对所有HTTP请求都会强制 [校验](https://ng-alain.com/auth/getting-started) 用户 Token
        // 然一般来说登录请求不需要校验，因此可以在请求URL加上：`/login?_allow_anonymous=true` 表示不触发用户 Token 校验
        this.http
          .post('/login/account?_allow_anonymous=true', {
            type: this.type,
            userName: this.userName.value,
            password: this.password.value,
          })
          .subscribe((res: any) => {
            if (res.msg !== 'ok') {
              this.error = res.msg;
              return;
            }
            // 清空路由复用信息
            this.reuseTabService.clear();
            // 设置用户Token信息
            this.tokenService.set(res.user);
            // 重新获取 StartupService 内容，我们始终认为应用信息一般都会受当前用户授权范围而影响
            this.startupSrv.load().then(() => {
              let url = this.tokenService.referrer.url || '/';
              if (url.includes('/passport')) url = '/';
              this.router.navigateByUrl(url);
            });
          });*/
	}

	// #region social

	open(type: string, openType: SocialOpenType = "href") {
		const callback = `/#/callback/${type}`;
		const url = `api/identity/OAuth2?provider=${type}&returnUrl=${this.osharp.urlEncode(callback)}`;

		// switch (type) {
		//   case 'QQ':
		//     url = 'api/identity/OAuth2?provider=';
		//     break;
		//   case 'auth0':
		//     url = `//cipchk.auth0.com/login?client=8gcNydIDzGBYxzqV0Vm1CX_RXH-wsWo5&redirect_uri=${decodeURIComponent(
		//       callback,
		//     )}`;
		//     break;
		//   case 'github':
		//     url = `//github.com/login/oauth/authorize?client_id=9d6baae4b04a23fcafa2&response_type=code&redirect_uri=${decodeURIComponent(
		//       callback,
		//     )}`;
		//     break;
		//   case 'weibo':
		//     url = `https://api.weibo.com/oauth2/authorize?client_id=1239507802&response_type=code&redirect_uri=${decodeURIComponent(
		//       callback,
		//     )}`;
		//     break;
		// }
		if (openType === "window") {
			this.socialService
				.login(url, "/", {
					type: "window"
				})
				.subscribe((res) => {
					if (res) {
						this.settingsService.setUser(res);
						this.router.navigateByUrl("/");
					}
				});
		} else {
			this.socialService.login(url, "/", {
				type: "href"
			});
		}
	}

	// #endregion

	ngOnDestroy(): void {
		if (this.interval$) clearInterval(this.interval$);
	}
}
