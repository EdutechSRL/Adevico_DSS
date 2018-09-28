using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Faq
{
    [Serializable]
    public class Faq : DomainBaseObjectMetaInfo<Int64>
    {
        //public virtual Int64 Id { get; set; }
        
        [Required(ErrorMessage="*"), StringLength(255, ErrorMessage="Must be under 255 characters")]
        public virtual String Question { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual String Answer { get; set; }
        public virtual Int32? Order { get; set; }

        private IList<Category> _onCategories = new List<Category>();
        public virtual IList<Category> onCategories
        {
            get { return _onCategories; }
            set { _onCategories = value; }
        }

        public virtual Int32 CommunityId { get; set; }
        //public virtual IList<File> Files { get; set; }

        public virtual IList<Faq_Int> Internationalizations { get; set; }
    }
}
