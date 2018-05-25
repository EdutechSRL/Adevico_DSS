using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Modules.CallForPapers.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

namespace lm.Comol.Modules.CallForPapers.AdvEconomic.Presentation
{
    public class AdvEcoTableExportPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        /// <summary>
        /// View: pagina di valutazione economica
        /// </summary>
        protected virtual View.iViewTableExport View
        {
            get { return (View.iViewTableExport)base.View; }
        }

        /// <summary>
        /// Servizio Bandi
        /// </summary>
        private ServiceCallOfPapers _ServiceCall;
        /// <summary>
        /// Servizio bandi
        /// </summary>
        private ServiceCallOfPapers CallService
        {
            get
            {
                if (_ServiceCall == null)
                    _ServiceCall = new ServiceCallOfPapers(AppContext);
                return _ServiceCall;
            }
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="oContext">Application Context</param>
        public AdvEcoTableExportPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="oContext">Application context</param>
        /// <param name="view">vista: pagina valutazione economica</param>
        public AdvEcoTableExportPresenter(iApplicationContext oContext, View.iViewTableExport view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }

        /// <summary>
        /// Inizializzazione view: non in uso
        /// </summary>
        public void InitView()
        {
           
        }

        /// <summary>
        /// Generazione dati all'interno dello stream per l'esportazione in XLSX
        /// </summary>
        /// <param name="documentStream">Stream</param>
        /// <param name="DocFormat">Formato: solo XLSX</param>
        public void ExportStream(
           System.IO.Stream documentStream,
           Telerik.Documents.SpreadsheetStreaming.SpreadDocumentFormat DocFormat = Telerik.Documents.SpreadsheetStreaming.SpreadDocumentFormat.Xlsx)
        {
            CallService.EcoTablesExportStream(View.EvaluationId, documentStream, DocFormat);
        }
    }
}
