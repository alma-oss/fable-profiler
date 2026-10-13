# Examples

All example code for this skill lives here. Examples are ordered from simplest to most complete and are self-contained.

## Basic Model Wiring

Initialise profiler state and apply an action through the provided update.

```fsharp
open Alma.Fable.Profiler.ProfilerModel

// Start with nothing to render.
let initial = ProfilerModel.empty

// Show a toolbar that arrived from upstream.
let shown, cmd = ProfilerModel.update initial (ProfilerAction.ShowProfiler (Some toolbar))

// Hide it again.
let hidden, _ = ProfilerModel.update shown (ProfilerAction.ShowProfiler None)
```

## Rendering The Toolbar

Render the toolbar in a view. `ignore` is passed when there is no post-render work.

```fsharp
open Fable.React
open Alma.Fable.Profiler

let render (model: ProfilerModel.ProfilerModel) : ReactElement =
    div [] [
        // ... rest of the page ...
        Profiler.view ignore model
    ]
```

Supply a real callback when something must run each time a toolbar is shown:

```fsharp
let private onProfilerRendered () =
    Browser.Dom.console.log "profiler toolbar rendered"

let renderWithCallback (model: ProfilerModel.ProfilerModel) : ReactElement =
    Profiler.view onProfilerRendered model
```

## Integrating Into An Elmish Program

Embed the profiler as a sub-model of a host application (`WebApi`) and delegate its action.

```fsharp
open Elmish
open Alma.Fable.Profiler

type Model = {
    Page: PageModel
    Profiler: ProfilerModel.ProfilerModel
}

type Msg =
    | PageMsg of PageMsg
    | ProfilerMsg of ProfilerModel.ProfilerAction

let init () =
    {
        Page = PageModel.empty
        Profiler = ProfilerModel.ProfilerModel.empty
    },
    Cmd.none

let update (msg: Msg) (model: Model) =
    match msg with
    | ProfilerMsg action ->
        let profiler, cmd = ProfilerModel.ProfilerModel.update model.Profiler action
        { model with Profiler = profiler }, Cmd.map ProfilerMsg cmd
    | PageMsg pageMsg ->
        let page, cmd = PageModel.update model.Page pageMsg
        { model with Page = page }, Cmd.map PageMsg cmd

let view (model: Model) (dispatch: Msg -> unit) =
    let dispatchProfiler: ProfilerModel.DispatchProfilerAction =
        ProfilerMsg >> dispatch

    Fable.React.div [] [
        PageView.render model.Page (PageMsg >> dispatch)
        Profiler.view ignore model.Profiler
    ]
```

A `DemoSystem` data source dispatches the toolbar once it has it:

```fsharp
let private showToolbar (dispatch: ProfilerModel.DispatchProfilerAction) toolbar =
    dispatch (ProfilerModel.ProfilerAction.ShowProfiler (Some toolbar))
```

## Testing The Update

Assert the state transitions of `ProfilerModel.update` (model layer only).

```fsharp
open Expecto
open Alma.Fable.Profiler.ProfilerModel

[<Tests>]
let tests =
    testList "ProfilerModel.update" [
        test "ShowProfiler (Some _) stores the toolbar" {
            let model, _ = ProfilerModel.update ProfilerModel.empty (ProfilerAction.ShowProfiler (Some toolbar))
            Expect.isSome model.Profiler "toolbar should be present"
        }

        test "ShowProfiler None clears the toolbar" {
            let shown, _ = ProfilerModel.update ProfilerModel.empty (ProfilerAction.ShowProfiler (Some toolbar))
            let hidden, _ = ProfilerModel.update shown (ProfilerAction.ShowProfiler None)
            Expect.isNone hidden.Profiler "toolbar should be cleared"
        }
    ]
```
