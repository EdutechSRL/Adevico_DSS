using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;

namespace lm.Comol.Core.BaseModules.Web.Controls
{
    public class ExtendedDropDown : DropDownList
    {
        public void AddItemGroup(string groupTitle)
        {
            this.Items.Add(new ListItem(groupTitle, "$$OPTGROUP$$OPTGROUP$$"));
        }

        protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.Items.Count > 0)
            {
                bool selected = false;
                bool optGroupStarted = false;
                for (int i = 0; i < this.Items.Count; i++)
                {
                    ListItem item = this.Items[i];
                    if (item.Enabled)
                    {
                        if (item.Value == "$$OPTGROUP$$OPTGROUP$$")
                        {
                            if (optGroupStarted)
                                writer.WriteEndTag("optgroup");
                            writer.WriteBeginTag("optgroup");
                            writer.WriteAttribute("label", item.Text);
                            writer.Write('>');
                            writer.WriteLine();
                            optGroupStarted = true;
                        }
                        else
                        {
                            writer.WriteBeginTag("option");
                            if (item.Selected)
                            {
                                if (selected)
                                {
                                    this.VerifyMultiSelect();
                                }
                                selected = true;
                                writer.WriteAttribute("selected", "selected");
                            }
                            writer.WriteAttribute("value", item.Value, true);
                            if (item.Attributes.Count > 0)
                            {
                                item.Attributes.Render(writer);
                            }
                            if (this.Page != null)
                            {
                                this.Page.ClientScript.RegisterForEventValidation(
                                    this.UniqueID,
                                    item.Value);
                            }
                            writer.Write('>');
                            HttpUtility.HtmlEncode(item.Text, writer);
                            writer.WriteEndTag("option");
                            writer.WriteLine();
                        }
                    }
                }

                if (optGroupStarted)
                {
                    writer.WriteEndTag("optgroup");
                }
            }
        }
    }
}