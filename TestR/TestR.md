#TestR


##Application
            
Represents an application that can be automated.
        
###Fields

####DefaultTimeout
Gets the default timeout (in milliseconds).
###Properties

####AutoClose
Gets or sets a flag to auto close the application when disposed of.
####FocusedElement

####Handle
Gets the handle for this window.
####Id

####IsRunning
Gets the value indicating that the process is running.
####Location
Gets the location of the application.
####Name

####Process
Gets the underlying process for this application.
####Size
Gets the size of the application.
####Timeout
Gets or sets the time out for delay request. Defaults to 5 seconds.
####SlowMotion
Gets or sets a flag to tell the browser to act slower. Defaults to false.
###Methods


####Constructor
Creates an instance of the application.
> #####Parameters
> **process:** The process for the application.


####Attach(System.String,System.String,System.Boolean)
Attaches the application to an existing process.
> #####Parameters
> **executablePath:** The path to the executable.

> **arguments:** The arguments for the executable. Arguments are optional.

> **refresh:** The setting to determine to refresh children now.

> #####Return value
> The instance that represents the application.

####Attach(System.IntPtr,System.Boolean)
Attaches the application to an existing process.
> #####Parameters
> **handle:** The main window handle of the executable.

> **refresh:** The setting to determine to refresh children now.

> #####Return value
> The instance that represents the application.

####Attach(System.Diagnostics.Process,System.Boolean)
Attaches the application to an existing process.
> #####Parameters
> **process:** The process to attach to.

> **refresh:** The setting to determine to refresh children now.

> #####Return value
> The instance that represents the application.

####AttachOrCreate(System.String,System.String,System.Boolean)
Attaches the application to an existing process.
> #####Parameters
> **executablePath:** The path to the executable.

> **arguments:** The arguments for the executable. Arguments are optional.

> **refresh:** The setting to determine to refresh children now.

> #####Return value
> The instance that represents the application.

####BringToFront
Brings the application to the front and makes it the top window.

####Close
Closes the window.

####CloseAll(System.String,System.Int32)
Closes all windows my name and closes them.
> #####Parameters
> **executablePath:** The path to the executable.

> **timeout:** The timeout to wait for the application to close.


####Create(System.String,System.String,System.Boolean)
Creates a new application process.
> #####Parameters
> **executablePath:** The path to the executable.

> **arguments:** The arguments for the executable. Arguments are optional.

> **refresh:** The flag to trigger loading to load state when creating the application. Defaults to true.

> #####Return value
> The instance that represents an application.

####Exists(System.String,System.String)
Checks to see if an application process exist by path and optional arguments.
> #####Parameters
> **executablePath:** The path to the executable.

> **arguments:** The arguments for the executable. Arguments are optional.

> #####Return value
> True if the application exists and false otherwise.

####IsInFront
Returns a value indicating if the windows is in front of all other windows.
> #####Return value
> 

####MoveWindow(System.Int32,System.Int32,System.Int32,System.Int32)
Move the window and resize it.
> #####Parameters
> **x:** The x coordinate to move to.

> **y:** The y coordinate to move to.

> **width:** The width of the window.

> **height:** The height of the window.


####Refresh
Refresh the list of items for the application.

####WaitForComplete(System.Int32)
Waits for the Process to not be busy.
> #####Parameters
> **minimumDelay:** The minimum delay in milliseconds to wait. Defaults to 0 milliseconds.


####Dispose(System.Boolean)
Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
> #####Parameters
> **disposing:** True if disposing and false if otherwise.


####OnElementClicked(TestR.Desktop.DesktopElement,System.Drawing.Point)
Triggers th element clicked event.
> #####Parameters
> **element:** The element that was clicked.

> **point:** The point that was clicked.


##Desktop.DesktopElement
            
Base element for desktop automation.
        
###Properties

####Enabled
Gets a value that indicates whether the element is enabled.
####Focused

####FocusedElement

####Height

####Id

####Item(System.String)

####KeyboardFocusable
Gets a value that indicates whether the element can be use by the keyboard.
####Location

####Name

####NativeElement
Gets the native element for the desktop element.
####ProcessId
Get the current process ID;
####Width

###Methods


####Constructor
Creates an instance of an element.
> #####Parameters
> **element:** The automation element for this element.

> **application:** The application parent for this element.

> **parent:** The parent element for this element.


####Click(System.Int32,System.Int32)

####Focus
Set focus on the element.

####FromFocus
Gets the element that is currently focused.
> #####Return value
> The element if found or null if not found.

####FromPoint(System.Drawing.Point)
Gets the element that is currently at the point.
> #####Parameters
> **point:** The point to try and detect at element at.

> #####Return value
> The element if found or null if not found.

####GetText
Gets the text value of the element.
> #####Return value
> The value of the element.

####MoveMouseTo(System.Int32,System.Int32)

####Refresh

####RightClick(System.Int32,System.Int32)

####SetText(System.String)
Sets the text value of the element.
> #####Parameters
> **value:** The text to set the element to.


####WaitForComplete(System.Int32)

####Dispose(System.Boolean)

####Create(UIAutomationClient.IUIAutomationElement,TestR.Application,TestR.Desktop.DesktopElement)
Creates an element from the automation element.
> #####Parameters
> **element:** The element to create.

> **application:** The application parent for this element.

> **parent:** The parent of the element to create.


####GetChildren(TestR.Desktop.DesktopElement)
Gets all the direct children of an element.
> #####Parameters
> **element:** The element to get the children of.

> #####Return value
> The list of children for the element.

####GetClickablePoint(System.Int32,System.Int32)
Gets the clickable point for the element.
> #####Parameters
> **x:** Optional X offset when calculating.

> **y:** Optional Y offset when calculating.

> #####Return value
> The clickable point for the element.

####UpdateChildren(TestR.Desktop.DesktopElement)
Updates the children for the provided element.
> #####Parameters
> **element:** The element to update.


##Desktop.Elements.Button
            
Represents a button element.
        
###Properties

####Toggled
Gets a flag indicating if the button is checked. Usable for split buttons.
####ToggleState
Gets the toggle state of the button.

##Desktop.Elements.Calendar
            
Represents a calendar element.
        

##Desktop.Elements.CheckBox
            
Represents a check box element.
        
###Properties

####Checked
Gets a flag indicating if the checkbox is checked.
####CheckedState
Gets the state of the checkbox.
####ReadOnly
Gets a value indicating whether the control can have a value set programmatically, or that can be edited by the user.
###Methods


####Toggle
Toggle the checkbox.

##Desktop.Elements.ComboBox
            
Represents a combo box element.
        
###Properties

####ExpandCollapseState
Gets the current expanded state of the combo box.
####ReadOnly
Gets a value indicating whether the control can have a value set programmatically, or that can be edited by the user.
####Text
Gets the text value.
###Methods


####Collapse
Collapses the combo box.

####Expand
Expands the combo box.

##Desktop.Elements.Custom
            
Represents a custom element.
        

##Desktop.Elements.DataGrid
            
Represents a datagrid element.
        

##Desktop.Elements.DataItem
            
Represents the data item.
        

##Desktop.Elements.Document
            
Represents a document element.
        
###Properties

####Text
Gets the text value.

##Desktop.Elements.Edit
            
Represents a edit element.
        
###Properties

####ReadOnly
Gets a value indicating whether the control can have a value set programmatically, or that can be edited by the user.
####Text
Gets the text value.

##Desktop.Elements.Group
            
Represents the group for a window.
        

##Desktop.Elements.Header
            
Represents the header.
        

##Desktop.Elements.HeaderItem
            
Represents the header item.
        

##Desktop.Elements.Hyperlink
            
Represents a hyperlink element.
        

##Desktop.Elements.Image
            
Represents a image element.
        

##Desktop.Elements.List
            
Represents the list for a window.
        

##Desktop.Elements.ListItem
            
Represents a list item element.
        

##Desktop.Elements.Menu
            
Represents a menu element.
        
###Properties

####
Gets the menu expand collapse state.
####
Gets a value indicating whether this menu item supports expanding and collapsing pattern.
###Methods


####
Performs mouse click at the center of the element.
> #####Parameters
> **x:** Optional X offset when clicking.

> **y:** Optional Y offset when clicking.


