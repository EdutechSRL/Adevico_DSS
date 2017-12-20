//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///    RadTreeNode Defs
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
var RadTreeViewKeyboardHooked = false;
var ActiveRadTreeView = null;

var globalCounter = 0;
var counterHandle = 0;

function RadTreeNode() 
{
	this.Parent = null;
	this.TreeView = null;
	this.Nodes = new Array();
	this.id = null;
	this.SignImage = null;
	this.SignImageExpanded = null;
	this.Image = null;
	this.ImageExpanded = null;
	this.ImageElmt = null;
	this.SignElmt = null;
	this.TextElmt = null;
	this.CheckElmt = null;
	this.Action = null;
	this.Index = 0;
	
	this.Text = null;
	this.Value = null;
	this.Category = null;
	
	this.NodeCss = null;
	this.NodeCssOver = null;
	this.NodeCssSelect = null;
	
	this.ContextMenuName = null;
	
	this.Enabled = true;
	this.Expanded = false;
	this.Checked = false;
	this.Selected = false;
}

RadTreeNode.prototype.Next = function()
{
	var nodeCollection = (this.Parent != null) ? this.Parent.Nodes : this.TreeView.Nodes;
	return (this.Index >= nodeCollection.length-1) ? null : nodeCollection[this.Index + 1];
}

RadTreeNode.prototype.Prev = function()
{
	var nodeCollection = (this.Parent != null) ? this.Parent.Nodes : this.TreeView.Nodes;
	return (this.Index <= 0) ? null : nodeCollection[this.Index - 1];
}

RadTreeNode.prototype.NextVisible = function()
{
	if (this.Expanded) 
	{
		return this.Nodes[0];	
	}
	if (this.Next() != null) 
	{
		return this.Next();
	}	
	if (this.Parent == null)
	{
		return null;
	}	
	if (this.Parent.Next() != null)
	{		
		return this.Parent.Next();
	}		
	return null;	
}

RadTreeNode.prototype.PrevVisible = function()
{	
	if (this.Prev() != null) 
	{
		if (this.Prev().Expanded)
		{		
			return this.Prev().Nodes[this.Prev().Nodes.length-1];
		}
		else
		{
			return this.Prev();
		}
	}		
	if (this.Parent == null)
	{
		return null;
	}	
	if (this.Parent != null)
	{
		return this.Parent;
	}			
	return null;
}


RadTreeNode.prototype.Init = function()
{	
	this.TextElmt = document.getElementById(this.id).getElementsByTagName("span")[0];
	this.CheckElmt = document.getElementById(this.id).getElementsByTagName("input")[0];		
	this.NodeCss = this.TreeView.NodeCss;	
	this.NodeCssOver = this.TreeView.NodeCssOver;
	this.NodeCssSelect = this.TreeView.NodeCssSelect;
	if (this.TextElmt.getAttribute("cssclass"))
	{
		this.NodeCss = this.TextElmt.getAttribute("cssclass");
	}
	if (document.getElementById(this.id).getAttribute("rtcmn") != null)
	{		
		this.ContextMenuName = document.getElementById(this.id).getAttribute("rtcmn");
	}
	
	var i;
	var innerItems = document.getElementById(this.id).getElementsByTagName("img");
	for (i=0; i<innerItems.length; i++)
	{
	    if (innerItems[i].getAttribute("rtimg") != null)
	    {
		    this.ImageElmt = innerItems[i];
		}
		if (innerItems[i].getAttribute("rtsign") != null)
		{
			this.SignElmt = innerItems[i];			
		}		
		if (document.getElementById("G" + this.id))
		{		
			this.Expanded = (document.getElementById("G" + this.id).style.display == 'block') ? true : false;
		}
	}	
}

RadTreeNode.prototype.Toggle = function()
{
	if (!this.Nodes.length) 
	{
		return;
	}
	
	this.TreeView.CancelAction = false;
	if (this.TreeView.BeforeClientToggle != null)
	{
		var s = this.TreeView.BeforeClientToggle + "(this);";
		eval(s);
	}
	if (this.TreeView.CancelAction)
	{
		return;
	}
	
	if (this.Expanded)
	{
		this.Collapse();
	}
	else
	{
		this.Expand();
	}
	
	if (this.TreeView.AfterClientToggle != null)
	{
		var s = this.TreeView.AfterClientToggle + "(this);";
		eval(s);
	}
}

