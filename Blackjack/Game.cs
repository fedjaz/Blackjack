using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    class Game
    {
        public List<Card> cards;
        public int bet;

        public Game(int bet, List<Card> cards)
        {
            this.cards = cards;
            this.bet = bet;
        }
        public void AddCart(Card card)
        {
            cards.Add(card);
        }

        public int GetScore()
        {
            List<int> scores = new List<int>();
            foreach (Card card in cards)
            {
                scores.Add((int)card.face);
            }
            scores.Sort();
            while(scores.Sum() > 21 && scores.Last() == 11)
            {
                scores[scores.Count - 1] = 1;
                scores.Sort();
            }
            return scores.Sum();
        }
    }
}
