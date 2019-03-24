import { PageRequest, PageCondition, SortCondition, ListSortDirection } from '../osharp.model';
import { STColumn, STReq, STRes, STComponent, STChange, STPage, STRequestOptions, STData } from '@delon/abc';
import { ViewChild, Injector, OnChanges, SimpleChanges } from '@angular/core';
import { OsharpService } from './osharp.service';

export abstract class STComponentBase {

  url: string;
  columns: STColumn[];
  request: PageRequest;
  req: STReq;
  res: STRes;
  page: STPage;
  @ViewChild('st') st: STComponent;

  osharp: OsharpService;
  constructor(injector: Injector) {
    this.osharp = injector.get(OsharpService);
  }

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
      method: 'POST', headers: { 'Content-Type': 'application/json' }, body: this.request, allInBody: true, process: this.RequestProcess
    };
    return req;
  }

  protected GetSTRes(): STRes {
    let res: STRes = { reName: { list: 'Rows', total: 'Total' }, process: this.ResponseDataProcess };
    return res;
  }

  protected GetSTPage(): STPage {
    let page: STPage = { showSize: true, showQuickJumper: true, toTop: true, toTopOffset: 0 };
    return page;
  }

  protected RequestProcess(opt: STRequestOptions): STRequestOptions {
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

  protected ResponseDataProcess(data: STData[]): STData[] {
    return data;
  }
}
