using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PPPAMI.Models;

namespace PPPAMI
{
    public class CharacterViewModel : INotifyPropertyChanged
    {
        private string name;
        private string selectedClass;
        private int level;
        private string selectedRace;
        private string errorMessage;

        public ObservableCollection<Character> Characters { get; set; }

        public CharacterViewModel()
        {
            Characters = new ObservableCollection<Character>();
            AddCharacterCommand = new Command(AddCharacter);
            Level = 1;
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public string SelectedClass
        {
            get => selectedClass;
            set
            {
                selectedClass = value;
                OnPropertyChanged();
            }
        }

        public int Level
        {
            get => level;
            set
            {
                level = value;
                OnPropertyChanged();
            }
        }

        public string SelectedRace
        {
            get => selectedRace;
            set
            {
                selectedRace = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCharacterCommand { get; }

        private void AddCharacter()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                ErrorMessage = "O nome do personagem não pode estar vazio.";
                return;
            }

            if (Characters.Any(c => c.Name == Name))
            {
                ErrorMessage = "Já existe um personagem com este nome.";
                return;
            }

            Characters.Add(new Character { Name = Name, Class = SelectedClass, Level = Level, Race = SelectedRace });
            Name = string.Empty;
            SelectedClass = string.Empty;
            Level = 1;
            SelectedRace = string.Empty;
            ErrorMessage = string.Empty;
        }

        public List<string> Classes { get; } = new List<string> { "Mago", "Assassino", "Lutador", "Suporte" };
        public List<string> Races { get; } = new List<string> { "Humano", "Elfo", "Anão" };

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
