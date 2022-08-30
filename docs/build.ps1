$version = Read-Host "请输入版本号"
docker build -t gmf520/osharp.docs:$version .
docker tag gmf520/osharp.docs:$version gmf520/osharp.docs:latest
Write-Host "构建完成: gmf520/osharp.docs:$version"
Write-Host "即将发布到dockerhub, 请输入账号密码"
$username = Read-Host "请输入账号"
$password = Read-Host "请输入密码"
docker login -u $username -p $password
docker push gmf520/osharp.docs:$version
docker push gmf520/osharp.docs:latest
Write-Host "发布完成: gmf520/osharp.docs:$version"