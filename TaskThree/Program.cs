using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace CTaskThree
{

    public static class CoordinatesExtensions
    {
        public static List<Coordinates> Rectangle(this Coordinates begin, Coordinates end)
        {
            var coordinates = new List<Coordinates>();
            for(int row = begin.Row; row <= end.Row; row++)
                for(char column = begin.Column; column <= end.Column; column ++)
                    coordinates.Add(new Coordinates(row,column));
            return coordinates;
        }
    }
    
    public class Coordinates : IEquatable<Coordinates>
    {
        public Coordinates(int row, char column)
        {
            Row = row;
            Column = column;
        }

        public int Row { get; }
        public char Column { get; }

        public static Coordinates Parse(string s)
        {
            //should put more complicated logic to support coordinates like 1AAAA
            var column = s.Last();
            var row = int.Parse(s.Remove(s.Length-1));
            
            return new Coordinates(row,column);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Row * 397) ^ Column.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"{Row}{Column}";
        }

        public bool Equals(Coordinates other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Row == other.Row && Column == other.Column;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Coordinates) obj);
        }
    }
    
    class Cell
    {
        public Cell(Coordinates coordinates)
        {
            Coordinates = coordinates;
        }

        private Coordinates Coordinates { get; }
        
        //not so good design decision, but dont have time
        public ShotResult Shot { get; set; }
        public Ship Ship { get; set; }
    }

   
    
    class Ship
    {
        enum BodyState
        {
            Alive,
            Hit
        }
        //should introduce a class for shipBody
        //could do even bool state
        private readonly Dictionary<Coordinates,BodyState> _bodyState = new Dictionary<Coordinates, BodyState>();
        public Ship(IEnumerable<Coordinates> body)
        {
            Body = body.ToArray();
            _bodyState = Body.ToDictionary(b => b, b => BodyState.Alive);
            CheckBody(Body);
        }

        private void CheckBody(Coordinates[] body)
        {
            //should add body integrity here
        }

        public Coordinates[] Body { get; }

        public void TakeHit(Coordinates coordinates)
        {
            if (_bodyState.ContainsKey(coordinates))
            {
                _bodyState[coordinates] = BodyState.Hit;
                IsHit = true;
                Console.WriteLine($"I'm hit at {coordinates}. Alive:{IsAlive()}");
            }            
        }
        public bool IsAlive()
        {
            return _bodyState.Values.Any(b => b == BodyState.Alive);
        }
    
        public bool IsHit { get; private set; }
        
        public static Ship Build(Coordinates begin, Coordinates end)
        {
             return new Ship(begin.Rectangle(end));
        }

       
    }

    class ShotResult
    {
        public ShotResult(Coordinates coordinates, bool hit)
        {
            Coordinates = coordinates;
            Hit = hit;
        }

        public Coordinates Coordinates { get; }
        public bool Hit { get;}
    }

    class Game
    {
        public Dictionary<Coordinates,Cell> Board { get; } = new Dictionary<Coordinates, Cell>();


        public IReadOnlyCollection<ShotResult> Shots => _shots;
        private readonly List<ShotResult> _shots = new List<ShotResult>();
        
        public IReadOnlyCollection<Ship> Ships => _ships;
        private readonly List<Ship> _ships = new List<Ship>();

        public Game(int boardSize)
        {
            char columnChar = 'A';

            for (int column = 0; column < boardSize; column++)
            {
              
                for (int row = 1; row <= boardSize; row++)
                {
                    var coordinates = new Coordinates(row, columnChar);
                    
                    Board.Add(coordinates, new Cell(coordinates));
                }
                columnChar++;

            }
        }

        //phase one
        public void AddShip(Ship ship)
        {
            _ships.Add(ship);
            foreach (var coordinates in ship.Body)
            {
                Board[coordinates].Ship = ship;
                Console.WriteLine($"Added ship to {coordinates}");
            }
        }

        //phase two
        public void AddShot(Coordinates coordinates)
        {
            Cell cell = Board[coordinates];
            bool hit = cell.Ship != null;
            Console.WriteLine($"Shooting at {coordinates}");
            ShotResult shotResult = new ShotResult(coordinates, hit);
            _shots.Add(shotResult);
            if (hit)
            {
                cell.Ship.TakeHit(coordinates);
                
            }
                
        }

        public int GetShipsSunk()
        {
            return _ships.Count(s => !s.IsAlive());
        }

        public int GetShipsHit()
        {
            return _ships.Count(s => s.IsHit);
        }
        
        
    }

    class Program
    {

        public static void Main(string[] args)
        {
           
            //(4, '1B 2C,2D 4D', '2B 2D 3D 4D 4A') 
            int boardSize = 4;
            string shipsString = "1B 2C,2D 4D";
            string shotsString = "2B 2D 3D 4D 4A";

            var result= RunGame(boardSize, shipsString, shotsString);
            Console.WriteLine(result);
        }
    

        private static string RunGame(int boardSize, string shipsString, string shotsString)
        {
            var game = new Game(boardSize);
            var ships = shipsString.Split(',').Select(s =>
            {
                var coordinates = s.Split(' ');
                //should add checks here
                var begin = Coordinates.Parse(coordinates[0].Trim());
                var end = Coordinates.Parse(coordinates[1].Trim());
                return Ship.Build(begin, end);
            }).ToList();

            var shots = shotsString.Split(" ").Select(s => Coordinates.Parse(s.Trim())).ToList();

            ships.ForEach(s => game.AddShip(s));
            shots.ForEach(s => game.AddShot(s));


            var sunk = game.GetShipsSunk();
            var hit = game.GetShipsHit();

            return sunk + "," + hit;
        }
    }
}