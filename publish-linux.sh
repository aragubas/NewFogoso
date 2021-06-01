#! /bin/bash
echo VS Code dont want to work properly so i made this script that SHOULD WORK

echo Deleting old builds...
rm -r Publish-Linux

echo Starting dotnet publish...
dotnet publish -r linux-x64 /p:PublishTrimmed=true -o Publish-Linux/