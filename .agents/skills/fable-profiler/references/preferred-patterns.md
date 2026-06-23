# Preferred Patterns

## Core Principles

- Treat the profiler as a thin, optional slice of your Elmish state: hold one `ProfilerModel` (or just its `Profiler` field) and feed it through the standard MVU loop.
- The toolbar is data-driven. The client never builds toolbar content; it receives a fully-formed `Profiler.Toolbar` (from `Alma.Profiler.Common`, produced server-side) and only renders it.
- Rendering is pull-based on presence: the view emits the `sf-toolbar` markup only when the model carries `Some` toolbar, and renders nothing (an empty fragment) when it is `None`.
- Styling is implicit. The SCSS is imported by the library itself when the view module loads — your bundler must be able to resolve `.scss` imports, but you never import the stylesheet yourself.

## Recommended API Usage

- Initialise profiler state with `ProfilerModel.empty` (toolbar `None`) so nothing renders until data arrives.
- Change profiler state exclusively through `ProfilerAction.ShowProfiler`, applied by `ProfilerModel.update`; it returns `(model, Cmd.none)`, so it slots directly into a parent `update` that also returns commands. See `examples.md` → Basic Model Wiring.
- Render with `Profiler.view refreshProfiler model`. The first argument is a side-effect callback invoked at render time whenever a toolbar is present — use it to schedule any post-render work (for example re-attaching toolbar behaviour). Pass `ignore` when you have nothing to do. See `examples.md` → Rendering The Toolbar.
- Pass a `DispatchProfilerAction` into whatever code obtains toolbar data, and have it dispatch `ProfilerAction.ShowProfiler (Some toolbar)` once data is ready (or `None` to hide). See `examples.md` → Integrating Into An Elmish Program.

## Error Handling

- There is nothing to catch in the view: a `None` toolbar is the normal "hide" path, not an error. Model the absence of data as `None` rather than constructing an empty toolbar.
- Failures belong upstream (fetching/parsing the toolbar payload). On failure, dispatch `ShowProfiler None` to keep the UI clean instead of rendering a partial toolbar.

## Composition

- Embed `ProfilerModel` as a sub-model of your application model and delegate its action to `ProfilerModel.update`, lifting the resulting `Cmd` with `Cmd.map`. See `examples.md` → Integrating Into An Elmish Program.
- Place `Profiler.view` once near the root of your view tree so the toolbar overlays the page consistently.

## Integration With Other Libraries

- `Alma.Profiler.Common` is the contract: the same `Toolbar`/`Item`/`DetailItem` values that the server-side profiler emits are what you pass to `ShowProfiler`. Keep the Common package version aligned between server and client.
- The view relies on `Fulma.Extensions.Wikiki.Tooltip` for detail/short-label tooltips and on Bulma (Fulma) classes for layout — ensure the consuming app already includes Bulma so the toolbar inherits expected styling.

## Naming Conventions

- Modules use `[<RequireQualifiedAccess>]`: always call `Profiler.view`, `ProfilerModel.update`, `ProfilerAction.ShowProfiler` fully qualified.
- The render entry point is `view` (historically renamed from `profiler`); use `Profiler.view`, not any older name.

## Testing Recommendations

- The valuable assertions are at the model layer: verify `ProfilerModel.update` transitions `empty` → `Some toolbar` on `ShowProfiler (Some _)` and back to `None` on `ShowProfiler None`. See `examples.md` → Testing The Update.
- Avoid asserting on the produced React markup; the rendering is declarative and stable, so model-level tests give the best signal per token.
