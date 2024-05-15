namespace PPPAMI;

public partial class Dice : ContentPage
{
	public Dice()
	{
		InitializeComponent();
	}

    public class DiceRoller
    {
        private int NumberSides { get; set; }

        public DiceRoller() { }
        public DiceRoller(int numSides)
        {
            NumberSides = numSides;
        }

        public int RollDice()
        {
            return new Random().Next(1, NumberSides + 1);
        }
    }

    public void OnClicked(object sender, EventArgs e)
    {
        try
        {
            int numberSides = (int)SelectorRolleSide.SelectedItem;
            int quantity = (int)SelectorSide.SelectedItem;

            DiceRoller dice = new DiceRoller(numberSides);

            string resultString = "";
            int total = 0;

            for (int i = 0; i < quantity; i++)
            {
                int roll = dice.RollDice();
                total += roll;
                resultString += $"Dado {i + 1} = {roll}\n";

                if (i < quantity - 1)
                    resultString += "\n";
            }

            RollInfoLabel.Text = $"Foram jogados {quantity} dado(s) de {numberSides} lados.";
            RandomNumber.Text = resultString;
            RandomNumber.Text = $"Resultado:\n\n{resultString}\n" +
                                $"Total: {total}";

        }
        catch
        {
            DisplayAlert("Alert", "Escolha a quantidade de dados/lados", "OK");
        }
    }

}