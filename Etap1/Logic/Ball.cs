using System.ComponentModel;

namespace Logic
{
    public class Ball
    {


        private double x { get; set; }
        private double y { get; set; }
        private double diameter { get; set; } = 20;
        private double xSpeed { get; set; }
        private double ySpeed { get; set; }


        public double X
        {
            get { return x; }

            set
            {
                x = value;
            }
        }

        public double Y
        {
            get { return y; }
            set
            {
                y = value;
            }
        }

        public double Diameter
        {
            get
            {
                return diameter;
            }
            set
            {
                diameter = value;
            }
        }

        public double XSpeed
        {
            get;
            set;
        }

        public double YSpeed
        {
            get;
            set;
        }


        public Ball(double x, double y, double xS, double yS)
        {
            this.x = x;
            this.y = y;
            this.XSpeed = xS;
            this.YSpeed = yS;
        }

    }

}
