using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Modules.Standard.Header.Business;
using lm.Comol.Modules.Standard.Header.Domain;

//using Comol.Entity;

namespace lm.Comol.Modules.Standard.Header.Presentation
{
    public class TopBarPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        private ServiceTopbar _Service;
        private ServiceTopbar Service
        {
            get
            {
                if (_Service == null)
                    _Service = new ServiceTopbar(AppContext);
                return _Service;
            }
        }
        protected virtual IViewTopBar View
        {
            get { return (IViewTopBar)base.View; }
        }

        public TopBarPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public TopBarPresenter(iApplicationContext oContext, IViewTopBar view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }


        public void InitView(DisplayNameMode mode= DisplayNameMode.name)
        {
            int IdUser = UserContext.CurrentUserID;
            Person person = Service.GetPerson(IdUser);

            if (UserContext.isAnonymous || person == null)
                View.LoadUnregisteredTopBar();
            else
            {
                Language language = Service.GetLanguage(person.LanguageID);
                RenderTopBar(true, person, language, mode);
            }
        }

        private void RenderTopBar(Boolean useCache, Person person, Language language, DisplayNameMode mode)
        {
            String render = "";
            render = (useCache) ? Service.GetCachedTopBar(person,language) : "";

            if (String.IsNullOrEmpty(render))
            {
                render = View.GetRenderTopBar(GetDisplayName(person, mode), Service.GetAvailableAuthenticationProviderTypes(person), person.TypeID, person.LanguageID, language.Name, Service.GetAvailableModules(person));
                if (useCache)
                    Service.SetCachedTopBar(person, language, render);
            }
            View.RenderTopBar(render);
        }

        private String GetDisplayName(Person person, DisplayNameMode mode)
        {
            if (person == null)
                return "";
            else
            {
                switch (mode)
                {
                    case DisplayNameMode.name:
                        return person.Name;
                    case DisplayNameMode.namesurname:
                        return person.NameAndSurname;
                    case DisplayNameMode.surname:
                        return person.Surname;
                    case DisplayNameMode.surnamename:
                        return person.SurnameAndName;
                    default:
                        return "";
                }
            }
        }
    }
}