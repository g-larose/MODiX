using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Services.Features.Blackjack
{
    public class BlackjackSystem
    {
        private int[] cardNumbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
        private string[] faceCards = { "Jack", "Queen", "King" };
        private string[] cardSuits = { "Clubs", "Spades", "Hearts", "Diamonds" };

        public int SelectedNumber { get; set; }
        public string? UserSelectedCard { get; set; }
        public string? BotSelectedCard { get; set; }

        public BlackjackSystem()
        {

            UserSelectedCard = GenerateCard();
            BotSelectedCard = GenerateCard();
        }

        private string GenerateCard()
        {
            var rnd = new Random();
            int numIndex = rnd.Next(0, cardNumbers.Length - 1);
            int suitIndex = rnd.Next(0, cardSuits.Length - 1);

            SelectedNumber = cardNumbers[numIndex];
            var faceIndex = rnd.Next(0, faceCards.Length - 1);

            var faceResult = SelectedNumber switch
            {
                10 => faceCards[faceIndex],
                11 => "Ace",
                _ => SelectedNumber.ToString()
            };

            return $"{faceResult} of {cardSuits[suitIndex]}";
        }
    }
}
