using GXPEngine;
using System.Collections.Generic;

namespace gxpengine_template.MyClasses.WaveFunctionCollapse
{
    public class Cell
    {
        public List<Tile> Possibilities { get; }
        public int X {  get; }
        public int Y { get; }
        public Tile CollapsedTile { get; set; }
        public Cell(int x, int y, List<Tile> possibilities) 
        { 
            Possibilities = possibilities;
            X = x;
            Y = y;
        }

    }
}
