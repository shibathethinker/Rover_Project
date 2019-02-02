using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rover_Project;
using RoverUtilityPackage;


namespace TestProject1
{
    [TestClass]
    public class UnitTestRover
    {
        public static List<RoverInputUtility> initialize(string initPosition, string commandString)
        {
           
            List<RoverInputUtility> inputList = new List<RoverInputUtility>();
            RoverInputUtility tempObj = new RoverInputUtility();
            tempObj.initialPosition = initPosition;
            tempObj.commandString = commandString;
            inputList.Add(tempObj);

            return inputList;
        }

        [TestMethod]
        public void TestMethod1()
        {            
            string boundaryString = "5 5";
            StringBuilder returnVal = RoverFactoryUser.simulateMovementAndReturnFinal(boundaryString, initialize("1 2 N", "LMLMLMLMM"));
            Assert.IsTrue("1 3 N".Equals(returnVal.ToString()));
        }

        [TestMethod]
        public void TestMethod2()
        {
            string boundaryString = "5 6";
            StringBuilder returnVal = RoverFactoryUser.simulateMovementAndReturnFinal(boundaryString, initialize("1 2 N", "LMLMLMLMM"));
            Assert.IsTrue("1 3 N".Equals(returnVal.ToString()));
        }

        [TestMethod]
        public void TestMethod3()
        {
            string boundaryString = "1 2";
            StringBuilder returnVal = RoverFactoryUser.simulateMovementAndReturnFinal(boundaryString, initialize("1 2 N", "LMLMLMLMM"));
            Assert.IsTrue("1 2 N".Equals(returnVal.ToString()));
        }

        [TestMethod]
        public void TestMethod4()
        {
            string boundaryString = "1 2";
            StringBuilder returnVal = RoverFactoryUser.simulateMovementAndReturnFinal(boundaryString, initialize("0 0 N", "LMLMLMLMM"));
            Assert.IsTrue("1 2 N".Equals(returnVal.ToString()));
        }

        /// <summary>
        /// Invalid argument exception should be thrown
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestMethod5()
        {
            string boundaryString = "0 0";
            StringBuilder returnVal = RoverFactoryUser.simulateMovementAndReturnFinal(boundaryString, initialize("1 2 N", "LMLMLMLMM"));            
        }

        [TestMethod]
        public void TestMethod6()
        {
            string boundaryString = "5 5";
            StringBuilder returnVal = RoverFactoryUser.simulateMovementAndReturnFinal(boundaryString, initialize("3 3 E", "MMRMMRMRRM"));
            Assert.IsTrue("5 1 E".Equals(returnVal.ToString()));
        }


        [TestMethod]
        public void TestMethod7()
        {
            string boundaryString = "5 5";
            StringBuilder returnVal = RoverFactoryUser.simulateMovementAndReturnFinal(boundaryString, initialize("3 3 E", "MMRMMRMRRM"));
            Assert.IsTrue("5 1 E".Equals(returnVal.ToString()));
        }

        [TestMethod]
        public void TestMethod8()
        {
            string boundaryString = "5 5";
            StringBuilder returnVal = RoverFactoryUser.simulateMovementAndReturnFinal(boundaryString, initialize("1 1 N", "RMMMMM"));
            Assert.IsTrue("5 1 E".Equals(returnVal.ToString()));
        }

        [TestMethod]
        public void TestMethod9()
        {
            string boundaryString = "5 5";
            StringBuilder returnVal = RoverFactoryUser.simulateMovementAndReturnFinal(boundaryString, initialize("1 1 N", "RMMMMMR"));
            Assert.IsTrue("5 1 S".Equals(returnVal.ToString()));
        }

        [TestMethod]
        public void TestMethod10()
        {
            string boundaryString = "5 5";
            StringBuilder returnVal = RoverFactoryUser.simulateMovementAndReturnFinal(boundaryString, initialize("1 1 N", "RMMMMML"));
            Assert.IsTrue("5 1 N".Equals(returnVal.ToString()));
        }

        [TestMethod]
        public void TestMethod11()
        {
            string boundaryString = "5 5";
            StringBuilder returnVal = RoverFactoryUser.simulateMovementAndReturnFinal(boundaryString, initialize("1 1 N", "RMMMMMLM"));
            Assert.IsTrue("5 2 N".Equals(returnVal.ToString()));
        }
    }
}
