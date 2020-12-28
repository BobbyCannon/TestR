## TestR

Integration testing framework for developers. TestR allows automating testing of web and desktop applications.
Currently we are supporting Chrome, Edge (chromium based) and Firefox*.

If you need InternetExplorer support then use TestR v2.1.1.

Minimal requirement for setup. Simply reference the TestR nuget then make some minor configuration for some browsers (firefox).

View more documentation <a href="https://docs.epiccoders.com/Page/35/TestR">here</a>.

### *** Known Issues ***

There seems to be an issue where Edge is adding a "hidden" edge://newtab that TestR then uses to automate. I have
yet to determine why it's happening.
