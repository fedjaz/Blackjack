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
        public Dealer(List<Card> cards, Player player, Queue<Card> deck) : base(cards)
        {
            this.deck = deck;
            this.player = player;
        }

        public void Play()
        {
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
                return;
            }
            while (GetScore() < playerMaxScore && GetScore() < 17)
            {
                AddCart(deck.Dequeue());
            }
            
        }

        public override void AddCart(Card card)
        {
            base.AddCart(card);
        }
    }
}
