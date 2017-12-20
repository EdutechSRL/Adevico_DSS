using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    [Serializable, CLSCompliant(true)]
    public class Token
    {
        public virtual Int64 Id { get; set; }
        public virtual TicketUser User { get; set; }
        public virtual Guid Code { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual Domain.Enums.TokenType Type {get; set;}
        //public virtual Boolean Validated { get; set; }

        public Token()
        {
            User = new TicketUser();
            Code = System.Guid.Empty;
            CreatedOn = DateTime.Now;
        }

        public Token(TicketUser User, Boolean CreateToken)
        {
            this.User = User;
            if (CreateToken)
                Code = System.Guid.NewGuid();
            else
                Code = System.Guid.Empty;
            
            CreatedOn = DateTime.Now;
        }

        public Token(TicketUser User, string Token)
        {
            this.User = User;
            try
            {
                Code = new System.Guid(Token);
            }
            catch
            {
                Code = System.Guid.Empty;
            }
            
            CreatedOn = DateTime.Now;
        }

        public Token(TicketUser User, Guid Token)
        {
            this.User = User;
            Code = Token;
            CreatedOn = DateTime.Now;
        }
    }


}
