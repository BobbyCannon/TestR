var TestR = TestR || {
	properties: [
		'cellIndex', 'checked', 'className', 'disabled', 'href', 'id', 'multiple', 'name', 'nodeType', 'readOnly', 'rowIndex', 'selected',
		'src', 'tagName', 'textContent', 'value'
	],
	autoId: 1,
	ignoredTags: ['script'],
	ignoredProperties: ['tagName', 'id', 'name'],
	resultElementId: 'testrResult',
	triggerEvent: function (element, eventName, values) {
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
	getElements: function () {
		var response = [];
		var allElements = document.body.getElementsByTagName('*');

		// Add element IDs so we can build element hierarchy.
		for (var i = 0; i < allElements.length; i++) {
			if (allElements[i].id === undefined || allElements[i].id === '') {
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

			var item = {
				id: element.id,
				parentId: element.parentNode.id,
				name: element.name || '',
				tagName: tagName,
				attributes: []
			};

			for (var j = 0; j < element.attributes.length; j++) {
				var attribute = element.attributes[j];

				if (attribute.nodeName === undefined || attribute.nodeName.length <= 0) {
					continue;
				}

				if (TestR.properties.contains(attribute.nodeName)) {
					continue;
				}

				item.attributes.push(attribute.nodeName);
				item.attributes.push(attribute.value);
			}

			for (var k = 0; k < TestR.properties.length; k++) {
				var name = TestR.properties[k];

				if (TestR.ignoredProperties.contains(name)) {
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
	getElementValue: function (id, name) {
		var element = document.getElementById(id);
		if (element === undefined || element === null) {
			return '';
		}

		var value = TestR.properties.contains(name) ? element[name] : element.attributes[name];

		if (value !== null && value !== undefined) {
			if (value.nodeValue) {
				return value.nodeValue;
			}

			return value.toString();
		}

		return '';
	},
	setElementValue: function (id, name, value) {
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
	removeElement: function (id) {
		var element = document.getElementById(id);
		element.parentNode.removeChild(element);
	},
	removeElementAttribute: function (id, name) {
		var element = document.getElementById(id);
		element.removeAttribute(name);
	},
	runScript: function (script) {
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
			resultElement.value = String(eval(script));
		} catch (error) {
			// Something went wrong so update the result with the error.
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