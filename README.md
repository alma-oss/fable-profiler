Fable.Profiler
==============

[![NuGet](https://img.shields.io/nuget/v/Alma.Fable.Profiler.svg)](https://www.nuget.org/packages/Alma.Fable.Profiler)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Alma.Fable.Profiler.svg)](https://www.nuget.org/packages/Alma.Fable.Profiler)
[![Tests](https://github.com/alma-oss/fable-profiler/actions/workflows/tests.yaml/badge.svg)](https://github.com/alma-oss/fable-profiler/actions/workflows/tests.yaml)

> Fable library with Profiler component.

---

## Install

Add following into `paket.references`
```
Alma.Fable.Profiler
```

## Release
1. Increment version in `Alma.Fable.Profiler.fsproj`
2. Update `CHANGELOG.md`
3. Commit new version and tag it

## Development
### Requirements
- [dotnet core](https://dotnet.microsoft.com/learn/dotnet/hello-world-tutorial)

### Build
```bash
./build.sh build
```

### Tests
```bash
./build.sh -t tests
```
