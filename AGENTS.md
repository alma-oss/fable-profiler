# AGENTS.md — Alma.Fable.Profiler

## Project Purpose

`Alma.Fable.Profiler` is a Fable (F#-to-JavaScript) NuGet library that provides a React-based profiler toolbar UI component for SAFE stack web applications. It renders a Symfony-style debug toolbar showing application info, queries, errors, and resource details using Elmish architecture and Feliz + Feliz.DaisyUI components.

## Tech Stack

- **Language:** F# (.NET 10) compiled to JavaScript via Fable
- **UI framework:** Feliz + Feliz.DaisyUI (Tailwind/daisyUI bindings) + Elmish
- **Package manager:** Paket
- **Build system:** FAKE (F# Make) via `build.sh`
- **NuGet package:** `Alma.Fable.Profiler`
- **Repository:** <https://github.com/alma-oss/fable-profiler>

## Key Dependencies

- `FSharp.Core ~> 10.0`
- `Fable.Core ~> 5` — Fable compiler core
- `Fable.Elmish ~> 5` — Elm architecture for F#/Fable
- `Fable.Elmish.React ~> 5` — React bindings for Elmish
- `Feliz ~> 3` — React DSL for Fable
- `Feliz.DaisyUI ~> 5` — daisyUI (Tailwind) component bindings, used here for the tooltip
- `Alma.Profiler.Common ~> 10.0` — shared profiler types (`Profiler.Toolbar`, `Profiler.Item`, `Profiler.DetailItem`, etc.)

## Commands

```bash
# Install dependencies
dotnet paket install

# Build
./build.sh build

# Run tests
./build.sh -t tests
```

## Project Structure

```
├── src/
│   └── Alma.Fable.Profiler/
│       ├── Alma.Fable.Profiler.fsproj  # Project file (includes fable/ content + SCSS)
│       ├── AssemblyInfo.fs             # Auto-generated assembly info
│       ├── Model.fs                    # Elmish model and messages (ProfilerModel, ProfilerAction)
│       ├── Profiler.fs                 # React view — toolbar rendering logic
│       ├── style.scss                  # SCSS styles for the profiler toolbar
│       └── paket.references            # Package references
├── build/                              # FAKE build scripts
├── paket.dependencies                  # Dependency definitions
└── fsharplint.json                     # Lint config
```

## Architecture

### Modules

1. **`ProfilerModel`** — Elmish model and update:
   - `ProfilerModel` record with `Profiler: Profiler.Toolbar option`
   - `ProfilerAction.ShowProfiler` — action to show/hide toolbar
   - `ProfilerModel.empty` / `ProfilerModel.update` — standard Elmish pattern

2. **`Profiler`** (view) — renders the toolbar as React elements:
   - `Profiler.view refreshProfiler model` — main render function
   - Renders each `Profiler.Item` as a toolbar block with icon, label, value, unit, and expandable detail panel
   - Detail items support: short labels, tooltips, color coding (Green/Yellow/Red/Gray), links
   - Uses Feliz.DaisyUI `Daisy.tooltip`/`tooltip` for hover details

### Visual Structure

```
┌─────────────────────────────────────────────────────┐
│ [Item1: value unit] [Item2: value] [Item3: value]   │  ← sf-toolbar
│  └─ Detail panel (on hover)                         │
│     ├─ Label: Value                                 │
│     └─ Label: Value                                 │
└─────────────────────────────────────────────────────┘
```

## Conventions

- **Fable library** — source files + SCSS are included in NuGet package under `fable/` for Fable compilation
- **Elmish MVU pattern** — model, update, view separation
- **Color coding** — Green (ok), Yellow (warning), Red (error), Gray (normal) via `Profiler.Color`
- **SCSS import** — `JsInterop.importAll "./style.scss"` for Fable bundler integration
- **`[<RequireQualifiedAccess>]`** on modules
- Types from `Alma.Profiler.Common` are shared between server-side `fprofiler` and this client-side library

## CI/CD

| Workflow | Trigger | What it does |
|---|---|---|
| `tests.yaml` | PR, daily at 03:00 UTC | `./build.sh -t tests` on ubuntu-latest with .NET 10 |
| `publish.yaml` | Tag push (`X.Y.Z`) | `./build.sh -t publish` → NuGet.org |
| `pr-check.yaml` | PR | Blocks fixup commits, runs ShellCheck |

## Release Process

1. Increment `<Version>` in `src/Alma.Fable.Profiler/Alma.Fable.Profiler.fsproj`
2. Update `CHANGELOG.md`
3. Commit and push a git tag matching the version (e.g., `9.0.1`)

## Pitfalls

- **No docker-compose / no local environment** — this is a pure Fable library, no runtime services
- **No tests directory visible** — tests may not exist or may be in a different location
- **SCSS file** — `style.scss` is bundled; changes affect toolbar appearance across all consuming apps
- **Fable content packaging** — `.fsproj` includes `*.fsproj; *.fs; *.scss;` as `Content` with `PackagePath="fable\"`
- **Paket.Restore.targets path** — uses `..\..\` relative path since the project is nested under `src/Alma.Fable.Profiler/`
- **Companion library** — this is the client-side counterpart of `fprofiler` (server-side); they share types via `Alma.Profiler.Common`
- **npm dependencies via Femto** — `.fsproj` `NpmDependencies` declares `tailwindcss` 4 and `daisyui` 5 as dev dependencies; Feliz 3 itself declares React 19. Femto installs packages only — consumers still wire the Tailwind bundler plugin and `@plugin "daisyui"` themselves
- **Tailwind content scanning** — consuming apps must point Tailwind at this package's Fable output, or daisyUI purges the `tooltip*` classes since they never appear in the app's own source. `@source` resolves relative to the CSS entry file and the right path depends on the app's Fable `outDir` (e.g. `@source "../output/fable_modules";`); `@source inline("tooltip tooltip-top tooltip-info tooltip-success tooltip-error");` pins the classes directly and avoids the path dependency
