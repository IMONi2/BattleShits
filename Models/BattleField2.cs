﻿using System;
using System.Security.AccessControl;
using System.Security.Cryptography;

namespace BattleShits.Models
{
    public class BattleField2
    {
        public int[,] PlayerBoard { get; set; } // Spelbräde (0 = tomt, 1 = skepp, 2 = träff, 3 = miss)
        public int[,] RobotBoard { get; set; }
        public List<(string name, int y, int x)> PlayerShips { get; set; }
        public List<(string name, int y, int x)> RobotShips { get; set; }
        public int id = 0;

        public int TitanicCount { get; private set; } = 0;
        public int LongshipCount { get; private set; } = 0;
        public int TriremeCount { get; private set; } = 0;
        public int BiremeCount { get; private set; } = 0;

        // Max antal skepp av varje typ
        public const int MaxTitanic = 1;
        public const int MaxLongship = 2;
        public const int MaxTrireme = 3;
        public const int MaxBireme = 4;

        public bool CanPlaceTitanic => TitanicCount < MaxTitanic;
        public bool CanPlaceLongship => LongshipCount < MaxLongship;
        public bool CanPlaceTrireme => TriremeCount < MaxTrireme;
        public bool CanPlaceBireme => BiremeCount < MaxBireme;

        public bool AllShipsPlaced =>
        TitanicCount == MaxTitanic &&
        LongshipCount == MaxLongship &&
        TriremeCount == MaxTrireme &&
        BiremeCount == MaxBireme;

        public int P1shots = 0;
        public int P2shots = 0;
        public int P1Hits = 0;
        public int P2Hits = 0;

        public string Winner = null;
        public BattleField2()
        {
            PlayerBoard = new int[10, 10];
            RobotBoard = new int[10, 10];
            PlayerShips = new List<(string name, int y, int x)>();
            RobotShips = new List<(string name, int y, int x)>();
            RandomShips();
        }

        // Lägg till skepp på specifika koordinater
        public void PlaceSingleShip(int x, int y)
        {
            PlayerShips.Add(("Single", x, y));
            PlayerBoard[y, x] = 1;
        }
        public void PlaceBireme(int gameId, string orientation, int x, int y)
        {

            // Validera orienteringen och positionen
            if (orientation == "hor")
            {
                string placementString1 = "x" + x + " " + "y" + y;
                string placementString2 = "x" + (x + 1) + " " + "y" + y;
                


                
            }
            else if (orientation == "vert")
            {
                PlayerBoard[y, x] = 1;
                PlayerBoard[y + 1, x] = 1;

                PlayerShips.Add(("Bireme" + id, y, x));
                PlayerShips.Add(("Bireme" + id, y + 1, x));
                id++;
            }
            else
            {
                Console.WriteLine("Ogiltig orientering eller startposition. Skeppet ryms inte på brädet.");
            }
        }
        public void PlaceTrireme(string orientation, int x, int y)
        {

            // Validera orienteringen och positionen
            if (orientation == "hor")
            {
                PlayerBoard[y, x] = 1;
                PlayerBoard[y, x + 1] = 1;
                PlayerBoard[y, x + 2] = 1;

                PlayerShips.Add(("Trireme" + id, y, x));
                PlayerShips.Add(("Trireme" + id, y, x + 1));
                PlayerShips.Add(("Trireme" + id, y, x + 2));
                TriremeCount++;
                id++;
            }
            else if (orientation == "vert")
            {
                PlayerBoard[y, x] = 1;
                PlayerBoard[y + 1, x] = 1;
                PlayerBoard[y + 2, x] = 1;

                PlayerShips.Add(("Trireme" + id, y, x));
                PlayerShips.Add(("Trireme" + id, y + 1, x));
                PlayerShips.Add(("Trireme" + id, y + 2, x));
                TriremeCount++;

                id++;
            }
            else
            {
                Console.WriteLine("Ogiltig orientering eller startposition. Skeppet ryms inte på brädet.");
            }
        }
        public void PlaceLongShip(string orientation, int x, int y)
        {
            if (!CanPlaceLongship)
            {
                Console.WriteLine("Du har redan placerat Longship!");
                return;
            }

            // Validera orienteringen och positionen
            if (orientation == "hor")
            {
                PlayerBoard[y, x] = 1;
                PlayerBoard[y, x + 1] = 1;
                PlayerBoard[y, x + 2] = 1;
                PlayerBoard[y, x + 3] = 1;

                PlayerShips.Add(("Longship" + id, y, x));
                PlayerShips.Add(("Longship" + id, y, x + 1));
                PlayerShips.Add(("Longship" + id, y, x + 2));
                PlayerShips.Add(("Longship" + id, y, x + 3));
                LongshipCount++;
                id++;
            }
            else if (orientation == "vert")
            {
                PlayerBoard[y, x] = 1;
                PlayerBoard[y + 1, x] = 1;
                PlayerBoard[y + 2, x] = 1;
                PlayerBoard[y + 3, x] = 1;

                PlayerShips.Add(("Longship" + id, y, x));
                PlayerShips.Add(("Longship" + id, y + 1, x));
                PlayerShips.Add(("Longship" + id, y + 2, x));
                PlayerShips.Add(("Longship" + id, y + 3, x));
                LongshipCount++;
                id++;
            }
            else
            {
                Console.WriteLine("Ogiltig orientering eller startposition. Skeppet ryms inte på brädet.");
            }
        }
        public void PlaceTitanic(string orientation, int x, int y)
        {
            if (!CanPlaceTitanic)
            {
                Console.WriteLine("Du har redan placerat Titanic!");
                return;
            }

            // Validera orienteringen och positionen
            if (orientation == "hor")
            {
                PlayerBoard[y, x] = 1;
                PlayerBoard[y, x + 1] = 1;
                PlayerBoard[y, x + 2] = 1;
                PlayerBoard[y, x + 3] = 1;
                PlayerBoard[y, x + 4] = 1;

                PlayerShips.Add(("Titanic" + id, y, x));
                PlayerShips.Add(("Titanic" + id, y, x + 1));
                PlayerShips.Add(("Titanic" + id, y, x + 2));
                PlayerShips.Add(("Titanic" + id, y, x + 3));
                PlayerShips.Add(("Titanic" + id, y, x + 4));
                TitanicCount++;
                id++;
            }
            else if (orientation == "vert")
            {
                PlayerBoard[y, x] = 1;
                PlayerBoard[y + 1, x] = 1;
                PlayerBoard[y + 2, x] = 1;
                PlayerBoard[y + 3, x] = 1;
                PlayerBoard[y + 4, x] = 1;

                PlayerShips.Add(("Titanic" + id, y, x));
                PlayerShips.Add(("Titanic" + id, y + 1, x));
                PlayerShips.Add(("Titanic" + id, y + 2, x));
                PlayerShips.Add(("Titanic" + id, y + 3, x));
                PlayerShips.Add(("Titanic" + id, y + 4, x));
                TitanicCount++;
                id++;
            }
            else
            {
                Console.WriteLine("Ogiltig orientering eller startposition. Skeppet ryms inte på brädet.");
            }
        }