RadTreeNode.prototype.Expand = function()
{
    document.getElementById("G" + this.id).style.display = "block";
	this.ImageOn();
	this.SignOn();
	this.Expanded = true;
	this.TreeView.UpdateExpandedState();
}

RadTreeNode.prototype.Collapse = function()
{
	document.getElementById("G" + this.id).style.display = "none";
	this.ImageOff();
	this.SignOff();
	this.Expanded = false;
	this.TreeView.UpdateExpandedState();
}

RadTreeNode.prototype.Highlight = function()
{
	if (!this.Enabled) return;
	
	this.TreeView.CancelAction = false;
	if (this.TreeView.BeforeClientHighlight != null)
	{		
		var s = this.TreeView.BeforeClientHighlight + "(this);";		
		eval(s);
		
		if (this.TreeView.CancelAction)
		{
			return;
		}				 
	}
	
	if (this.TreeView.MultipleSelect && this.TreeView.IE && event.ctrlKey)
	{
		if (this.Selected)
		{
			this.TextElmt.className = this.NodeCss;
			this.Selected = false;
			this.TreeView.UpdateSelectedState();
			
			if (this.TreeView.AfterClientHighlight != null)
			{		
				var s = this.TreeView.AfterClientHighlight + "(this);";		
				eval(s);				 
			}
			return;
		}
	}
	else
	{
		this.TreeView.UnSelectAllNodes();
	}
	
	this.TextElmt.className = this.NodeCssSelect;
	this.TreeView.SelectNode(this);
	
	if (this.TreeView.AfterClientHighlight != null)
	{		
		var s = this.TreeView.AfterClientHighlight + "(this);";		
		eval(s);				 
	}
}


RadTreeNode.prototype.ExecuteAction = function()
{	
	if (this.TreeView.BeforeClientClick != null)
	{
		var s = this.TreeView.BeforeClientClick + "(this);";
		this.TreeView.CancelAction = false;
		eval(s);	
		
		if (this.TreeView.CancelAction)
		{
			return;
		}
	}
	if (this.Action != null)
	{		
		if (this.Action.substr(0,12) == "__doPostBack" && this.TreeView.CausesValidation)
		{			
			if (typeof(Page_ClientValidate) != 'function' ||  Page_ClientValidate())
			{
				eval(this.Action);	
			}
		}
		else
		{	
			eval(this.Action);
		}
	}	
}

RadTreeNode.prototype.Select = function()
{	
	if (!this.Enabled) return;
	this.Highlight();
	this.TreeView.LastHighlighted = this;
	if (!this.TreeView.CancelAction)
	{
		this.ExecuteAction();
	}
}



RadTreeNode.prototype.UnSelect = function()
{
	this.TextElmt.className = this.NodeCss;
	this.Selected = false;
}

RadTreeNode.prototype.Hover = function()
{	
	if (!this.Enabled) return;
	this.TreeView.LastHighlighted = this;
	if (!this.Selected)
	{
		this.TextElmt.className = this.NodeCssOver;
	}
}

RadTreeNode.prototype.UnHover = function()
{
	if (!this.Enabled) return;
	this.TreeView.LastHighlighted = null;
	if (!this.Selected)
	{
		this.TextElmt.className = this.NodeCss;
	}
}

RadTreeNode.prototype.CheckBoxClick = function(e)
{	
	if (this.TreeView.BeforeClientCheck != null)
	{
		var s = this.TreeView.BeforeClientCheck + "(this);";
		this.TreeView.CancelAction = false;		
		eval(s);
		
		if (this.TreeView.CancelAction)
		{
			e.returnValue = false;
			return;
		}
	}	
	
	this.Checked = !this.Checked;
	this.TreeView.UpdateCheckedState();
	
	if (this.TreeView.AfterClientCheck != null)
	{		
		var s = this.TreeView.AfterClientCheck + "(this);";		
		eval(s);
	}
}

RadTreeNode.prototype.Check = function()
{
	if (this.CheckElmt != null)
	{		
		this.CheckElmt.checked = true;
		this.Checked = true;
		this.TreeView.UpdateCheckedState();
	}	
}

RadTreeNode.prototype.UnCheck = function()
{
	if (this.CheckElmt != null)
	{
		this.CheckElmt.checked = false;
		this.Checked = false;
		this.TreeView.UpdateCheckedState();
	}	
}

