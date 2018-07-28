# 日志模块：Log4NetPack

* 级别：PackLevel.Core
* 启动顺序：1
* 位置：OSharp.Log4Net.dll
> 注：日志组件应当尽早初始化，以满足其他模块记录日志的需要，所以日志模块启动级别比较优先

---

`Log4NetPack`为系统提供基于`log4net`的日志记录功能。基职责如下：
* 基于`log4net`日志记录组件，对`Microsoft.Extensions.Logging`日志框架的`ILogger`、`ILoggerProvider`进行日志记录实现。
* 实现将日志以不同级别（Debug/Info/Warn/Error）按时间输出到不同文件中