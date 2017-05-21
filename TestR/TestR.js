var TestR = TestR ||
{
	properties: [
		'cellIndex', 'checked', 'className', 'disabled', 'href', 'id', 'multiple', 'name', 'nodeType', 'readOnly',
		'rowIndex', 'selected', 'src', 'tagName', 'textContent', 'value'
	],
	autoId: 1,
	ignoredTags: ['script'],
	resultElementId: 'testrResult',
	triggerEvent: function(element, eventName, values) {
		var eventObj = document.createEventObject
			? document.createEventObject()
			: document.createEvent('Events');

		if (eventObj.initEvent) {
			eventObj.initEvent(eventName, true, true);
		}

		for (var i = 0; i < values.length; i++) {
			eventObj[values[i].key] = values[i].value;
		}

		element.dispatchEvent
			? element.dispatchEvent(eventObj)
			: element.fireEvent('on' + eventName, eventObj);
	},
	getElementLocation: function(id) {
		var element = document.getElementById(id);
		var box = element.getBoundingClientRect();
		var borderWidth = (window.outerWidth - window.innerWidth) / 2;
		var x = window.screenX + borderWidth;
		var y = window.screenY + window.outerHeight - window.innerHeight - borderWidth;
		var top = Math.round(y + box.top);
		var left = Math.round(x + box.left);
		return JSON.stringify({ x: left, y: top });
	},
	getElements: function(forParentId) {
		var response = [];
		var allElements = document.getElementsByTagName('*');
		var i;

		// Add element IDs so we can build element hierarchy.
		for (i = 0; i < allElements.length; i++) {
			var currentId = TestR.getValueFromElement(allElements[i], 'id');
			if (currentId === null || currentId === undefined || currentId === '') {
				allElements[i].id = 'testR-' + TestR.autoId++;
			}
		}

		for (i = 0; i < allElements.length; i++) {
			var element = allElements[i];
			var tagName = (element.tagName).toLowerCase();

			if (element.id === TestR.resultElementId) {
				continue;
			}

			if (TestR.ignoredTags.contains(tagName)) {
				continue;
			}

			var elementId = TestR.getValueFromElement(element, 'id');
			var elementName = TestR.getValueFromElement(element, 'name') || '';
			var parentId = TestR.getValueFromElement(element.parentNode, 'id') || '';

			if (forParentId !== undefined && parentId !== forParentId) {
				continue;
			}

			var item = {
				id: elementId,
				parentId: parentId,
				name: elementName,
				tagName: tagName,
				attributes: []
			};

			item.width = element.offsetWidth;
			item.height = element.offsetHeight;

			for (var j = 0; j < element.attributes.length; j++) {
				var attribute = element.attributes[j];

				if (attribute.nodeName === undefined || attribute.nodeName.length <= 0) {
					continue;
				}

				if (item[attribute.nodeName]) {
					continue;
				}

				item.attributes.push(attribute.nodeName);
				item.attributes.push(attribute.nodeValue);
			}

			for (var k = 0; k < TestR.properties.length; k++) {
				var name = TestR.properties[k];

				if (item[name] || name === 'textContent') {
					continue;
				}

				if (element[name] !== null && element[name] !== undefined) {
					item.attributes.push(name);
					if (typeof element[name] === 'string') {
						item.attributes.push(element[name]);
					} else {
						item.attributes.push(JSON.stringify(element[name]));
					}
				}
			}

			response.push(item);
		}

		return response;
	},
	getElementValue: function(id, name) {
		var element = document.getElementById(id);
		if (element === undefined || element === null) {
			return null;
		}

		return TestR.getValueFromElement(element, name);
	},
	getSelectText: function(elementId) {
		var element = document.getElementById(elementId);
		if (element.selectedIndex === -1) {
			return null;
		}

		return element.options[element.selectedIndex].text;
	},
	getValueFromElement: function(element, name) {
		try {
			if (element === undefined || element === null) {
				return null;
			}

			var value = element[name];
			if ((value === null || value === undefined) || (element.nodeType === 1 && typeof (value) === 'object')) {
				value = element.attributes[name];
			}

			if (value !== null && value !== undefined) {
				return (value.value || value.nodeValue || value).toString();
			}
		} catch (e) {
			return null;
		}

		return null;
	},
	setElementValue: function(id, name, value) {
		var element = document.getElementById(id);
		if (element === undefined || element === null) {
			return;
		}

		if (TestR.properties.contains(name)) {
			element[name] = value;
		} else {
			element.setAttribute(name, value);
		}
	},
	setSelectText: function(elementId, value) {
		var element = document.getElementById(elementId);

		for (var i = 0; i < element.options.length; i++) {
			if (element.options[i].text === value) {
				element.options[i].selected = true;
				return;
			} else if (element.options[i].text.lastIndexOf(value,0) === 0) {
				element.options[i].selected = true;
				return;
			}
		}
	},
	removeElement: function(id) {
		var element = document.getElementById(id);
		element.parentNode.removeChild(element);
	},
	removeElementAttribute: function(id, name) {
		var element = document.getElementById(id);
		element.removeAttribute(name);
	},
	rightClick: function(id) {
		var element = document.getElementById(id);
		var evt = element.ownerDocument.createEvent('MouseEvents');
		var rightClickButtonCode = 2;

		evt.initMouseEvent('contextmenu', true, true, element.ownerDocument.defaultView, 1,
			0, 0, 0, 0, false, false, false, false, rightClickButtonCode, null);

		if (document.createEventObject) {
			// dispatch for IE
			return element.fireEvent('onclick', evt);
		} else {
			// dispatch for firefox + others
			return !element.dispatchEvent(evt);
		}
	},
	runScript: function(script) {
		// decode the script.
		script = script
			.replace(/&quot;/g, '"')
			.replace(/&#39;/g, "'")
			.replace(/&lt;/g, '<')
			.replace(/&gt;/g, '>')
			.replace(/&amp;/g, '&');

		// Attempt to find the result element.
		var resultElement = document.getElementById(TestR.resultElementId);

		// Check to see if the element was found.
		if (resultElement === undefined || resultElement === null) {
			// Create new element (input "hidden") then set the value.
			resultElement = document.createElement('input');
			resultElement.id = TestR.resultElementId;
			resultElement.type = 'hidden';

			if (document.body && document.body.appendChild) {
				document.body.appendChild(resultElement);
			}
		}

		try {
			// Clear the results first then set the new value.
			resultElement.value = '';
			resultElement.value = eval(script);
		} catch (error) {
			// Something went wrong so update the result with the error.
			console.log(error.message);
			resultElement.value = error.message;
		}
	}
};

Array.prototype.contains = function (obj) {
	for (var i = 0; i < this.length; i++) {
		if (this[i] === obj) {
			return true;
		}
	}

	return false;
};