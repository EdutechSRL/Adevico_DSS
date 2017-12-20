using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel
{
    [Serializable(), CLSCompliant(true)]
    public class ModulePage : DomainBaseObject<long>
    {
        public virtual Boolean isDefault { get; set; }
        public virtual string Name { get; set; }
        public virtual string Url { get; set; }
        public virtual ModuleDefinition ModuleDefinition { get; set; }
        public virtual string ModuleCode { get; set; }
        public virtual Boolean isForPortal { get; set; }
        public virtual Boolean ByConfig { get; set; }
        public virtual Boolean WithPlaceHolders { get; set; }
        public virtual IList<ModulePageTranslation> Translations { get; set; }

        public virtual String GetUrlWithContext(int IdCommunity) {
            String iResult = Url.Replace("#IdCommunity#", IdCommunity.ToString());
            return iResult;
        }
        public virtual String GetUrlWithContext(int IdCommunity,int IdUser)
        {
            return GetUrlWithContext(IdCommunity).Replace("#IdUser#", IdUser.ToString());
        }
        public virtual String GetUrlWithContext(int IdCommunity, int IdUser, String CultureInfo)
        {
            return GetUrlWithContext(IdCommunity, IdUser).Replace("#CultureInfo#", CultureInfo);
        }
    }


    [Serializable(), CLSCompliant(true)]
    public class ModulePageTranslation : DomainBaseObject<long>
    {
        public virtual ModulePage ModulePage { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual Language Language { get; set; }
        public ModulePageTranslation() { }
    }
}