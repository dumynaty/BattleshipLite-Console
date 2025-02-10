using GameLibrary.Models;
using GameLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;
using System.ComponentModel;

namespace BattleshipLite
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Game introduction
            GameIntro();
            // Create Board
            int boardSize = ChooseBoardSize();


            // Create Player1: Naming, Ship locations
            PlayerModel player = new PlayerModel();
            InitializePlayer(player, boardSize, "Player1");
            PlayerModel opponent = new PlayerModel();
            InitializePlayer(opponent, boardSize, "Player2");

            // Play Game
            PlayGame(player, opponent);

            // Identify Winner and Display Statistics
            EndGame(player, opponent);

            Console.ReadLine();
        }

        

        private static void EndGame(PlayerModel player, PlayerModel opponent)
        {
            var winner = GameLogic.IsGameOver(player, opponent);
            Console.WriteLine();
            Console.WriteLine("GAME OVER!!!");
            Console.WriteLine($"Congratulations, { winner.Name } for winning the game!");
            Console.WriteLine($"You took { GameLogic.DisplayStatistics(winner) } shots to destroy all of { opponent.Name }'s Ships!");
        }

        private static void PlayGame(PlayerModel player, PlayerModel opponent)
        {
            do
            {
                Console.Clear();
                Console.WriteLine($"{player.Name}'s Grid:");
                DisplayGrid(player.OwnGrid);
                Console.WriteLine();
                Console.WriteLine("Opponent's Grid:");
                DisplayGrid(player.TargetGrid);

                if (AttackTurn(player, opponent) == true)
                {
                    var temporaryPosition = player;
                    player = opponent;
                    opponent = temporaryPosition;
                    Console.ReadLine();
                }
            }
            while (GameLogic.IsGameOver(player, opponent) == null);
        }

        private static bool AttackTurn(PlayerModel player, PlayerModel opponent)
        {
            bool attacked = false;
            while (attacked == false)
            {
                Console.Write($"{ player.Name }, choose an attack spot: ");
                char[] attackSpot = Console.ReadLine().ToUpper().ToArray();
                if (GameLogic.ValidateSpot(attackSpot, player.OwnGrid.GridCount) == true)
                {
                    for (int i = 0; i < opponent.OwnGrid.GridSpots.Count(); i++)
                    {
                        if (opponent.OwnGrid.GridSpots[i].SpotLetter == attackSpot[0].ToString()
                            && opponent.OwnGrid.GridSpots[i].SpotNumber == Int32.Parse(attackSpot[1].ToString()))
                        {
                            if (opponent.OwnGrid.GridSpots[i].Status == GridSpotStatus.Empty)
                            {
                                player.TargetGrid.GridSpots[i].Status = GridSpotStatus.Miss;
                                opponent.OwnGrid.GridSpots[i].Status = GridSpotStatus.Miss;
                                attacked = true;
                                Console.WriteLine($"Attack on {attackSpot[0]}{attackSpot[1]} is a Miss!");
                            }
                            else if (opponent.OwnGrid.GridSpots[i].Status == GridSpotStatus.Ship)
                            {
                                player.TargetGrid.GridSpots[i].Status = GridSpotStatus.Sunk;
                                opponent.OwnGrid.GridSpots[i].Status = GridSpotStatus.Sunk;
                                attacked = true;
                                Console.WriteLine($"Attack on {attackSpot[0]}{attackSpot[1]} is a Hit!");
                            }
                            else
                            {
                                Console.WriteLine("You already attacked there!");
                            }
                            break;
                        }
                    }

                }
            }
            return attacked;
        }

        private static void AskUsersName(PlayerModel player, string playerNumber)
        {
            Console.Write($"{playerNumber}, please enter your name: ");
            player.Name = Console.ReadLine();
        }

        private static void InitializePlayer(PlayerModel player, int boardSize, string playerNumber)
        {
            AskUsersName(player, playerNumber);
            player.OwnGrid.GridCount = boardSize;
            player.TargetGrid.GridCount = boardSize;
            GameLogic.CreateBoard(player);
            PlaceUsersShips(player);
        }

        private static void PlaceUsersShips(PlayerModel player)
        {
            Console.Clear();
            Console.WriteLine($"{ player.Name }, choose your battleships locations!");

            // Apply ship location
            int placedShips = 0;
            while (placedShips != player.OwnGrid.GridCount)
            {
                DisplayGrid(player.OwnGrid);
                Console.Write($"Place ship { placedShips + 1 } on spot: ");
                char[] shipSpot = Console.ReadLine().ToUpper().ToArray();
                Console.WriteLine();

                if (GameLogic.ValidateSpot(shipSpot, player.OwnGrid.GridCount) == true)
                {
                    if (GameLogic.AddShip(player, shipSpot[0], shipSpot[1]) == true)
                    {
                        placedShips++;
                    }
                }
            }
            Console.Clear();
        }

        private static void DisplayGrid(BoardModel Grid)
        {
            GridSpotCoordonate coordonates = new GridSpotCoordonate();

            Console.Write("  ");
            for (int i = 0; i < Grid.GridCount; i++)
            {
                Console.Write($" { coordonates.numbers[i] } ");
            }
            Console.WriteLine();


            int counter = 0;
            int lineCounter = 0;
            foreach (var Spot in Grid.GridSpots)
            {
                if (counter == Grid.GridCount)
                {
                    Console.WriteLine();
                    counter = 0;
                }
                if (counter == 0)
                {
                    Console.Write($" {coordonates.letters[lineCounter]}");
                    lineCounter++;
                }
                
                counter++;

                if (Spot.Status == GridSpotStatus.Empty)
                {
                    Console.Write(" - ");
                }
                else if (Spot.Status == GridSpotStatus.Miss)
                {
                    Console.Write(" o ");
                }
                else if (Spot.Status == GridSpotStatus.Ship)
                {
                    Console.Write(" ♣ ");
                }
                else if (Spot.Status == GridSpotStatus.Sunk)
                {
                    Console.Write(" X ");
                }
            }
            Console.WriteLine();
        }

        private static int ChooseBoardSize()
        {
            int[] possibleSize = { 3, 4, 5, 6, 7, 8, 9 };
            int boardSize = 0;
            Console.Write($"Please choose the board size: (3,4,5,6,7,8,9): ");
            do
            {
                try
                {
                    boardSize =  int.Parse(Console.ReadLine().ToString());
                    if (!(boardSize >= 3 && boardSize <= 9))
                    {
                        Console.WriteLine("Board size not within limits!");
                        throw new Exception();
                    }
                    break;
                }
                catch
                {
                    Console.Write("Choose board size again: ");
                }
            } while (!possibleSize.Contains(boardSize));

            return boardSize;
        }

        private static void GameIntro()
        {
            Console.WriteLine("-------------------------------");
            Console.WriteLine("  Welcome to BattleShip Lite!");
            Console.WriteLine("       Created by DitaN");
            Console.WriteLine("-------------------------------");
            Console.WriteLine();
        }

    }
}
