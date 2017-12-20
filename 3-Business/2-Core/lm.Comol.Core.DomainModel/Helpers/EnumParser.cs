using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Helpers
{
    public static class EnumParser<T>
        {
            public static T GetByString(string Expression, T defaultValue)
            {
                T iResponse = defaultValue;
                if (!String.IsNullOrEmpty(Expression))
                    if (Enum.IsDefined(typeof(T), Expression))
                        iResponse = (T)Enum.Parse(typeof(T), Expression);

                return iResponse;
            }

            public static String convertToString(T eff)
            {
                return Enum.GetName(eff.GetType(), eff);
            }

            public static T converToEnum<T>(String enumValue)  
            {
                return (T)Enum.Parse(typeof(T), enumValue);
            }

        }

}
