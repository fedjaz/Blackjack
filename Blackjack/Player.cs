using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    class Player
    {
        public int money;
        public List<Game> games;
        public int active_game_num;
        public Game active_game;
        bool hasActiveGames;

        public Player(int money)
        {
            this.money = money;
            games = new List<Game>();
            active_game_num = 0;
        }

        public void NewGame(Game game)
        {
            hasActiveGames = true;
            games.Add(game);
            if(games.Count == 1)
            {
                active_game = games[0];
                active_game_num = 0;
            }
        }
        
        public bool HasActiveGames()
        {
            return hasActiveGames;
        }

        public void NextGame()
        {
            if(active_game.GetScore() > 21)
            {
                active_game.status = Game.Statuses.TooMuch;
            }
            else
            {
                active_game.status = Game.Statuses.Quit;
            }
            if(active_game_num + 1 == games.Count)
            {
                hasActiveGames = false;
            }
            else
            {
                active_game_num++;
                active_game = games[active_game_num];
            }
        }
    }
}
