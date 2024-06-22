using PPPAMI.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace PPPAMI.ViewModels
{
    public class PersonagemViewModel : INotifyPropertyChanged
    {
        private string _nome = string.Empty;
        private string _classe = string.Empty;
        private int _nivel = 1;
        private string _raca = string.Empty;
        private PersonagemModel _selectedPersonagem;

        public ObservableCollection<string> Classes { get; }
        public ObservableCollection<string> Racas { get; }
        public ObservableCollection<PersonagemModel> Personagens { get; }
        public ObservableCollection<PersonagemModel> Equipe { get; }

        public string Nome
        {
            get => _nome;
            set
            {
                if (_nome != value)
                {
                    _nome = value;
                    OnPropertyChanged(nameof(Nome));
                }
            }
        }

        public string Classe
        {
            get => _classe;
            set
            {
                if (_classe != value)
                {
                    _classe = value;
                    OnPropertyChanged(nameof(Classe));
                }
            }
        }

        public int Nivel
        {
            get => _nivel;
            set
            {
                if (_nivel != value)
                {
                    _nivel = value;
                    OnPropertyChanged(nameof(Nivel));
                }
            }
        }

        public string Raca
        {
            get => _raca;
            set
            {
                if (_raca != value)
                {
                    _raca = value;
                    OnPropertyChanged(nameof(Raca));
                }
            }
        }

        public PersonagemModel SelectedPersonagem
        {
            get => _selectedPersonagem;
            set
            {
                if (_selectedPersonagem != value)
                {
                    _selectedPersonagem = value;
                    OnPropertyChanged(nameof(SelectedPersonagem));
                    OnPersonagemTapped();
                }
            }
        }

        public ICommand AddPersonagemCommand { get; }
        public ICommand NivelChangedCommand { get; }

        public PersonagemViewModel()
        {
            Classes = new ObservableCollection<string> { "Mago", "Assassino", "Lutador", "Suporte" };
            Racas = new ObservableCollection<string> { "Humano", "Elfo", "Anão" };
            Personagens = new ObservableCollection<PersonagemModel>();
            Equipe = new ObservableCollection<PersonagemModel>();

            AddPersonagemCommand = new Command(AddPersonagem);
            NivelChangedCommand = new Command<double>(OnNivelChanged);
        }

        private void AddPersonagem()
        {
            if (string.IsNullOrEmpty(Nome) || string.IsNullOrEmpty(Classe) || string.IsNullOrEmpty(Raca))
            {
                Application.Current.MainPage.DisplayAlert("Aviso", "Preencha todos os campos corretamente.", "OK");
                return;
            }

            if (Personagens.Any(p => p.Nome == Nome))
            {
                Application.Current.MainPage.DisplayAlert("Aviso", "Já existe um personagem com esse nome.", "OK");
                return;
            }

            var novoPersonagem = new PersonagemModel
            {
                Nome = Nome,
                Classe = Classe,
                Nivel = Nivel,
                Raca = Raca
            };

            Personagens.Add(novoPersonagem);
            Nome = string.Empty;
            Classe = string.Empty;
            Nivel = 1;
            Raca = string.Empty;
        }

        private void OnNivelChanged(double nivel)
        {
            Nivel = (int)nivel;
        }

        private void OnPersonagemTapped()
        {
            if (Equipe.Count >= 5)
            {
                Application.Current.MainPage.DisplayAlert("Aviso", "Já existem 5 aventureiros na sua equipe!", "OK");
                return;
            }

            if (SelectedPersonagem != null && !Equipe.Contains(SelectedPersonagem))
            {
                Equipe.Add(SelectedPersonagem);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
