using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Rover_Project
{
    class Program
    {
      
        static void Main(string[] args)
        {
            
        }
    }



 #region allInterfaces
    public interface IPosition
    {
        int xCo { get; set; }
        int yCo { get; set; }
        char direction { get; set; }
    }
    public interface IRover
    {
        int xCo { get; set; }
        int yCo { get; set; }
        /// <summary>
        /// The current head direction of the rover
        /// possible values - N/S/E/W
        /// </summary>
        char direction { get; set; }
        /// <summary>
        /// e.g commands - L/R/M 
        /// </summary>
        /// <param name="command"></param>
        void move(char command);
        /// <summary>
        /// Returns the current position of the rover
        /// </summary>
        /// <returns></returns>
        IPosition getCurrentPosition();
    }
    public interface IMovementStrategy
    {
        /// <summary>
        /// Allows to dynamically change the movement algorithm of the implementing Rover object
        /// e.g - 'Over the air udpate' to the Rover        
        /// </summary>
        /// <param name="command"></param>
        void move(char command);
    }
    
    #endregion

#region allClasses
    public class Position : IPosition
    {
        public int xCo { get; set; }
        public int yCo { get; set; }
        public char direction { get; set; }

        public Position(int x, int y, char dir)
        {
            xCo = x; yCo = y; direction = dir;
        }
    }
    public class Boundary
    {
        public int topRightX { get; set; }
        public int topRightY { get; set; }

        public Boundary(int x, int y)
        { topRightX = x; topRightY = y; }        
    }
    /// <summary>
    /// Implements the movement strategy asked in the question
    /// Different strategy class can be used if required, dynamically
    /// </summary>
    public class MovementStrategy1 : IMovementStrategy
    {
        private IRover context { get; set; }
        public MovementStrategy1(IRover c)
        {  context = c; }
        public void move(char command)
        {
            switch (command)
            {
                case 'L': rotateHead(command); break;
                case 'R': rotateHead(command); break;
                case 'M': moveOnePoint(); break;
            }
        }

        private void rotateHead(char command)
        {
            List<char> leftRotationLogic = new List<char> { 'N','W','S','E'};
            List<char> rightRotationLogic = new List<char> { 'N','E','S','W'};
            if (command == 'L')
                rotateHeadAction(leftRotationLogic);
            else if (command == 'R')
                rotateHeadAction(rightRotationLogic);
        }

        private void rotateHeadAction(List<char> rotationLogic)
        {
            char currDir = this.context.direction;

            for (int i = 0; i < rotationLogic.Count; i++)
            {
                if (rotationLogic[i] == currDir)
                { 
                this.context.direction=rotationLogic[(i+1)%rotationLogic.Count];
                break;
                }
            }        
        }
        /// <summary>
        /// Move one point in the same direction as the head
        /// </summary>
        private void moveOnePoint()
        {
            switch(this.context.direction)
            {
                case 'N': this.context.yCo += 1;
                    break;
                case 'S': this.context.yCo -= 1;
                    break;
                case 'E': this.context.xCo += 1;
                    break;
                case 'W': this.context.xCo -= 1;
                    break;
            }
        }
    }

    public class Rover : IRover
    {
        private int x;
        public int xCo
        {
            get { return x; }
            set
            {//Always check the boundary before updating co-ordinates
                if (0 <= value && value <= this.areaBoundary.topRightX)
                    x = value;
            }
        }
        private int y;
        public int yCo
        {
            get { return y; }
            set
            {
                if (0 <= value && value <= this.areaBoundary.topRightY)
                    y = value;
            }
        }
        /// <summary>
        /// The current head direction of the rover
        /// </summary>
        public char direction { get; set; }
        private Boundary areaBoundary { get; set; }
        public IMovementStrategy _strategy { get; set; }

        public Rover(int x,int y,char dir,Boundary bnd)
        {
            if(x<0||y<0||dir==null||bnd.topRightX<0||bnd.topRightY<0||x>bnd.topRightX||y>bnd.topRightY)
                throw new ArgumentException("Can not instantiate Rover Object: Invalid parameter values");
            
            areaBoundary=bnd;
            xCo=x;
            yCo=y;
            direction=dir;
        }

        public void move(char command)
        {
            if(this._strategy!=null)
            this._strategy.move(command);
            else //Use default strategy
            defaultMoveStrategy(command);
        }

        private void defaultMoveStrategy(char command)
        {
            switch (command)
            {
                case 'M': moveOnePoint(); break;
            }
        }

        private void moveOnePoint()
        {
            switch (this.direction)
            {
                case 'N': this.yCo += 1;
                    break;
                case 'S': this.yCo -= 1;
                    break;
                case 'E': this.xCo += 1;
                    break;
                case 'W': this.xCo -= 1;
                    break;
            }
        }
        /// <summary>
        /// If the second paramter is passed null then the default movement strategy is used
        /// Use this move method if you want to change the strategy after the Rover object has been instantiated
        /// </summary>
        /// <param name="command"></param>
        /// <param name="strategy"></param>
        public void move(char command, IMovementStrategy strategy)
        {
            if (strategy != null)
                strategy.move(command);
            else
                defaultMoveStrategy(command);
        }
        /// <summary>
        /// Return the current position details of the Rover
        /// </summary>
        /// <returns></returns>
        public IPosition getCurrentPosition()
        {
            return new Position(this.xCo,this.yCo,this.direction);
        }
    }

    public class RoverFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startX">start position of the Rover (X-CoOrd)</param>
        /// <param name="startY">start position of the Rover (Y-CoOrd)</param>
        /// <param name="direction">start direction of the Rover</param>
        /// <param name="areaTopRightX">boundary of the area (TopRight x CoOrd)</param>
        /// <param name="areaTopRightY">boundary of the area (TopRight y CoOrd)</param>
        /// <returns></returns>
        public IRover getRoverInstance(int startX,int startY,char direction,int areaTopRightX,int areaTopRightY)
        {
            Boundary mars = new Boundary(areaTopRightX,areaTopRightY);
            Rover roverInstance = new Rover(startX, startY, direction, mars);
            roverInstance._strategy = new MovementStrategy1(roverInstance);

            return roverInstance;
        }
    }

#endregion

}
