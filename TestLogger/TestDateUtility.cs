using LogComponent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestLogger
{
    [TestClass]
    public class TestDateUtility
    {
        [TestMethod]
        public void TestIsSameDateTrue()
        {
            DateTime dateOne = new DateTime(2020, 6, 6, 23, 59, 59);
            DateTime dateTwo = new DateTime(2020, 6, 6, 21, 49, 55);

            Assert.IsTrue(DateUtility.IsSameDate(dateOne, dateTwo));
        }

        [TestMethod]
        public void TestIsSameDateFalse()
        {
            DateTime dateOne = new DateTime(2020, 6, 6, 23, 59, 59);
            DateTime dateTwo = new DateTime(2020, 6, 7, 0, 0, 0);

            Assert.IsFalse(DateUtility.IsSameDate(dateOne, dateTwo));
        }



    }
}
