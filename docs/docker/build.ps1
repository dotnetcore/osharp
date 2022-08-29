$version = Read-Host "请输入版本号"
docker build -t gmf520/mkdocs-material:$version .
docker tag gmf520/mkdocs-material:$version gmf520/mkdocs-material:latest
Write-Host "构建完成: gmf520/mkdocs-material:$version"
Write-Host "即将发布到dockerhub, 请输入账号密码"
$username = Read-Host "请输入账号"
$password = Read-Host "请输入密码"
docker login -u $username -p $password
docker push gmf520/mkdocs-material:$version
docker push gmf520/mkdocs-material:latest
Write-Host "发布完成: gmf520/mkdocs-material:$version"