using Data;
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
        public int Width { get; } = 500;
        public int Height { get; } = 500;
        private bool _isStartEnabled { get; set; } = true;

        private bool _isStopEnabled { get; set; } = false;

        private string _inputNumber = "3";

        public ICommand OnClickStartButton { get; set; }

        public ICommand OnClickStopButton { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<BallModel> Balls { get; }

        private ModelAbstractAPI _modelAPI;

        public bool IsStartEnabled
        {
            get { return _isStartEnabled; }
            set
            {
                _isStartEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsStopEnabled
        {
            get { return _isStopEnabled; }
            set
            {
                _isStopEnabled = value;
                OnPropertyChanged();
            }
        }

        public ViewModelAPI()
        {
            OnClickStartButton = new RelayCommand(() => StartButtonHandle());
            OnClickStopButton = new RelayCommand(() => StopButtonHandle());
            _modelAPI = ModelAbstractAPI.CreateApi(Width, Height);
            Balls = _modelAPI.Balls;
        }

        public void StopButtonHandle()
        {
            _modelAPI.Stop();
            Balls.Clear();
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
                _modelAPI.AddBalls(value);
            }

        }

        public string InputNumber
        {
            get { return _inputNumber; }
            set
            {
                _inputNumber = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
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