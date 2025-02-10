using GameLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary
{
    public class GameLogic
    {
        public static void CreateBoard(PlayerModel player)
        {
            GridSpotCoordonate coordonate = new GridSpotCoordonate();

            for (int i = 0; i < player.OwnGrid.GridCount; i++)
            {
                for (int j = 0; j < player.OwnGrid.GridCount; j++)
                {
                    GridSpotModel gridSpot = new GridSpotModel();

                    gridSpot.SpotLetter = coordonate.letters[i];
                    gridSpot.SpotNumber = coordonate.numbers[j];
                    gridSpot.Status = GridSpotStatus.Empty;

                    player.OwnGrid.GridSpots.Add(gridSpot);
                }
            }

            for (int i = 0; i < player.TargetGrid.GridCount; i++)
            {
                for (int j = 0; j < player.TargetGrid.GridCount; j++)
                {
                    GridSpotModel gridSpot = new GridSpotModel();

                    gridSpot.SpotLetter = coordonate.letters[i];
                    gridSpot.SpotNumber = coordonate.numbers[j];
                    gridSpot.Status = GridSpotStatus.Empty;

                    player.TargetGrid.GridSpots.Add(gridSpot);
                }
            }
        }

        public static int DisplayStatistics(PlayerModel player)
        {
            int shotsTaken = 0;
            foreach (var Shot in player.TargetGrid.GridSpots)
            {
                if (Shot.Status != GridSpotStatus.Empty)
                {
                    shotsTaken++;
                }
            }
            return shotsTaken;
        }

        public static bool AddShip(PlayerModel player, char letter, char number)
        {
            foreach (var Spot in player.OwnGrid.GridSpots)
            {
                if (Spot.SpotLetter == letter.ToString() && Spot.SpotNumber == Int32.Parse(number.ToString()))
                {
                    if (Spot.Status == GridSpotStatus.Empty)
                    {
                        Spot.Status = GridSpotStatus.Ship;
                        return true;
                    }
                    break;
                }
            }
            return false;
        }

        public static bool ValidateSpot(char[] shipSpot, int GridCount)
        {
            if (shipSpot.Length != 2)
                return false;

            GridSpotCoordonate possibleCoordonate = new GridSpotCoordonate();

            bool validateLetter = false;
            bool validateNumber = false;
            try
            {
                for (int i = 0; i < GridCount; i++)
                {
                    if (shipSpot[0] == possibleCoordonate.letters[i].ToArray()[0])
                    {
                        validateLetter = true;
                        break;
                    }
                }
                for (int i = 0; i < GridCount; i++)
                {
                    if (Int32.Parse(shipSpot[1].ToString()) == possibleCoordonate.numbers[i])
                    {
                        validateNumber = true;
                        break;
                    }
                }
            }
            catch { }

            return validateLetter && validateNumber;
        }

        public static PlayerModel IsGameOver(PlayerModel player, PlayerModel opponent)
        {
            int playerSunkShips = 0;
            int opponentSunkShips = 0;
            for (int i = 0; i < player.OwnGrid.GridSpots.Count(); i++)
            {
                if (player.TargetGrid.GridSpots[i].Status == GridSpotStatus.Sunk)
                {
                    playerSunkShips++;
                    if (playerSunkShips == player.OwnGrid.GridCount) return player;

                }
                if (opponent.TargetGrid.GridSpots[i].Status == GridSpotStatus.Sunk)
                {
                    opponentSunkShips++;
                    if (opponentSunkShips == opponent.OwnGrid.GridCount) return opponent;
                }
            }
            return null;
        }
    }
}
