import { Injectable, } from '@angular/core';
import { Http } from "@angular/http";
import 'rxjs/add/operator/toPromise';

/** 分页数据 */
export interface PageData<T> {
    rows: Array<T>;
    total: number;
}

/** Ajax操作结果 */
export interface AjaxResult {
    type: string;
    content: string;
    data: object;
}

/** 实体服务接口，定义实体服务基本操作 */
export interface IEntityService<TEntity> {

    /** 读取列表数据 */
    Read(): Promise<PageData<TEntity>>;
}

/** 实体服务基类 */
@Injectable()
export abstract class EntityServiceBase<TEntity> implements IEntityService<TEntity>{

    constructor(protected http: Http) {
    }

    /** 读取列表数据 */
    abstract Read(): Promise<PageData<TEntity>>;

    protected HandleError(error: any): Promise<any> {
        console.error("An error occurred:", error);
        return Promise.reject(error.message || error);
    }
}