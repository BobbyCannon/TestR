#region References

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json.Linq;
using TestR.Extensions;
using TestR.Web.Elements;
using Object = TestR.Web.Elements.Object;

#endregion

namespace TestR.Web
{
	/// <summary>
	/// Represents a collection of specific type of elements.
	/// </summary>
	/// <typeparam name="T"> The type of element. </typeparam>
	public class ElementCollection<T> : Collection<T>
		where T : Element
	{
		#region Constructors

		/// <summary>
		/// Initializes an instance of the ElementCollection class.
		/// </summary>
		/// <param name="collection"> The collection of elements to add to the new collection. </param>
		public ElementCollection(IEnumerable<T> collection)
		{
			this.AddRange(collection);
		}

		#endregion

		#region Indexers

		/// <summary>
		/// Access an element by the ID.
		/// </summary>
		/// <param name="id"> The ID of the element. </param>
		public T this[string id]
		{
			get { return this.FirstOrDefault(x => x.Id == id); }
		}

		#endregion
	}

	/// <summary>
	/// Represents a collection of elements.
	/// </summary>
	public class ElementCollection : Collection<Element>
	{
		#region Constructors

		/// <summary>
		/// Initializes an instance of the ElementCollection class.
		/// </summary>
		public ElementCollection()
		{
		}