####
Collapse the menu item.

####
Expand the menu item.

##Desktop.Elements.MenuBar
            
Represents the menu bar for a window.
        

##Desktop.Elements.MenuItem
            
Represents the menu item for a window.
        
###Properties

####IsExpanded
Gets the menu expand collapse state.
####SupportsExpandingCollapsing
Gets a value indicating whether this menu item supports expanding and collapsing pattern.
###Methods


####Click(System.Int32,System.Int32)
Performs mouse click at the center of the element.
> #####Parameters
> **x:** Optional X offset when clicking.

> **y:** Optional Y offset when clicking.


####Collapse
Collapse the menu item.

####Expand
Expand the menu item.

##Desktop.Elements.Pane
            
Represents a edit element.
        

##Desktop.Elements.ProgressBar
            
Represents a progress bar element.
        

##Desktop.Elements.RadioButton
            
Represents a radio button element.
        

##Desktop.Elements.ScrollBar
            
Represents the scroll bar for a window.
        

##Desktop.Elements.SemanticZoom
            
Represents a semantic zoom element.
        

##Desktop.Elements.Separator
            
Represents the separator element.
        

##Desktop.Elements.Slider
            
Represents a semantic zoom element.
        

##Desktop.Elements.Spinner
            
Represents a spinner element.
        

##Desktop.Elements.SplitButton
            
Represents a split button element.
        

##Desktop.Elements.StatusBar
            
Represents the status bar for a window.
        

##Desktop.Elements.TabControl
            
Represents the tab control.
        

##Desktop.Elements.TabItem
            
Represents the tab item.
        

##Desktop.Elements.Table
            
Represents the table element.
        

##Desktop.Elements.Text
            
Represents the text for a window.
        

##Desktop.Elements.Thumb
            
Represents the thumb for a window.
        

##Desktop.Elements.TitleBar
            
Represents the title bar for a window.
        
###Properties

####CloseButton
Gets the close button.
####MaximizeButton
Gets the maximize button.
####MinimizeButton
Gets the maximize button.

##Desktop.Elements.ToolBar
            
Represents the tool bar for a window.
        

##Desktop.Elements.ToolTip
            
Represents a tooltip element.
        

##Desktop.Elements.Tree
            
Represents the tree for a window.
        

##Desktop.Elements.TreeItem
            
Represents a tree item element.
        

##Desktop.Elements.Window
            
Represents a window for an application.
        
###Properties

####Location
Gets the location of the element.
####StatusBar
Gets the status bar for the window. Returns null if the window does not have a status bar.
####TitleBar
Gets the title bar for the window. Returns null if the window does not have a title bar.
###Methods


####BringToFront
Bring the window to the front.

####Close
Closes a window.

####Move(System.Int32,System.Int32)
Move the window.
> #####Parameters
> **x:** The x value of the position to move to.

> **y:** The y value of the position to move to.


####Resize(System.Int32,System.Int32)
Resize the window.
> #####Parameters
> **width:** The width to set.

> **height:** The height to set.


####WaitWhileBusy
Waits for the window to no longer be busy.

##Desktop.Pattern.ExpandCollapsePattern
            
Represents the Windows expand / collapse pattern.
        
###Properties

####ExpandCollapseState
Gets the current expanded state of the element.
####IsExpanded
Gets the value indicating the element is expanded.
###Methods


####Collapse
Collapses the element.

####Create(TestR.Desktop.DesktopElement)
Create a new pattern for the provided element.
> #####Parameters
> **element:** The element that supports the pattern.

> #####Return value
> The pattern if we could find one else null will be returned.

####Expand
Expands the element.

##Desktop.Pattern.ExpandCollapseState
            
Represents the state of the expand collapse pattern.
        
###Fields

####Collapsed
No children are visible.
####Expanded
All children are visible.
####PartiallyExpanded
Some, but not all, children are visible.
####LeafNode
The element does not expand or collapse.

##Desktop.Pattern.TogglePattern
            
Represents the Windows toggle pattern.
        
###Properties

####Toggled
Gets the toggled value.
####ToggleState
Gets the toggle state of the element.
###Methods


####Create(TestR.Desktop.DesktopElement)
Create a new pattern for the provided element.
> #####Parameters
> **element:** The element that supports the pattern.

> #####Return value
> The pattern if we could find one else null will be returned.

####Toggle
Toggle the element.

##Desktop.Pattern.ToggleState
            
Represents the state of a element with toggle pattern support.
        
###Fields

####On
The element is selected, checked, marked or otherwise activated.
####Off
The element is not selected, checked, marked or otherwise activated.
####Indeterminate
The element is in an indeterminate state.

##Desktop.Pattern.TransformPattern
            
Represents the Windows transform pattern.
        
###Methods


####Create(TestR.Desktop.DesktopElement)
Create a new pattern for the provided element.
> #####Parameters
> **element:** The element that supports the pattern.

> #####Return value
> The pattern if we could find one else null will be returned.

####Move(System.Int32,System.Int32)
Move the element.
> #####Parameters
> **x:** The x value of the position to move to.

> **y:** The y value of the position to move to.


####Resize(System.Int32,System.Int32)
Resize the element.
> #####Parameters
> **width:** The width to set.

> **height:** The height to set.


##Desktop.Pattern.ValuePattern
            
Represents the Windows value pattern.
        
###Properties

####IsReadOnly
Gets a value determining if the pattern is read only.
####Value
Gets the value of the pattern.
###Methods


####Create(TestR.Desktop.DesktopElement)
Create a new pattern for the provided element.
> #####Parameters
> **element:** The element that supports the pattern.

> #####Return value
> The pattern if we could find one else null will be returned.

####SetValue(System.String)
Set the value of the pattern.
> #####Parameters
> **value:** The value to set.


##Element
            
Represents an automation element.
        
###Properties

####BoundingRectangle
Gets the area of the element.
####Focused
Gets a value that indicates whether the element is focused.
####FullId
Gets the full id of the element which include all parent IDs prefixed to this element ID. Gets the ID of this element in the element host. Includes full host namespace. Ex. GrandParent\Parent\Element
####Height
Gets the width of the element.
####Item(System.String)
Gets or sets an attribute or property by name. The ID of the attribute or property to read.
####Location
Gets the location of the element.
####Size
Gets the size of the element.
####Width
Gets the width of the element.
####
Access an element by the Full ID, ID, or Name. The ID of the element. The element if found or null if not found.
####
Gets the parent element of this element collection.
####
Gets the application for this element host.
####
Gets a hierarchical list of elements.
####
Gets a flat list of elements.
####
Gets the current focused element.
####
Gets the ID of this element host.
####
Gets or sets the name of the element.
####
Gets the parent element of this element.
###Methods


####Constructor
Instantiates an instance of an element.
> #####Parameters
> **application:** 

> **parent:** 


####Click(System.Int32,System.Int32)
Performs mouse click at the center of the element.
> #####Parameters
> **x:** Optional X offset when clicking.

> **y:** Optional Y offset when clicking.


####Focus
Set focus on the element.

####MoveMouseTo(System.Int32,System.Int32)
Moves mouse cursor to the center of the element.
> #####Parameters
> **x:** Optional X offset when clicking.

> **y:** Optional Y offset when clicking.


####RightClick(System.Int32,System.Int32)
Performs mouse right click at the center of the element.
> #####Parameters
> **x:** Optional X offset when clicking.

> **y:** Optional Y offset when clicking.


####Constructor
Initializes an instance of the ElementCollection class.
> #####Parameters
> **parent:** 


####
Adds items to the .
> #####Parameters
> **items:** The objects to add to the .

> #####Exceptions
> **System.NotSupportedException:** The is read-only.


####
Check to see if this collection contains an element.
> #####Parameters
> **id:** The id to search for.

> #####Return value
> True if the id is found, false if otherwise.

####
Get an element from the collection using the provided condition.
> #####Parameters
> **id:** An ID of the element to get.

> **includeDescendants:** The flag that determines to include descendants or not.

> #####Return value
> The element matching the condition.

####
Get an element from the collection using the provided condition.
> #####Parameters
> **condition:** A function to test each element for a condition.

> **includeDescendants:** The flag that determines to include descendants or not.

