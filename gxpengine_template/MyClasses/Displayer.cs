using GXPEngine;

namespace gxpengine_template.MyClasses.WaveFunctionCollapse
{
    public class Displayer : GameObject
    {
        readonly Tile[] _tiles = new Tile[6];
        readonly Grid grid;
        readonly int _time = 0;
        int _timer = 0;
        int _endTime = 50;
        //TOP RIGHT DOWN LEFT
        public Displayer() 
        {
            _tiles[0] = new Tile(0,true,true,"ABA", "ABA", "ABA", "ABA");
            _tiles[1] = new Tile(1,true,false,"AAA", "ABA", "AAA", "ABA");
            _tiles[2] = new Tile(2,false,true,"AAA", "ABA", "ABA", "ABA");
            _tiles[3] = new Tile(3,false,false,"ABA", "ABA", "AAA", "AAA");
            _tiles[4] = new Tile(4,true,true,"AAA", "AAA", "AAA", "AAA");
            _tiles[5] = new Tile(5,false,false,"AAA", "AAA", "AAA", "ABA");

            grid = new Grid(500, 11, _tiles);
            AddChild(grid);
            grid.SetXY(0, 0);

        }
        void Update()
        {
            if(!grid.Done && _timer-- < 0)
            {
                _timer = _time;
                Iterate();

            }

            if (grid.Done && --_endTime < 0)
            {
                Destroy();
                ((MyGame)MyGame.main).LoadScene();
            }

        }

        void Iterate()
        {
            Cell lowestCell = grid.GetLeastEntropyCell();
            grid.CollapseCell(lowestCell);
            grid.Propagate(lowestCell);
        }
    }
}
