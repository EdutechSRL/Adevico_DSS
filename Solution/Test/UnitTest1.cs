using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Data;


namespace Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CSharpNewSession()
        {
            ISession actual = null;
            actual = SessionHelper.GetNewSession();

            Assert.IsNotNull(actual);
        }
        [TestMethod]
        public void CSharpFactoryBuilder()
        {
            //Dim actual As ISessionFactory
            //actual = SessionHelper.FactoryBuilder
            //Assert.IsNotNull(actual)

            ISessionFactory actual = null;

            actual = SessionHelper.FactoryBuilder();

            Assert.IsNotNull(actual);
        }
    }
}
