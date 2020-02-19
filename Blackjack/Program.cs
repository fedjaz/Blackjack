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
                Dealer dealer = new Dealer(new List<Card>() { deck.Dequeue(), deck.Dequeue()}, player, deck);
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
                    PrintDealerInformation(dealer);
                    Actions action = GetAction(player, game);
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
                    if(action == Actions.Split)
                    {
                        player.NewGame(new Game(new List<Card> { game.cards[1] }));
                        game.cards.RemoveAt(1);
                    }
                }
                dealer.Play();
                PrintUserInformation(player);
                PrintDealerInformation(dealer);
                Console.WriteLine("Итог раунда:");
                for (int i = 0; i < player.games.Count; i++)
                {
                    Game game = player.games[i];
                    int score = game.GetScore(), dealerScore = dealer.GetScore();
                    Console.WriteLine("Рука #" + (i + 1) + ":");
                    if(game.status == Game.Statuses.Quit && dealer.status != Game.Statuses.TooMuch)
                    {
                        if(score > dealerScore)
                        {
                            Console.WriteLine("У вас больше, вы выиграли!");
                            player.money += game.bet * 2;
                        }
                        else if(score < dealerScore)
                        {
                            Console.WriteLine("У вас меньше, вы проиграли!");
                        }
                        else
                        {
                            Console.WriteLine("Ничья!");
                            player.money += game.bet;
                        }
                    }
                    if(game.status == Game.Statuses.Quit && dealer.status == Game.Statuses.TooMuch)
                    {
                        Console.WriteLine("У дилера перебор, вы выиграли!");
                        player.money += game.bet * 2;
                    }
                    if (game.status == Game.Statuses.TooMuch && dealer.status == Game.Statuses.Quit)
                    {
                        Console.WriteLine("У вас перебор!");
                    }
                    if(game.status == Game.Statuses.TooMuch && dealer.status == Game.Statuses.TooMuch)
                    {
                        Console.WriteLine("У вас и у дилера перебор, ничья!");
                        player.money += game.bet;
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
                Console.Write("Рука #" + (j + 1));
                if(j == player.active_game_num && player.HasActiveGames())
                {
                    Console.WriteLine("(текущая):");
                }
                else
                {
                    Console.WriteLine(":");
                }
                    
                Game curGame = player.games[j];
                PrintCards(curGame.cards, 0);
                Console.WriteLine("Ваши очки: " + player.active_game.GetScore());
                if(curGame.status == Game.Statuses.TooMuch)
                {
                    Console.WriteLine("Перебор!");
                }
            }
        }

        static void PrintDealerInformation(Dealer dealer)
        {
            Console.WriteLine("Карты дилера:");
            PrintCards(dealer.cards, (dealer.IsHidden() ? 1 : 0));
            Console.WriteLine("Очки дилера: " + (dealer.IsHidden() ? "x" : dealer.GetScore().ToString()));
        }

        static Actions GetAction(Player player, Game game)
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
                if (response == 1 || response == 3)
                {
                    return (Actions)response;
                }
                if(response == 2)
                {
                    if(player.money < game.bet)
                    {
                        Console.WriteLine("У вас не хватает денег!");
                        continue;
                    }
                    return Actions.Double;
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

        static void PrintCards(List<Card> cards, int hidden)
        {
            for (int i = 0; i < cards.Count - hidden; i++)
            {
                string symbol = "";
                Card card = cards[i];
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
            for (int i = 0; i < hidden; i++)
            {
                Console.Write("x  ");
            }
            Console.WriteLine();
            for (int i = 0; i < cards.Count - hidden; i++)
            {
                string symbol = "";
                Card card = cards[i];
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
            for (int i = 0; i < hidden; i++)
            {
                Console.Write("x  ");
            }
            Console.WriteLine();
        }
    }
}
