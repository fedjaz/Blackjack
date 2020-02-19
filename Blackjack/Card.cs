using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    class Card
    {
        public enum Suits
        {
            Hearts,
            Diamonds,
            Clubs, 
            Spades
        }
        public enum Faces
        {
            Deuces = 2,
            Treys = 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8,
            Nine = 9,
            Ten = 10,
            Jack = 11,
            Queen = 12,
            King = 13,
            Ace = 14
        }
        public Suits suit;
        public Faces face;

        public Card(Suits suit, Faces face)
        {
            this.suit = suit;
            this.face = face;
        }

        public static List<Card> RandomDeck()
        {
            List<Card> deck = new List<Card>();
            foreach(Suits suit in Enum.GetValues(typeof(Suits)))
            {
                foreach(Faces face in Enum.GetValues(typeof(Faces)))
                {
                    deck.Add(new Card(suit, face));
                }
            }
            deck.Shuffle();
            return deck;
        }

        
    }

    static class Extensions
    {
        public static void Shuffle(this List<Card> cards)
        {
            Random r = new Random();
            for (int i = 0; i < 1000; i++)
            {
                int a = r.Next(0, cards.Count);
                int b = r.Next(0, cards.Count);
                Card c = cards[a];
                cards[a] = cards[b];
                cards[b] = c;
            }
        }
    }
}
