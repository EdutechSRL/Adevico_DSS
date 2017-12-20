using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    [Serializable()]
    public class CategoryTranslation : DomainBaseObjectMetaInfo<long>
    {
        public virtual Category Category { get; set; }
        /// <summary>
        /// Nome categoria
        /// </summary>
        public virtual String Name { get; set; }
        /// <summary>
        /// Descrizione (alt sul nome nella treeview)
        /// </summary>
        public virtual String Description { get; set; }

        /// <summary>
        /// Lingua relativa
        /// </summary>
        public virtual int LanguageId { get; set; }

        /// <summary>
        /// Codice lingua
        /// </summary>
        public virtual String LanguageCode { get; set; }

        /// <summary>
        /// Nome lingua
        /// </summary>
        public virtual String LanguageName { get; set; }

        //public virtual Boolean IsMulti
        //{
        //    get
        //    {
        //        return (LanguageId <= 0) ? true : false;
        //    }
        //}

    }
}
