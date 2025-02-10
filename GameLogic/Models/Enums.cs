using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary.Models
{
    public enum GridSpotStatus
    {
        Empty,
        Ship,
        Sunk,
        Miss
    }

    public class GridSpotCoordonate
    {
        public List<string> letters = new List<string>
        {
            "A", "B", "C", "D", "E", "F", "G", "H", "I"
        };
        public List<int> numbers = new List<int>
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9
        };
    }
}