> #####Return value
> The element matching the condition.

####
Get an element from the collection using the provided ID.
> #####Parameters
> **id:** An ID of the element to get.

> **includeDescendants:** The flag that determines to include descendants or not.

> #####Return value
> The child element for the condition.

####
Get an element from the collection using the provided condition.
> #####Parameters
> **condition:** A function to test each element for a condition.

> **includeDescendants:** The flag that determines to include descendants or not.

> #####Return value
> The child element for the condition.

####
Gets a collection of element of the provided type.
> #####Return value
> The collection of elements of the provided type.

####Constructor
Instantiates an instance of an element host.

####
Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.

####
Get the child from the children.
> #####Parameters
> **id:** An ID of the element to get.

> **recursive:** Flag to determine to include descendants or not.

> **wait:** Wait for the child to be available. Will auto refresh on each pass.

> #####Return value
> The child element for the ID.

####
Get the child from the children.
> #####Parameters
> **condition:** A function to test each element for a condition.

> **recursive:** Flag to determine to include descendants or not.

> **wait:** Wait for the child to be available. Will auto refresh on each pass.

> #####Return value
> The child element for the condition.

####
Get the child from the children.
> #####Parameters
> **id:** An ID of the element to get.

> **recursive:** Flag to determine to include descendants or not.

> **wait:** Wait for the child to be available. Will auto refresh on each pass.

> #####Return value
> The child element for the ID.

####
Get the child from the children.
> #####Parameters
> **condition:** A function to test each element for a condition.

> **recursive:** Flag to determine to include descendants or not.

> **wait:** Wait for the child to be available. Will auto refresh on each pass.

> #####Return value
> The child element for the condition.

####
Refresh the list of children for this host.

####
Update the children for this element.

####
Waits for the host to complete the work. Will wait until no longer busy.
> #####Parameters
> **minimumDelay:** The minimum delay in milliseconds to wait. Defaults to 0 milliseconds.


####
Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
> #####Parameters
> **disposing:** True if disposing and false if otherwise.


####
Invoke this method when the children changes.

####
Invoke this method when the host is closing.

####
Handles the excited event.
> #####Parameters
> **sender:** 

> **e:** 


##ElementCollection
            
Represents a collection of elements.
        
###Properties

####Item(System.String)
Access an element by the Full ID, ID, or Name. The ID of the element. The element if found or null if not found.
####Parent
Gets the parent element of this element collection.
###Methods


####Constructor
Initializes an instance of the ElementCollection class.
> #####Parameters
> **parent:** 


####Add(TestR.Element[])
Adds items to the .
> #####Parameters
> **items:** The objects to add to the .

> #####Exceptions
> **System.NotSupportedException:** The is read-only.


####Contains(System.String)
Check to see if this collection contains an element.
> #####Parameters
> **id:** The id to search for.

> #####Return value
> True if the id is found, false if otherwise.

####Get(System.String,System.Boolean)
Get an element from the collection using the provided condition.
> #####Parameters
> **id:** An ID of the element to get.

> **includeDescendants:** The flag that determines to include descendants or not.

> #####Return value
> The element matching the condition.

####Get(System.Func{TestR.Element,System.Boolean},System.Boolean)
Get an element from the collection using the provided condition.
> #####Parameters
> **condition:** A function to test each element for a condition.

> **includeDescendants:** The flag that determines to include descendants or not.

> #####Return value
> The element matching the condition.

####Get``1(System.String,System.Boolean)
Get an element from the collection using the provided ID.
> #####Parameters
> **id:** An ID of the element to get.

> **includeDescendants:** The flag that determines to include descendants or not.

> #####Return value
> The child element for the condition.

####Get``1(System.Func{``0,System.Boolean},System.Boolean)
Get an element from the collection using the provided condition.
> #####Parameters
> **condition:** A function to test each element for a condition.

> **includeDescendants:** The flag that determines to include descendants or not.

> #####Return value
> The child element for the condition.

####OfType``1
Gets a collection of element of the provided type.
> #####Return value
> The collection of elements of the provided type.

##ElementHost
            
Represents a host for a set of elements.
        
###Properties

####Application
Gets the application for this element host.
####Children
Gets a hierarchical list of elements.
####Elements
Gets a flat list of elements.
####FocusedElement
Gets the current focused element.
####Id
Gets the ID of this element host.
####Name
Gets or sets the name of the element.
####Parent
Gets the parent element of this element.
###Methods


####Constructor
Instantiates an instance of an element host.

####Dispose
Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.

####Get(System.String,System.Boolean,System.Boolean)
Get the child from the children.
> #####Parameters
> **id:** An ID of the element to get.

> **recursive:** Flag to determine to include descendants or not.

> **wait:** Wait for the child to be available. Will auto refresh on each pass.

> #####Return value
> The child element for the ID.

####Get(System.Func{TestR.Element,System.Boolean},System.Boolean,System.Boolean)
Get the child from the children.
> #####Parameters
> **condition:** A function to test each element for a condition.

> **recursive:** Flag to determine to include descendants or not.

> **wait:** Wait for the child to be available. Will auto refresh on each pass.

> #####Return value
> The child element for the condition.

####Get``1(System.String,System.Boolean,System.Boolean)
Get the child from the children.
> #####Parameters
> **id:** An ID of the element to get.

> **recursive:** Flag to determine to include descendants or not.

> **wait:** Wait for the child to be available. Will auto refresh on each pass.

> #####Return value
> The child element for the ID.

####Get``1(System.Func{``0,System.Boolean},System.Boolean,System.Boolean)
Get the child from the children.
> #####Parameters
> **condition:** A function to test each element for a condition.

> **recursive:** Flag to determine to include descendants or not.

> **wait:** Wait for the child to be available. Will auto refresh on each pass.

> #####Return value
> The child element for the condition.

####Refresh
Refresh the list of children for this host.

####UpdateChildren
Update the children for this element.

####WaitForComplete(System.Int32)
Waits for the host to complete the work. Will wait until no longer busy.
> #####Parameters
> **minimumDelay:** The minimum delay in milliseconds to wait. Defaults to 0 milliseconds.


####Dispose(System.Boolean)
Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
> #####Parameters
> **disposing:** True if disposing and false if otherwise.


####OnChildrenUpdated
Invoke this method when the children changes.

####OnClosed
Invoke this method when the host is closing.

####OnExited(System.Object,System.EventArgs)
Handles the excited event.
> #####Parameters
> **sender:** 

> **e:** 


##Extensions
            
Container for all extension methods.
        
###Methods


####AddRange``1(System.Collections.Generic.ICollection{``0},System.Collections.Generic.IEnumerable{``0})
Adds a range of items to the collection.
> #####Parameters
> **collection:** The collection to add the items to.

> **items:** The items to add to the collection.


####AsJToken(System.String)
Deserialize JSON data into a JToken class.
> #####Parameters
> **data:** The JSON data to deserialize.

> #####Return value
> The JToken class of the data.

####Contains(System.String,System.String,System.StringComparison)
Check to see if the string contains the value.
> #####Parameters
> **source:** The source string value.

> **value:** The value to search for.

> **comparisonType:** The type of comparison to use when searching.

> #####Return value
> True if the value is found and false if otherwise.

####FirstValue(System.Collections.Generic.IEnumerable{System.String})
Return the first string that is not null or empty.
> #####Parameters
> **collection:** The collection of string to parse.


####ForEach``1(System.Collections.Generic.IEnumerable{``0},System.Action{``0})
Performs an action on each item in the collection.
> #####Parameters
> **collection:** The collection of items to run the action with.

> **action:** The action to run against each item in the collection.


####ForEachDisposable``1(System.Collections.Generic.IEnumerable{``0},System.Action{``0})
Performs an action on each item in the collection.
> #####Parameters
> **collection:** The collection of items to run the action with.

> **action:** The action to run against each item in the collection.


####GetWindowLocation(System.Diagnostics.Process)
Get the main window location for the process.
> #####Parameters
> **process:** The process that contains the window.

> #####Return value
> The location of the window.

####GetWindows(System.Diagnostics.Process,TestR.Application)
Gets all windows for the process.
> #####Parameters
> **process:** The process to get windows for.

> **application:** The application the elements are for.

> #####Return value
> The array of windows.

