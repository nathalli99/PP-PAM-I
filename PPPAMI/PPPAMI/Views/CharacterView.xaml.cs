using Microsoft.Maui.Controls;

namespace PPPAMI.Views
{
    public partial class CharacterView : ContentPage
    {
        public CharacterView()
        {
            InitializeComponent();
        }

        private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            levelLabel.Text = $"N�vel: {Math.Round(e.NewValue)}";
        }
    }
}
