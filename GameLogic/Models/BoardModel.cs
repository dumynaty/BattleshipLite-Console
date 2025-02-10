using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary.Models
{
    public class BoardModel
    {
        public int GridCount { get; set; } = 0;
        public List<GridSpotModel> GridSpots { get; set; } = new List<GridSpotModel>();
    }
}
