using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Core.Business
{
    public class BaseCoreServices
    {
        protected BaseModuleManager Manager { get; set; }
        protected iUserContext UC { set; get; }


        public BaseCoreServices() { }
        public BaseCoreServices(iApplicationContext oContext)
        {
            this.Manager = new BaseModuleManager(oContext.DataContext);
            this.UC = oContext.UserContext;
        }
        public BaseCoreServices(iDataContext oDC)
        {
            this.Manager = new BaseModuleManager(oDC);
            this.UC = null;
        }

        protected void CheckNullParameters(params object[] items)
        {
            var ok = true;
            foreach (var item in items)
            {
                if (item == null)
                {
                    ok = false;
                    break;
                }
            }
            if (!ok)
            {
                throw new ArgumentNullException();
            }
        }

        protected void CheckNullAllParameters(params object[] items)
        {
            var ok = true;
            

            Int32 idx = 0;

            Exception ex = new ArgumentNullException();

            foreach (var item in items)
            {
                if (item == null)
                {
                    ok = false;
                    idx += 1;
                    ex.Data.Add(idx, item);
                }
            }
            if (!ok)
            {

                throw ex;
            }
        }

        protected static bool ContainsAllItems<T>(List<T> a, List<T> b)
        {
            return !b.Except(a).Any();
        }
    }
}