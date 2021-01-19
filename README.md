Fable.Profiler
==============

> Fable library with Profiler component.

---

## Install

Add following into `paket.dependencies`
```
git ssh://git@bitbucket.lmc.cz:7999/archi/nuget-server.git master Packages: /nuget/
# LMC Nuget dependencies:
nuget Lmc.Fable.Profiler
```

Add following into `paket.references`
```
Lmc.Fable.Profiler
```

### Styles
Add styles to your main `scss` file.


```scss
// Profiler
@import "../../.fable/Lmc.Fable.Profiler.1.0.0/style.scss";
```

Or you can copy a style to the `node_modules` and use `~` for accessing it as a npm package
```
node_modules/Lmc.Fable.Profiler
└── style.scss
```

```scss
// Profiler
@import "~Lmc.Fable.Profiler/style.scss";
```

## Release
1. Increment version in `Fable.Profiler.fsproj`
2. Update `CHANGELOG.md`
3. Commit new version and tag it
4. Run `$ fake build target release`
5. Go to `nuget-server` repo, run `faket build target copyAll` and push new versions

## Development
### Requirements
- [dotnet core](https://dotnet.microsoft.com/learn/dotnet/hello-world-tutorial)
- [FAKE](https://fake.build/fake-gettingstarted.html)

### Build
```bash
fake build
```

### Watch
```bash
fake build target watch
```
