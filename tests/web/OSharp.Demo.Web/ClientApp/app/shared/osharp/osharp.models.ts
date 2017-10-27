import { Injectable, } from '@angular/core';
import { Http } from "@angular/http";
import 'rxjs/add/operator/toPromise';

import { DataResult, DataSourceRequestState, translateDataSourceResultGroups } from "@progress/kendo-data-query";
import { GridDataResult } from "@progress/kendo-angular-grid";
import { Observable } from 'rxjs/Observable';

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
    Read(state?: DataSourceRequestState): Observable<DataResult>;
}

/** 实体服务基类 */
@Injectable()
export abstract class EntityServiceBase<TEntity> implements IEntityService<TEntity>{

    constructor(protected http: Http) {
    }

    /** 读取列表数据 */
    abstract Read(state?: DataSourceRequestState): Observable<DataResult>;

    protected ReadInternal(url: string, state?: DataSourceRequestState): Observable<GridDataResult> {
        const hasGroups = state.group && state.group.length;

        return this.http.post(url, state).map(res => res.json()).map(res => (<GridDataResult>{
            data: hasGroups ? translateDataSourceResultGroups(res.rows) : res.rows,
            total: res.total
        }));
    }
}