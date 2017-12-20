using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers
{
    [Serializable]
    public class Signature : lm.Comol.Core.DomainModel.DomainBaseObjectMetaInfo<Int64>
    {
        public virtual Int64 SubVersion { get; set; }
        //public virtual int Version { get; set; }
        public virtual TemplateVersion TemplateVersion { get; set; }
        public virtual Boolean IsActive { get; set; }
        public virtual Int16 Placing { get; set; }

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

        /// <summary>
        /// lm.Comol.Core.DomainModel.Helpers.Export.PageMaskingInclude
        /// </summary>
        /// <remarks>
        ///     none = 0,
        ///     All = 1,
        ///     Even = 2,
        ///     Odd = 4,
        ///     First = 8,
        ///     Last = 16,
        ///     Range = 32
        /// </remarks>
        public virtual Int16 PagePlacingMask { get; set; }

        /// <summary>
        /// Elenco pagine in gruppi seprati da virgola.
        /// Ogni gruppo può contenere la singola pagina o un range di pagine nel formato: pagina iniziale - pagina finale
        /// </summary>
        /// <example>
        /// 1,2,5-7 : indica le pagine: 1, 2, 5, 6 e 7
        /// </example>
        public virtual String PagePlacingRange { get; set; }

        public virtual Signature Copy(
            TemplateVersion TemplateVersion, Boolean IsActive, int Version
            ,Person Person, String ipAddrees, String IpProxyAddress)
        {
            Signature NewSignature = new Signature();
            NewSignature.CreateMetaInfo(Person, ipAddrees, IpProxyAddress);

            NewSignature.TemplateVersion = TemplateVersion;
            NewSignature.SubVersion = SubVersion;
            NewSignature.IsActive = IsActive;

            NewSignature.Placing = this.Placing;
            NewSignature.Position = this.Position;
            NewSignature.Text = this.Text;
            NewSignature.IsHTML = this.IsHTML;

            NewSignature.HasImage = this.HasImage;
            NewSignature.Path = this.Path;
            NewSignature.Width = this.Width;
            NewSignature.Height = this.Height;

            NewSignature.HasPDFPositioning = this.HasPDFPositioning;
            NewSignature.PosBottom = this.PosBottom;
            NewSignature.PosLeft = this.PosLeft;

            PagePlacingMask = this.PagePlacingMask;
            PagePlacingRange = this.PagePlacingRange;

            return NewSignature;
        }
    }
}