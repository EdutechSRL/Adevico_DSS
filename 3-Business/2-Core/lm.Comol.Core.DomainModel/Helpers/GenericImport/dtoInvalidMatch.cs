using System;
namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public class dtoInvalidMatch<T>
    {
        public virtual T Attribute { get; set; }
        public virtual InvalidMatch InvalidMatch { get; set; }
    }
}