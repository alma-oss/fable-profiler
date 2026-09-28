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

Requires Fable 5 and React 19 (via Feliz 3). Tooltips are rendered with Feliz.DaisyUI, so
the consuming app needs Tailwind CSS v4 with the daisyUI plugin. Femto installs the
`tailwindcss` and `daisyui` npm packages declared by this library; the app still has to
wire Tailwind into its bundler (e.g. `@tailwindcss/vite` or `@tailwindcss/postcss`) and
load daisyUI in its CSS entry file.

Tailwind must also scan this package's Fable output or the `tooltip*` classes get purged.
`@source` is resolved relative to the CSS entry file, and the actual path depends on the
app's Fable `outDir`, e.g.:
```css
@import "tailwindcss";
@plugin "daisyui";
@source "../output/fable_modules";
```
Path-independent alternative — pin the classes directly instead of scanning a path:
```css
@source inline("tooltip tooltip-top tooltip-info tooltip-success tooltip-error");
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
