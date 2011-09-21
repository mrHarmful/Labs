using Lab2_Pankov_Quiz;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Lab1Tests
{
    
    
    /// <summary>
    ///This is a test class for AnswerTest and is intended
    ///to contain all AnswerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AnswerTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        public void EqualsTest()
        {
            Assert.IsTrue(new Answer("asd", true) == new Answer("asd", true));
            Assert.IsTrue(new Answer("asd", true) != new Answer("asd", false));
            Assert.IsFalse(new Answer("asd", true).Equals(new DateTime()));
            Assert.IsTrue(new Answer("asd", true).Equals((Object)new Answer("asd", true)));
        }
    }
}
