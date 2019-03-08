:: scripts\\ng.cmd build
@IF EXIST "%~dp0\node.exe" (
  "%~dp0\node.exe" --max_old_space_size=4096 "%~dp0\..\node_modules\@angular\cli\bin\ng" %*
) ELSE (
  @SETLOCAL
  @SET PATHEXT=%PATHEXT:;.JS;=;%
  node --max_old_space_size=4096 "%~dp0\..\node_modules\@angular\cli\bin\ng" %*
)
