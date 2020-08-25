import { Injectable, Injector } from "@angular/core";
import { ListNode, AjaxResult, AjaxResultType, AuthConfig, VerifyCode, FilterOperate, FilterOperateEntry } from "@shared/osharp/osharp.model";
import { NzMessageService, NzMessageDataOptions } from "ng-zorro-antd";
import { Router } from "@angular/router";
import { Buffer } from "buffer";
import { Observable, of } from "rxjs";
import { List } from "linqts";
import { _HttpClient } from "@delon/theme";
import { ACLService } from "@delon/acl";
import { ErrorData } from "@delon/form";

@Injectable({
	providedIn: "root"
})
export class OsharpService {
	constructor(private injector: Injector, public msgSrv: NzMessageService, public http: _HttpClient, private aclSrv: ACLService) {}

	public get router(): Router {
		return this.injector.get(Router);
	}

	//#region 远程验证

	private timeout1;

	private timeout2;

	// #endregion

	// #region 消息方法

	private msgOptions: NzMessageDataOptions = {
		nzDuration: 1000 * 3,
		nzAnimate: true,
		nzPauseOnHover: true
	};

	// #endregion

	// #region 静态数据

	data = {
		accessType: [ { id: 0, text: "匿名访问" }, { id: 1, text: "登录访问" }, { id: 2, text: "角色访问" } ],
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
		dataAuthOperations: [ { id: 0, text: "读取" }, { id: 1, text: "更新" }, { id: 2, text: "删除" } ],
		operateType: [ { id: 1, text: "新增" }, { id: 2, text: "更新" }, { id: 3, text: "删除" } ],
		ajaxResultType: [
			{ id: 200, text: "成功" },
			{ id: 203, text: "消息" },
			{ id: 401, text: "未登录" },
			{ id: 403, text: "无权操作" },
			{ id: 404, text: "不存在" },
			{ id: 423, text: "锁定" },
			{ id: 500, text: "错误" }
		],
		packLevel: [ { id: 1, text: "Core" }, { id: 10, text: "Framework" }, { id: 20, text: "Application" }, { id: 30, text: "Business" } ]
	};

	// #region 工具方法

	/**
   * URL编码
   * @param url 待编码的URL
   */
	urlEncode(url: string): string {
		return encodeURIComponent(url);
	}
	/**
   * URL解码
   * @param url 待解码的URL
   */
	urlDecode(url: string): string {
		return decodeURIComponent(url);
	}

