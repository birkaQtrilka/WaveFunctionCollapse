using GXPEngine;
using System.Collections.Generic;
using System.Linq;

namespace gxpengine_template.MyClasses.WaveFunctionCollapse
{
    public class Grid : EasyDraw
    {
        readonly Cell[,] _cells;
        readonly float _cellWidth;
        
        public bool Done { get;private set; }
        int collapsedTileCount;

        public Grid(int pixelSize, int columns, Tile[] allTiles) : base(pixelSize + 1, pixelSize + 1, false)
        {
            _cells = new Cell[columns,columns];

            List<Tile> rotatedTiles = GenerateRotateTileStates(allTiles);

            Clear(50, 30, 12, 150);
            NoFill();
            ShapeAlign(CenterMode.Min, CenterMode.Min);
            _cellWidth = pixelSize / (float)columns;

            var statesList = allTiles.Concat(rotatedTiles);
            GenerateGrid(columns, statesList);
            
            PopulateGrid(columns, allTiles);
        }

        void PopulateGrid(int columns, Tile[] allTiles)
        {
            for (int y = 0; y < columns; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    if (x == 0 || y == 0 || x == _cells.GetLength(1) - 1 || y == _cells.GetLength(0) - 1)
                    {
                        Cell cell = _cells[y, x];
                        CollapseCellWithTile(cell, allTiles[allTiles.Length - 2]);
                        Propagate(cell);
                    }
                }
            }
        }

        void GenerateGrid(int columns, IEnumerable<Tile> statesList)
        {
            for (int y = 0; y < columns; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    Rect(x * _cellWidth, y * _cellWidth, _cellWidth, _cellWidth);

                    _cells[y, x] = new Cell(x, y, new List<Tile>(statesList));
                }
            }
        }

        List<Tile> GenerateRotateTileStates(Tile[] unrotatedTiles)
        {
            List<Tile> rotatedTiles = new List<Tile>();

            foreach (Tile tile in unrotatedTiles)
            {
                for (int i = 1; i < 4; i++)
                {
                    if ((tile.SymetryH && i % 2 == 0) || (tile.SymetryV && i % 2 != 0))
                        continue;

                    Tile newTile = tile.Clone();
                    for (int J = 0; J < i; J++)
                        newTile.Rotate();

                    rotatedTiles.Add(newTile);
                }
            }

            return rotatedTiles;
        }

        public Cell GetLeastEntropyCell()
        {
            Cell min = null;
            foreach (Cell cell in _cells)
            {
                if (cell.CollapsedTile != null) continue;//is a collapsed cell
                if(min == null) min = cell;

                if(cell.Possibilities.Count < min.Possibilities.Count)
                    min = cell;
            }
            return min;
        }

        public void CollapseCell(Cell cell)
        {
            if (cell.Possibilities.Count == 0)
            {
                Done = true;
                return;
            }
            //if there are more than 1 possibility, chose everything except cap tile
            //else choose cap tile
            int randomIndex = Utils.Random(0, cell.Possibilities.Count);//possibilities is not indexed
            if (cell.Possibilities.Count > 1)
               while(cell.Possibilities[randomIndex].currentFrame == 5)//5 is number of cap tile index
                   randomIndex = Utils.Random(0, cell.Possibilities.Count);
            Tile prototype = cell.Possibilities[randomIndex];
            CollapseCellWithTile(cell, prototype);
        }

        public void CollapseCellWithTile(Cell cell, Tile prototype)
        {
            cell.Possibilities.Clear();
            cell.CollapsedTile = prototype.Clone();
            Tile tile = cell.CollapsedTile;

            AddChild(tile);
            tile.SetXY(cell.X * _cellWidth + 0.5f * _cellWidth, cell.Y * _cellWidth + 0.5f * _cellWidth);
            tile.width = (int)_cellWidth;
            tile.height = (int)_cellWidth;

            if (++collapsedTileCount == _cells.Length) Done = true;
        }

        public void Propagate(Cell cell)
        {
            foreach (CellAndDir val in GetNeighbouringCellsAndDir(cell.X, cell.Y))
            {
               
                Cell neighbour = val.cell;
                if (neighbour.CollapsedTile != null) continue;

                int dir = (int)val.dir;

                string mySockets = cell.CollapsedTile.Sockets[dir];
                //constrain
                for (int i = 0; i < neighbour.Possibilities.Count; i++)
                {
                    List<Tile> possibilities = neighbour.Possibilities;
                    Tile possibility = possibilities[i];
                    //here are the rules
                    if (possibility.Sockets[(dir + 2) % 4] != mySockets)
                        possibilities.RemoveAt(i--);
                }
            }
        }

        List<CellAndDir> GetNeighbouringCellsAndDir(int x, int y)
        {
            List<CellAndDir> neighbours = new List<CellAndDir>();
            if (x - 1 >= 0)
                neighbours.Add(new CellAndDir(_cells[y, x - 1], NeighbourDir.Left));
            if(x + 1 < _cells.GetLength(1))
                neighbours.Add(new CellAndDir(_cells[y, x + 1], NeighbourDir.Right));
            if (y - 1 >= 0)
                neighbours.Add(new CellAndDir(_cells[y - 1, x], NeighbourDir.Up));
            if (y + 1 < _cells.GetLength(0))
                neighbours.Add(new CellAndDir(_cells[y + 1, x], NeighbourDir.Down));
            return neighbours;
        }

        readonly struct CellAndDir
        {
            public readonly Cell cell;
            public readonly NeighbourDir dir;

            public CellAndDir(Cell cell, NeighbourDir dir)
            {
                this.cell = cell;
                this.dir = dir;
            }
        }
    }
}
