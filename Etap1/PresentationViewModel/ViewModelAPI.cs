using PresentationModel;
using PresentationViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PresentationViewModel
{
    public class ViewModelAPI : INotifyPropertyChanged
    {

        public bool isStartEnabled { get; set; } = true;

        public bool isStopEnabled { get; set; } = false;

        public string inputNumber = "10";

        public ICommand OnClickStartButton { get; set; }

        public ICommand OnClickStopButton { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<BallModel> ballList { get; set; } = new ObservableCollection<BallModel>();

        private ModelAbstractAPI modelAPI;

        public bool IsStartEnabled
        {
            get { return isStartEnabled; }
            set
            {
                isStartEnabled = value;
                OnPropertyChanged(nameof(isStartEnabled));
            }
        }

        public bool IsStopEnabled
        {
            get { return isStopEnabled; }
            set
            {
                isStopEnabled = value;
                OnPropertyChanged(nameof(isStopEnabled));
            }
        }

        public ViewModelAPI()
        {
            OnClickStartButton = new RelayCommand(() => StartButtonHandle());
            OnClickStopButton = new RelayCommand(() => StopButtonHandle());
            modelAPI = ModelAbstractAPI.CreateApi();
        }

        public void StopButtonHandle()
        {
            modelAPI.Stop();
            ballList.Clear();
            this.IsStartEnabled = true;
            this.IsStopEnabled = false;

        }

        public void StartButtonHandle()
        {
            int value = getInputValue();
            if (value > 0)
            {
                this.IsStartEnabled = false;
                this.IsStopEnabled = true;
                modelAPI.AddBalls(value);
                modelAPI.AddModelBalls();
                foreach (BallModel ball in modelAPI.BallModels)
                {
                    ballList.Add(ball);

                }
                OnPropertyChanged(nameof(ballList));
                modelAPI.Start();
            }

        }

        public string InputNumber
        {
            get { return inputNumber; }
            set
            {
                inputNumber = value;
                OnPropertyChanged(nameof(InputNumber));
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName =  null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public int getInputValue()
        {
            if (Int32.TryParse(InputNumber, out int value) && InputNumber != "0")
            {
                return Int32.Parse(InputNumber);
            }

            return 0;
        }
    }
}