<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ActivitySelector.ascx.vb" Inherits="Comunita_OnLine.UC_ActivitySelector" %>


<div class="dialog dlgurl" title="Add Url">
    <div class="fieldobject taskselect">
        <div class="fieldrow">
            <label class="fieldlabel" for="">Select Task:</label>
            <div class="dropdown enabled"><!--
                                --><input type="hidden"/><!--
                                --><span class="ddselector">Seleziona</span><!--
                                --><span class="selector">
	                                    <span class="selectoricon">&nbsp;</span>
	                                    <span class="listwrapper">
						                    <span class="arrow"></span>
						                    <ul class="items">
						                        <li class="item">
						                           <a class="ddbutton activeselected" data-text="categoria 1" data-id="ctg-1">
						                           	<span class="icon">&nbsp;</span>
						                           	<span class="categoryname">categoria 1</span>
						                           </a>
						                            <ul class="items">
						                                <li class="item">
						                                    <a class="ddbutton" data-text="categoria 2" data-id="ctg-2">
						                                    	<span class="icon">&nbsp;</span>
						                                    	<span class="categoryname">categoria 2</span>
						                                    </a>
						                                    <ul class="items">
						                                        <li class="item">
						                                            <a class="ddbutton" data-text="categoria 3 nome molto lungo loremuipsumdolorsit amet categoria 3 nome molto lungo loremuipsumdolorsit amet" data-id="ctg-3">
						                                            	<span class="icon">&nbsp;</span>
						                                            	<span class="categoryname">categoria 3 nome molto lungo loremuipsumdolorsit amet</span>
						                                            </a>
						                                            <ul class="items">

						                                            </ul>
						                                        </li>
						                                    </ul>
						                                </li>
						                            </ul>
						                        </li>
						                        <li class="item">
						                            <a class="ddbutton" data-text="categoria 4" data-id="ctg-4">
						                            	<span class="icon">&nbsp;</span>
						                            	<span class="categoryname">categoria 4</span>
						                            </a>
						                            <ul class="items">
						                                <li class="item">
						                                   <a class="ddbutton" data-text="categoria 5" data-id="ctg-5">
						                                   	<span class="icon">&nbsp;</span>
						                                   	<span class="categoryname">categoria 5</span>
						                                   </a>
						                                    <ul class="items">

						                                    </ul>
						                                </li>
						                            </ul>
						                        </li>
						                    </ul>
	           						 </span>
                                   </span>
                            </div>
        </div>
    </div>
<div class="fieldobject attachmentinput">
    <div class="fieldrow first">
        <label class="fieldlabel" for="">Url:</label>
        <input class="inputtext inputurl" type="text"/>
    </div>
    <div class="fieldrow">
        <label class="fieldlabel" for="">Url:</label>
        <input class="inputtext inputurl" type="text"/>
    </div>
    <div class="fieldrow">
        <label class="fieldlabel" for="">Url:</label>
        <input class="inputtext inputurl" type="text"/>
    </div>
    <div class="fieldrow">
        <label class="fieldlabel" for="">Url:</label>
        <input class="inputtext inputurl" type="text"/>
    </div>
    <div class="fieldrow last">
        <label class="fieldlabel" for="">Url:</label>
        <input class="inputtext inputurl" type="text"/>
    </div>
</div>
    <div class="fieldobject commands">
        <div class="fieldrow buttons right">
            <input type="submit" value="Ok"/> <a class="linkMenu close" href="">Cancel</a>
        </div>
    </div>
</div>
