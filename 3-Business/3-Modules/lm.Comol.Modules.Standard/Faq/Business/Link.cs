using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Faq
{
    public class IdLink
    {
        public virtual Faq Faq { get; set; }
        public virtual Category Category { get; set; }

        public IdLink()
        { 
        }
        public IdLink(Faq Faq, Category Category)
        {
            this.Faq = Faq;
            this.Category = Category;
        }
        public override int GetHashCode()
        {
            return Faq.Id.GetHashCode() ^ Category.Id.GetHashCode(); // ^ -> XOR logico...
        }

    }

    public class Link
    {
        public virtual IdLink Id { get; set; }

        public Link() { }
        public Link(IdLink Id)
        {
            this.Id = Id;
        }
        public Link(Faq Faq, Category Category)
        {
            this.Id = new IdLink(Faq, Category);
        }

    }
}