####GetWindowSize(System.Diagnostics.Process)
Gets the size of the main window for the process.
> #####Parameters
> **process:** The process to size.

> #####Return value
> The size of the main window.

####Split(System.String,System.String,System.StringSplitOptions)
Splits a by a single separator.
> #####Parameters
> **value:** The string to be split.

> **separator:** The character to deliminate the string.

> **options:** The options to use when splitting.

> #####Return value
> The array of strings.

####ToInt(System.String)
Converts the string to an integer.
> #####Parameters
> **item:** The item to convert to an integer.

> #####Return value
> The JSON data of the object.

####ToJson``1(``0,System.Boolean)
Serializes the object to JSON.
> #####Parameters
> **item:** The item to serialize.

> **camelCase:** The flag to use camel case. If true then camel case else pascel case.

> #####Return value
> The JSON data of the object.

##Native.Keyboard
            
Represents the keyboard and allows for simulated input.
        
###Methods


####IsControlPressed
Determinse if the control key is pressed.
> #####Return value
> True if either control key is pressed.

####StartMonitoring
Start monitoring the keyboard for keystrokes.

####StopMonitoring
Stop monitoring the keyboard for keystrokes.

####TypeText(System.String)
Types text as keyboard input.
> #####Parameters
> **value:** 


##Native.Mouse
            
Represents the mouse and allows for simulated input.
        
###Fields

####MouseEvent.Unknown
Unknown event.
####MouseEvent.LeftButtonDown
Event for left button press.
####MouseEvent.LeftButtonUp
Event for left button release.
####MouseEvent.MouseMove
Event for mouse moving.
####MouseEvent.MouseWheel
Event for mouse wheel moving.
####MouseEvent.RightButtonDown
Event for right button press.
####MouseEvent.RightButtonUp
Event for right button release.
####
Gets the cursor that represents default and wait.
####
Gets the cursor that represents a pointer.
####
Gets the cursor that represents text edit.
####
Gets the cursor that represents wait.
###Properties

####Cursor
Gets the current cursor for the mouse.
####
Gets the current cursor for the mouse.
####
Gets a list of cursors that represent wait cursors.
###Methods


####GetCursorPosition
Gets the current position of the mouse.
> #####Return value
> The point location of the mouse cursor.

####LeftClick(System.Drawing.Point)
Left click at the provided point.
> #####Parameters
> **point:** The point in which to click.


####MoveTo(System.Drawing.Point)
Sets the mouse to the provide point.
> #####Parameters
> **point:** The point in which to move to.


####RightClick(System.Drawing.Point)
Right click at the provided point.
> #####Parameters
> **point:** The point in which to click.


####
Determines whether the specified is equal to the current .
> #####Parameters
> **obj:** The object to compare with the current object.

> #####Return value
> true if the specified object is equal to the current object; otherwise, false.

####
Serves as a hash function for a particular type.
> #####Return value
> A hash code for the current .

####
Returns a string that represents the current object.
> #####Return value
> A string that represents the current object.

####
Filters out a message before it is dispatched.
> #####Parameters
> **m:** The message to be dispatched. You cannot modify this message.

> #####Return value
> true to filter the message and stop it from being dispatched; false to allow the message to continue to the next filter or control.

##Native.Mouse.MouseEvent
            
Represents a mouse event.
        
###Fields

####Unknown
Unknown event.
####LeftButtonDown
Event for left button press.
####LeftButtonUp
Event for left button release.
####MouseMove
Event for mouse moving.
####MouseWheel
Event for mouse wheel moving.
####RightButtonDown
Event for right button press.
####RightButtonUp
Event for right button release.

##Native.MouseCursor
            
Represents a cursor for the mouse.
        
###Fields

####DefaultAndWait
Gets the cursor that represents default and wait.
####Pointer
Gets the cursor that represents a pointer.
####ShapedCursor
Gets the cursor that represents text edit.
####Wait
Gets the cursor that represents wait.
###Properties

####Current
Gets the current cursor for the mouse.
####WaitCursors
Gets a list of cursors that represent wait cursors.
###Methods


####Equals(System.Object)
Determines whether the specified is equal to the current .
> #####Parameters
> **obj:** The object to compare with the current object.

> #####Return value
> true if the specified object is equal to the current object; otherwise, false.

####GetHashCode
Serves as a hash function for a particular type.
> #####Return value
> A hash code for the current .

####ToString
Returns a string that represents the current object.
> #####Return value
> A string that represents the current object.

##Native.MouseMessageFilter
            
The filter to capture mouse messages.
        
###Methods


####PreFilterMessage(System.Windows.Forms.Message@)
Filters out a message before it is dispatched.
> #####Parameters
> **m:** The message to be dispatched. You cannot modify this message.

> #####Return value
> true to filter the message and stop it from being dispatched; false to allow the message to continue to the next filter or control.

##Utility
            
Represents the Utility class.
        
###Fields

####DefaultWaitDelay
The detail wait delay.
####DefaultWaitTimeout
The default wait timeout.
###Methods


####Wait(System.Func{System.Boolean},System.Double,System.Int32)
Runs the action until the action returns true or the timeout is reached. Will delay in between actions of the provided time.
> #####Parameters
> **action:** The action to call.

> **timeout:** The timeout to attempt the action. This value is in milliseconds.

> **delay:** The delay in between actions. This value is in milliseconds.

> #####Return value
> Returns true of the call completed successfully or false if it timed out.

####Wait``1(``0,System.Func{``0,System.Boolean},System.Double,System.Int32)
Runs the action until the action returns true or the timeout is reached. Will delay in between actions of the provided time.
> #####Parameters
> **input:** The input to pass to the action.

> **action:** The action to call.

> **timeout:** The timeout to attempt the action. This value is in milliseconds.

> **delay:** The delay in between actions. This value is in milliseconds.

> #####Return value
> Returns true of the call completed successfully or false if it timed out.

##Web.Browser
            
This is the base class for browsers.
             
        
###Fields

####DefaultTimeout
Gets the default timeout (in milliseconds).
####
The debugging argument for starting the browser.
####
The name of the browser.
####
The name of the browser.
####
The debugging argument for starting the browser.
####
The name of the browser.
####
The name of the browser.
####
Represents a Chrome browser.
####
Represents an Internet Explorer browser.
####
Represents a Firefox browser.
####
Represents an Edge browser.
####
Represents all browser types.
###Properties

####ActiveElement
Gets the current active element.
####AutoClose
Gets or sets a flag to auto close the browser when disposed of. Defaults to false.
####AutoRefresh
Gets or sets a flag that allows elements to refresh when reading properties. Defaults to true.
####BrowserType
Gets the type of the browser.
####FocusedElement

####Id
Gets the ID of the browser.
####JavascriptLibraries
Gets a list of JavaScript libraries that were detected on the page.
####RawHtml
Gets the raw HTML of the page.
####Uri
Gets the URI of the current page.
####
Gets the type of the browser.
####
Gets the type of the browser.
####
Gets the type of the browser.
####
Gets the type of the browser.
####
Gets the raw HTML of the page.
###Methods


####Constructor
Initializes a new instance of the Browser class.

####AttachBrowsers(TestR.Web.BrowserType)
Attach browsers for each type provided.
> #####Parameters
> **type:** The type of the browser to attach to.


####AttachOrCreate(TestR.Web.BrowserType)
Attach or create browsers for each type provided.
> #####Parameters
> **type:** The type of the browser to attach to or create.


####AttachToBrowser(System.Diagnostics.Process)
Attach process as a browser.
> #####Parameters
> **process:** The process of the browser to attach to.

> #####Return value
> The browser if successfully attached or otherwise null.

####BringToFront
Brings the application to the front and makes it the top window.

####CloseBrowsers(TestR.Web.BrowserType)
Closes all browsers of the provided type.
> #####Parameters
> **type:** The type of the browser to close.


####CreateBrowsers(TestR.Web.BrowserType)
Create browsers for each type provided.
> #####Parameters
> **type:** The type of the browser to create.


####ExecuteScript(System.String,System.Boolean)
Execute JavaScript code in the current document.
> #####Parameters
> **script:** The script to run.

> **expectResponse:** The script will return response.

> #####Return value
> The response from the script.

