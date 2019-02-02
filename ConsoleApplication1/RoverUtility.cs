using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rover_Project;

namespace RoverUtilityPackage
{

    public class RoverUtility
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
        
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the plateau boundary (space separated. e.g. 5 5 )");
            string boundaryString = Console.ReadLine();

            while (true)
            {
                Console.WriteLine("Enter rover's initial position (e.g. 1 2 E)");
                string initialPosition = Console.ReadLine();
                Console.WriteLine("Enter rover's move (e.g. LMRM)");
                string commandString = Console.ReadLine();
                try
                {
                    StringBuilder returnVal = RoverFactoryUser.simulateMovementAndReturnFinal(boundaryString, initialize(initialPosition, commandString));
                    Console.WriteLine("Rover's fnal position>>>" + returnVal.ToString());
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Something went wrong. Please check input are provided as suggested");
                }

                finally
                {
                    Console.WriteLine("\n\n");
                }
            }
        }
    }
    public class RoverInputUtility
    {
        public string initialPosition { get; set; }
        public string commandString { get; set; }
    }
    /// <summary>
    /// Utility class to use the RoverFactory
    /// This is specifically created for use with the command line and running test cases
    /// </summary>
    public class RoverFactoryUser
    {

        public static StringBuilder simulateMovementAndReturnFinal(string boundaryString, List<RoverInputUtility> roverInputs)
        {
            string[] bnd = boundaryString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder returnOutput = new StringBuilder("");

            RoverFactory rf = new RoverFactory();

            foreach (RoverInputUtility obj in roverInputs)
            {
                string[] posArr = obj.initialPosition.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                IRover roverObj = rf.getRoverInstance(Int32.Parse(posArr[0]), Int32.Parse(posArr[1]), char.Parse(posArr[2]), Int32.Parse(bnd[0]), Int32.Parse(bnd[1]));

                char[] cmdArrD = obj.commandString.ToCharArray();
                char[] cmdArr = (from d in cmdArrD where d != ' ' select d).ToArray();
                foreach (char s in cmdArr)
                    roverObj.move(s);

                IPosition posObj = roverObj.getCurrentPosition();
                if (returnOutput.Length != 0) returnOutput.Append("\n");
                returnOutput.Append(posObj.xCo).Append(" ").Append(posObj.yCo).Append(" ").Append(posObj.direction);
            }

            return returnOutput;
        }


    }
}
