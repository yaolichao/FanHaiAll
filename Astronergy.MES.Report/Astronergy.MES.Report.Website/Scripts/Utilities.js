var agent = navigator.userAgent.toLowerCase();
var opera = (agent.indexOf("opera") > -1);
var opera9 = (agent.indexOf("opera/9") > -1);
var safari = agent.indexOf("safari") > -1;
var DXie = (agent.indexOf("msie") > -1 && !opera);
var IE55 = (agent.indexOf("5.5") > -1 && DXie);
var DXIE7CompatibilityMode = (DXie && document.documentMode == 7 && agent.indexOf("trident") > -1);
var DXDemoIEGreaterOrEqual8 = (__aspxIE && __aspxBrowserVersion && __aspxBrowserVersion >= 8) || GetDXDemoIEGreaterOrEqual8();
var DemoWindowOnResizeLockRequired =  DXDemoIEGreaterOrEqual8;
var ns = (agent.indexOf("mozilla") > -1 || DXagent.indexOf("netscape") > -1 || DXagent.indexOf("firefox") > -1) && !safari && !DXie && !opera;
var DefaultThemeCookieName = "DemoCurrentTheme";

function GetDXDemoIEGreaterOrEqual8(){
    if(DXie){
        var msie = "msie";
        var versionIndex = agent.indexOf(msie);
        var version = parseInt(agent.slice(versionIndex + msie.length));
        return version >= 8;
    } 
    return false;
}

function fixPng(element) {
    if(/MSIE (5\.5|6).+Win/.test(navigator.userAgent)) {
        if(element.tagName=='IMG' && /\.png$/.test(element.src)) {
            var src = element.src;
            element.src = '../Images/blank.gif';
            element.runtimeStyle.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + src + "')";
        }
    }
}

var WidthCorrectAllowed = true;
function CorrectWidth() {
    if (WidthCorrectAllowed) {
        WidthCorrectAllowed = false;
        var divSpacer = document.getElementById('divSpacer');
        if (divSpacer != null)
            return divSpacer.offsetWidth > 800 ? '800px' : 'auto';
        return 'auto';
    }
}

function isIE() {
    return (document.all && !window.opera) ? true : false;
}
function MoveFooter() {
    var spacer = document.getElementById("SpacerDiv");
    var footer =  document.getElementById("Footer");
    if (!DXDemoIsExists(spacer) || !DXDemoIsExists(footer))
        return;

    if (!isIE())
        footer.style.visibility = "hidden";

    if (DXIE7CompatibilityMode) {
        var demoForm = document.forms[0];
        var heightForm = demoForm.offsetHeight;
    }
    spacer.style.height = "0px";
    if (DXIE7CompatibilityMode)
        demoForm.style.height = heightForm + "px";

    var lastChildHeight = 0;
    var lastChild = null;
    lastChild = GetLastChild(document.body.lastChild);
    if (lastChild != null)
        lastChildHeight = GetAbsoluteY(lastChild) + lastChild.offsetHeight;
    spacer.top = GetDocumentClientHeight() - lastChildHeight;
    if (Math.abs(spacer.top) == spacer.top)
         spacer.style.height = spacer.top + "px";

    if (!isIE())
        footer.style.visibility = "";
}
function GetLastChild(element) {
    if (element != null) {
        var top = GetAbsoluteY(element);
        var height = element.offsetHeight;
        if (top == 0 && height == 0 || element.nodeName == "#text")
            return GetLastChild(element.previousSibling);
    }
    return element;
}

var WindowOnResizeLockCont = 0;
attachEventToElement(window, "resize", WindowOnResize);
function WindowOnResize(evt){
	if(!DemoWindowOnResizeLockRequired || WindowOnResizeLockCont == 0){
        WindowOnResizeLockCont++;
	    
	    MoveFooter();
		
		window.setTimeout("DcWindowOnResizeUnlock()", 0);
	}
}
function DcWindowOnResizeUnlock(){
    WindowOnResizeLockCont--;
}

var changeMonitorTimeoutId = -1;
var changeMonitorParts = [];

