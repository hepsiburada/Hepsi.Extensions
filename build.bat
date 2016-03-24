@echo off
.nuget\NuGet.exe restore .nuget\packages.config  -PackagesDirectory packages
powershell -ExecutionPolicy unrestricted -file build.ps1 %*