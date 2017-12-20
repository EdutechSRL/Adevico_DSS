<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AggregationModal.ascx.vb" Inherits="Comunita_OnLine.UC_AggregationModal" %>
<div class="addnewfield aggregation">
    <div class="dialogcontent clearfix">
        <div class="leftfield clearfix child">
            <div>
                <span id="CPHservice_CTRLaddCriterion_LBgenericCriteria" class="title">Fuzzy methods</span>
                <ul class="fieldtypes">

                    <li class="fieldtype">
                        <input id="CPHservice_CTRLaddCriterion_RBtypesIntegerRange" type="radio" name="ctl00$CPHservice$CTRLaddCriterion$fieldType" value="2" checked="checked">
                        <label for="CPHservice_CTRLaddCriterion_RBtypesIntegerRange" id="CPHservice_CTRLaddCriterion_LBtypesIntegerRange">Fuzzy methods 1</label>
                    </li>
                    <li class="fieldtype">
                        <input id="CPHservice_CTRLaddCriterion_RBtypesDecimalRange" type="radio" name="ctl00$CPHservice$CTRLaddCriterion$fieldType" value="3" onclick="javascript: setTimeout('__doPostBack(\'ctl00$CPHservice$CTRLaddCriterion$RBtypesDecimalRange\',\'\')', 0)">
                        <label for="CPHservice_CTRLaddCriterion_RBtypesDecimalRange" id="CPHservice_CTRLaddCriterion_LBtypesDecimalRange">Fuzzy methods 2</label>
                    </li>
                    <li class="fieldtype">
                        <input id="CPHservice_CTRLaddCriterion_RBtypesStringRange" type="radio" name="ctl00$CPHservice$CTRLaddCriterion$fieldType" value="4" onclick="javascript: setTimeout('__doPostBack(\'ctl00$CPHservice$CTRLaddCriterion$RBtypesStringRange\',\'\')', 0)">
                        <label for="CPHservice_CTRLaddCriterion_RBtypesStringRange" id="CPHservice_CTRLaddCriterion_LBtypesStringRange">Fuzzy methods 3</label>
                    </li>
                </ul>
            </div>
        </div>
        <div class="rightfield clearfix child preview">
            <div id="CPHservice_CTRLaddCriterion_DVpreview" class="divpreview">
                <span id="CPHservice_CTRLaddCriterion_LBcriterionName" class="title">Scenario 1</span>
                <span>


                    <div class="fieldobject fuzzyaggregationdescriptor">
                        <div class="fieldrow fielddescription">
                            <span class="description">bla bla bla</span>
                        </div>
                        <div class="fieldrow skills options">
                            <label for="" class="fieldlabel">Necessary Skills</label>
                            <div class="inlinewrapper">
                                <span class="skill option first">skill 1 <span class="level">(level)</span></span>
                                <span class="skill option">skill 2 <span class="level">(level)</span></span>
                                <span class="skill option last">skill 3 <span class="level">(level)</span></span>
                            </div>
                        </div>
                        <div class="fieldrow goodfor options">
                            <label for="" class="fieldlabel">Good for</label>
                            <div class="inlinewrapper">
                                <span class="option first">good for 1</span>
                                <span class="option">good for 2</span>
                                <span class="option last" class="option">good for 3</span>
                            </div>
                        </div>
                        <div class="fieldrow badfor options">
                            <label for="" class="fieldlabel">Bad for</label>
                            <div class="inlinewrapper">
                                <span class="option first">bad for 1</span>
                                <span class="option">bad for 2</span>
                                <span class="option last">bad for 3</span>
                            </div>
                        </div>
                    </div>

                </span>

                <!-- gruppo opzioni nidificato dentro divpreview -->
                <div class="options">
                    <span id="CPHservice_CTRLaddCriterion_LBcriterionOptions" class="title">Opzioni di "scenario 1"</span>
                    <div class="fieldobject singleline vatcode">
                        <div class="fieldrow fielddescription">
                            <span id="CPHservice_CTRLaddCriterion_LBstandardOptionsDescription" class="description">bla bla</span>
                        </div>
                        <div class="fieldrow fieldinput">
                            <label for="CPHservice_CTRLaddCriterion_TXBstandardCriteriaNumber" id="CPHservice_CTRLaddCriterion_LBstandardCriteriaNumber_t" class="description">Option:</label>
                            <input name="ctl00$CPHservice$CTRLaddCriterion$TXBstandardCriteriaNumber" type="text" value="1" id="CPHservice_CTRLaddCriterion_TXBstandardCriteriaNumber" class="inputtext">
                            <span id="CPHservice_CTRLaddCriterion_RNVstandardCriteriaNumber" style="visibility: hidden;">*</span>
                            <label for="CPHservice_CTRLaddCriterion_TXBstandardCriteriaNumber" id="CPHservice_CTRLaddCriterion_LBstandardCriteriaNumberHelp" class="inlinetooltip">tip</label>
                        </div>
                    </div>
                </div>
                <!-- fine gruppo opzioni -->

            </div>
            &nbsp;
        </div>
    </div>
</div>