function trace_event(sender, args, event_name, info) {
	var name = sender.name;
	var pos = name.lastIndexOf("_");
	if(pos > -1)
	    name = name.substring(pos + 1);
	
	var text = ["<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\">",
	    get_trace_line("Sender", name, 100),
	    get_trace_line("EventType", event_name),
	    get_object_info("Arguments", args),
	    get_object_info("Information", info),
	    "</table><br />"].join('');
	changeMonitorParts.push(text);
	if(changeMonitorTimeoutId == -1)
	    changeMonitorTimeoutId = _aspxSetTimeout("update_monitor_value();", 0);
}
function get_object_info(name, obj) {
    if(_aspxIsExists(obj)) {
        var text = [];
        for(var key in obj) {
            if(key != "inherit" && key != "constructor")
                text.push(key, " = ", obj[key], "<br />");
        }
        return get_trace_line(name, text.join(''));
    }
    return "";
}
function get_trace_line(name, text, width) {
    var parts = ["<tr><td valign=\"top\""];
    if(_aspxIsExists(width))
        parts.push(" style=\"width: ", width, "px;\"");
    parts.push(">", name, ":</td><td valign=\"top\">", text, "</td></tr>");
    return parts.join('');
}

function update_monitor_value() {
    var memo = document.getElementById("Events");
    if(memo != null) {
        memo.innerHTML = Trim(memo.innerHTML + changeMonitorParts.join(''));
        changeMonitorTimeoutId = -1;
        changeMonitorParts = [];
	    memo.scrollTop = 100000;
	}
}
function clear_monitor() {
    var memo = document.getElementById("Events");
    if(memo != null) {
        memo.innerHTML = "";
        memo.scrollTop = 0;
    }
}
function LTrim( value ) {	
	var re = /\s*((\S+\s*)*)/;
	return value.replace(re, "$1");	
}
function RTrim( value ) {	
	var re = /((\s*\S+)*)\s*/;
	return value.replace(re, "$1");	
}
function Trim( value ) {	
	return LTrim(RTrim(value));	
}

function screenshot(src){
    var screenLeft = document.all && !document.opera ? window.screenLeft : window.screenX;
	var screenWidth = screen.availWidth;
	var screenHeight = screen.availHeight;
    var zeroX = Math.floor((screenLeft < 0 ? 0 : screenLeft) / screenWidth) * screenWidth;
    
	var windowWidth = 475;
	var windowHeight = 325;
	var windowX = parseInt((screenWidth - windowWidth) / 2);
	var windowY = parseInt((screenHeight - windowHeight) / 2);
	if(windowX + windowWidth > screenWidth)
		windowX = 0;
	if(windowY + windowHeight > screenHeight)
		windowY = 0;

    windowX += zeroX;

	var popupwnd = window.open(src,'_blank',"left=" + windowX + ",top=" + windowY + ",width=" + windowWidth + ",height=" + windowHeight + ", scrollbars=no, resizable=no", true);
	if (popupwnd != null && popupwnd.document != null && popupwnd.document.body != null) {
	    popupwnd.document.body.style.margin = "0px"; 
    }
}
function DXDemoIsExists(obj) {
    return (typeof(obj) != "undefined") && (obj != null);
}
function DXDemoIsFocusableTag(tagName) {
    tagName = tagName.toLowerCase();
    return (tagName == "input" ||tagName == "textarea" ||tagName == "select" || 
		tagName == "button" || tagName == "a");
}
function DXDemoIsFocusable(element) {
    if (!DXDemoIsExists(element) || !DXDemoIsFocusableTag(element.tagName))
		return false;
    var current = element;
    while(DXDemoIsExists(current)) {
		if (current.tagName.toLowerCase() == "body")
			return true;
		if (current.disabled || element.style.display == "none" || element.style.visibility == "hidden")
		    return false;
		current = current.parentNode;
    }
    return true;
}
function DXDemoActivateFormControl(controlId) {
    var control = document.getElementById(controlId);
    if (DXDemoIsExists(control) && DXDemoIsFocusable(control))
		control.focus();
}
function DemoActivateLabels() {
    var labels = document.getElementsByTagName("label");
    for (var index = 0; index < labels.length; index++) {
        labels[index].onclick = function () {
            DXDemoActivateFormControl(this.getAttribute('htmlfor') || this.getAttribute('for'));
        }
    }
}
function DemoHideFocusRects(container) {    
    if (container == null)
        return;
    hyperlinks = container.getElementsByTagName("a");
    for (var index = 0; index < hyperlinks.length; index++) {
        hyperlinks[index].onfocus = function () { this.blur(); }
    }
}
attachEventToElement(window, "load", WindowOnLoad);
function WindowOnLoad(evt){
	DemoActivateLabels();
	MoveFooter();
	PrepareThemes();
}
function PrepareThemes() {
}
function GetCurrentThemeCookieName() {
    if(_aspxIsExists(CurrentThemeCookieName))
        return CurrentThemeCookieName;
    return DefaultThemeCookieName;
}
function GetCurrentThemeFromCookies() {
    return ASPxClientUtils.GetCookie(GetCurrentThemeCookieName());
}
function SaveCurrentThemeToCookies(name) {
    ASPxClientUtils.SetCookie(GetCurrentThemeCookieName(), name);
}

