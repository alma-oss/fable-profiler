namespace Alma.Fable.Profiler

[<RequireQualifiedAccess>]
module Profiler =
    open Fable.Core
    open Feliz
    open Feliz.DaisyUI

    open Alma.Profiler.Common
    open ProfilerModel

    JsInterop.importAll "./style.scss"

    let private statusColor = function
        | Some Profiler.Yellow -> "sf-toolbar-status sf-toolbar-status-yellow"
        | Some Profiler.Green -> "sf-toolbar-status sf-toolbar-status-green"
        | Some Profiler.Red -> "sf-toolbar-status sf-toolbar-status-red"
        | Some Profiler.Gray -> "sf-toolbar-status sf-toolbar-status-normal"
        | _ -> ""

    let private tooltipColor = function
        | Some Profiler.Yellow -> [ tooltip.info ]
        | Some Profiler.Green -> [ tooltip.success ]
        | Some Profiler.Red -> [ tooltip.error ]
        | _ -> []

    let private infoGroupPiece ({ ShortLabel = shortLabel; Label = (Profiler.Label label); Value = (Profiler.Value value); Detail = detail; Color = color; Link = link }: Profiler.DetailItem) =
        Html.div [
            prop.className "sf-toolbar-info-piece"
            prop.children [
                let shortLabelValue =
                    match shortLabel with
                    | Some (Profiler.Label shortLabel) -> Some shortLabel
                    | _ -> None

                let link (label: string) =
                    match link with
                    | Some (Profiler.Link link) -> Html.a [ prop.href link; prop.text label ]
                    | _ -> Html.text label

                match shortLabelValue with
                | Some shortLabel ->
                    Daisy.tooltip [
                        tooltip.text label
                        tooltip.top
                        yield! tooltipColor color
                        prop.children [ Html.b [ prop.children [ link shortLabel ] ] ]
                    ]
                | _ -> Html.b [ prop.children [ link label ] ]

                match detail with
                | Some (Profiler.ValueDetail detail) ->
                    Daisy.tooltip [
                        tooltip.text detail
                        tooltip.top
                        prop.children [ Html.span [ prop.className (color |> statusColor); prop.text value ] ]
                    ]
                | _ -> Html.span [ prop.className (color |> statusColor); prop.text value ]
            ]
        ]

    let private infoGroup values =
        Html.div [
            prop.className "sf-toolbar-info"
            prop.children [
                Html.div [
                    prop.className "sf-toolbar-info-group"
                    prop.children (values |> List.map infoGroupPiece)
                ]
            ]
        ]

    let private itemStatus ({ Color = color; Value = (Profiler.Value value) }: Profiler.Status) =
        Html.span [ prop.className (color |> statusColor); prop.text value ]

    let private itemLabel (Profiler.Label label) =
        Html.span [ prop.className "sf-toolbar-label"; prop.text label ]

    let private itemUnit (Profiler.Unit unit) =
        Html.span [ prop.className "sf-toolbar-label"; prop.text unit ]

    let private itemValue (Profiler.Value value) =
        Html.span [ prop.className "sf-toolbar-value"; prop.text value ]

    let private itemNormal (item: Profiler.Item) =
        Html.div [
            prop.className (sprintf "sf-toolbar-block %s" (item.ItemColor |> statusColor))
            prop.children [
                Html.a [
                    prop.children [
                        Html.div [
                            prop.className "sf-toolbar-icon"
                            prop.children [
                                yield!
                                    match item.StatusIcon with
                                    | Some status ->
                                        [
                                            itemStatus status
                                            Html.text " "
                                        ]
                                    | _ -> []

                                yield!
                                    match item.Label with
                                    | Some label ->
                                        [
                                            itemLabel label
                                            Html.text " "
                                        ]
                                    | _ -> []

                                itemValue item.Value

                                yield!
                                    match item.Unit with
                                    | Some unit ->
                                        [
                                            Html.text " "
                                            itemUnit unit
                                        ]
                                    | _ -> []
                            ]
                        ]
                    ]
                ]

                infoGroup item.Detail
            ]
        ]

    let view refreshProfiler ({ Profiler = profiler }: ProfilerModel) =
        match profiler with
        | Some (Profiler.Toolbar profiler) ->
            refreshProfiler()

            Html.div [
                prop.className "sf-toolbar"
                prop.children [
                    Html.div [
                        prop.className "sf-toolbarreset clear-fix"
                        prop.children (profiler |> List.map itemNormal)
                    ]
                ]
            ]
        | _ -> Html.none
