using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPProjectBattleship2
{
    class EditDatabase
    {
    }
    partial class Players
    {
        public Players(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }
    }
    partial class Games
    { 
        public Games(int iD, string title, string creatorFK, string opponentFK, bool? complete, ICollection<Attacks> attacks, Players players, ICollection<GameShipConfigurations> gameShipConfigurations, Players players1)
        {
            this.ID = iD;
            this.Title = title;
            this.CreatorFK = creatorFK;
            this.OpponentFK = opponentFK;
            this.Complete = complete;
            this.Attacks = attacks;
            this.Players = players;
            this.GameShipConfigurations = gameShipConfigurations;
            this.Players1 = players1;
        }
    }
    partial class Attacks
    {
        public Attacks() { }
        public Attacks(int iD, string coordinate, bool hit, int? gameFK, Games games)
        {
            this.ID = iD;
            this.Coordinate = coordinate;
            this.Hit = hit;
            this.GameFK = gameFK;
            this.Games = games;
        }
    }
    partial class Ships
    {
        public Ships() { }
        public Ships(int iD, string title, int size)
        {
            this.ID = iD;
            this.Title = title;
            this.Size = size;
        }
    }
    partial class GameShipConfigurations
    { 
        public GameShipConfigurations() { }
        public GameShipConfigurations(int ID, string PlayerFK, int GameFK, string Coordinate, Games games, Players players) 
        {
            this.ID = ID;
            this.PlayerFK = PlayerFK;
            this.GameFK = GameFK;
            this.Players = players;
            this.Coordinate = Coordinate;
            this.Games = Games;
        }
    }

    }

