@echo off
set tool=ProtoGen
 
set proto=PBCommon.proto
set pb=PBCommon
 
%tool%\protogen.exe -i:%proto% -o:%pb%.cs
 
pause