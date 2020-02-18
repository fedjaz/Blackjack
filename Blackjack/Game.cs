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
        public enum Statuses
        {
            Quit,
            TooMuch,
            Active
        }
        public Statuses status;
        public Game(List<Card> cards)
        {
            this.cards = cards;
            status = Statuses.Active;
        }
        public Game(int bet, List<Card> cards)
        {
            this.cards = cards;
            this.bet = bet;
            status = Statuses.Active;
        }
        public virtual void AddCart(Card card)
        {
            cards.Add(card);
            if(GetScore() > 21)
            {
                status = Statuses.TooMuch;
            }
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
