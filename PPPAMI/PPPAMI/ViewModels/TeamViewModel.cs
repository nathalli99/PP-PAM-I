using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using PPPAMI.Models;

namespace PPPAMI.ViewModels
{
    public class TeamViewModel : INotifyPropertyChanged
    {
        private Character selectedCharacter;
        private string errorMessage;
        private bool isAddButtonEnabled = true;

        public TeamViewModel()
        {
            AvailableCharacters = new ObservableCollection<Character>();
            Team = new Team();
            AddCharacterCommand = new Command(AddCharacterToTeam, () => IsAddButtonEnabled);
            RemoveCharacterCommand = new Command<Character>(RemoveCharacterFromTeam);

            AvailableCharacters.Add(new Character { Name = "Lux", Class = "Mago", Level = 8, Race = "Humano" });
            AvailableCharacters.Add(new Character { Name = "Zed", Class = "Assassino", Level = 3, Race = "Elfo" });
            AvailableCharacters.Add(new Character { Name = "Garen", Class = "Lutador", Level = 7, Race = "Humano" });
            AvailableCharacters.Add(new Character { Name = "Milio", Class = "Suporte", Level = 5, Race = "Elfo" });
            AvailableCharacters.Add(new Character { Name = "Teemo", Class = "Mago", Level = 9, Race = "Anão" });
            AvailableCharacters.Add(new Character { Name = "Irelia", Class = "Lutador", Level = 7, Race = "Elfo" });
        }

        public ObservableCollection<Character> AvailableCharacters { get; set; }
        public Team Team { get; }
        public Character SelectedCharacter
        {
            get => selectedCharacter;
            set
            {
                selectedCharacter = value;
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
        public bool IsAddButtonEnabled
        {
            get => isAddButtonEnabled;
            set
            {
                isAddButtonEnabled = value;
                OnPropertyChanged();
            }
        }
        public ICommand AddCharacterCommand { get; }
        public ICommand RemoveCharacterCommand { get; }

        private void AddCharacterToTeam()
        {
            if (Team.TeamMembers.Count >= 5)
            {
                ErrorMessage = "Já existem 5 aventureiros na sua equipe.";
                Application.Current.MainPage.DisplayAlert("Erro", ErrorMessage, "OK");
                return;
            }

            if (SelectedCharacter != null && !Team.TeamMembers.Contains(SelectedCharacter))
            {
                Team.TeamMembers.Add(SelectedCharacter);
                SelectedCharacter = null;
                ErrorMessage = string.Empty;
                if (Team.TeamMembers.Count >= 5)
                {
                    IsAddButtonEnabled = false;
                }
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Erro", "Selecione um aventureiro para adicionar à equipe.", "OK");
            }
        }

        private void RemoveCharacterFromTeam(Character character)
        {
            if (character != null && Team.TeamMembers.Contains(character))
            {
                Team.TeamMembers.Remove(character);
                IsAddButtonEnabled = true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
