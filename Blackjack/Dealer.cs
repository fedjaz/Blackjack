using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    class Dealer : Game
    {
        Player player;
        Queue<Card> deck;
        bool isHidden;
        public Dealer(List<Card> cards, Player player, Queue<Card> deck) : base(cards)
        {
            isHidden = true;
            this.deck = deck;
            this.player = player;
        }

        public void Play()
        {
            isHidden = false;
            int playerMaxScore = 0;
            foreach(Game game in player.games)
            {
                if(game.status == Statuses.Quit)
                {
                    playerMaxScore = Math.Max(playerMaxScore, game.GetScore());
                }
            }
            if(playerMaxScore == 0)
            {
                status = Statuses.Quit;
                return;
            }
            while (GetScore() < playerMaxScore && GetScore() < 17)
            {
                AddCart(deck.Dequeue());
            }
            if(GetScore() <= 21)
            {
                status = Statuses.Quit;
            }
            else
            {
                status = Statuses.TooMuch;
            }
        }

        public bool IsHidden()
        {
            return isHidden;
        }
        public override void AddCart(Card card)
        {
            base.AddCart(card);
        }
    }
}
