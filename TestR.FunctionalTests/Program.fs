open Microsoft.VisualStudio.TestTools.UnitTesting
open System
open TestR.Web

let mutable (browser : Browser) = null
let Chrome = BrowserType.Chrome
let Firefox = BrowserType.Firefox
let InternetExplorer = BrowserType.InternetExplorer
let elements () = browser.Elements

let url location =
    browser.NavigateTo(location)

let start b = 
    browser <- 
        match b with
        | BrowserType.InternetExplorer -> Browsers.InternetExplorer.AttachOrCreate()
        | BrowserType.Chrome -> Browsers.Chrome.AttachOrCreate()
        | BrowserType.Firefox -> Browsers.Firefox.AttachOrCreate()
        | _ -> Browsers.InternetExplorer.AttachOrCreate()

let waitFor id =
    let r = TestR.Helpers.Utility.Wait(fun () -> elements () |> Seq.filter (fun x -> x.Id = id) |> Seq.length > 0)
    match r with
    | true -> Some (elements () |> Seq.find (fun x -> x.Id = id))
    | false -> None

let waitForThenClick id =
    let e = waitFor id
    match e with
    | Some x -> x.Click()
    | None -> Assert.Fail("Failed to find the element.")

let ( == ) item value =
    if (item <> value) then
        Assert.Fail("Item does not equal value.")

let printElementIds () =
    elements () |> Seq.iter (fun x -> printfn "%s" x.Id)

[<EntryPoint>]
[<STAThread>]
let main argv = 
    use browser = Browsers.InternetExplorer.AttachOrCreate()
    start Chrome
    url "http://localhost:8080/index.html"
    //printElementIds ()
    waitForThenClick "inputButton"
    Console.ReadKey() |> ignore
    0