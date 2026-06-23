# Anti-Patterns

Each entry follows **mistake → why → fix**.

## Common Mistakes

- **Calling `Profiler.view` with only the model.** → `view` is curried as `view refreshProfiler model`; omitting the first argument yields a partially applied function, not React markup. → Always supply the callback first; pass `ignore` when there is no post-render work: `Profiler.view ignore model`.

- **Importing `style.scss` yourself.** → The view module already runs `JsInterop.importAll "./style.scss"` on load, so a second import duplicates styles or breaks the build. → Do nothing; just ensure the bundler resolves `.scss` imports.

- **Mutating `ProfilerModel.Profiler` directly to show/hide the toolbar.** → Bypassing the action skips the MVU flow and desyncs from Elmish, making state changes untracked. → Route every change through `ProfilerAction.ShowProfiler` applied by `ProfilerModel.update`.

- **Constructing an empty `Profiler.Toolbar` to "hide" the bar.** → The view keys off `Some`/`None`; a `Some` empty toolbar still renders the container. → Dispatch `ShowProfiler None` to hide.

- **Assuming `view` renders something when the toolbar is `None`.** → With `None` it returns an empty fragment by design, so layout that expects a node there will be surprised. → Treat "no toolbar" as "renders nothing" and lay out accordingly.

- **Expecting `refreshProfiler` to run on every render.** → It is invoked only on the branch where a toolbar is present, not when the toolbar is `None`. → Put logic that must run regardless elsewhere; reserve the callback for toolbar-present side effects.

## Do Not Use / Avoid

- **Avoid the legacy `Profiler.profiler` name.** → It was renamed to `Profiler.view`; the old name no longer exists. → Use `Profiler.view`.

- **Avoid older namespaces such as `Lmc.Fable.Profiler`.** → The library moved to the `Alma` namespace; old `open`/`Lmc`-prefixed references will not resolve. → Open `Alma.Fable.Profiler`.

- **Avoid building toolbar data on the client.** → The client is a renderer; fabricating `Item`/`DetailItem` values duplicates server logic and drifts from the real profiler. → Consume the `Profiler.Toolbar` produced via `Alma.Profiler.Common` upstream.

## Wrong Abstractions

- **Wrapping `ProfilerModel.update` to also return custom commands.** → It already returns `(model, Cmd.none)` in the standard shape; re-wrapping adds indirection for no gain. → Compose it in the parent `update` and lift its `Cmd` with `Cmd.map`.

- **Mismatched `Alma.Profiler.Common` versions between server and client.** → The toolbar types are the shared contract; divergent versions cause shape mismatches at the boundary. → Keep the Common package version aligned on both sides.