####ForEachBrowser(System.Action{TestR.Web.Browser},TestR.Web.BrowserType)
Process an action against a new instance of each browser type provided.
> #####Parameters
> **action:** The action to perform against each browser.

> **type:** The type of the browser to process against.


####GetTestScript
Inserts the test script into the current page.

####MoveWindow(System.Int32,System.Int32,System.Int32,System.Int32)
Move the window and resize it.
> #####Parameters
> **x:** The x coordinate to move to.

> **y:** The y coordinate to move to.

> **width:** The width of the window.

> **height:** The height of the window.


####NavigateTo(System.String,System.String)
Navigates the browser to the provided URI.
> #####Parameters
> **uri:** The URI to navigate to.

> **expectedUri:** The expected URI to navigate to.


####Refresh
Refresh the state because the browser page may have changed state.

####RemoveElement(TestR.Web.WebElement)
Removes the element from the page. * Experimental
> #####Parameters
> **element:** The element to remove.


####RemoveElementAttribute(TestR.Web.WebElement,System.String)
Removes an attribute from an element.
> #####Parameters
> **element:** The element to remove the attribute from.

> **name:** The name of the attribute to remove.


####WaitForComplete(System.Int32)

####WaitForNavigation(System.String,System.Nullable{System.TimeSpan})
Wait for the browser page to redirect to a provided URI.
> #####Parameters
> **uri:** The expected URI to land on. Defaults to empty string if not provided.

> **timeout:** The timeout before giving up on the redirect. Defaults to Timeout if not provided.


####BrowserNavigateTo(System.String)
Browser implementation of navigate to
> #####Parameters
> **uri:** The URI to navigate to.


####Dispose(System.Boolean)
Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
> #####Parameters
> **disposing:** True if disposing and false if otherwise.


####ExecuteJavaScript(System.String,System.Boolean)
Execute JavaScript code in the current document.
> #####Parameters
> **script:** The code script to execute.

> **expectResponse:** The script will return response.

> #####Return value
> The response from the execution.

####GetBrowserUri
Reads the current URI directly from the browser.
> #####Return value
> The current URI that was read from the browser.

####InjectTestScript
Injects the test script into the browser.

####RefreshElements
Refreshes the element collection for the current page.

####DetectJavascriptLibraries
Runs script to detect specific libraries.

####Constructor
Initializes a new instance of the Chrome class.
> #####Parameters
> **application:** The window of the existing browser.


####
Attempts to attach to an existing browser.
> #####Return value
> The browser instance or null if not found.

####
Attempts to attach to an existing browser.
> #####Return value
> The browser instance or null if not found.

####
Attempts to attach to an existing browser. If one is not found then create and return a new one.
> #####Return value
> The browser instance.

####
Attempts to create a new browser. If one is not found then we'll make sure it was started with the remote debugger argument. If not an exception will be thrown.
> #####Return value
> The browser instance.

####
Navigates the browser to the provided URI.
> #####Parameters
> **uri:** The URI to navigate to.


####
Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
> #####Parameters
> **disposing:** True if disposing and false if otherwise.


####
Execute JavaScript code in the current document.
> #####Parameters
> **script:** The code script to execute.

> **expectResponse:** The script will return response.

> #####Return value
> The response from the execution.

####
Reads the current URI directly from the browser.
> #####Return value
> The current URI that was read from the browser.

####
Connect to the Chrome browser debugger port.
> #####Exceptions
> **System.Exception:** All debugging sessions are taken.


####Constructor
Initializes a new instance of the Browser class.

####
Attempts to attach to an existing browser.
> #####Return value
> An instance of an Internet Explorer browser.

####
Attempts to attach to an existing browser.
> #####Return value
> The browser instance or null if not found.

####
Attempts to attach to an existing browser. If one is not found then create and return a new one.
> #####Return value
> An instance of an Internet Explorer browser.

####
Creates a new instance of an Edge browser.
> #####Return value
> An instance of an Edge browser.

####
Move the window and resize it.
> #####Parameters
> **x:** The x coordinate to move to.

> **y:** The y coordinate to move to.

> **width:** The width of the window.

> **height:** The height of the window.


####
Browser implementation of navigate to
> #####Parameters
> **uri:** The URI to navigate to.


####
Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
> #####Parameters
> **disposing:** True if disposing and false if otherwise.


####
Execute JavaScript code in the current document.
> #####Parameters
> **script:** The code script to execute.

> **expectResponse:** The script will return response.

> #####Return value
> The response from the execution.

####
Reads the current URI directly from the browser.
> #####Return value
> The current URI that was read from the browser.

####Constructor
Initializes a new instance of the Firefox class.
> #####Parameters
> **application:** The window of the existing browser.


####
Attempts to attach to an existing browser.
> #####Return value
> The browser instance or null if not found.

####
Attempts to attach to an existing browser.
> #####Return value
> The browser instance or null if not found.

####
Attempts to attach to an existing browser. If one is not found then create and return a new one.
> #####Return value
> The browser instance.

####
The Firefox browser must have the "listen 6000" command run in the console to enable remote debugging. A newly created browser will not be able to connect until someone manually starts the remote debugger.
Attempts to create a new browser.
> #####Return value
> The browser instance.

####
Navigates the browser to the provided URI.
> #####Parameters
> **uri:** The URI to navigate to.


####
Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
> #####Parameters
> **disposing:** True if disposing and false if otherwise.


####
Execute JavaScript code in the current document.
> #####Parameters
> **script:** The code script to execute.

> **expectResponse:** The script will return response.

> #####Return value
> The response from the execution.

####
Reads the current URI directly from the browser.
> #####Return value
> The current URI that was read from the browser.

####
Connect to the Firefox browser debugger port.
> #####Exceptions
> **System.Exception:** All debugging sessions are taken.


####
Attempts to attach to an existing browser.
> #####Return value
> An instance of an Internet Explorer browser.

####
Attempts to attach to an existing browser.
> #####Return value
> The browser instance or null if not found.

####
Attempts to attach to an existing browser. If one is not found then create and return a new one.
> #####Return value
> An instance of an Internet Explorer browser.

####
Creates a new instance of an Internet Explorer browser.
> #####Return value
> An instance of an Internet Explorer browser.

####
Navigates the browser to the provided URI.
> #####Parameters
> **uri:** The URI to navigate to.


####
Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
> #####Parameters
> **disposing:** True if disposing and false if otherwise.


####
Execute JavaScript code in the current document.
> #####Parameters
> **script:** The code script to execute.

> **expectResponse:** The script will return response.

> #####Return value
> The response from the execution.

####
Reads the current URI directly from the browser.
> #####Return value
> The current URI that was read from the browser.

####

####
Creates an instance of the InternetExplorer.
> #####Return value
> An instance of Internet Explorer.

####
Disconnects from the current browser and finds the new instance.

##Web.Browsers.Chrome
            
Represents an Chrome browser.
        
###Fields

####DebugArgument
The debugging argument for starting the browser.
####BrowserName
The name of the browser.
###Properties

####BrowserType
Gets the type of the browser.
###Methods


####Constructor
Initializes a new instance of the Chrome class.
> #####Parameters
> **application:** The window of the existing browser.


####Attach
Attempts to attach to an existing browser.
> #####Return value
> The browser instance or null if not found.

####Attach(System.Diagnostics.Process)
Attempts to attach to an existing browser.
> #####Return value
> The browser instance or null if not found.

####AttachOrCreate
Attempts to attach to an existing browser. If one is not found then create and return a new one.
> #####Return value
> The browser instance.

####Create
Attempts to create a new browser. If one is not found then we'll make sure it was started with the remote debugger argument. If not an exception will be thrown.
> #####Return value
> The browser instance.

####BrowserNavigateTo(System.String)
Navigates the browser to the provided URI.
> #####Parameters
> **uri:** The URI to navigate to.


####Dispose(System.Boolean)
Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
> #####Parameters
> **disposing:** True if disposing and false if otherwise.


####ExecuteJavaScript(System.String,System.Boolean)
Execute JavaScript code in the current document.
> #####Parameters
> **script:** The code script to execute.

> **expectResponse:** The script will return response.

> #####Return value
> The response from the execution.

