using Microsoft.Maui.Controls;
using PPPAMI.ViewModels;

namespace PPPAMI.Views
{
    public partial class PersonagemView : ContentPage
    {
        public PersonagemView()
        {
            InitializeComponent();
        }

        private void NivelSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (BindingContext is PersonagemViewModel viewModel)
            {
                viewModel.NivelChangedCommand.Execute(e.NewValue);
            }
        }
    }
}
