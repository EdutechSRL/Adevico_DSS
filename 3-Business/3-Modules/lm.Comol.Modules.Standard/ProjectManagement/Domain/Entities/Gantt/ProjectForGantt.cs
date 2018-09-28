using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", ElementName="project", IsNullable = false)]

    public partial class ProjectForGantt
    {
        [System.Xml.Serialization.XmlElementAttribute("task", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<ActivityForGantt> Items {get;set;}
        public ProjectForGantt()
        {
            Items = new List<ActivityForGantt>();
        }
    }

}