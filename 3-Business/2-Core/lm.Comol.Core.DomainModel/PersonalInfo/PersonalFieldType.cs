using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.PersonalInfo
{
    [Serializable,Flags]
    public enum PersonalFieldType
    {
        None = 0,
        Login = 1 ,
        Name = 2,
        Surname = 4, 
        BirthDate = 8,
        TelephoneNumber = 16,
        Mobile = 32,
        Fax = 64,
        Mail = 128,
        Address = 256,
        TaxCode = 512,
        CompanyCode = 1024,
        CompanyTaxCode = 2048,
        VatCode = 4096,
        Actions = 8192,
        UsageTime = 16384
    }
}