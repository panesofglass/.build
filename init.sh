#!/bin/bash
echo "Initializing ..."
cp build.cmd ../
cp build.sh ../
if [ ! -f ../build.fsx && ! -f ../build.template ]; then
    cp build.template ../
    cp init.fsx ../
fi
if [ ! -f ../.paket ]; then
    mkdir ../.paket
    cp .paket/*.* ../.paket/
fi
if [ ! -f ../paket.dependencies ]; then
    cp paket.dependencies ../
fi
echo "Initialization complete."
