﻿/*
 * 版权属于：yitter(yitter@126.com)
 * 开源地址：https://gitee.com/yitter/idgenerator
 * 版权协议：MIT
 * 版权说明：只要保留本版权，你可以免费使用、修改、分发本代码。
 * 免责条款：任何因为本代码产生的系统、法律、政治、宗教问题，均与版权所有者无关。
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace OSharp.Data.Snows
{
    /// <summary>
    /// 默认实现
    /// </summary>
    public class DefaultIdGenerator : IIdGenerator
    {
        private ISnowWorker _SnowWorker { get; set; }

        public Action<OverCostActionArg> GenIdActionAsync
        {
            get => _SnowWorker.GenAction;
            set => _SnowWorker.GenAction = value;
        }


        public DefaultIdGenerator(IdGeneratorOptions options)
        {
            if (options == null)
            {
                throw new ApplicationException("options error.");
            }

            if (options.BaseTime < DateTime.Now.AddYears(-50) || options.BaseTime > DateTime.Now)
            {
                throw new ApplicationException("BaseTime error.");
            }

            if (options.SeqBitLength + options.WorkerIdBitLength > 22)
            {
                throw new ApplicationException("error：WorkerIdBitLength + SeqBitLength <= 22");
            }

            var maxWorkerIdNumber = Math.Pow(2, options.WorkerIdBitLength) - 1;
            if (options.WorkerId < 0 || options.WorkerId > maxWorkerIdNumber)
            {
                throw new ApplicationException("WorkerId error. (range:[1, " + maxWorkerIdNumber + "]");
            }

            if (options.SeqBitLength < 2 || options.SeqBitLength > 21)
            {
                throw new ApplicationException("SeqBitLength error. (range:[2, 21])");
            }

            var maxSeqNumber = Math.Pow(2, options.SeqBitLength) - 1;
            if (options.MaxSeqNumber < 0 || options.MaxSeqNumber > maxSeqNumber)
            {
                throw new ApplicationException("MaxSeqNumber error. (range:[1, " + maxSeqNumber + "]");
            }

            var maxValue = maxSeqNumber; // maxSeqNumber - 1;
            if (options.MinSeqNumber < 1 || options.MinSeqNumber > maxValue)
            {
                throw new ApplicationException("MinSeqNumber error. (range:[1, " + maxValue + "]");
            }

            switch (options.Method)
            {
                case 1:
                    _SnowWorker = new SnowWorkerM1(options);
                    break;
                case 2:
                    _SnowWorker = new SnowWorkerM2(options);
                    break;
                default:
                    _SnowWorker = new SnowWorkerM1(options);
                    break;
            }

            if (options.Method == 1)
            {
                Thread.Sleep(500);
            }
        }


        public long NewLong()
        {
            return _SnowWorker.NextId();
        }
    }
}
