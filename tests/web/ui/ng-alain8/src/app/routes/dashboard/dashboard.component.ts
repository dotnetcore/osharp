import { Component, OnInit, AfterViewInit, } from '@angular/core';

import * as moment from 'moment';
import { _HttpClient } from '@delon/theme';


@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.less']
})
export class DashboardComponent implements AfterViewInit {

  dateFormat = 'yyyy/MM/dd';
  pickerRanges = {
    '今天': [moment().toDate(), moment().toDate()],
    '昨天': [moment().subtract(1, 'days').toDate(), moment().subtract(1, 'days').toDate()],
    '最近7天': [moment().subtract(6, 'days').toDate(), moment().toDate()],
    '最近30天': [moment().subtract(29, 'days').toDate(), moment().toDate()],
    '本月': [moment().startOf("month").toDate(), moment().endOf("month").toDate()],
    '上月': [moment().subtract(1, "months").startOf("month").toDate(), moment().subtract(1, "months").endOf("month").toDate()],
    '全部': [moment("1-1-1", "MM-DD-YYYY").toDate(), moment("12-31-9999", "MM-DD-YYYY").toDate()]
  };

  summaries: Summary[] = [];
  lineChartData: any[] = [];

  constructor(private http: _HttpClient) { }

  ngAfterViewInit(): void {
    this.rangePickerChange(this.pickerRanges.最近30天);
  }

  rangePickerChange(e) {
    if (e.length === 0) {
      return;
    }
    const start = e[0].toLocaleDateString()
    const end = e[1].toLocaleDateString();
    this.summaryData(start, end);
    this.userLine(start, end);
  }

  /** 统计数据 */
  summaryData(start, end) {
    const url = `api/admin/dashboard/SummaryData?start=${start}&end=${end}`;
    this.http.get(url).subscribe((res: any) => {
      if (!res) {
        return;
      }
      this.summaries = [];
      this.summaries.push({ data: `${res.users.ValidCount} / ${res.users.TotalCount}`, text: '用户：已激活 / 总计', bgColor: 'bg-primary' });
      this.summaries.push({ data: `${res.roles.AdminCount} / ${res.roles.TotalCount}`, text: '角色：管理 / 总计', bgColor: 'bg-success' });
      this.summaries.push({ data: `${res.modules.SiteCount} / ${res.modules.AdminCount} / ${res.modules.TotalCount}`, text: '模块：前台 / 管理 / 总计', bgColor: 'bg-orange' });
      this.summaries.push({ data: `${res.functions.ControllerCount} / ${res.functions.TotalCount}`, text: '功能：控制器 / 总计', bgColor: 'bg-magenta' });
    });
  }

  /** 用户曲线 */
  userLine(start, end) {
    let url = `api/admin/dashboard/LineData?start=${start}&end=${end}`;
    this.http.get(url).subscribe((res: any[]) => {
      if (!res || !res.length) {
        return;
      }
      for (const item of res) {
        this.lineChartData.push({
          x: new Date(item.Date),
          y1: item.DailyCount,
          y2: item.DailySum
        });
      }
    });
  }
}

export class Summary {
  data: string;
  text: string;
  bgColor = 'bg-primary';
}