function SetClientWidth(element, clientWidth) {
    var currentStyle = _aspxGetCurrentStyle(element);
    var newClientWidth = clientWidth - _aspxPxToInt(currentStyle.paddingLeft) - _aspxPxToInt(currentStyle.paddingRight) -
        _aspxPxToInt(currentStyle.borderLeftWidth) - _aspxPxToInt(currentStyle.borderRightWidth);
    element.style.width = newClientWidth + "px";
}
function GetAbsoluteX(curEl){
    var pos = 0;
    var isFirstCycle = true;
    while(curEl != null) {
        pos += curEl.offsetLeft;
        if(curEl.offsetParent != null && !opera && !DXopera9) {
            pos -= curEl.scrollLeft;
        }
        if (DXie && !isFirstCycle && curEl.tagName != "TABLE")
                pos += curEl.clientLeft;
        isFirstCycle = false;
        
        curEl = curEl.offsetParent;
    }
    return pos;
}
function GetAbsoluteY(curEl){
    var pos = 0;
    while(curEl != null) {
        pos += curEl.offsetTop;
        if(curEl.offsetParent != null && !opera && !DXopera9) {
            pos -= curEl.scrollTop;
        }
        curEl = curEl.offsetParent;
    }
    return pos;
}
function GetDocumentClientHeight(){
    if (safari) 
        return window.innerHeight;
    if(IE55 || opera || document.documentElement.clientHeight == 0)
        return document.body.clientHeight;
    return document.documentElement.clientHeight;
}
function GetDocumentScrollTop(){
    if(!safari && (IE55 || document.documentElement.scrollTop == 0))
        return document.body.scrollTop;
    return document.documentElement.scrollTop;
}
function GetDocumentScrollLeft(){
    if(!safari && ( IE55 || document.documentElement.scrollLeft == 0))
        return document.body.scrollLeft;
    return document.documentElement.scrollLeft;
}
function attachEventToElement(element, eventName, func) {
    if(ns || safari)
        element.addEventListener(eventName, func, true);
    else {
        if(eventName.toLowerCase().indexOf("on") != 0) 
            eventName = "on"+eventName;
        element.attachEvent(eventName, func);
    }
}

//Begin Expand/Collapse
var sectionStates = new Array();
function ExpandCollapse(imageItemId)
{
    noReentry = true; // Prevent entry to OnLoadImage
    var imageItem = _aspxGetElementById(imageItemId);
	if (ItemCollapsed(imageItemId) == true)
	{
		imageItem.src = "../Images/ExpandedButton.gif";
		imageItem.alt = "Collapse";
		ExpandSection(imageItem);
	}
	else
	{
		imageItem.src = "../Images/CollapsedButton.gif";
		imageItem.alt = "Expand";
		CollapseSection(imageItem);
	}
	noReentry = false;
}
function ExpandCollapse_CheckKey(evt, imageItemId) {
    if(_aspxGetKeyCode(evt) == 13)
        ExpandCollapse(imageItemId);
}
function ChangeExpanded(imageItem, state, style) 
{
    try
    {
        var element = imageItem.parentNode.parentNode;
        var span = element.nextSibling;
	    span.style.display	= style;
	    sectionStates[imageItem.id] = state;
	}
	catch (e)
	{
	}
}
function ExpandSection(imageItem)
{
    ChangeExpanded(imageItem, "e", "");
}

function CollapseSection(imageItem)
{
    ChangeExpanded(imageItem, "c", "none");
}
function ItemCollapsed(imageId)
{
	return sectionStates[imageId] != "e";
}
function CorrectCodeRenderWidth(pageControl) {
    var tabContent = pageControl.GetContentElement(pageControl.activeTabIndex);
    var divCodeRender = _aspxGetChildsByClassName(tabContent, "cr-div");
    for(var index = 0; index < divCodeRender.length; index++) {
        if((divCodeRender[index].offsetWidth) != pageControl.GetContentsCell().clientWidth)
            SetClientWidth(divCodeRender[index], pageControl.GetContentsCell().clientWidth);
    }
}

