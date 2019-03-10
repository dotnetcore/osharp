import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router/src/utils/preactivation';
import { CanActivateChild, CanLoad, ActivatedRouteSnapshot, RouterStateSnapshot, Route, Router, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { ACLService, ACLCanType, ACLType, DelonACLConfig } from '@delon/acl';

/**
 * OSharp权限检查路由守卫，在路由执行前检查路由的执行权限，
 * 需在路由配置中进行相应的角色或功能点配置，形如：{path: xxx, data: {guard: ["注册会员"]}} 或者 {path: xxx, data: {guard: ["Root.Site.XXX","Root.Admin.XXX"]}}。
 */
@Injectable({
  providedIn: 'root'
})
export class OsharpGuard implements CanActivate, CanActivateChild, CanLoad {

  path: ActivatedRouteSnapshot[];
  route: ActivatedRouteSnapshot;

  constructor(
    private aclSrv: ACLService,
    private router: Router,
    private options: DelonACLConfig
  ) { }

  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | Observable<boolean> | UrlTree {
    return this.canActivate(childRoute, state);
  }

  canLoad(route: Route): boolean | Observable<boolean> {
    return this.process((route.data && route.data.guard) || null);
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | Observable<boolean> | UrlTree {
    let flag = this.process((route.data && route.data.guard) || null);
    if (flag) {
      return true;
    }
    return this.router.parseUrl(this.options.guard_url);
  }

  process(guard: ACLCanType): boolean | Observable<boolean> {
    if (!guard) {
      return true;
    }
    let aclType = this.parseACLType(guard);
    if (!this.aclSrv.data.abilities || !this.aclSrv.data.abilities.length) {
      return false;
    }
    return this.aclSrv.can(aclType);
  }

  parseACLType(guard: ACLCanType): ACLType {
    if (typeof guard === 'number' || Array.isArray(guard) && typeof guard[0] === 'number') {
      throw new Error("路由中的 data:{guard} 不能为数字");
    }

    if (typeof guard === 'string') {
      if (this.isAbilitie(guard)) {
        return { ability: [guard] };
      }
      return { role: [guard] };
    }

    let ability: string[] = [], role: string[] = [];
    let type: ACLType = {};
    if (Array.isArray(guard)) {
      (guard as string[]).forEach((val: string) => {
        if (this.isAbilitie(val)) {
          ability.push(val);
        } else {
          role.push(val);
        }
      });
      type.role = role && role.length > 0 ? role : null;
      type.ability = ability && ability.length > 0 ? ability : null;
      return type;
    }
    return guard as ACLType;
  }

  isAbilitie(val: string): boolean {
    return val && val.startsWith("Root.");
  }
}
