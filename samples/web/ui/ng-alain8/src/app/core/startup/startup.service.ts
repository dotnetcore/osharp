import { Injectable, Injector, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { zip } from "rxjs";
import { catchError } from "rxjs/operators";
import { MenuService, SettingsService, TitleService, ALAIN_I18N_TOKEN, _HttpClient, User } from "@delon/theme";
import { ACLService } from "@delon/acl";
import { TranslateService } from "@ngx-translate/core";
import { I18NService } from "../i18n/i18n.service";

import { NzIconService } from "ng-zorro-antd/icon";
import { ICONS_AUTO } from "../../../style-icons-auto";
import { ICONS } from "../../../style-icons";
import { IdentityService } from "@shared/osharp/services/identity.service";

import Heartbeats from "heartbeats";

/**
 * Used for application startup
 * Generally used to get the basic data of the application, like: Menu Data, User Data, etc.
 */
@Injectable({
	providedIn: "root"
})
export class StartupService {
	constructor(
		iconSrv: NzIconService,
		private injector: Injector,
		private menuService: MenuService,
		private translate: TranslateService,
		@Inject(ALAIN_I18N_TOKEN) private i18n: I18NService,
		private settingService: SettingsService,
		private aclService: ACLService,
		private titleService: TitleService,
		private httpClient: HttpClient
	) {
		iconSrv.addIcon(...ICONS_AUTO, ...ICONS);
	}

	private get identity(): IdentityService {
		return this.injector.get(IdentityService);
	}

	// 心跳业务
	private viaHeartbeats() {
		const heart = Heartbeats.createHeart(1000);
		// 每5秒检测AccessToken有效期，如过期则使用RefreshToken来刷新
		heart.createEvent(5, () => {
			if (!this.identity) {
				return;
			}
			this.identity.tryRefreshToken();
		});
	}

	private viaHttp(resolve: any) {
		zip(
			this.httpClient.get(`assets/osharp/i18n/${this.i18n.defaultLang}.json`),
			this.httpClient.get("assets/osharp/app-data.json"),
			this.httpClient.get("api/common/systeminfo"),
			this.httpClient.get("api/auth/getauthinfo"), // 获取当前用户的权限点string[]
			this.httpClient.get("api/identity/profile") // 获取用户信息
		)
			.pipe(
				catchError(([ langData, appData, sysInfo, authInfo, userInfo ]) => {
					resolve(null);
					return [ langData, appData, sysInfo, authInfo, userInfo ];
				})
			)
			.subscribe(
				([ langData, appData, sysInfo, authInfo, userInfo ]) => {
					// Setting language data
					this.translate.setTranslation(this.i18n.defaultLang, langData);
					this.translate.setDefaultLang(this.i18n.defaultLang);

					// Application data
					const res: any = appData;
					if (res && res.app) {
						res.app.cliVersion = sysInfo.Object.CliVersion;
						res.app.osharpVersion = sysInfo.Object.OSharpVersion;
					}
					// Application information: including site name, description, year
					this.settingService.setApp(res.app);
					// User information: including name, avatar, email address
					if (userInfo && userInfo.UserName) {
						let user: User = { name: userInfo.UserName, avatar: userInfo.HeadImg, email: userInfo.Email, nickName: userInfo.NickName };
						this.settingService.setUser(user);
					}
					// ACL: Set the permissions to full, https://ng-alain.com/acl/getting-started
					// this.aclService.setFull(true);
					this.aclService.setAbility(authInfo);
					// Menu data, https://ng-alain.com/theme/menu
					this.menuService.add(res.menu);
					// Can be set page suffix title, https://ng-alain.com/theme/title
					this.titleService.suffix = res.app.name;
				},
				() => {},
				() => {
					resolve(null);
				}
			);
	}

	load(): Promise<any> {
		// only works with promises
		// https://github.com/angular/angular/issues/15088
		return new Promise((resolve, reject) => {
			// http
			this.viaHttp(resolve);

			// heartbeats
			this.viaHeartbeats();
		});
	}
}