RadTreeNode.prototype.ImageOn = function() { if (this.ImageExpanded != null) this.ImageElmt.src = this.ImageExpanded; }
RadTreeNode.prototype.ImageOff = function() { if (this.Image != null )this.ImageElmt.src = this.Image; }
RadTreeNode.prototype.SignOn = function() { if (this.SignImageExpanded != null) this.SignElmt.src = this.SignImageExpanded; }
RadTreeNode.prototype.SignOff = function() { if (this.SignImage != null )this.SignElmt.src = this.SignImage; }
   
   
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///    RadTreeView Defs
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
   
function RadTreeView(uniqueId, id, firstNodeId, _nodeCss, _nodeCssOver, _nodeCssSelect, _multipleSelect, _ie, _serverId, _dragAndDrop)
{
	this.ID = _serverId;
	this.UniqueId = uniqueId;
	this.Container = id;
	this.Nodes = new Array();
	this.AllNodes = new Array();
	this.NodeCss = _nodeCss;
	this.NodeCssOver = _nodeCssOver;
	this.NodeCssSelect = _nodeCssSelect;	
	this.MultipleSelect = _multipleSelect;
	this.IE = _ie;
	this.SelectedNode = null;
	
	this.ExpandedState = null;
	this.CheckedState = null;
	this.SelectedState = null;
	
	this.DragAndDrop = _dragAndDrop;
	this.DragMode = false;
	this.DragSource = null;
	this.DragDest = null;
	this.DragClone = null;
	this.LastHighlighted = null;
	this.MouseInside = false;
	
	// Client-side events
	this.BeforeClientClick = null;
	this.BeforeClientHighlight = null;
	this.AfterClientHighlight = null;
	this.BeforeClientDrop = null;
	this.BeforeClientToggle = null;
	this.AfterClientToggle = null;
	this.BeforeClientContextClick = null;
	this.AfterClientContextClick = null;
	this.BeforeClientCheck = null;	
	this.AfterClientCheck = null;
	
	this.CausesValidation = true;	
	this.CancelAction = false;
	this.ContextMenuVisible = false;
	
	this.LoadTree(firstNodeId, this.Nodes, null);
	
	if (!RadTreeViewKeyboardHooked && this.IE)	
	{
		RadTreeViewKeyboardHooked = true;
		document.attachEvent('onkeydown', this.KeyDown);
	}

	if (this.DragAndDrop && this.IE)
	{		
		document.attachEvent('onmousemove', this.MouseMove);
	}	
}

RadTreeView.prototype.FindNode = function(node)
{
	var i;
	for (i=0;i<this.AllNodes.length;i++)
	{
		if (this.AllNodes[i].id == node)
		{
			return this.AllNodes[i];
		}
	}
	return null;
}

RadTreeView.prototype.WalkTree = function(arr)
{
	var i;
	for (i=0; i<arr.length;i++)
	{
		if (arr[i].Nodes.length > 0)
		{
			this.WalkTree(arr[i].Nodes);
		}
	}
}

RadTreeView.prototype.LoadTree = function(node, arr, parent)
{
    var parentHandle = document.getElementById(node).parentNode;
    var allNodes = parentHandle.childNodes;
    var lastNode = null;
    var index = 0;
    
    var i; var lastNodeId = "";
    for (i=0; i<allNodes.length; i++)
    {		
		if ( ("G" + lastNodeId) == allNodes[i].id)
		{
			this.LoadTree(document.getElementById("G" + lastNodeId).firstChild.id, rtNode.Nodes, rtNode);
		}
		else
		{
			var rtNode = new RadTreeNode();
			rtNode.TreeView = this;
			rtNode.Parent = parent;
			rtNode.id = allNodes[i].id;
			rtNode.SignImage = allNodes[i].getAttribute("rt_S");
			rtNode.SignImageExpanded = allNodes[i].getAttribute("rt_SE");
			rtNode.Image = allNodes[i].getAttribute("rt_I");
			rtNode.ImageExpanded = allNodes[i].getAttribute("rt_IE");
			rtNode.Action = allNodes[i].getAttribute("rt_A");
			rtNode.Text = allNodes[i].getAttribute("rttext");
			rtNode.Value = allNodes[i].getAttribute("rtvalue");
			rtNode.Category = allNodes[i].getAttribute("rtcat");
			if (allNodes[i].getAttribute("rtsel") != null) 
			{				
				rtNode.Selected = true;
				this.SelectedNode = rtNode;
			}
			if (allNodes[i].getAttribute("rt_checked") != null) rtNode.Checked = true;
			if (allNodes[i].getAttribute("rtdis") != null) rtNode.Enabled = false;
			rtNode.Index = index;
			rtNode.Init();
			
			arr[arr.length] = rtNode;
			this.AllNodes[this.AllNodes.length] = rtNode;
			index++;
		}
		var lastNodeId = allNodes[i].id;
    }
}


