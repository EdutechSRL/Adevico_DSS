﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Eco = lm.Comol.Modules.CallForPapers.AdvEconomic;

namespace lm.Comol.Modules.CallForPapers.AdvEconomic.Presentation.View
{
    /// <summary>
    /// iView valutazione economica
    /// </summary>
    public interface iViewEcoEvaluation : CallForPapers.Presentation.IViewBase
    {
        /// <summary>
        /// Bind della View
        /// </summary>
        /// <param name="evaluations"></param>
        void BindView(Eco.dto.dtoEcoEvaluation evaluations);
        /// <summary>
        /// Id commissione
        /// </summary>
        long CommId { get; }
        /// <summary>
        /// Id valutazione economica
        /// </summary>
        long EvalId { get; }

        /// <summary>
        /// Invia azione utente
        /// </summary>
        /// <param name="CommunityId">Id comunità</param>
        /// <param name="ModuleId">Id modulo</param>
        /// <param name="action">Tipo azione</param>
        /// <param name="objectType">Tipo oggetto</param>
        /// <param name="ObjectId">Id oggetto</param>
        void SendUserAction(
            int CommunityId,
            int ModuleId,
            lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType action,
            lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ObjectType objectType,
            string ObjectId
            );

    }

}
