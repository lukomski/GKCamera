using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKFOpenTk
{
    class Program
    {
        static void Main(string[] args)
        {
            GameWindow window = new GameWindow(width: 1000, height: 600);
            Game game = new Game(window);

            window.Run();
        }
    }
}
