

namespace PPPAMI.Models
{
    public class DiceModel
    {
        private int NumberSides { get; set; }

        public DiceModel(int numSides)
        {
            NumberSides = numSides;
        }

        public int RollDice()
        {
            return new Random().Next(1, NumberSides + 1);
        }
    }
}

