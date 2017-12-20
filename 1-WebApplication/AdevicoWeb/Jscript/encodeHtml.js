function encodeMyHtml() {
	encodedHtml = escape(encodeHtml.htmlToEncode.value);
	encodedHtml = encodedHtml.replace(/\//g,"%2F");
	encodedHtml = encodedHtml.replace(/\?/g,"%3F");
	encodedHtml = encodedHtml.replace(/=/g,"%3D");
	encodedHtml = encodedHtml.replace(/&/g,"%26");
	encodedHtml = encodedHtml.replace(/@/g,"%40");
	encodeHtml.htmlEncoded.value = encodedHtml;
}
function testEncodedHtml() {
	testEncodedHtmlArea.innerHTML = unescape(encodeHtml.htmlEncoded.value);
}
