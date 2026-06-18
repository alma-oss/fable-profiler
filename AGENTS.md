# AGENTS.md — Alma.Fable.Profiler

This repo ships Agent Skill for the `Alma.Fable.Profiler` library. Compatible agents discover it automatically; see `.agents/skills/fable-profiler/SKILL.md`.

## Project Purpose

`Alma.Fable.Profiler` is a Fable (F#-to-JavaScript) NuGet library that provides a React-based profiler toolbar UI component for SAFE stack web applications. It renders a Symfony-style debug toolbar showing application info, queries, errors, and resource details using Elmish architecture and Fulma (Bulma CSS) components.

## Tech Stack

- **Language:** F# (.NET 10) compiled to JavaScript via Fable
- **UI framework:** Fable.React + Fulma (Bulma CSS bindings) + Elmish
- **Package manager:** Paket
- **Build system:** FAKE (F# Make) via `build.sh`
- **NuGet package:** `Alma.Fable.Profiler`
- **Repository:** <https://github.com/alma-oss/fable-profiler>

## Key Dependencies

- `FSharp.Core ~> 10.0`
- `Fable.Core ~> 4` — Fable compiler core
- `Fable.Elmish ~> 4` — Elm architecture for F#/Fable
- `Fable.Elmish.React ~> 4` — React bindings for Elmish
- `Fulma ~> 3` — Bulma CSS framework bindings
- `Fulma.Extensions.Wikiki.Tooltip ~> 4` — tooltip extension
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
   - Uses Fulma `Tooltip` extension for hover details

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
