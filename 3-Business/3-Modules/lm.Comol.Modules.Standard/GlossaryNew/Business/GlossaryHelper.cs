using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace lm.Comol.Modules.Standard.GlossaryNew.Business
{
    public static class GlossaryHelper
    {
        public static Boolean ValidateFields(Object obj, out List<string> resourceErrorList)
        {
            var result = true;
            resourceErrorList = new List<string>();

            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                var attibutes = property.GetCustomAttributes(false).ToDictionary(a => a.GetType().Name, a => a);

                if (property.PropertyType == typeof (String))
                {
                    foreach (var attribute in attibutes)
                    {
                        if (attribute.Key == "RequiredAttribute")
                        {
                            var requiredAttribute = (RequiredAttribute) attribute.Value;
                            var value = property.GetValue(obj, null) as String;
                            if (String.IsNullOrWhiteSpace(value))
                            {
                                resourceErrorList.Add(requiredAttribute.ErrorMessageResourceName);
                                result = false;
                            }
                        }
                    }
                }
                else if (property.PropertyType == typeof (Int32))
                {
                    foreach (var attribute in attibutes)
                    {
                        if (attribute.Key == "RangeAttribute")
                        {
                            var rangeAttribute = (RangeAttribute) attribute.Value;
                            var value = Convert.ToInt32(property.GetValue(obj, null));
                            var minValue = (Int32) rangeAttribute.Minimum;
                            var maxValue = (Int32) rangeAttribute.Maximum;
                            if (value < minValue || value > maxValue)
                            {
                                resourceErrorList.Add(rangeAttribute.ErrorMessageResourceName);
                                result = false;
                            }
                        }
                    }
                }
                else if (property.PropertyType == typeof (Int64))
                {
                    foreach (var attribute in attibutes)
                    {
                        if (attribute.Key == "RangeAttribute")
                        {
                            var rangeAttribute = (RangeAttribute) attribute.Value;
                            var value = Convert.ToInt64(property.GetValue(obj, null));
                            var minValue = (double) rangeAttribute.Minimum;
                            var maxValue = (double) rangeAttribute.Maximum;
                            if (value < minValue || value > maxValue)
                            {
                                resourceErrorList.Add(rangeAttribute.ErrorMessageResourceName);
                                result = false;
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}