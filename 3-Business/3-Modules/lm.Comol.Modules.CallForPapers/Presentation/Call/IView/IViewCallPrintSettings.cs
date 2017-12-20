using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewCallPrintSettings : iDomainView
    {
        void Initialize(CallPrintSettings settings, long moduleId);
        void UpdateSettings(ref CallPrintSettings settings);
        //CallPrintSettings GetSettings();

    }
}