//����һ��showhidediv�ķ�����ֱ�Ӹ�ID����
function showhidediv(targetid,thisid){
var sbtitle=document.getElementById(targetid);
//alert(sbtitle.style.display);
if(sbtitle.style.display=='block'){
   sbtitle.style.display='none';
   //document.getElementById('ControlExpand').width = "16px"
   //thisid.src="~/Images/collapse.jpg";
   }else{
   sbtitle.style.display='block';
  // document.getElementById('ControlExpand').width = "234px"
   //thisid.src = "~/Images/expand.jpg";
   }
}


function splitterOnResize(splitterBar, width)
	{
		// do any other work that needs to happen when the 
		// splitter resizes. this is a good place to handle 
		// any complex resizing rules, etc.


		// if the vertical splitter fired this event, 
		// resize the right side and reconfigure the horizontal splitter
		if ( splitterBar && splitterBar.id == "vsbSplitter" )
		{
			// make sure the width is in number format
			if (typeof width == "string")
			{
				width = new Number(width.replace("px",""));
			}
			// the default resizing works fine for firefox, but not on IE 7
			if(_isIE)
			{
				var rightWidth = splitterBar.totalWidth-width-splitterBar.splitterWidth;
				alert("OK");
//				document.getElementById("tdRight").style.width = rightWidth + "px";
//				document.getElementById("tblRight").style.width = rightWidth + "px";
				document.getElementById("tdTop").style.width = rightWidth + "px";
				document.getElementById("tdBottom").style.width = rightWidth + "px";


				var divWidth = rightWidth-10; // adjust for IE borders
				document.getElementById("divTop").style.width = divWidth + "px";
				document.getElementById("divBottom").style.width = divWidth + "px";
			}

			if(sbar_hsbSplitter != null)
			{
				sbar_hsbSplitter.configure();
			}
		}
	}


	function simulateDoubleClick(splitter)
	{
		if(_isIE)
		{
			splitter.splitterBar.fireEvent('ondblclick');
		}
		else
		{
			var evnt=document.createEvent('MouseEvents');
			evnt.initEvent('dblclick',true,true);
			splitter.splitterBar.dispatchEvent(evnt);
		}
	}

	function setMinWidth(splitter, minwidth)
	{
		splitter.minWidth = getInt(minwidth);
		if ( splitter.hdnMinWidth )
		{
			splitter.hdnMinWidth.value = splitter.minWidth + "px";
		}
		var width = getInt(splitter.splitterBar.style.left) - getInt(splitter.offsetLeft);
		if(width < getInt(minwidth))
		{
			splitter.splitterBar.style.left = getInt(minwidth) + getInt(splitter.offsetLeft) + "px";
			VwdCmsSplitterBar_setTargetWidth(splitter);
		}
	}

	function setMaxWidth(splitter, maxwidth)
	{
		splitter.maxWidth = getInt(maxwidth);
		if ( splitter.hdnMaxWidth )
		{
			splitter.hdnMaxWidth.value = splitter.maxWidth + "px";
		}
		var width = getInt(splitter.splitterBar.style.left) - getInt(splitter.offsetLeft);
		if(width > getInt(maxwidth))
		{
			splitter.splitterBar.style.left = getInt(maxwidth) + getInt(splitter.offsetLeft) + "px";
			VwdCmsSplitterBar_setTargetWidth(splitter);
		}
	}

	function setMinHeight(splitter, minheight)
	{
		splitter.minHeight = getInt(minheight);
		if ( splitter.hdnMinHeight )
		{
			splitter.hdnMinHeight.value = splitter.minHeight + "px";
		}
		var height = getInt(splitter.splitterBar.style.top) - getInt(splitter.offsetTop);
		if(height < getInt(minheight))
		{
			splitter.splitterBar.style.top = getInt(minheight) + getInt(splitter.offsetTop) + "px";
			VwdCmsSplitterBar_setTargetHeight(splitter);
		}
	}

	function setMaxHeight(splitter, maxheight)
	{
		splitter.maxHeight = getInt(maxheight);
		if ( splitter.hdnMaxHeight )
		{
			splitter.hdnMaxHeight.value = splitter.maxHeight + "px";
		}
		var height = getInt(splitter.splitterBar.style.top) - getInt(splitter.offsetTop);
		if(height > getInt(maxheight))
		{
			splitter.splitterBar.style.top = getInt(maxheight) + getInt(splitter.offsetTop) + "px";
			VwdCmsSplitterBar_setTargetHeight(splitter);
		}
	}