		/// <summary>
		/// Initializes an instance of the ElementCollection class.
		/// </summary>
		/// <param name="collection"> The collection of elements to add to this collection. </param>
		public ElementCollection(IEnumerable<Element> collection)
		{
			this.AddRange(collection);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets a list of all abbreviation (abbr) elements.
		/// </summary>
		public ElementCollection<Abbreviation> Abbreviations
		{
			get { return OfType<Abbreviation>(); }
		}

		/// <summary>
		/// Gets a list of all address (address) elements.
		/// </summary>
		public ElementCollection<Address> Addresses
		{
			get { return OfType<Address>(); }
		}

		/// <summary>
		/// Gets a list of all area (area) elements.
		/// </summary>
		public ElementCollection<Area> Areas
		{
			get { return OfType<Area>(); }
		}

		/// <summary>
		/// Gets a list of all article (article) elements.
		/// </summary>
		public ElementCollection<Article> Articles
		{
			get { return OfType<Article>(); }
		}

		/// <summary>
		/// Gets a list of all block quote (blockquote) elements.
		/// </summary>
		public ElementCollection<BlockQuote> BlockQuotes
		{
			get { return OfType<BlockQuote>(); }
		}

		/// <summary>
		/// Gets a list of all bold (b) elements.
		/// </summary>
		public ElementCollection<Bold> Bolds
		{
			get { return OfType<Bold>(); }
		}

		/// <summary>
		/// Gets a list of all button (button, input[type="button"]) elements.
		/// </summary>
		public ElementCollection<Button> Buttons
		{
			get { return OfType<Button>(); }
		}

		/// <summary>
		/// Gets a list of all input checkboxes (input[type="checkbox"]) elements.
		/// </summary>
		public ElementCollection<CheckBox> Checkboxes
		{
			get { return OfType<CheckBox>(); }
		}

		/// <summary>
		/// Gets a list of all code (code) elements.
		/// </summary>
		public ElementCollection<Code> Codes
		{
			get { return OfType<Code>(); }
		}

		/// <summary>
		/// Gets a list of all Division (div) elements.
		/// </summary>
		public ElementCollection<Division> Divisions
		{
			get { return OfType<Division>(); }
		}

		/// <summary>
		/// Gets a list of all field set (fieldset) elements.
		/// </summary>
		public ElementCollection<FieldSet> FieldSets
		{
			get { return OfType<FieldSet>(); }
		}

		/// <summary>
		/// Gets a list of all form (form) elements.
		/// </summary>
		public ElementCollection<Form> Forms
		{
			get { return OfType<Form>(); }
		}

		/// <summary>
		/// Gets a list of all header (h1, h2, h3, h4, h5) elements.
		/// </summary>
		public ElementCollection<Header> Headers
		{
			get { return OfType<Header>(); }
		}

		/// <summary>
		/// Gets a list of all horizontal rule (hr) elements.
		/// </summary>
		public ElementCollection<HorizontalRule> HorizontalRules
		{
			get { return OfType<HorizontalRule>(); }
		}

		/// <summary>
		/// Gets a list of all image (img, input[type="image"]) elements.
		/// </summary>
		public ElementCollection<Image> Images
		{
			get { return OfType<Image>(); }
		}

		/// <summary>
		/// Gets a list of all label (label) elements.
		/// </summary>
		public ElementCollection<Label> Labels
		{
			get { return OfType<Label>(); }
		}

		/// <summary>
		/// Gets a list of all legend (legend) elements.
		/// </summary>
		public ElementCollection<Legend> Legends
		{
			get { return OfType<Legend>(); }
		}

		/// <summary>
		/// Gets a list of all line break (br) elements.
		/// </summary>
		public ElementCollection<LineBreak> LineBreaks
		{
			get { return OfType<LineBreak>(); }
		}

		/// <summary>
		/// Gets a list of all link (a) elements.
		/// </summary>
		public ElementCollection<Link> Links
		{
			get { return OfType<Link>(); }
		}

		/// <summary>
		/// Gets a list of all list items (li) elements.
		/// </summary>
		public ElementCollection<ListItem> ListItems
		{
			get { return OfType<ListItem>(); }
		}

		/// <summary>
		/// Gets a list of all option (option) elements.
		/// </summary>
		public ElementCollection<Option> Options
		{
			get { return OfType<Option>(); }
		}

		/// <summary>
		/// Gets a list of all ordered list (ol) elements.
		/// </summary>
		public ElementCollection<OrderedList> OrderedLists
		{
			get { return OfType<OrderedList>(); }
		}

		/// <summary>
		/// Gets a list of all paragraph (p) elements.
		/// </summary>
		public ElementCollection<Paragraph> Paragraphs
		{
			get { return OfType<Paragraph>(); }
		}

		/// <summary>
		/// Gets a list of all input radio button (input[type="radio"]) elements.
		/// </summary>
		public ElementCollection<RadioButton> RadioButtons
		{
			get { return OfType<RadioButton>(); }
		}

		/// <summary>
		/// Gets a list of all select (select) elements.
		/// </summary>
		public ElementCollection<Select> Selects
		{
			get { return OfType<Select>(); }
		}

		/// <summary>
		/// Gets a list of all small (small) elements.
		/// </summary>
		public ElementCollection<Small> Smalls
		{
			get { return OfType<Small>(); }
		}

		/// <summary>
		/// Gets a list of all span elements.
		/// </summary>
		public ElementCollection<Span> Spans
		{
			get { return OfType<Span>(); }
		}

		/// <summary>
		/// Gets a list of all strong (strong) elements.
		/// </summary>
		public ElementCollection<Strong> Strongs
		{
			get { return OfType<Strong>(); }
		}

		/// <summary>
		/// Gets a list of all table body (tbody) elements.
		/// </summary>
		public ElementCollection<TableBody> TableBodies
		{
			get { return OfType<TableBody>(); }
		}

		/// <summary>
		/// Gets a list of all table column (td) elements.
		/// </summary>
		public ElementCollection<TableColumn> TableColumns
		{
			get { return OfType<TableColumn>(); }
		}

		/// <summary>
		/// Gets a list of all table head (thead) elements.
		/// </summary>
		public ElementCollection<TableHead> TableHeads
		{
			get { return OfType<TableHead>(); }
		}

		/// <summary>
		/// Gets a list of all table row (tr) elements.
		/// </summary>
		public ElementCollection<TableRow> TableRows
		{
			get { return OfType<TableRow>(); }
		}

		/// <summary>
		/// Gets a list of all table (table) elements.
		/// </summary>
		public ElementCollection<Table> Tables
		{
			get { return OfType<Table>(); }
		}

		/// <summary>
		/// Gets a list of all text area (textarea) elements.
		/// </summary>
		public ElementCollection<TextArea> TextArea
		{
			get { return OfType<TextArea>(); }
		}

		/// <summary>
		/// Gets a list of all text input (input[type="text"]) elements.
		/// </summary>
		public ElementCollection<TextInput> TextInputs
		{
			get { return OfType<TextInput>(); }
		}

		/// <summary>
		/// Gets a list of all underline (u) elements.
		/// </summary>
		public ElementCollection<Underline> Underlines
		{
			get { return OfType<Underline>(); }
		}

		/// <summary>
		/// Gets a list of all unordered list (ul) elements.
		/// </summary>
		public ElementCollection<UnorderedList> UnorderedLists
		{
			get { return OfType<UnorderedList>(); }
		}

		#endregion

		#region Indexers

		/// <summary>
		/// Access an element by the ID.
		/// </summary>
		/// <param name="id"> The ID of the element. </param>
		public Element this[string id]
		{
			get { return this.FirstOrDefault(x => x.Id == id); }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Adds a collection of elements and initializes them as their specific element type.
		/// </summary>
		/// <param name="token"> The collection of elements to add. </param>
		/// <param name="browser"> The browser the element is for. </param>
		public void Add(JToken token, Browser browser)
		{
			var element = new Element(token, browser, this);

			switch (element.TagName)
			{
				case "a":
					Add(new Link(token, browser, this));
					return;

				case "abbr":
					Add(new Abbreviation(token, browser, this));
					return;

				case "acronym":
					Add(new Acronym(token, browser, this));
					return;

				case "address":
					Add(new Address(token, browser, this));
					return;

				case "applet":
					Add(new Applet(token, browser, this));
					return;

				case "area":
					Add(new Area(token, browser, this));
					return;

				case "article":
					Add(new Article(token, browser, this));
					return;

				case "aside":
					Add(new Aside(token, browser, this));
					return;

				case "audio":
					Add(new Audio(token, browser, this));
					return;

				case "b":
					Add(new Bold(token, browser, this));
					return;

				case "base":
					Add(new Base(token, browser, this));
					return;

				case "basefont":
					Add(new BaseFont(token, browser, this));
					return;

				case "bdi":
					Add(new BiDirectionalIsolation(token, browser, this));
					return;

				case "bdo":
					Add(new BiDirectionalOverride(token, browser, this));
					return;

				case "big":
					Add(new Big(token, browser, this));
					return;

				case "blockquote":
					Add(new BlockQuote(token, browser, this));
					return;

				case "body":
					Add(new Body(token, browser, this));
					return;

				case "br":
					Add(new LineBreak(token, browser, this));
					return;

				case "button":
					Add(new Button(token, browser, this));
					return;

				case "canvas":
					Add(new Canvas(token, browser, this));
					return;

				case "caption":
					Add(new Caption(token, browser, this));
					return;

				case "center":
					Add(new Center(token, browser, this));
					return;

				case "cite":
					Add(new Cite(token, browser, this));
					return;

				case "code":
					Add(new Code(token, browser, this));
					return;

				case "col":
					Add(new Column(token, browser, this));
					return;

				case "colgroup":
					Add(new ColumnGroup(token, browser, this));
					return;

				case "datalist":
					Add(new DataList(token, browser, this));
					return;

				case "dd":
					Add(new DescriptionListDefinition(token, browser, this));
					return;

				case "del":
					Add(new Deleted(token, browser, this));
					return;

				case "details":
					Add(new Details(token, browser, this));
					return;

				case "dfn":
					Add(new Definition(token, browser, this));
					return;

				case "dialog":
					Add(new Dialog(token, browser, this));
					return;

				case "dir":
					Add(new Directory(token, browser, this));
					return;

				case "div":
					Add(new Division(token, browser, this));
					return;

				case "dl":
					Add(new DescriptionList(token, browser, this));
					return;

				case "dt":
					Add(new DescriptionListTerm(token, browser, this));
					return;

				case "em":
					Add(new Emphasis(token, browser, this));
					return;

				case "embed":
					Add(new Embed(token, browser, this));
					return;

				case "fieldset":
					Add(new FieldSet(token, browser, this));
					return;

				case "figcaption":
					Add(new FigureCaption(token, browser, this));
					return;

				case "figure":
					Add(new Figure(token, browser, this));
					return;

				case "font":
					Add(new Font(token, browser, this));
					return;

				case "footer":
					Add(new Footer(token, browser, this));
					return;

				case "form":
					Add(new Form(token, browser, this));
					return;

				case "frame":
					Add(new Frame(token, browser, this));
					return;

				case "frameset":
					Add(new FrameSet(token, browser, this));
					return;

				case "head":
					Add(new Head(token, browser, this));
					return;

				case "header":
					Add(new Header(token, browser, this));
					return;

				case "hgroup":
					Add(new HeadingGroup(token, browser, this));
					return;

				case "h1":
				case "h2":
				case "h3":
				case "h4":
				case "h5":
				case "h6":
					Add(new Header(token, browser, this));
					return;

				case "hr":
					Add(new HorizontalRule(token, browser, this));
					return;

				case "html":
					Add(new Html(token, browser, this));
					return;

				case "i":
					Add(new Italic(token, browser, this));
					return;

				case "iframe":
					Add(new InlineFrame(token, browser, this));
					return;

				case "img":
					Add(new Image(token, browser, this));
					return;

				case "input":
					var type = element.GetAttributeValue("type", false).ToLower();
					switch (type)
					{
						case "checkbox":
							Add(new CheckBox(token, browser, this));
							return;

						case "image":
							Add(new Image(token, browser, this));
							return;

						case "button":
						case "submit":
						case "reset":
							Add(new Button(token, browser, this));
							return;

						case "email":
						case "hidden":
						case "number":
						case "password":
						case "search":
						case "tel":
						case "text":
						case "url":
							Add(new TextInput(token, browser, this));
							return;

						case "radio":
							Add(new RadioButton(token, browser, this));
							return;

						default:
							Add(element);
							return;
					}

				case "ins":
					Add(new Insert(token, browser, this));
					return;

				case "kbd":
					Add(new Keyboard(token, browser, this));
					return;

				case "keygen":
					Add(new KeyGenerator(token, browser, this));
					return;

				case "label":
					Add(new Label(token, browser, this));
					return;

				case "legend":
					Add(new Legend(token, browser, this));
					return;

				case "li":
					Add(new ListItem(token, browser, this));
					return;

				case "link":
					Add(new StyleSheetLink(token, browser, this));
					return;

				case "main":
					Add(new Main(token, browser, this));
					return;

				case "map":
					Add(new Map(token, browser, this));
					return;

				case "mark":
					Add(new Mark(token, browser, this));
					return;

				case "menu":
					Add(new Menu(token, browser, this));
					return;

				case "menuitem":
					Add(new MenuItem(token, browser, this));
					return;

				case "meta":
					Add(new Metadata(token, browser, this));
					return;

				case "meter":
					Add(new Meter(token, browser, this));
					return;

				case "nav":
					Add(new Navigation(token, browser, this));
					return;

				case "noframes":
					Add(new NoFrames(token, browser, this));
					return;

				case "noscript":
					Add(new NoScript(token, browser, this));
					return;

				case "object":
					Add(new Object(token, browser, this));
					return;

				case "ol":
					Add(new OrderedList(token, browser, this));
					return;

				case "optgroup":
					Add(new OptionGroup(token, browser, this));
					return;

				case "option":
					Add(new Option(token, browser, this));
					return;

				case "output":
					Add(new Output(token, browser, this));
					return;

				case "p":
					Add(new Paragraph(token, browser, this));
					return;

				case "param":
					Add(new Parameter(token, browser, this));
					return;

				case "pre":
					Add(new PreformattedText(token, browser, this));
					return;

				case "progress":
					Add(new Progress(token, browser, this));
					return;

				case "q":
					Add(new Quotation(token, browser, this));
					return;

				case "rp":
					Add(new RubyExplanation(token, browser, this));
					return;

				case "rt":
					Add(new RubyTag(token, browser, this));
					return;

				case "ruby":
					Add(new Ruby(token, browser, this));
					return;

				case "s":
					Add(new StrikeThrough(token, browser, this));
					return;

				case "samp":
					Add(new Sample(token, browser, this));
					return;

				case "script":
					Add(new Script(token, browser, this));
					return;

				case "section":
					Add(new Section(token, browser, this));
					return;

				case "select":
					Add(new Select(token, browser, this));
					return;

				case "small":
					Add(new Small(token, browser, this));
					return;

				case "source":
					Add(new Source(token, browser, this));
					return;

				case "span":
					Add(new Span(token, browser, this));
					return;

				case "strike":
					Add(new Strike(token, browser, this));
					return;

				case "strong":
					Add(new Strong(token, browser, this));
					return;

				case "style":
					Add(new Style(token, browser, this));
					return;

				case "sub":
					Add(new SubScript(token, browser, this));
					return;

				case "table":
					Add(new Table(token, browser, this));
					return;

				case "tbody":
					Add(new TableBody(token, browser, this));
					return;

				case "td":
					Add(new TableColumn(token, browser, this));
					return;

				case "textarea":
					Add(new TextArea(token, browser, this));
					return;

				case "tfoot":
					Add(new TableFooter(token, browser, this));
					return;

				case "th":
					Add(new TableHeaderColumn(token, browser, this));
					return;

				case "thead":
					Add(new TableHead(token, browser, this));
					return;

				case "time":
					Add(new Time(token, browser, this));
					return;

				case "title":
					Add(new Title(token, browser, this));
					return;

				case "tr":
					Add(new TableRow(token, browser, this));
					return;

				case "track":
					Add(new Track(token, browser, this));
					return;

				case "tt":
					Add(new TeletypeText(token, browser, this));
					return;

				case "u":
					Add(new Underline(token, browser, this));
					return;

				case "ul":
					Add(new UnorderedList(token, browser, this));
					return;

				case "var":
					Add(new Variable(token, browser, this));
					return;

				case "video":
					Add(new Video(token, browser, this));
					return;

				case "wbr":
					Add(new WordBreakOpportunity(token, browser, this));
					return;

				default:
					Add(element);
					return;
			}
		}

		/// <summary>
		/// Adds an JArray collection of elements to this collection.
		/// </summary>
		/// <param name="collection"> The collection of elements. </param>
		/// <param name="browser"> The browser the element belong to. </param>
		public void AddRange(JArray collection, Browser browser)
		{
			collection.ForEach(x => Add(x, browser));
		}

		/// <summary>
		/// Checks to see if the collection contains the provided ID.
		/// </summary>
		/// <param name="id"> The ID to check for. </param>
		/// <returns> True if the key is found or false if otherwise. </returns>
		public bool ContainsKey(string id)
		{
			return this.Any(x => x.Id == id);
		}

		/// <summary>
		/// Get the child from the children.
		/// </summary>
		/// <param name="id"> An ID of the element to get. </param>
		/// <param name="recursive"> Flag to determine to include descendants or not. </param>
		/// <returns> The child element for the condition. </returns>
		public T1 Get<T1>(string id, bool recursive = true) where T1 : Element
		{
			return Get<T1>(x => (x.Id == id) || (x.Name == id), recursive);
		}

		/// <summary>
		/// Get the child from the children.
		/// </summary>
		/// <param name="condition"> A function to test each element for a condition. </param>
		/// <param name="recursive"> Flag to determine to include descendants or not. </param>
		/// <returns> The child element for the condition. </returns>
		public T1 Get<T1>(Func<T1, bool> condition, bool recursive = true) where T1 : Element
		{
			var children = OfType<T1>().ToList();
			var response = children.FirstOrDefault(condition);
			if (!recursive)
			{
				return response;
			}

			if (response != null)
			{
				return response;
			}

			foreach (var child in this)
			{
				response = child.Get(condition, true, false);
				if (response != null)
				{
					return response;
				}
			}

			return null;
		}

		/// <summary>
		/// Gets a collection of element of the provided type.
		/// </summary>
		/// <typeparam name="T"> The type of the element for the collection. </typeparam>
		/// <returns> The collection of elements of the provided type. </returns>
		public ElementCollection<T> OfType<T>() where T : Element
		{
			return new ElementCollection<T>(this.Where(x => (x.GetType() == typeof(T)) || x is T).Cast<T>());
		}

		/// <summary>
		/// Prints out all children as a debug string.
		/// </summary>
		/// <param name="prefix"> Prefix to the debug information. </param>
		/// <param name="verbose"> Option to print verbose information. </param>
		public ElementCollection PrintDebug(string prefix = "", bool verbose = true)
		{
			foreach (var item in this)
			{
				if (verbose)
				{
					Console.WriteLine(prefix + item.ToDetailString().Replace(Environment.NewLine, ", "));
				}
				else
				{
					Console.WriteLine(prefix + item.Id);
				}

				item.Children.PrintDebug(prefix + "    ", verbose);
			}

			return this;
		}

		/// <summary>
		/// Sets the element title to the element id of all elements.
		/// </summary>
		public ElementCollection ShowDebugTitle()
		{
			foreach (var item in this)
			{
				item["title"] = item.Id;
				item.Children.ShowDebugTitle();
			}

			return this;
		}

		#endregion
	}
}