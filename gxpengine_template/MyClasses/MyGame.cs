using GXPEngine;
using gxpengine_template.MyClasses.WaveFunctionCollapse;

namespace gxpengine_template.MyClasses
{
    public class MyGame : Game
    {
        static void Main()
        {
            new MyGame().Start();
        }

        public MyGame() : base(500, 500,false,false)
        {
           LoadScene();
        }

        public void LoadScene()
        {
            LateAddChild(new TheGame());
        }

        
    }
}
