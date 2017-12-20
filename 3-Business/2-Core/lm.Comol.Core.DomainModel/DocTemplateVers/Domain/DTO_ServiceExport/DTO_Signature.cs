using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport
{
    [Serializable]
    public class DTO_Signature
    {
        public virtual Int64 Id { get; set; }

        public virtual Int16 Order { get; set; }

        public virtual SignaturePosition Position { get; set; }

        public virtual String Text { get; set; }
        public virtual Boolean IsHTML { get; set; }

        public virtual Boolean HasImage { get; set; }
        public virtual string Path { get; set; }
        public virtual Int16 Width { get; set; }
        public virtual Int16 Height { get; set; }

        public virtual Boolean HasPDFPositioning { get; set; }
        public virtual Single PosBottom { get; set; }
        public virtual Single PosLeft { get; set; }

        public virtual int PagePlacingMask { get; set; }
        public virtual String PagePlacingRange { get; set; }
    }
}
