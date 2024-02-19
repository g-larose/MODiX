using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Services.Features.Blackjack
{
    public class BlackjackSystem
    {
        private readonly int[] cardNumbers = [2, 3, 4, 5, 6, 7, 8, 9, 10, 11];
        private readonly string[] faceCards = ["Jack", "Queen", "King"];
        private readonly string[] cardSuits = ["Clubs", "Spades", "Hearts", "Diamonds"];

        public int SelectedNumber { get; set; }
        public Hand[]? UserSelectedCards { get; set; }
        public Hand[]? BotSelectedCards { get; set; }

        public BlackjackSystem()
        {

            UserSelectedCards = [GenerateCard(), GenerateCard(), GenerateCard(), GenerateCard(), GenerateCard()];

            BotSelectedCards  = [GenerateCard(), GenerateCard(), GenerateCard(), GenerateCard(), GenerateCard()];

        }

        private Hand GenerateCard()
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
            var card = new Hand();
            card.Card = $"{faceResult} of {cardSuits[suitIndex]}";
            card.Value = SelectedNumber;

            return card;
        }
    }
}
