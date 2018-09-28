using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.Menu.Domain;

namespace lm.Comol.Modules.Standard.Header.Presentation
{
    public interface IViewPortalHeader : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        Int32 IdHeaderCommunity { get; set; }
        String GetPortalName { get; }
        String GetAdministrationName { get; }
        MenuBarType MenubarType {get;set;}

        void InitalizeControl();
        void DisplayName(String Name);
        void BindLogo(Int32 idCommunity,Int32 idOrganization, String languageCode);
        
        //void BindSkin();    //Aggiungere eventuali parametri se servono

        void LoadMenuBar(String htmlMenu);
        //void LoadTopBar();

        //Altri parametri: nome comunità, logo, ... ???
    }
}
