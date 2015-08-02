Integration testing framework for developers. TestR allows automating testing of web applications. Currently we are supporting Internet Explorer, Chrome, and Firefox*. We have full automation support for Internet Explorer and Chrome. 

* Firefox must be manually started and the "listen 6000" command issued to start the Firefox remote debugging port.

##### To install TestR, run one of the following command in the  Package Manager Console.

* Install-Package TestR (without PowerShell)
* Install-Package TestR.PowerShell (with PowerShell)


### Searching Bing using Internet Explorer

```
using (var browser = InternetExplorerBrowser.AttachOrCreate())
{
	browser.NavigateTo("http://bing.com");
	browser.Elements.TextInputs["sb_form_q"].TypeText("Bobby Cannon");
	browser.Elements["sb_form_go"].Click();
	browser.WaitForRedirect();
}
```

### Searching Amazon using Chrome

```
using (var browser = ChromeBrowser.AttachOrCreate()) 
{
	browser.NavigateTo("http://amazon.com");
	browser.Elements.TextInputs["twotabsearchtextbox"].TypeText("protein powder");
	browser.Elements.First(x => x.GetAttributeValue("title") == "Go").Click();
	browser.WaitForRedirect();
}
```

### Same example using FireFox

```
// Don't Forget: Firefox debug port must be started manually (listen 6000).
using (var browser = FirefoxBrowser.AttachOrCreate()) 
{
	browser.NavigateTo("http://amazon.com");
	browser.Elements.TextInputs["twotabsearchtextbox"].TypeText("protein powder");
	browser.Elements.First(x => x.GetAttributeValue("title") == "Go").Click();
	browser.WaitForRedirect();
}
```

#### Coming Soon

* More element attributes.
* More specific element implementation with their unique attributes.

#### Known Issues

* IMPORTANT: KB3025390 an Update for IE 11 for Windows 8.1 will cause TestR to stop working. You'll need to remove this update to allow TestR to work properly. Special thanks to Gerald Fishel and Michael Bergerman for all their help in notifying me of the issue and finding the issue.
* Firefox debug port must be started manually (listen 6000).
* Internet Explorer will fail if you cross security boundaries like going from Internet to Intranet sites.

