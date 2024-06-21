using PPPAMI.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using System.Windows.Input;

namespace PPPAMI.ViewModels
{
    public class SpellsViewModel : INotifyPropertyChanged
    {
        private string _selectedLevel;
        private string _selectedSchool;

        public ObservableCollection<string> Levels { get; }
        public ObservableCollection<string> Schools { get; }
        public ObservableCollection<SpellModel> Spells { get; }

        public string SelectedLevel
        {
            get => _selectedLevel;
            set
            {
                if (_selectedLevel != value)
                {
                    _selectedLevel = value;
                    OnPropertyChanged(nameof(SelectedLevel));
                }
            }
        }

        public string SelectedSchool
        {
            get => _selectedSchool;
            set
            {
                if (_selectedSchool != value)
                {
                    _selectedSchool = value;
                    OnPropertyChanged(nameof(SelectedSchool));
                }
            }
        }

        public ICommand FilterSpellsCommand { get; }

        public SpellsViewModel()
        {
            Levels = new ObservableCollection<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            Schools = new ObservableCollection<string> { "abjuration", "conjuration", "divination", "enchantment", "evocation", "illusion", "necromancy", "transmutation" };
            Spells = new ObservableCollection<SpellModel>();

            FilterSpellsCommand = new Command(async () => await FilterSpells());

            // Carregar todas as magias ao iniciar
            _ = LoadAllSpells();
        }

        private async Task LoadAllSpells()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = "https://www.dnd5eapi.co/api/spells";
                    var response = await client.GetStringAsync(url);
                    var spells = JsonSerializer.Deserialize<SpellApiResponse>(response);

                    if (spells?.Results != null)
                    {
                        Spells.Clear();
                        foreach (var spell in spells.Results)
                        {
                            var spellDetail = await client.GetStringAsync($"https://www.dnd5eapi.co{spell.Url}");
                            var spellDetailObj = JsonSerializer.Deserialize<SpellDetail>(spellDetail);

                            if (spellDetailObj != null)
                            {
                                Spells.Add(new SpellModel
                                {
                                    Name = spellDetailObj.Name,
                                    Level = spellDetailObj.Level.ToString(),
                                    School = spellDetailObj.School.Name
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., logging, user notification)
                System.Diagnostics.Debug.WriteLine($"Error fetching spells: {ex.Message}");
            }
        }

        private async Task FilterSpells()
        {
            if (string.IsNullOrEmpty(SelectedLevel) || string.IsNullOrEmpty(SelectedSchool))
            {
                // Handle case where no level or school is selected
                return;
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = $"https://www.dnd5eapi.co/api/spells?level={SelectedLevel}&school={SelectedSchool}";
                    var response = await client.GetStringAsync(url);
                    var spells = JsonSerializer.Deserialize<SpellApiResponse>(response);

                    if (spells?.Results != null)
                    {
                        Spells.Clear();
                        foreach (var spell in spells.Results)
                        {
                            var spellDetail = await client.GetStringAsync($"https://www.dnd5eapi.co{spell.Url}");
                            var spellDetailObj = JsonSerializer.Deserialize<SpellDetail>(spellDetail);

                            if (spellDetailObj != null)
                            {
                                Spells.Add(new SpellModel
                                {
                                    Name = spellDetailObj.Name,
                                    Level = spellDetailObj.Level.ToString(),
                                    School = spellDetailObj.School.Name
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., logging, user notification)
                System.Diagnostics.Debug.WriteLine($"Error fetching spells: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private class SpellApiResponse
        {
            public List<SpellResult>? Results { get; set; }
        }

        private class SpellResult
        {
            public string Url { get; set; } = string.Empty;
        }

        private class SpellDetail
        {
            public string Name { get; set; } = string.Empty;
            public int Level { get; set; }
            public School? School { get; set; }
        }

        private class School
        {
            public string Name { get; set; } = string.Empty;
        }
    }
}


