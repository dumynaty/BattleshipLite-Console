using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary.Models
{
    public class PlayerModel
    {
        public string Name { get; set; }
        public BoardModel OwnGrid { get; set; } = new BoardModel();
        public BoardModel TargetGrid { get; set; } = new BoardModel();
    }
}
