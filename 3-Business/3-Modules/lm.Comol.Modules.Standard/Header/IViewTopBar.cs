using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Header.Presentation
{
    public interface IViewTopBar : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        void LoadUnregisteredTopBar();
        void LoadTopBar(String userName, List<int> providerTypes, int idProfileType, int idLanguage, String languageName, List<String> availableModules);

        String GetRenderTopBar(String userName, List<int> providerTypes, int idProfileType, int idLanguage, String languageName, List<String> availableModules);
        void RenderTopBar(String render);
    }
}