using PPPAMI.ViewModels;

namespace PPPAMI.Views
{
    public partial class SpellsView : ContentPage
    {
        public SpellsView()
        {
            InitializeComponent();
            BindingContext = new SpellsViewModel();
        }
    }
}
