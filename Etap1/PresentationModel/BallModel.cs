using Logic;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace PresentationModel
{
    public class BallModel : INotifyPropertyChanged
    {
        private double x;
        private double y;
        private double diameter;

        public event PropertyChangedEventHandler? PropertyChanged;

        public double X
        {
            get { return x; }
            set
            {
                x = value;
                OnPropertyChanged("X");
            }
        }

        public double Y
        {
            get { return y; }
            set
            {
                y = value;
                OnPropertyChanged("Y");
            }
        }

        public double Diameter
        {
            get { return diameter; }
        }


        public BallModel(double x, double y, double diameter)
        {
            this.x = x;
            this.y = y;
            this.diameter = diameter;
        }

        public void Move(double x, double y)
        {
            this.x = x;
            this.y = y;
        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
