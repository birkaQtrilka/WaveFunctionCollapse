using GXPEngine;
namespace gxpengine_template.MyClasses.WaveFunctionCollapse
{
    public enum NeighbourDir
    {
        Up,
        Right,
        Down,
        Left, 
    }
    public class Tile : AnimationSprite
    {
        public string[] Sockets { get; }
        public bool SymetryH { get; }
        public bool SymetryV { get; }
        public Tile(int frame, bool horizontalSymetry,bool verticalSymetry, params string[] sockets) : base("ArtTiles.PNG", 6, 1, -1, false, false)
        {
            SetOrigin(width/2, height/2);
            SetFrame(frame);
            Sockets = sockets;
            SymetryH = horizontalSymetry;
            SymetryV = verticalSymetry;
        }

        public void Rotate()
        {
            string lastSocket = Sockets[Sockets.Length - 1];
            for (int i = Sockets.Length - 1; i >= 1; i--)
            {
                Sockets[i] = Sockets[i - 1];
            }

            Sockets[0] = lastSocket;

            rotation += 90;
        }

        public Tile Clone()
        {
            var nt = new Tile(currentFrame,SymetryH,SymetryV, (string[])Sockets.Clone());
            nt.rotation = rotation;
            return nt;
        }
    }
}
