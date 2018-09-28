using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
     [Serializable]   
    public class dtoEduPath:dtoGenericItem    
     {
         public Boolean IsMooc { get; set; }
         public StatusStatistic statusStat { get; set; }
  
         public IList<dtoUnit> Units { get; set; }
        
         public dtoEduPath()
       :base()
         {          
             Units = new List<dtoUnit>();
         }

         public dtoEduPath(Path path, Status personalStatus, RoleEP role)
             : base(path, personalStatus, role)
         {
             Units = new List<dtoUnit>();
             IsMooc = path.IsMooc;
         }

         public dtoEduPath(Path path, Status personalStatus, RoleEP role, IList<dtoUnit> units)
             : base(path, personalStatus, role)
         {
             Units = units;
             IsMooc = path.IsMooc;
         }
     }
}