RadTreeView.prototype.Toggle = function(node) {  this.FindNode(node).Toggle(); }
RadTreeView.prototype.Select = function(node) { this.FindNode(node).Select(); }
RadTreeView.prototype.Hover = function(node) { this.FindNode(node).Hover(); }
RadTreeView.prototype.UnHover = function(node) { this.FindNode(node).UnHover(); }
RadTreeView.prototype.CheckBoxClick = function(node, e) { this.FindNode(node).CheckBoxClick(e); }
RadTreeView.prototype.Highlight = function(node) { this.FindNode(node).Highlight(); }


RadTreeView.prototype.SelectNode = function(node)
{
	this.SelectedNode = node;
	node.Selected = true;
	this.UpdateSelectedState();
}

RadTreeView.prototype.GetSelectedNodes = function()
{
	var i;
	var selNodes = new Array();
	for (i=0; i<this.AllNodes.length; i++)
	{
		if (this.AllNodes[i].Selected)
		{
			selNodes[selNodes.length] = this.AllNodes[i];
		}
	}
		
	return selNodes;
}

RadTreeView.prototype.UnSelectAllNodes = function(node)
{
	var i;
	for (i=0; i<this.AllNodes.length;i++)
	{
		if (this.AllNodes[i].Selected)
		{
			this.AllNodes[i].UnSelect();
		}
	}
}

RadTreeView.prototype.KeyDown = function(e) 
{	
	try	{ var dummy = tlrkTreeViews.length;	} catch (exception) { return; };	

	var i;
	var LastActiveRadTreeView = null;
	
	for (i=0; i<tlrkTreeViews.length; i++)
	{
		if (e.srcElement.contains(document.getElementById(tlrkTreeViews[i].Container)))
		{
			LastActiveRadTreeView = tlrkTreeViews[i];
			break;
		}
	}
	
	if (LastActiveRadTreeView != null)
	{			
		if(event.keyCode == 107 || event.keyCode == 109 || event.keyCode == 37 || event.keyCode == 39)
		{
			if (LastActiveRadTreeView.SelectedNode != null)
				LastActiveRadTreeView.SelectedNode.Toggle();
		}
		if(event.keyCode == 40) // down
		{
			if (LastActiveRadTreeView.SelectedNode != null)
			{				
				if (LastActiveRadTreeView.SelectedNode.NextVisible() != null)
				{
					LastActiveRadTreeView.SelectedNode.NextVisible().Highlight();
				}
			}
		}
		if(event.keyCode == 38) // up
		{
			if (LastActiveRadTreeView.SelectedNode != null)
			{				
				if (LastActiveRadTreeView.SelectedNode.PrevVisible() != null)
				{
					LastActiveRadTreeView.SelectedNode.PrevVisible().Highlight();
				}
			}
		}
		if(event.keyCode == 13) // Enter
		{
			if (LastActiveRadTreeView.SelectedNode != null)
			{				
				LastActiveRadTreeView.SelectedNode.ExecuteAction();				
			}
		}
		if(event.keyCode == 32) // Space
		{
			if (LastActiveRadTreeView.SelectedNode != null)
			{				
				LastActiveRadTreeView.SelectedNode.CheckBoxClick();				
			}
		}		
	}
}

RadTreeView.prototype.UpdateExpandedState = function() 
{
	this.ExpandedState = "";
	this.DoUpdateExpandedState(this.Nodes);
	document.getElementById(this.UniqueId + "_expanded").value = this.ExpandedState;
}

RadTreeView.prototype.DoUpdateExpandedState = function(arr) 
{
	var i;
	for (i=0; i<arr.length;i++)
	{
		var onoff = (arr[i].Expanded) ? "1" : "0";
		this.ExpandedState += onoff;
		if (arr[i].Nodes.length > 0)
		{
			this.DoUpdateExpandedState(arr[i].Nodes);
		}
	}	
}

RadTreeView.prototype.UpdateCheckedState = function() 
{
	this.CheckedState = "";
	this.DoUpdateCheckedState(this.Nodes);
	document.getElementById(this.UniqueId + "_checked").value = this.CheckedState;
}


