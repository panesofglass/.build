# .build
Minimal [ProjectScaffold](https://fsprojects.github.io/ProjectScaffold) build utility for use with existing projects

To use, run

```
git submodule add https://github.com/panesofglass/.build .build
cd .build
init.cmd # or init.sh
cd ..
```

You can then remove the submodule or keep it around for future updates. The `init.cmd` and `init.sh` scripts will not overwrite changes to your `build.config.fsx` and `build.fsx` files.
