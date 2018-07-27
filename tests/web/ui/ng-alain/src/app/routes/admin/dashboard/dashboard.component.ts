import { Component, OnInit, } from '@angular/core';

import * as G2 from '@antv/g2';
import * as moment from 'moment';

@Component({
  selector: 'admin-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  pickerRanges = {
    '今天': [moment().format('YYYY-MM-DD'), moment().format('YYYY-MM-DD')],
    '昨天': [moment().subtract(1, 'days').format('YYYY-MM-DD'), moment().subtract(1, 'days').format('YYYY-MM-DD')],
    '最近7天': [moment().subtract(6, 'days').format('YYYY-MM-DD'), moment().format('YYYY-MM-DD')],
    '最近30天': [moment().subtract(29, 'days').format('YYYY-MM-DD'), moment().format('YYYY-MM-DD')],
    '本月': [moment().startOf("month").format('YYYY-MM-DD'), moment().endOf("month").format('YYYY-MM-DD')],
    '上月': [moment().subtract(1, "month").startOf("month").format('YYYY-MM-DD'), moment().subtract(1, "month").endOf("month").format('YYYY-MM-DD')],
    '全部': [moment("1-1-1", "MM-DD-YYYY").format('YYYY-MM-DD'), moment("12-31-9999", "MM-DD-YYYY").format('YYYY-MM-DD')]
  };

  title = 'app';
  data = {};
  chart;
  graph;

  ngOnInit(): void {
    this.chartData();
  }

  rangePickerChange(e) {
    console.log(e);
  }

  chartData() {
    this.data = [
      { genre: 'Sports', sold: 275 },
      { genre: 'Strategy', sold: 115 },
      { genre: 'Action', sold: 120 },
      { genre: 'Shooter', sold: 350 },
      { genre: 'Other', sold: 150 }
    ];
    this.chart = new G2.Chart({
      container: 'c1', // 指定图表容器 ID
      width: 600, // 指定图表宽度
      height: 300 // 指定图表高度
    });

    this.chart.source(this.data);
    this.chart.interval().position('genre*sold').color('genre');
    //  渲染图表
    this.chart.render();
  }
}
