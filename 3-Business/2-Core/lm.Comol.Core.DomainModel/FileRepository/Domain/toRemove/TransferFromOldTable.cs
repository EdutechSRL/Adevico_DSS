using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain.toRemove
{
    public class TransferFromOldTable
    {
        public virtual long FLDS_id {get;set;}
        public virtual Int32 FLDS_CMNT_id {get;set;}
        public virtual Int32 FLDS_TPMT_id {get;set;}
        public virtual Int32 FLDS_Ordine {get;set;}
        public virtual Int32 FLDS_PRSN_ID {get;set;}
        public virtual String FLDS_nome {get;set;}
        public virtual String FLDS_descrizione {get;set;}
        public virtual DateTime FLDS_dataInserimento {get;set;}
        public virtual long FLDS_dimensione {get;set;}
        public virtual String ContentType {get;set;}
  
        public virtual long FLDS_numeroScaricamenti {get;set;}
        public virtual String FLDS_path {get;set;}
        public virtual Int32 FLDS_RLPC_id {get;set;}
        public virtual Int32 FLDS_CTGR_id {get;set;}

        public virtual Boolean FLDS_visibile {get;set;}
        public virtual Int32 FLDS_Livello {get;set;}
        public virtual Boolean FLDS_isFile {get;set;}
        public virtual long FLDS_padreID {get;set;}
        public virtual long FLDS_CloneID {get;set;}

        public virtual Boolean FLDS_isVirtual {get;set;}

        public virtual Boolean FLDS_isDeleted {get;set;}
        public virtual Boolean FLDS_IsPersonal {get;set;}
        public virtual Guid  FLDS_GUID {get;set;}

        public virtual DateTime? _ModifiedOn {get;set;}
        public virtual Int32 _ModifiedBy {get;set;}
        public virtual Int32 IdRepositoryItemType {get;set;}
        public virtual Boolean IsDownloadable {get;set;}
        public virtual String Extension {get;set;}
        public virtual Guid CloneUniqueID {get;set;}
        public virtual Boolean IsInternal {get;set;}
        public virtual Boolean AllowUpload {get;set;}

	
         public virtual Int32 ServiceActionAjax {get;set;}
        public virtual String ServiceOwner {get;set;}
        public virtual String OwnerFullyQualifiedName {get;set;}
        public virtual Int32 OwnerTypeID {get;set;}
         public virtual long  OwnerLongID {get;set;}
        public virtual Int32  Discriminator {get;set;}
        public virtual Boolean changedname{ get; set; }
        public virtual Boolean updated { get; set; }

    }


    public class liteTransferFromOldTable
    {
        public virtual long FLDS_id { get; set; }
        public virtual Boolean changedname { get; set; }
        public virtual Boolean updated { get; set; }

    }
}
