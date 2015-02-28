#!/bin/bash
echo "Initializing ..."
cp build.cmd ../
cp build.sh ../
if [ ! -f ../build.config.fsx ]; then
    cp build.config.fsx ../
fi
if [ ! -f ../build.fsx ]; then
    cp build.fsx ../
fi
if [ ! -f ../.paket ]; then
    mkdir ../.paket
    cp .paket/*.* ../.paket/
fi
echo "Initialization complete."
