---
name: fable-profiler
description: Use whenever generating or reviewing F#/Fable client code that renders a Symfony-style debug profiler toolbar in a SAFE/Elmish app — calling `Profiler.view`, wiring `ProfilerModel`, dispatching `ProfilerAction.ShowProfiler`, or threading a `DispatchProfilerAction`. Trigger also on mentions of `ProfilerModel.update`, `ProfilerModel.empty`, `Profiler.Toolbar`, the `sf-toolbar` markup, or consuming `Alma.Profiler.Common` toolbar data on the Fable side.
---

# Fable-Profiler

Library: [alma-oss/fable-profiler](https://github.com/alma-oss/fable-profiler)
NuGet: `Alma.Fable.Profiler`

## Purpose

`Alma.Fable.Profiler` is a Fable (F#-to-JavaScript) library that renders a Symfony-style debug profiler toolbar as React elements for SAFE/Elmish web applications. It consumes profiler data (`Profiler.Toolbar`) produced by the server side and exposes a small Elmish model plus a single view function, with toolbar styling bundled as SCSS that is imported automatically.

## When to Use

- Displaying a server-provided profiler toolbar (app info, queries, errors, resource details) inside a Fable/Elmish UI.
- Integrating profiler state into an existing Elmish `model`/`update`/`view` loop.
- Reviewing F# code that consumes `Alma.Profiler.Common` toolbar types on the client.

## When NOT to Use

- Producing or measuring profiler data — that is the server side's responsibility; this library only renders it.
- Non-Elmish or non-Fable UIs, or any environment without a React render host.
- Custom toolbar styling needs — the SCSS ships fixed with the package.

## Main Concepts

- `ProfilerModel` — Elmish record holding the optional toolbar to render (`Profiler: Profiler.Toolbar option`).
- `ProfilerAction.ShowProfiler` — the only action; sets (or clears) the toolbar in the model.
- `DispatchProfilerAction` — alias for `ProfilerAction -> unit`, the dispatcher passed into integrating code.
- `ProfilerModel.empty` — initial model with no toolbar.
- `ProfilerModel.update` — folds a `ProfilerAction` into the model, returning `(model, Cmd.none)`.
- `Profiler.view` — render function taking a `refreshProfiler` callback and the model; emits the `sf-toolbar` React markup only when a toolbar is present.
- `Profiler.Toolbar` / `Profiler.Item` / `Profiler.DetailItem` / `Profiler.Status` — `Alma.Profiler.Common` data shapes the view reads.
- `Profiler.Color` — `Green | Yellow | Red | Gray` severity, mapped to toolbar status CSS and tooltip styles.

## Related Libraries

- `Alma.Profiler.Common` — shared profiler types (`Toolbar`, `Item`, `DetailItem`, single-case DUs `Label`/`Value`/`Unit`/`Link`); the server-side counterpart `fprofiler` produces them.
- `Fable.Elmish` / `Fable.Elmish.React` — the MVU runtime this library plugs into.
- `Fulma` and `Fulma.Extensions.Wikiki.Tooltip` — Bulma bindings used for rendering and detail tooltips.

## Keywords for Search

fable-profiler, Alma.Fable.Profiler, profiler toolbar, sf-toolbar, Symfony toolbar, ProfilerModel, ProfilerAction, ShowProfiler, DispatchProfilerAction, Profiler.view, refreshProfiler, Profiler.Toolbar, Profiler.Item, DetailItem, Alma.Profiler.Common, Elmish, Fable, Fulma, Bulma, tooltip, severity color, debug toolbar

## Reference Files

- For composition principles, MVU wiring, and recommended API usage, read `references/preferred-patterns.md`.
- For known pitfalls and incorrect assumptions, read `references/anti-patterns.md`.
- For worked, self-contained code examples, read `references/examples.md`.
