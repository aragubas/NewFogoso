#! /bin/bash
echo VS Code dont want to work properly so i made this script that SHOULD WORK

echo Deleting old builds...
rm -r Publish-Win64

echo Starting dotnet publish...
dotnet publish -r win-x64 /p:PublishTrimmed=true -o Publish-Win64/ --self-contained --no-restore 