####GetBrowserUri
Reads the current URI directly from the browser.
> #####Return value
> The current URI that was read from the browser.

####Connect
Connect to the Chrome browser debugger port.
> #####Exceptions
> **System.Exception:** All debugging sessions are taken.


##Web.Browsers.Edge
            
Represents an Edge browser.
        
###Fields

####BrowserName
The name of the browser.
###Properties

####BrowserType
Gets the type of the browser.
###Methods


####Constructor
Initializes a new instance of the Browser class.

####Attach
Attempts to attach to an existing browser.
> #####Return value
> An instance of an Internet Explorer browser.

####Attach(System.Diagnostics.Process)
Attempts to attach to an existing browser.
> #####Return value
> The browser instance or null if not found.

####AttachOrCreate
Attempts to attach to an existing browser. If one is not found then create and return a new one.
> #####Return value
> An instance of an Internet Explorer browser.

####Create
Creates a new instance of an Edge browser.
> #####Return value
> An instance of an Edge browser.

####MoveWindow(System.Int32,System.Int32,System.Int32,System.Int32)
Move the window and resize it.
> #####Parameters
> **x:** The x coordinate to move to.

> **y:** The y coordinate to move to.

> **width:** The width of the window.

> **height:** The height of the window.


####BrowserNavigateTo(System.String)
Browser implementation of navigate to
> #####Parameters
> **uri:** The URI to navigate to.


####Dispose(System.Boolean)
Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
> #####Parameters
> **disposing:** True if disposing and false if otherwise.


####ExecuteJavaScript(System.String,System.Boolean)
Execute JavaScript code in the current document.
> #####Parameters
> **script:** The code script to execute.

> **expectResponse:** The script will return response.

> #####Return value
> The response from the execution.

####GetBrowserUri
Reads the current URI directly from the browser.
> #####Return value
> The current URI that was read from the browser.

##Web.Browsers.Firefox
            
Represents a Firefox browser.
        
###Fields

####DebugArgument
The debugging argument for starting the browser.
####BrowserName
The name of the browser.
###Properties

####BrowserType
Gets the type of the browser.
###Methods


####Constructor
Initializes a new instance of the Firefox class.
> #####Parameters
> **application:** The window of the existing browser.


####Attach
Attempts to attach to an existing browser.
> #####Return value
> The browser instance or null if not found.

####Attach(System.Diagnostics.Process)
Attempts to attach to an existing browser.
> #####Return value
> The browser instance or null if not found.

####AttachOrCreate
Attempts to attach to an existing browser. If one is not found then create and return a new one.
> #####Return value
> The browser instance.

####Create
The Firefox browser must have the "listen 6000" command run in the console to enable remote debugging. A newly created browser will not be able to connect until someone manually starts the remote debugger.
Attempts to create a new browser.
> #####Return value
> The browser instance.

####BrowserNavigateTo(System.String)
Navigates the browser to the provided URI.
> #####Parameters
> **uri:** The URI to navigate to.


####Dispose(System.Boolean)
Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
> #####Parameters
> **disposing:** True if disposing and false if otherwise.


####ExecuteJavaScript(System.String,System.Boolean)
Execute JavaScript code in the current document.
> #####Parameters
> **script:** The code script to execute.

> **expectResponse:** The script will return response.

> #####Return value
> The response from the execution.

####GetBrowserUri
Reads the current URI directly from the browser.
> #####Return value
> The current URI that was read from the browser.

####Connect
Connect to the Firefox browser debugger port.
> #####Exceptions
> **System.Exception:** All debugging sessions are taken.


##Web.Browsers.InternetExplorer
            
Represents an Internet Explorer browser.
        
###Fields

####BrowserName
The name of the browser.
###Properties

####BrowserType
Gets the type of the browser.
####RawHtml
Gets the raw HTML of the page.
###Methods


####Attach
Attempts to attach to an existing browser.
> #####Return value
> An instance of an Internet Explorer browser.

####Attach(System.Diagnostics.Process)
Attempts to attach to an existing browser.
> #####Return value
> The browser instance or null if not found.

####AttachOrCreate
Attempts to attach to an existing browser. If one is not found then create and return a new one.
> #####Return value
> An instance of an Internet Explorer browser.

####Create
Creates a new instance of an Internet Explorer browser.
> #####Return value
> An instance of an Internet Explorer browser.

####BrowserNavigateTo(System.String)
Navigates the browser to the provided URI.
> #####Parameters
> **uri:** The URI to navigate to.


####Dispose(System.Boolean)
Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
> #####Parameters
> **disposing:** True if disposing and false if otherwise.


####ExecuteJavaScript(System.String,System.Boolean)
Execute JavaScript code in the current document.
> #####Parameters
> **script:** The code script to execute.

> **expectResponse:** The script will return response.

> #####Return value
> The response from the execution.

####GetBrowserUri
Reads the current URI directly from the browser.
> #####Return value
> The current URI that was read from the browser.

####WaitForComplete(System.Int32)

####CreateInternetExplorerClass
Creates an instance of the InternetExplorer.
> #####Return value
> An instance of Internet Explorer.

####ReinitializeBrowser
Disconnects from the current browser and finds the new instance.

##Web.BrowserType
            
The type of the browser.
        
###Fields

####Chrome
Represents a Chrome browser.
####InternetExplorer
Represents an Internet Explorer browser.
####Firefox
Represents a Firefox browser.
####Edge
Represents an Edge browser.
####All
Represents all browser types.

##Web.Elements.Abbreviation
            
Represents a browser abbreviation element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Acronym
            
Represents a browser Acronym element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Address
            
Represents a browser address element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Applet
            
Represents a browser Applet element.
        
###Properties

####Code
Gets or sets the code attribute. This should be a URL to the applet class file. Specifies the file name of a Java applet.
####Height
Gets or sets the height attribute.
####Object
Gets or sets the object attribute. Specifies a reference to a serialized representation of an applet.
####Width
Gets or sets the width attribute.
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Area
            
Represents a browser BoundingRectangle element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Article
            
Represents a browser article element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Aside
            
Represents a browser Aside element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Audio
            
Represents a browser Audio element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Base
            
Represents a browser Base element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.BaseFont
            
Represents a browser BaseFont element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.BiDirectionalIsolation
            
Represents a browser BiDirectionalIsolation element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.BiDirectionalOverride
            
Represents a browser BiDirectionalOverride element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Big
            
Represents a browser Big element.
            
Not supported in HTML5 use CSS instead.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.BlockQuote
            
Represents a block quotes element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Body
            
Represents a browser Body element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Bold
            
Represents a browser bold element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Button
            
Represent a browser button element.
        
###Properties

####AutoFocus
Gets or sets the autofocus attribute. HTML5: Specifies that a button should automatically get focus when the page loads.
####Disabled
Gets or sets the disabled attribute. Specifies that a button should be disabled.
####Form
Gets or sets the form attribute. HTML5: Specifies one or more forms the button belongs to.
####FormAction
Gets or sets the form action attribute. HTML5: Specifies where to send the form-data when a form is submitted. Only for type="submit".
####FormEncType
Gets or sets the form encoded type attribute. HTML5: Specifies how form-data should be encoded before sending it to a server. Only for type="submit".
####FormMethod
Gets or sets the form method attribute. HTML5: Specifies how to send the form-data (which HTTP method to use). Only for type="submit".
####FormNoValidate
Gets or sets the form no validate attribute. HTML5: Specifies that the form-data should not be validated on submission. Only for type="submit".
####FormTarget
Gets or sets the form target attribute. HTML5: Specifies where to display the response after submitting the form. Only for type="submit".
####Text
Gets or sets the value attribute.
####Value
Gets or sets the value attribute.
###Methods


####Constructor
Initializes an instance of a Button browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Canvas
            
Represents a browser Canvas element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Caption
            
Represents a browser Caption element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Center
            
Represents a browser Center element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.CheckBox
            
Represent a browser input checkbox element.
        
###Properties

####Checked
Gets or sets the checked attribute. Specifies that an element should be pre-selected when the page loads (for type="checkbox" or type="radio").
####Disabled
Gets or sets the disabled attribute. Specifies that a button should be disabled.
####Form
Gets or sets the form attribute. HTML5: Specifies one or more forms the button belongs to.
####FormNoValidate
Gets or sets the form no validate attribute. HTML5: Specifies that the form-data should not be validated on submission. Only for type="submit".
####Text
Gets or sets the value attribute.
####Value
Gets or sets the value attribute.
###Methods


