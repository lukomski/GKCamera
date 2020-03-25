using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GKFOpenTk
{
    class Game
    {
        public GameWindow window;

        public Game(GameWindow window)
        {
            this.window = window;

            window.Load += Window_Load;
            window.RenderFrame += Window_RenderFrame;
            window.UpdateFrame += Window_UpdateFrame;
            window.Closing += Window_Closing;
        }

        private void Window_Load(object sender, EventArgs e)
        {
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void Window_UpdateFrame(object sender, FrameEventArgs e)
        {
            Console.WriteLine("Update");
        }

        private void Window_RenderFrame(object sender, FrameEventArgs e)
        {
            GL.ClearColor(Color.FromArgb(5, 5, 25));
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //GL.Begin(PrimitiveType.Triangles);
            //GL.Color3(Color.Red);
            //GL.Vertex2(0, 0);
            //GL.Vertex2(1, 1);
            //GL.Color3(Color.Green);
            //GL.Vertex2(-1, 1);
            //GL.End();

            GL.Begin(PrimitiveType.Lines);
            GL.Disable(EnableCap.DepthTest);
            GL.LineWidth(3);// not working
            GL.Color3(Color.Red);
            GL.Vertex2(-1, 1);
            GL.Vertex2(1, -1);
            GL.End();

            GL.Flush();
            window.SwapBuffers();
        }

        
    }
}
