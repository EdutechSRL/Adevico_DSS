using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public interface IViewScormStatisticsUpdate : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        //long PreloadedFileId { get; }
        //long PreloadedLinkId { get; }
        long FileId { get; set; }

        dtoEvaluation GetScormEvaluation(int IdUser, long IdFile);
        List<long> EvaluateLinks(int IdUser, List<long> links , dtoEvaluation evaluation);

    }
}