        public void RandomShips()
        {
            Random random = new Random();
            while (RobotShips.Count < 3)
            {
                int x = random.Next(0, 3);
                int y = random.Next(0, 3);
                var newCoord = ("Double", y, x);

                if (!RobotShips.Contains(newCoord))
                {
                    RobotShips.Add(newCoord);
                    RobotBoard[y, x] = 1;
                }
            }
        }

        // Spelare skjuter skott på en koordinat
        public bool Shoot(int x, int y)
        {
            P1shots++;
            if (RobotBoard[y, x] == 1) // Träff på skepp
            {
                P1Hits++;
                RobotBoard[y, x] = 2; // Markera träff
                Console.WriteLine($"Träff på skepp vid koordinat ({y}, {x})!");

                // Kontrollera om hela skeppet är sänkt
                var shipCoordinates = RobotShips.Where(s => s.y == y && s.x == x).ToList();
                foreach (var ship in shipCoordinates)
                {
                    bool isSunk = true;

                    // Kolla om alla delar av skeppet är träffade (dvs. markeras som 2 på brädet)
                    foreach (var coord in RobotShips.Where(s => s.name == "name"))
                    {

                        if (RobotBoard[coord.y, coord.x] != 2) // Om någon del av skeppet inte är träffad
                        {
                            Console.WriteLine("You hit part of a bireme");
                            isSunk = false;
                            break;

                        }
                    }

                    if (isSunk)
                    {
                        Console.WriteLine("You sunk a bireme!");
                    }
                }

                return true;
            }
            else if (RobotBoard[y, x] == 0) // Miss
            {
                RobotBoard[y, x] = 3; // Markera miss
                Console.WriteLine($"Miss vid koordinat ({y}, {x})");
                return false;
            }
            return false;
        }



        public bool RobotShoot()
        {
            P2shots++;
            Random random = new Random();
            int x = random.Next(0, 3);
            int y = random.Next(0, 3);

            if (PlayerBoard[y, x] == 1) // Träff på skepp
            {
                P2Hits++;
                PlayerBoard[y, x] = 2; // Markera träff
                Console.WriteLine($"Robot träffade ditt skepp vid koordinat ({y}, {x})!");

                // Kontrollera om hela skeppet är sänkt
                var shipCoordinates = PlayerShips.Where(s => s.y == y && s.x == x).ToList();
                foreach (var ship in shipCoordinates)
                {
                    bool isSunk = true;

                    // Kolla om alla delar av skeppet är träffade (dvs. markeras som 2 på brädet)
                    foreach (var coord in PlayerShips.Where(s => s.name == "name"))
                    {

                        if (PlayerBoard[coord.y, coord.x] != 2) // Om någon del av skeppet inte är träffad
                        {
                            Console.WriteLine("Robot hit part of a bireme");
                            isSunk = false;
                            break;

                        }
                    }

                    if (isSunk)
                    {
                        Console.WriteLine("Robot sunk a bireme!");
                    }
                }

                return true;
            }
            else if (PlayerBoard[y, x] == 0) // Miss
            {
                PlayerBoard[y, x] = 3; // Markera miss
                Console.WriteLine($"Robot missade vid koordinat ({y}, {x})");
                return false;
            }
            return false;
        }

        public bool AreAllPlayerShipsSunk()
        {
            if (P2Hits == 30)
            {
                Winner = "Player 2";
                return true;
            }
            return false;
        }
        public bool AreAllRobotShipsSunk()
        {
            if (P1Hits == 3)
            {
                Winner = "Player 1";
                return true;
            }
            return false;
        }


    }

}