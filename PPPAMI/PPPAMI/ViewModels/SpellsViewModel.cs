using PPPAMI.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using System.Windows.Input;

namespace PPPAMI.ViewModels
{
    public class SpellsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<SpellModel> spells;
        public ObservableCollection<SpellModel> Spells
        {
            get => spells;
            set
            {
                spells = value;
                OnPropertyChanged(nameof(Spells));
            }
        }

        private ObservableCollection<SpellDetailModel> spellDetails;
        public ObservableCollection<SpellDetailModel> SpellDetails
        {
            get => spellDetails;
            set
            {
                spellDetails = value;
                OnPropertyChanged(nameof(SpellDetails));
            }
        }

        private string selectedLevel;
        public string SelectedLevel
        {
            get => selectedLevel;
            set
            {
                selectedLevel = value;
                OnPropertyChanged(nameof(SelectedLevel));
            }
        }

        private string selectedSchool;
        public string SelectedSchool
        {
            get => selectedSchool;
            set
            {
                selectedSchool = value;
                OnPropertyChanged(nameof(SelectedSchool));
            }
        }

        private SpellModel selectedSpell;
        public SpellModel SelectedSpell
        {
            get => selectedSpell;
            set
            {
                selectedSpell = value;
                OnPropertyChanged(nameof(SelectedSpell));
                if (selectedSpell != null)
                {
                    FetchSpellDetailsAsync(selectedSpell.Index);
                }
            }
        }

        public ObservableCollection<string> Levels { get; set; }
        public ObservableCollection<string> Schools { get; set; }

        public ICommand FilterSpellsCommand { get; }

        public SpellsViewModel()
        {
            spells = new ObservableCollection<SpellModel>();
            spellDetails = new ObservableCollection<SpellDetailModel>();
            Levels = new ObservableCollection<string>
            {
                "1",
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
                "8",
                "9"
            };
            Schools = new ObservableCollection<string>
            {
                "Abjuration",
                "Conjuration",
                "Divination",
                "Enchantment",
                "Evocation",
                "Illusion",
                "Necromancy",
                "Transmutation"
            };

            FilterSpellsCommand = new Command(async () => await FetchSpellsAsync());

            FetchSpellsAsync();
        }

        private async Task FetchSpellsAsync()
        {
            var client = new HttpClient();
            string url = "https://www.dnd5eapi.co/api/spells";

            if (!string.IsNullOrEmpty(SelectedLevel) || !string.IsNullOrEmpty(SelectedSchool))
            {
                url += "?";
                if (!string.IsNullOrEmpty(SelectedLevel))
                {
                    url += $"level={SelectedLevel}&";
                }
                if (!string.IsNullOrEmpty(SelectedSchool))
                {
                    url += $"school={SelectedSchool}&";
                }
                url = url.TrimEnd('&');
            }

            Debug.WriteLine($"Constructed URL: {url}");

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/json");

            try
            {
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var spellList = JsonSerializer.Deserialize<SpellListResponse>(content);

                Debug.WriteLine($"Spells retrieved: {spellList?.Results?.Count ?? 0}");

                if (spellList?.Results != null && spellList.Results.Count > 0)
                {
                    Spells.Clear();
                    foreach (var spell in spellList.Results)
                    {
                        Spells.Add(spell);
                    }
                }
                else
                {
                    Spells.Clear();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching spells: {ex.Message}");
            }
        }

        private async Task FetchSpellDetailsAsync(string spellIndex)
        {
            var client = new HttpClient();
            string url = $"https://www.dnd5eapi.co/api/spells/{spellIndex}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/json");

            try
            {
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var spellDetail = JsonSerializer.Deserialize<SpellDetailModel>(content);

                SpellDetails.Clear();
                SpellDetails.Add(spellDetail);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching spell details: {ex.Message}");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class SpellListResponse
    {
        public List<SpellModel> Results { get; set; }
    }

    public class SpellDetailModel
    {
        public string Index { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