RadTreeView.prototype.DoUpdateCheckedState = function(arr)
{
	var i;
	for (i=0; i<arr.length;i++)
	{
		var onoff = (arr[i].Checked) ? "1" : "0";
		this.CheckedState += onoff;
		if (arr[i].Nodes.length > 0)
		{
			this.DoUpdateCheckedState(arr[i].Nodes);
		}
	}
}

RadTreeView.prototype.UpdateSelectedState = function() 
{
	this.SelectedState = "";
	this.DoUpdateSelectedState(this.Nodes);
	document.getElementById(this.UniqueId + "_selected").value = this.SelectedState;
}

RadTreeView.prototype.DoUpdateSelectedState = function(arr)
{
	var i;
	for (i=0; i<arr.length;i++)
	{
		var onoff = (arr[i].Selected) ? "1" : "0";
		this.SelectedState += onoff;
		if (arr[i].Nodes.length > 0)
		{
			this.DoUpdateSelectedState(arr[i].Nodes);
		}
	}
}

RadTreeView.prototype.HideClone = function() 
{
	if (!this.MouseInside)
	{
		document.body.removeChild(this.DragClone);
		this.DragClone = null;
	}
}


RadTreeView.prototype.ContextMenuClick = function(e,p1,p2) 
{	
	this.HideContextMenu(this.LastHighlighted.ContextMenuName);
	
	if (this.BeforeClientContextClick != null)
	{		
		ContextClickCurrentNode = this.LastHighlighted;
		ContextClickCurrentMenuItem = p1;
		
		var s = this.BeforeClientContextClick + "(ContextClickCurrentNode, ContextClickCurrentMenuItem);";		
		this.CancelAction = false;
		eval(s);
		
		if (this.CancelAction)
		{
			return;
		}
	}
	
	if (p2)
	{
		var postBack = "__doPostBack('" + this.UniqueId + "','" + "@CC:" + this.LastHighlighted.id + "#" + p1 + "')";
		eval(postBack);
	}
}

RadTreeView.prototype.ContextMenu = function(e) 
{	
	var src = (this.IE) ? e.srcElement : e.target;
	if (this.LastHighlighted != null && this.LastHighlighted.ContextMenuName != null)
	{
		if (!this.ContextMenuVisible)
		{			
			if (!this.FindNode(src.parentNode.id).Selected)
			{
				this.Highlight(src.parentNode.id)
			}
			this.ShowContextMenu(this.LastHighlighted.ContextMenuName, e.x, e.y);
		}		
	}
		
	e.returnValue = false;	
}

RadTreeView.prototype.ShowContextMenu = function(name, x, y)
{	
	var menu = document.getElementById("rtvcm" + name);
	menu.style.display = "block";
	menu.style.left = x + document.body.scrollLeft + "px";
	menu.style.top = y + document.body.scrollTop + "px";	
	
	this.ContextMenuVisible = true;
}

RadTreeView.prototype.HideContextMenu = function(name)
{
	var menu = document.getElementById("rtvcm" + name);
	menu.style.display = "none";
	
	this.ContextMenuVisible = false;
}

RadTreeView.prototype.ClickDispatcher = function(e) 
{
	if (this.ContextMenuVisible)
	{		
		this.HideContextMenu(this.LastHighlighted.ContextMenuName);
	}
	
	var src = (this.IE) ? e.srcElement : e.target;	
	if (src.tagName == "SPAN")
	{
		this.Select(src.parentNode.id);
	}
	if (src.tagName == "IMG" && src.getAttribute("rtsign") != null)
	{
		var myNode = this.FindNode(src.parentNode.id);
		this.Toggle(src.parentNode.id);
	}
	if (src.tagName == "INPUT")
	{
		if (src.type == "checkbox")
		{			
			this.CheckBoxClick(src.parentNode.id, e);
		}
	}
}

RadTreeView.prototype.DoubleClickDispatcher = function(e) 
{
	var src = (this.IE) ? e.srcElement : e.target;
	if (src.tagName == "SPAN")
	{
		this.Toggle(src.parentNode.id);
	}
}

RadTreeView.prototype.MouseOverDispatcher = function(e) 
{	
	var src = (this.IE) ? e.srcElement : e.target;
	if (src.tagName == "SPAN")
	{
		this.Hover(src.parentNode.id);
	}
}

RadTreeView.prototype.MouseOutDispatcher = function(e) 
{	
	var src = (this.IE) ? e.srcElement : e.target;
	if (src.tagName == "SPAN")
	{
		this.UnHover(src.parentNode.id);
	}	
}