####Constructor
Initializes an instance of a check box browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Cite
            
Represents a browser Cite element.
            
In HTML5 this tag defines the title of a work. In HTML4 this tag defines a citation.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Code
            
Represents a browser code element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Column
            
Represents a browser Column (col) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.ColumnGroup
            
Represents a browser ColumnGroup element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.DataList
            
Represents a browser DataList element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Definition
            
Represents a browser Definition (dfn) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Deleted
            
Represents a browser Deleted (del) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.DescriptionList
            
Represents a browser DescriptionList (dl) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.DescriptionListDefinition
            
Represents a browser DescriptionListDefinition (dd) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.DescriptionListTerm
            
Represents a browser DescriptionListTerm (dt) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Details
            
Represents a browser Details element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Dialog
            
Represents a browser Dialog element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Directory
            
Represents a browser Directory (dir) element.
            
Not supported in HTML5 so use CSS instead.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Division
            
Represents a browser division (div) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Embed
            
Represents a browser Embed element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Emphasis
            
Represents a browser Emphasis (em) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.FieldSet
            
Represents a browser fieldset element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Figure
            
Represents a browser Figure element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.FigureCaption
            
Represents a browser FigureCaption (figcapture) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Font
            
Represents a browser Font element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Footer
            
Represents a browser Footer element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Form
            
Represent a browser form element.
        
###Properties

####Action
Gets or sets the action attribute. Specifies where to send the form-data when a form is submitted.
####AutoComplete
Gets or sets the auto complete attribute. HTML5: Specifies whether a form should have autocomplete on or off.
####EncType
Gets or sets the encoded type attribute. Specifies how the form-data should be encoded when submitting it to the server (only for method="post").
####Method
Gets or sets the method attribute. Specifies the HTTP method to use when sending form-data.
####NoValidate
Gets or sets the no validate attribute. HTML5: Specifies that the form should not be validated when submitted.
####Target
Gets or sets the target attribute. Specifies where to display the response that is received after submitting the form.
###Methods


####Constructor
Initializes an instance of a form browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Frame
            
Represents a browser Frame element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.FrameSet
            
Represents a browser FrameSet element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Head
            
Represents a browser Head element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Header
            
Represents a browser header element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.HeadingGroup
            
Represents a browser HeadingGroup (hgroup) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.HorizontalRule
            
Represents a browser horizontal rule element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Html
            
Represents a browser Html element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Image
            
Represents a browser text input["image"] element.
        
###Properties

####Alt
Gets or sets the alt attribute. Specifies an alternate text for images.
####Height
Gets or sets the height attribute. HTML5: Specifies the height of an input element.
####Src
Gets or sets the value attribute. Specifies the URL of the image to use as a submit button.
####Value
Gets or sets the value attribute. Specifies the value of an input element.
####Width
Gets or sets the width attribute. HTML5: Specifies the width of an input element.
###Methods


####Constructor
Initializes an instance of a Image browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.InlineFrame
            
Represents a browser InlineFrame element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Insert
            
Represents a browser Insert (ins) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Italic
            
Represents a browser Italic element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Keyboard
            
Represents a browser Keyboard (kbd) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.KeyGenerator
            
Represents a browser KeyGenerator (keygen) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Label
            
Represents a browser label element.
        
###Properties

####For
Gets or sets the element id (for) attribute. Specifies which form element a label is bound to.
####Form
Gets or sets the form id (form) attribute. HTML5: Specifies one or more forms the label belongs to.
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Legend
            
Represents a browser legend element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.LineBreak
            
Represent a browser line break element.
        
###Methods


####Constructor
Initializes an instance of a line break browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Link
            
Represent a browser link element.
        
###Properties

####Download
Gets or set the download attribute. HTML5: Specifies that the target will be downloaded when a user clicks on the hyper link.
####Href
Gets or set the hypertext reference (href) attribute. Specifies the URL of the page the link goes to.
####Media
Gets or set the media attribute. HTML5: Specifies what media/device the linked document is optimized for.
####Rel
Gets or set the hypertext reference of this link. The rel attribute specifies the relationship between the current document and the linked document. Only used if the href attribute is present.
####Target
Gets or set the target of this link. Specifies where to open the linked document.
####Type
Gets or set the media type of this link. The Internet media type of the linked document. Look at IANA Media Types for a complete list of standard media types.
###Methods


####Constructor
Initializes an instance of a Link browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.ListItem
            
Represents a browser list item element.
        
###Properties

####Value
Gets or sets the value attribute. Specifies the value of a list item. The following list items will increment from that number (only for ordered lists).
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Main
            
Represents a browser Main element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Map
            
Represents a browser Map element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Mark
            
Represents a browser Mark element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Menu
            
Represents a browser Menu element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.MenuItem
            
Represents a browser MenuItem element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Metadata
            
Represents a browser Metadata (meta) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Meter
            
Represents a browser Meter element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Navigation
            
Represents a browser Navigation (nav) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.NoFrames
            
Represents a browser NoFrames element.
            
Not supported in HTML5.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.NoScript
            
Represents a browser NoScript element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Object
            
Represents a browser Object element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Option
            
Represents a browser option element.
        
###Properties

####Disabled
Gets or sets the disabled attribute. Specifies that an option should be disabled.
####Label
Gets or sets the label attribute. Specifies a shorter label for an option.
####Selected
Gets or sets the selected attribute. Specifies that an option should be pre-selected when the page loads.
####Value
Gets or sets the value for this select. Specifies the value to be sent to a server.
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.OptionGroup
            
Represents a browser OptionGroup (optgroup) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.OrderedList
            
Represents a browser ordered list element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Output
            
Represents a browser Output element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Paragraph
            
Represents a browser paragraph (p) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Parameter
            
Represents a browser Parameter (param) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.PreformattedText
            
Represents a browser PreformattedText (pre) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Progress
            
Represents a browser Progress element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Quotation
            
Represents a browser Quotation (q) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.RadioButton
            
Represent a browser input radio button element.
        
###Properties

####Checked
Gets or sets the checked attribute. Specifies that an element should be pre-selected when the page loads (for type="checkbox" or type="radio").
####Disabled
Gets or sets the disabled attribute. Specifies that a button should be disabled.
####Form
Gets or sets the form attribute. HTML5: Specifies one or more forms the button belongs to.
####FormNoValidate
Gets or sets the form no validate attribute. HTML5: Specifies that the form-data should not be validated on submission. Only for type="submit".
####Text
Gets or sets the value attribute.
####Value
Gets or sets the value attribute.
###Methods


####Constructor
Initializes an instance of a input radio button browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Ruby
            
Represents a browser Ruby element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.RubyExplanation
            
Represents a browser RubyExplanation (rp) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.RubyTag
            
Represents a browser RubyTag (rt) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Sample
            
Represents a browser Sample (samp) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Script
            
Represents a browser Script element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Section
            
Represents a browser Section element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Select
            
Represents a browser select element.
        
###Properties

####AutoFocus
Gets or sets the autofocus attribute. HTML5: Specifies that the drop-down list should automatically get focus when the page loads.
####Disabled
Gets or sets the disabled attribute. Specifies that a drop-down list should be disabled.
####Form
Gets or sets the form id (form) attribute. HTML5: Defines one or more forms the select field belongs to.
####Multiple
Gets or sets the multiple attribute. Specifies that multiple options can be selected at once.
####Required
Gets or sets the required attribute. HTML5: Specifies that the user is required to select a value before submitting the form.
####SelectedOption
Returns the selected option or null if nothing is selected.
####OptionCount
Gets or sets the size attribute. Defines the number of visible options in a drop-down list.
####Value
Gets or sets the value for this select.
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Small
            
Represents a browser small element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Source
            
Represents a browser Source element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Span
            
Represents a browser span element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Strike
            
Represents a browser Strike element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.StrikeThrough
            
Represents a browser StrikeThrough (s) element.
            
The strike through element was deprecated in HTML4 use the delete (del) tag instead.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Strong
            
