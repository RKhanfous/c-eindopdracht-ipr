using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public interface INetworkHandler
    {
        void TellGameStart(List<Player> players);
        void TellGameOver(List<Player> players);
        void TellNewTurn(Player currentPlayer, string currentWord, List<Player> players);
        void TellTurnOver(List<Player> players, string currentWord);
        void TellGameReset(List<Player> players);
        void TellAboutNewPlayer(List<Player> lists, Player player);
    }
}
