import { PageRequest, PageCondition, SortCondition, ListSortDirection } from '../osharp.model';
import { STColumn, STReq, STRes, STComponent, STChange, STPage } from '@delon/abc';
import { ViewChild, Injector, OnChanges, SimpleChanges } from '@angular/core';

export abstract class STComponentBase {

  url: string;
  columns: STColumn[];
  request: PageRequest;
  req: STReq;
  res: STRes;
  page: STPage;
  @ViewChild('st') st: STComponent;

  constructor() { }

  protected InitBase() {
    this.request = new PageRequest();
    this.columns = this.GetSTColumns();
    this.req = this.GetSTReq();
    this.res = this.GetSTRes();
    this.page = this.GetSTPage();
  }

  /**
   * 重写以获取表格的列设置Columns
   */
  protected abstract GetSTColumns(): STColumn[];

  protected GetSTReq(): STReq {
    let req: STReq = {
      method: 'POST', headers: { 'Content-Type': 'application/json' }, body: this.request, allInBody: true, process: opt => {
        if (opt.body.PageCondition) {
          let page: PageCondition = opt.body.PageCondition;
          page.PageIndex = opt.body.pi;
          page.PageSize = opt.body.ps;
          if (opt.body.sort) {
            page.SortConditions = [];
            let sorts = opt.body.sort.split('-');
            sorts.forEach((item: string) => {
              let sort = new SortCondition();
              let num = item.lastIndexOf('.');
              sort.SortField = item.substr(0, num);
              sort.ListSortDirection = item.substr(num + 1) === 'ascend' ? ListSortDirection.Ascending : ListSortDirection.Descending;
              page.SortConditions.push(sort);
            });
          } else {
            page.SortConditions = [];
          }
        }
        return opt;
      }
    };
    return req;
  }

  protected GetSTRes(): STRes {
    let res: STRes = { reName: { list: 'Rows', total: 'Total' } };
    return res;
  }

  protected GetSTPage(): STPage {
    let page: STPage = { showSize: true, showQuickJumper: true, toTop: true, toTopOffset: 0 };
    return page;
  }
}
