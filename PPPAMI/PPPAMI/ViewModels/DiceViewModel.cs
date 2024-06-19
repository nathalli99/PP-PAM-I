using PPPAMI.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace PPPAMI.ViewModels
{
    public class DiceViewModel : INotifyPropertyChanged
    {
        private int _selectedSides;
        private int _selectedQuantity;
        private string _resultString = string.Empty;

        public ObservableCollection<int> Sides { get; }
        public ObservableCollection<int> Quantities { get; }

        public int SelectedSides
        {
            get => _selectedSides;
            set
            {
                if (_selectedSides != value)
                {
                    _selectedSides = value;
                    OnPropertyChanged(nameof(SelectedSides));
                }
            }
        }

        public int SelectedQuantity
        {
            get => _selectedQuantity;
            set
            {
                if (_selectedQuantity != value)
                {
                    _selectedQuantity = value;
                    OnPropertyChanged(nameof(SelectedQuantity));
                }
            }
        }

        public string ResultString
        {
            get => _resultString;
            set
            {
                if (_resultString != value)
                {
                    _resultString = value;
                    OnPropertyChanged(nameof(ResultString));
                }
            }
        }

        public ICommand RollDiceCommand { get; }

        public DiceViewModel()
        {
            Sides = new ObservableCollection<int> { 4, 6, 10, 20, 100 };
            Quantities = new ObservableCollection<int> { 1, 2, 3, 4, 5, 6 };

            RollDiceCommand = new Command(OnRollDice);
        }

        private void OnRollDice()
        {
            try
            {
                DiceModel dice = new DiceModel(SelectedSides);

                string result = "";
                int total = 0;

                for (int i = 0; i < SelectedQuantity; i++)
                {
                    int roll = dice.RollDice();
                    total += roll;
                    result += $"Dado {i + 1} = {roll}\n";
                }

                ResultString = $"Resultado:\n{result}\nTotal: {total}";
            }
            catch
            {
                Application.Current.MainPage.DisplayAlert("Aviso", "Escolha a quantidade de dados e lados", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
