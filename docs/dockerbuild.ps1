$version = Read-Host "请输入版本号"
docker build -t mkdocs-material:$($version) .
docker tag mkdocs-material:$($version) gmf520/mkdocs-material:$($version)
docker tag mkdocs-material:$($version) gmf520/mkdocs-material:latest
Write-Host "即将发布到dockerhub，请输入账号密码"
$username = Read-Host "请输入账号"
$password = Read-Host "请输入密码"
docker login -u $username -p $password
docker push gmf520/mkdocs-material:$($version)
docker push gmf520/mkdocs-material:latest
Write-Host ("构建并发布成功: gmf520/mkdocs-material:$($version)")