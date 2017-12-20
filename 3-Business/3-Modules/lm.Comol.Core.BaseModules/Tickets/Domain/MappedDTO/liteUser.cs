using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    [Serializable, CLSCompliant(true)]
    public class liteUser
    {
        public virtual Int64 Id { get; set; }
        /// <summary>
        /// Se interno, riferimento alla person
        /// </summary>
        public virtual lm.Comol.Core.DomainModel.litePerson Person { get; set; }
        /// <summary>
        /// Nome
        /// </summary>
        public virtual String Name { get; set; }
        /// <summary>
        /// Cognome
        /// </summary>
        public virtual String Surname { get; set; }

        public virtual string SurnameAndName
        {
            get { return Surname + " " + Name; }
            set { }
        }

    }
}
