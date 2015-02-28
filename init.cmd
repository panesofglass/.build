@echo off
echo "Initializing ..."
copy build.cmd ..\
copy build.sh ..\
if not exist ..\build.config.fsx (
    copy build.config.fsx ..\
)
if not exist ..\build.fsx (
    copy build.fsx ..\
)
if not exist ..\.paket (
    mkdir ..\.paket
    copy .paket\*.* ..\.paket\
)
echo "Initialization complete."