	/**
   * Base64字符串解码
   * @param base64 待编码的字符串
   */
	fromBase64(base64: string): string {
		return new Buffer(base64, "base64").toString();
	}
	/**
   * Base64字符串编码
   * @param str 待解码的Base64字符串
   */
	toBase64(str: string): string {
		return new Buffer(str).toString("base64");
	}
	/**
   * 获取URL中Hash串中的查询参数值
   * @param url URL字符串
   * @param name 参数名
   */
	getHashURLSearchParams(url: string, name: string): string {
		if (url.indexOf("#") >= 0) {
			url = this.subStr(url, "#");
		}
		if (url.indexOf("?") >= 0) {
			url = this.subStr(url, "?");
		}
		const params = new URLSearchParams(url);
		return params.get(name);
	}
	/**
   * 提供首尾字符串截取中间的字符串
   * @param str 待截取的字符串
   * @param start 起始的字符串
   * @param end 结束的字符串
   */
	subStr(str: string, start: string = null, end: string = null): string {
		let startIndex = 0;
		let endIndex = str.length;
		if (start) {
			startIndex = str.indexOf(start) + start.length;
		}
		if (end) {
			endIndex = str.indexOf(end);
		}
		return str.substr(startIndex, endIndex - startIndex);
	}
	/**
   * 从集合中删除符合条件的项
   * @param items 集合
   * @param exp 删除项查询表达式
   */
	remove<T>(items: Array<T>, exp: (value: T, index: number, obj: T[]) => boolean) {
		const index = items.findIndex(exp);
		items.splice(index, 1);
		return items;
	}
	/**
   * 值转文字
   * @param id 待转换的值
   * @param array 数据节点集合
   * @param defaultText 转换失败时的默认文字
   */
	valueToText(id: number, array: { id: number; text: string }[] | ListNode[], defaultText: string = null) {
		let text = defaultText == null ? id.toString() : defaultText;
		array.forEach((item) => {
			if (item.id === id) {
				text = item.text;
				return false;
			}
			return true;
		});
		return text;
	}
	/**
   * 展开集合拼接字符串
   * @param array 待展开的集合
   * @param separator 分隔符
   */
	expandAndToString(array: Array<any>, separator: string = ",") {
		let result = "";
		if (!array || !array.length) {
			return result;
		}
		array.forEach((item) => {
			result = result + item.toString() + separator;
		});
		return result.substr(0, result.length - separator.length);
	}
	/**
   * 下载数据
   * @param filename 存储的文件名
   * @param content 下载得到的内容
   */
	download(filename: string, content: string) {
		const urlObject = window.URL;
		const blob = new Blob([ content ]);
		const saveLink = document.createElement("a");
		saveLink.href = urlObject.createObjectURL(blob);
		saveLink.download = filename;
		const ev = document.createEvent("MouseEvents");
		ev.initMouseEvent("click", true, false, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
		saveLink.dispatchEvent(ev);
	}
	/**
   * 打开Email的网站
   * @param email Email地址
   */
	openMailSite(email: string) {
		const host = this.subStr(email, "@");
		const url = `http://mail.${host}`;
		window.open(url);
	}

	goto(url: string) {
		setTimeout(() => this.router.navigateByUrl(url));
	}

	/**
   * 处理Ajax结果
   * @param res HTTP响应
   * @param onSuccess 成功后的调用
   * @param onFail 失败后的调用
   */
	ajaxResult(res, onSuccess?, onFail?) {
		if (!res || !res.Type) {
			return;
		}
		const result = res as AjaxResult;
		const type = result.Type;
		const content = result.Content;
		switch (type) {
			case AjaxResultType.Info:
				this.info(content);
				break;
			case AjaxResultType.NoFound:
				this.router.navigateByUrl("/nofound");
				break;
			case AjaxResultType.UnAuth:
				this.warning("用户未登录或登录已失效");
				this.router.navigateByUrl("/passport/login");
				break;
			case AjaxResultType.Success:
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
	}

	/**
   * 处理Ajax错误
   * @param xhr 错误响应
   */
	ajaxError(xhr) {
		switch (xhr.status) {
			case 401:
				this.warning("用户未登录或登录已失效");
				this.router.navigateByUrl("/identity/login");
				break;
			case 404:
				this.router.navigateByUrl("/nofound");
				break;
			default:
				this.error(`发生错误：${xhr.status}: ${xhr.statusText}`);
				break;
		}
	}
	remoteSFValidator(url: string, error: ErrorData): ErrorData[] | Observable<ErrorData[]> {
		clearTimeout(this.timeout1);
		return new Observable((observer) => {
			this.timeout1 = setTimeout(() => {
				this.http.get(url).subscribe((res) => {
					if (res !== true) {
						observer.next([]);
					} else {
						observer.next([ error ]);
					}
				});
			}, 800);
		});
	}
	remoteInverseSFValidator(url: string, error: ErrorData): ErrorData[] | Observable<ErrorData[]> {
		clearTimeout(this.timeout2);
		return new Observable((observer) => {
			this.timeout2 = setTimeout(() => {
				this.http.get(url).subscribe((res) => {
					if (res !== true) {
						observer.next([ error ]);
					} else {
						observer.next([]);
					}
				});
			}, 800);
		});
	}
	//#endregion

	//#region 验证码处理

	/**
   * 获取验证码
   */
	refreshVerifyCode(): Observable<VerifyCode> {
		const url = "api/common/verifycode";
		return this.http.get(url, null, { responseType: "text" }).map((res) => {
			const str = this.fromBase64(res.toString());
			const strs: string[] = str.split("#$#");
			const code: VerifyCode = new VerifyCode();
			code.image = strs[0];
			code.id = strs[1];
			return code;
		});
	}

	//#endregion

	/**
   * 获取树节点集合
   * @param root 根节点
   * @param array 节点集合
   */
	getTreeNodes(root: any, array: Array<any>) {
		array.push(root);
		if (root.hasChildren) {
			for (const item of root.Items) {
				this.getTreeNodes(item, array);
			}
		}
	}

	/**
   * 检查URL的功能权限
   * @param url 要检查权限的后端URL
   */
	checkUrlAuth(url: string): Promise<boolean> {
		if (!url.startsWith("https:") && !url.startsWith("http") && !url.startsWith("/")) {
			url = `/${url}`;
		}
		url = this.urlEncode(url);
		return this.http.get<boolean>("api/auth/CheckUrlAuth?url=" + url).toPromise();
	}

	/**
   * 获取当前用户的权限点数据(string[])，如本地 ACLServer 中不存在，则从远程获取，并更新到 ACLServer 中
   */
	getAuthInfo(refresh?: boolean): Observable<string[]> {
		if (!refresh && this.aclSrv.data.abilities && this.aclSrv.data.abilities.length) {
			const authInfo: string[] = this.aclSrv.data.abilities as string[];
			return of(authInfo);
		}

		const url = "api/auth/getauthinfo";
		return this.http.get<string[]>(url).map((auth) => {
			this.aclSrv.setAbility(auth);
			return auth;
		});
	}

	getOperateEntries(operates: FilterOperate[]): FilterOperateEntry[] {
		return new List(operates).Select((m) => new FilterOperateEntry(m)).ToArray();
	}

	/**
   * 消息加载中
   * @param msg 消息字符串
   */
	loading(msg) {
		return this.msgSrv.loading(msg, this.msgOptions);
	}
	/**
   * 成功的消息
   * @param msg 消息字符串
   */
	success(msg) {
		return this.msgSrv.success(msg, this.msgOptions);
	}
	/**
   * 消息的消息
   * @param msg 消息字符串
   */
	info(msg) {
		return this.msgSrv.info(msg, this.msgOptions);
	}
	/**
   * 警告的消息
   * @param msg 消息字符串
   */
	warning(msg) {
		return this.msgSrv.warning(msg, this.msgOptions);
	}
	/**
   * 错误的消息
   * @param msg 消息字符串
   */
	error(msg) {
		return this.msgSrv.error(msg, {
			nzDuration: 1000 * 6,
			nzAnimate: true,
			nzPauseOnHover: true
		});
	}

	// #endregion
}

//#region 组件基类

/**
 * 组件基类，实现了权限控制
 */
export abstract class ComponentBase {
	protected osharp: OsharpService;

	/**
   * 权限字典，以模块代码为键，是否有权限为值
   */
	public auth: any | { [key: string]: boolean } = {};
	private authConfig: AuthConfig = null;

	constructor(injector: Injector) {
		this.osharp = injector.get(OsharpService);
	}

	/**
   * 重写以返回权限控制配置信息
   */
	protected abstract AuthConfig(): AuthConfig;

	/**
   * 初始化并执行权限检查，检查结果存储到 this.auth 中
   */
	async checkAuth() {
		if (this.authConfig == null) {
			this.authConfig = this.AuthConfig();
			this.authConfig.funcs.forEach((key) => (this.auth[key] = true));
		}
		const position = this.authConfig.position;
		const codes = await this.osharp.getAuthInfo().toPromise();
		if (!codes) {
			return this.auth;
		}
		const list = new List(codes);
		for (const key in this.auth) {
			if (this.auth.hasOwnProperty(key)) {
				let path = key;
				if (!path.startsWith("Root.")) {
					path = `${position}.${path}`;
				}
				this.auth[key] = list.Contains(path);
			}
		}
		return this.auth;
	}
}

//#endregion
