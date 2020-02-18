using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    class Program
    {
        enum Actions
        {
            Hit = 1,
            Double = 2,
            Quit = 3,
            Split = 4
        }
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Нажмите любую кнопку, чтобы начать игру");
            Console.ReadKey();

            Player player = new Player(1000);
            while (true)
            {
                Queue<Card> deck = new Queue<Card>(Card.RandomDeck());
                Console.Clear();
                Console.WriteLine("Ваша ставка(денег на счету: " + player.money+"): ");
                int bet;
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out bet) && bet > 0)
                    {
                        if(bet > player.money)
                        {
                            Console.WriteLine("У вас не хватает денег для такой ставки, попробуйте еще раз");

                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Вы ввели некорректное число");
                    }
                }
                
                player.money -= bet;
                player.NewGame(new Game(bet, new List<Card> { deck.Dequeue(), deck.Dequeue() }));
                while (player.HasActiveGames())
                {
                    Game game = player.active_game;
                    PrintUserInformation(player);
                    Actions action = GetAction(game);
                    if(action == Actions.Hit)
                    {
                        game.AddCart(deck.Dequeue());
                        PrintUserInformation(player);
                        if(game.status == Game.Statuses.TooMuch)
                        {
                            player.NextGame();
                        }
                    }
                    if(action == Actions.Quit)
                    {
                        player.NextGame();
                    }
                    if(action == Actions.Double)
                    {
                        player.money -= game.bet;
                        game.bet *= 2;
                        game.AddCart(deck.Dequeue());
                        player.NextGame();
                    }
                    
                }

                player.games = new List<Game>();
                Console.ReadKey();
            }
        }

        static void PrintUserInformation(Player player)
        {
            Console.Clear();
            Console.WriteLine("Деньги: " + player.money);
            for (int j = 0; j < player.games.Count; j++)
            {
                Console.WriteLine("Рука #" + (j + 1) + ":");
                Game curGame = player.games[j];
                for (int i = 0; i < curGame.cards.Count; i++)
                {
                    string symbol = "";
                    Card card = curGame.cards[i];
                    if ((int)card.face < 10)
                    {
                        symbol = ((int)card.face).ToString();
                    }
                    else if ((int)card.face < 11)
                    {
                        if (card.face == Card.Faces.Jack)
                            symbol = "J";
                        if (card.face == Card.Faces.Queen)
                            symbol = "Q";
                        if (card.face == Card.Faces.King)
                            symbol = "K";
                    }
                    else
                    {
                        symbol = "A";
                    }
                    Console.Write(symbol + "  ");
                }
                Console.WriteLine();
                for (int i = 0; i < curGame.cards.Count; i++)
                {
                    string symbol = "";
                    Card card = curGame.cards[i];
                    switch (card.suit)
                    {
                        case Card.Suits.Diamonds:
                            symbol = "♦";
                            break;
                        case Card.Suits.Clubs:
                            symbol = "♣";
                            break;
                        case Card.Suits.Hearts:
                            symbol = "♥";
                            break;
                        case Card.Suits.Spades:
                            symbol = "♠";
                            break;
                    }
                    Console.Write(symbol + "  ");
                }
                Console.WriteLine();
                Console.WriteLine("Ваши очки: " + player.active_game.GetScore());
                if(curGame.status == Game.Statuses.TooMuch)
                {
                    Console.WriteLine("Перебор!");
                }
            }
        }

        static Actions GetAction(Game game)
        {
            Console.WriteLine("Действия");
            List<Card> cards = game.cards;
            bool canSplit = cards.Count == 2 && cards[0].face == cards[1].face;
            Console.WriteLine("1. Взять еще 1 карту");
            Console.WriteLine("2. Удвоить ставку и взять последнюю карту");
            Console.WriteLine("3. Завершить");
            if(canSplit)
            {
                Console.WriteLine("4. Разделить");
            }
            while (true)
            {
                int response;
                if(!int.TryParse(Console.ReadLine(), out response))
                {
                    Console.WriteLine("Вы ввели некорректный вариант");
                    continue;
                }
                if (response < 4 && response > 0)
                {
                    return (Actions)response;
                }
                else if (response == 4 && canSplit)
                {
                    return Actions.Split;
                }
                else
                {
                    Console.WriteLine("Вы ввели некорректный вариант");
                }
            }
        }
    }
}