RadTreeView.prototype.MouseEnterDispatcher = function(e) 
{
	this.MouseInside = true;
}

RadTreeView.prototype.MouseLeaveDispatcher = function(e) 
{
	this.MouseInside = false;
	if (this.DragClone != null)
	{
		this.HideClone();
	}
}

RadTreeView.prototype.MouseUp = function(e) 
{
	this.DragMode = false;
	if (this.DragClone != null && this.DragAndDrop)
	{
		document.body.removeChild(this.DragClone);
		this.DragClone = null;
		if (this.LastHighlighted != null && this.DragSource.id != this.LastHighlighted.id)
		{
			this.DragDest = this.LastHighlighted;
			
			if (this.BeforeClientDrop != null)
			{
				GlobalSourceNode = this.DragSource;
				GlobalDestNode = this.DragDest;
				var s = this.BeforeClientDrop + "(GlobalSourceNode,GlobalDestNode);";
				this.CancelAction = false;
				eval(s);
				if (this.CancelAction)
				{
					return;
				}
			}
			
			var postBack = "__doPostBack('" + this.UniqueId + "','" + "@ND:" + this.DragSource.id + "/" + this.DragDest.id + "')";
			eval(postBack);
		}
	}	
}

RadTreeView.prototype.MouseDown = function(e) 
{
	this.DragMode = true;
	if (this.LastHighlighted != null)
	{		
		if (this.MultipleSelect && (!this.LastHighlighted.Selected)) return;
		
		var newx = e.x + document.body.scrollLeft + 15;
		var newy = e.y + document.body.scrollTop + 6;
		
		this.DragSource = this.LastHighlighted;
		this.DragClone = this.LastHighlighted.TextElmt.cloneNode(true);
		
		var res = "";
		
		if (this.MultipleSelect)
		{
			for (var i=0; i<this.AllNodes.length; i++)
			{
				if (this.AllNodes[i].Selected)
				{				
					res += this.AllNodes[i].TextElmt.innerHTML + "<BR>";				
				}
			}	
		}
		else
		{
			res = this.LastHighlighted.TextElmt.innerHTML;
		}
		
		this.DragClone.innerHTML = res;
		
		
		this.DragClone.style.position = "absolute";
		this.DragClone.style.visibility = "hidden";
		this.DragClone.style.top = newy;
		this.DragClone.style.left = newx;
		
		document.body.appendChild(this.DragClone);	
	}
}

RadTreeView.prototype.MouseMove = function(e) 
{	
	if (ActiveRadTreeView != null)
	{		
		if (ActiveRadTreeView.DragMode && ActiveRadTreeView.DragClone != null)
		{			
			var newx = e.x + document.body.scrollLeft + 15;
			var newy = e.y + document.body.scrollTop + 6;
			ActiveRadTreeView.DragClone.style.top = newy;
			ActiveRadTreeView.DragClone.style.left = newx;
			ActiveRadTreeView.DragClone.style.visibility = "visible";
		}
	}
}

function rtvDispatcher(t,w,e,p1,p2)
{
	try	{ var dummy = tlrkTreeViews.length;	} catch (exception) { return; };		
	if (tlrkTreeViews)
	{
		var i;
		var targetTree = null;
		for (i=0; i<tlrkTreeViews.length; i++)
		{
			if (tlrkTreeViews[i].ID == t)
			{
				targetTree = tlrkTreeViews[i];
				break;
			}
		}
		
		if (targetTree.ContextMenuVisible && w != "mclick" && w != "cclick") return;
		ActiveRadTreeView = targetTree;
		
		
		switch (w)
		{
			case "mover"	 : targetTree.MouseOverDispatcher(e); break;
			case "mout"		 : targetTree.MouseOutDispatcher(e); break;
			case "menter"	 : targetTree.MouseEnterDispatcher(e); break;
			case "mleave"	 : targetTree.MouseLeaveDispatcher(e); break;
			case "mclick"	 : targetTree.ClickDispatcher(e); break;
			case "mdclick"	 : targetTree.DoubleClickDispatcher(e); break;
			case "mdown"	 : targetTree.MouseDown(e); break;
			case "mup"		 : targetTree.MouseUp(e); break;
			case "context"   : targetTree.ContextMenu(e); break;
			case "cclick"	 : targetTree.ContextMenuClick(e,p1,p2); break;
		}
	}
}
