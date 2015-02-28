@echo off
echo "Initializing ..."
copy build.cmd ..\
copy build.sh ..\
if not exist ..\build.template && not exist ..\build.fsx (
    copy build.template ..\
    copy init.fsx ..\
)
if not exist ..\.paket (
    mkdir ..\.paket
    copy .paket\*.* ..\.paket\
)
if not exist ..\paket.dependencies (
    copy paket.dependencies ..\
)
echo "Initialization complete."
