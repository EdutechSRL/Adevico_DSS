//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using lm.Comol.Core.DomainModel;
//using System.ComponentModel.DataAnnotations;

//namespace lm.Comol.Modules.EduPath.Domain
//{
//    [Serializable()]
//    [Flags]
//    public enum BaseStatusDeleted
//    {
//        None = 0,
//        Manual = 1, //diretto
//        Automatic = 2,//ES: lo imposto sugli assignment dell'attivita' quando cancello l'attivita'
//        Cascade = 4, //cancello perchè ho cancellato il padre
//        //Owner = 8, //usato in Statistic e Assignment quando cancello l'item (Ep, Unit, Activity, SubActivity) che li interessano
//        //Parent = 16,//usato in Statistic e Assignment quando cancello l'item padre (Ep, Unit, Activity, SubActivity) che li interessano
//    }

//    /// <summary>
//    /// Layer Super Type
//    /// </summary>
//    [Serializable()]
//    public abstract class DomainBase
//    {
//        /// <summary>
//        /// Id
//        /// </summary>        
//        [Required()]
//        public virtual Int64 Id { get; protected set; }

//        /// <summary>
//        /// Timestamp Version
//        /// </summary>
//        public virtual Byte[] TimeStamp { get; protected set; }

//        /// <summary>
//        /// Delete status
//        /// </summary>        
//        public virtual BaseStatusDeleted Deleted { get; set; }
        
//    }
//}
