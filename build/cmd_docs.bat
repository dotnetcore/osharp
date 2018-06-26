::首先要下载安装 docfx工具，地址：https://github.com/dotnet/docfx/releases/download/v2.36.2/docfx.zip
::下载后解压，并把路径添加到环境变量 Path 中
docfx init -q -o ../docs
docfx metadata ../osharp-ns20.sln
move _api ../docs/_api
echo 请手动处理 _api 到 api 的文件步骤
pause
docfx ../docs/docfx.json