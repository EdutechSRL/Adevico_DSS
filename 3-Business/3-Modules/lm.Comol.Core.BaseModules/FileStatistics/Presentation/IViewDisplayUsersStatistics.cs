using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileStatistics.Presentation
{
    public interface IViewDisplayUsersStatistics : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        void InitializeNoPermission(int idCommunity);
        ///// <summary>
        ///// Richiama il SUO presenter e gli passa DIRETTAMENTE i parametri
        ///// </summary>
        ///// <param name="FilesIds"></param>
        ///// <param name="UsersIds"></param>
        //void InitializeView(IList<Int64> FilesIds, IList<Int32> UsersIds); //-> richiama il MIO presenter e gli passa direttamente i parametri


        /// <summary>
        /// Richiamata dal SUO presenter per settare effettivamente i dati nei repeater/gridview/etc...
        /// </summary>
        /// <param name="FilesData"></param>
        void SetData(IList<lm.Comol.Modules.ScormStat.ColPerson> FilesData); //Persenter mio
        void SendReport(IList<lm.Comol.Modules.ScormStat.ColPerson> Persons, String CommunityName, Boolean isPdf);
    }
}
