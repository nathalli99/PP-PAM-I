using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace PPPAMI
{
    public partial class Personagem : ContentPage
    {
        private List<PersonagemModel> personagens = new List<PersonagemModel>();
        private List<PersonagemModel> equipe = new List<PersonagemModel>();

        public Personagem()
        {
            InitializeComponent();
            aventureirosListView.ItemsSource = personagens;
        }

        private void AdicionarPersonagem_Clicked(object sender, EventArgs e)
        {
            string nome = nomeEntry.Text.Trim();
            string classe = classePicker.SelectedItem as string;
            int nivel = (int)nivelSlider.Value;
            string raca = racaPicker.SelectedItem as string;

            if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(classe) || string.IsNullOrEmpty(raca))
            {
                DisplayAlert("Erro", "Por favor, preencha todos os campos corretamente.", "OK");
                return;
            }

            if (personagens.Exists(p => p.Nome == nome))
            {
                DisplayAlert("Erro", "Já existe um personagem com esse nome.", "OK");
                return;
            }

            var novoPersonagem = new PersonagemModel
            {
                Nome = nome,
                Classe = classe,
                Nivel = nivel,
                Raca = raca
            };

            personagens.Add(novoPersonagem);
            aventureirosListView.ItemsSource = null;
            aventureirosListView.ItemsSource = personagens;

            nomeEntry.Text = string.Empty;
            classePicker.SelectedItem = null;
            nivelSlider.Value = 1;
            racaPicker.SelectedItem = null;
        }

        private void NivelSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            nivelLabel.Text = $"Nível do Aventureiro: {Math.Round(e.NewValue)}";
        }

        private void OnAventureiroTapped(object sender, ItemTappedEventArgs e)
        {
            var personagemSelecionado = e.Item as PersonagemModel;

            if (equipe.Count >= 5)
            {
                DisplayAlert("Erro", "Já existem 5 aventureiros na sua equipe!", "OK");
                return;
            }

            if (!equipe.Contains(personagemSelecionado))
            {
                equipe.Add(personagemSelecionado);
                RenderizarEquipe();
            }
        }

        private void RenderizarEquipe()
        {
            equipeLayout.Children.Clear();

            foreach (var personagem in equipe)
            {
                StackLayout personagemLayout = new StackLayout
                {
                    Margin = new Thickness(0, 10),
                    Padding = new Thickness(10),
                };

                Label nomeLabel = new Label
                {
                    Text = $"Nome: {personagem.Nome}",
                    FontSize = 16,
                    Margin = new Thickness(0, 5)
                };

                Label classeLabel = new Label
                {
                    Text = $"Classe: {personagem.Classe}",
                    FontSize = 16,
                    Margin = new Thickness(0, 5)
                };

                Label nivelLabel = new Label
                {
                    Text = $"Nível: {personagem.Nivel}",
                    FontSize = 16,
                    Margin = new Thickness(0, 5)
                };

                Label racaLabel = new Label
                {
                    Text = $"Raça: {personagem.Raca}",
                    FontSize = 16,
                    Margin = new Thickness(0, 5)
                };

                personagemLayout.Children.Add(nomeLabel);
                personagemLayout.Children.Add(classeLabel);
                personagemLayout.Children.Add(nivelLabel);
                personagemLayout.Children.Add(racaLabel);

                equipeLayout.Children.Add(personagemLayout);
            }
        }
    }

    public class PersonagemModel
    {
        public string Nome { get; set; }
        public string Classe { get; set; }
        public int Nivel { get; set; }
        public string Raca { get; set; }
    }
}
