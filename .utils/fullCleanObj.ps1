Get-Item */*/obj -Exclude node_modules | ForEach-Object{Write-Host $_; Remove-Item $_ -Force -Recurse -ErrorAction SilentlyContinue}