Represents a browser strong element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Style
            
Represents a browser Style element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.StyleSheetLink
            
Represents a browser StyleSheetLink (link) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.SubScript
            
Represents a browser SubScript (sub) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Summary
            
Represents a browser Summary element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.SuperScriptText
            
Represents a browser SuperScriptText (sup) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Table
            
Represents a browser table (table) element.
        
###Properties

####Sortable
Gets or sets the sortable attribute. Specifies that the table should be sortable.
####
Gets or sets the column span (colspan) attribute. Specifies the number of columns a cell should span.
####
Gets or sets the header id (headers) attribute. Specifies one or more header cells a cell is related to.
####
Gets or sets the row span (rowspan) attribute. Sets the number of rows a cell should span.
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.TableBody
            
Represents a browser table body (tbody) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.TableColumn
            
Represents a browser table column (td) element.
        
###Properties

####ColumnSpan
Gets or sets the column span (colspan) attribute. Specifies the number of columns a cell should span.
####Headers
Gets or sets the header id (headers) attribute. Specifies one or more header cells a cell is related to.
####RowSpan
Gets or sets the row span (rowspan) attribute. Sets the number of rows a cell should span.
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.TableFooter
            
Represents a browser TableFooter (tfoot) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.TableHead
            
Represents a browser table head (thead) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.TableHeaderColumn
            
Represents a browser TableHeaderColumn (th) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.TableRow
            
Represents a browser table row (tr) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.TeletypeText
            
Represents a browser TeletypeText (tt) element.
            
This tag is not supported in HTML5 so use CSS instead.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.TextArea
            
Represents a browser text area element.
        
###Properties

####AutoFocus
Gets or sets the autofocus attribute. HTML5: Specifies that an input element should automatically get focus when the page loads.
####Cols
Gets or sets the cols attribute. Specifies the visible width of a text area.
####Disabled
Gets or sets the disabled attribute. Specifies that a button should be disabled.
####Form
Gets or sets the form attribute. HTML5: Specifies one or more forms the text area belongs to.
####MaxLength
Gets or sets the max length attribute. HTML5: Specifies the maximum number of characters allowed in the text area.
####PlaceHolder
Gets or sets the place holder attribute. HTML5: Specifies a short hint that describes the expected value of an input element.
####ReadOnly
Gets or sets the read only attribute. Specifies that an input field is read-only
####Required
Gets or sets the required attribute. HTML5: Specifies that a text area is required/must be filled out.
####Rows
Gets or sets the rows attribute. Specifies the visible number of lines in a text area.
####Text
Gets or sets the value attribute. Specifies the value of an input element.
####TypingDelay
Gets the delay (in milliseconds) between each character.
####Value
Gets or sets the value attribute. Specifies the value of an input element.
####Wrap
Gets or sets the wrap attribute. HTML5: Specifies how the text in a text area is to be wrapped when submitted in a form.
###Methods


####Constructor
Initializes an instance of a text area browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


####TypeText(System.String,System.Boolean)
Type text into the element.
> #####Parameters
> **value:** The value to be typed.

> **reset:** Resets the text in the element before typing the text.


##Web.Elements.TextInput
            
Represents a browser text input element.
        
###Properties

####AutoFocus
Gets or sets the autofocus attribute. HTML5: Specifies that an input element should automatically get focus when the page loads.
####Disabled
Gets or sets the disabled attribute. Specifies that a button should be disabled.
####Pattern
Gets or sets the pattern attribute. HTML5: Specifies a regular expression that an input element's value is checked against.
####PlaceHolder
Gets or sets the place holder attribute. HTML5: Specifies a short hint that describes the expected value of an input element.
####ReadOnly
Gets or sets the read only attribute. Specifies that an input field is read-only
####Step
Gets or sets the step attribute. HTML5: Specifies the legal number intervals for an input field.
####Text
Gets or sets the value attribute. Specifies the value of an input element.
####TypingDelay
Gets the delay (in milliseconds) between each character.
####Value
Gets or sets the value attribute. Specifies the value of an input element.
###Methods


####Constructor
Initializes an instance of a TextInput browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


####TypeText(System.String,System.Boolean)
Type text into the element.
> #####Parameters
> **value:** The value to be typed.

> **reset:** Clear the input before typing the text.


##Web.Elements.Time
            
Represents a browser Time element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Title
            
Represents a browser Title element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Track
            
Represents a browser Track element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Underline
            
Represents a browser underline element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.UnorderedList
            
Represents a browser unordered list element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Variable
            
Represents a browser Variable (var) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.Video
            
Represents a browser Video element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.Elements.WordBreakOpportunity
            
Represents a browser WordBreakOpportunity (wbr) element.
        
###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** The parent host for this element.


##Web.JavaScriptLibrary
            
Specifies different JavaScript libraries that are detected after page navigation in the browser.
        
###Fields

####Angular
AngularJS
####JQuery
JQuery
####Moment
Moment
####Bootstrap2
Bootstrap 2
####Bootstrap3
Bootstrap 3

##Web.WebElement
            
Represents an element for a browser.
        
###Fields

####_propertiesToRename
Properties that need to be renamed when requested.
###Properties

####Browser
Gets the browser this element is currently associated with.
####Focused

####FocusedElement

####Height

####Id

####Item(System.String)

####Location

####Name

####TagName
Gets the tag element name.
####Text
Gets or sets the text content.
####Title
Gets or sets the title attribute. Specifies extra information about an element.
####Width

###Methods


####Constructor
Initializes an instance of a browser element.
> #####Parameters
> **element:** The browser element this is for.

> **browser:** The browser this element is associated with.

> **parent:** 


####Click(System.Int32,System.Int32)

####FireEvent(System.String,System.Collections.Generic.Dictionary{System.String,System.String})
Fires an event on the element.
> #####Parameters
> **eventName:** The events name to fire.

> **eventProperties:** The properties for the event.


####Focus
Focuses on the element.

####GetAttributeValue(System.String)
Gets an attribute value by the provided name.
> #####Parameters
> **name:** The name of the attribute to read.

> #####Return value
> The attribute value.

####GetAttributeValue(System.String,System.Boolean)
Gets an attribute value by the provided name.
> #####Parameters
> **name:** The name of the attribute to read.

> **refresh:** A flag to force the element to refresh.

> #####Return value
> The attribute value.

####GetStyleAttributeValue(System.String)
Gets an attribute style value by the provided name.
> #####Parameters
> **name:** The name of the attribute style to read.

> #####Return value
> The attribute style value.

####GetStyleAttributeValue(System.String,System.Boolean)
Gets an attribute style value by the provided name.
> #####Parameters
> **name:** The name of the attribute style to read.

> **forceRefresh:** A flag to force the element to refresh.

> #####Return value
> The attribute style value.

####Highlight(System.Boolean)
Highlight or resets the element.
> #####Parameters
> **highlight:** If true the element is highlight yellow. If false the element is returned to its original color.


####MoveMouseTo(System.Int32,System.Int32)

####Refresh

####RightClick(System.Int32,System.Int32)

####SetAttributeValue(System.String,System.String)
Sets an attribute value by the provided name.
> #####Parameters
> **name:** The name of the attribute to write.

> **value:** The value to be written.


####SetStyleAttributeValue(System.String,System.String)
Sets an attribute style value by the provided name.
> #####Parameters
> **name:** The name of the attribute style to write.

> **value:** The style value to be written.


####WaitForComplete(System.Int32)

####Dispose(System.Boolean)

####GetKeyCodeEventProperty(System.Char)
Get the key code event properties for the character.
> #####Parameters
> **character:** The character to get the event properties for.

> #####Return value
> An event properties for the character.

####TriggerElement
Triggers the element via the Angular function "trigger".

####AddOrUpdateElementAttribute(System.String,System.String)
Add or updates the cached attributes for this element.
> #####Parameters
> **name:** 

> **value:** 


####GetCachedAttribute(System.String)
Gets the attribute from the local cache.
> #####Parameters
> **name:** The name of the attribute.

> #####Return value
> Returns the value or null if the attribute was not found.

####SetElementAttributeValue(System.String,System.String)
Sets the element attribute value. If the attribute is not found we'll add it.
> #####Parameters
> **name:** 

> **value:** 
