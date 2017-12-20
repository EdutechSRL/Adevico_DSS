
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public enum ColumnType 
    {
        normal = 0,
        computed = 1,
        display = 2,
        errors = 3,
    }
}