using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media.Media3D;
using System.Numerics;

namespace GKFOpenTk
{
    internal class Game
    {
        public GameWindow window;
        private List<Block> blocks = new List<Block>();
        private Camera camera = new Camera();

        public Game(GameWindow window)
        {
            this.window = window;

            window.Load += Window_Load;
            window.RenderFrame += Window_RenderFrame;
            window.UpdateFrame += Window_UpdateFrame;
            window.Closing += Window_Closing;

            // set camera
            Camera camera = new Camera();


            Point3D point = new Point3D(0, 0, 10);
            var shapes = new System.Numerics.Vector3(300, 300, 300);
            Block block = new Block(point, shapes);
            blocks.Add(new Block(point, shapes));

            List<Tuple<Point3D, Point3D>> l = block.getEdges();
            foreach (var p in l)
            {

                Console.WriteLine("p: " + p.ToString());
            }


            
        }

        private void Window_Load(object sender, EventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void Window_UpdateFrame(object sender, FrameEventArgs e)
        {
          //  Console.WriteLine("Update");
        }

        private void Window_RenderFrame(object sender, FrameEventArgs e)
        {
            GL.ClearColor(Color.FromArgb(5, 5, 25));
            GL.Clear(ClearBufferMask.ColorBufferBit);
           // Point p = new Point(0.5, 0.2);
            this.drawLine(new Point(-500, 500), new PointF(1 * window.Width, -1 * window.Height), Color.Purple);


            foreach (Block block in blocks)
            {
                for (int i = 0; i <  block.getEdges().Count; i++) 
                {
                    var p = block.getEdges()[i];
                    Point3D a = p.Item1;
                    Point3D b = p.Item2;

                    Console.WriteLine("x:" + a.X + "y:" + a.Y + "z:" + a.Z + " x:" + b.X + "y:" + b.Y + "z:" + b.Z);

                    var pA = ComputePointOnPlane(camera, a, camera.Plane);
                    var pB = ComputePointOnPlane(camera, b, camera.Plane);
                    
                    // var v = new System.Numerics.Vector3((Single) pA.X, (Single)vP.Y, (Single)vP.Z);

                    Color color = Color.Red;

                    switch (i)
                    {
                        case 0:
                            color = Color.Green;
                            break;
                        case 1:
                            color = Color.Yellow;
                            break;
                        case 2:
                            color = Color.Blue;
                            break;
                        case 3:
                            color = Color.Magenta;
                            break;
                        case 4:
                            color = Color.Cyan;
                            break;
                        case 5:
                            color = Color.Aqua;
                            break;
                        case 6:
                            color = Color.Bisque;
                            break;
                    }

                    if (i == 0)
                    {
                        color = Color.Olive;
                    }


                    this.drawLine(new Point((int)pA.X, (int)pA.Y), new PointF((int)pB.X, (int)pB.Y), color);

                }
            }
            Console.WriteLine("\n");

            



            GL.Flush();
            window.SwapBuffers();
        }

        private void drawLine(PointF a, PointF b, Color color)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Disable(EnableCap.DepthTest);
            GL.LineWidth(3);// not working
            GL.Color3(color);
            GL.Vertex2(a.X/window.Width, a.Y/window.Height);
            GL.Vertex2(b.X/window.Width, b.Y/window.Height);
            GL.End();
        }

        private static float ComputeDistance(System.Numerics.Vector3 point, Plane plane)
        {
            float dot = System.Numerics.Vector3.Dot(plane.Normal, point);
            float value = dot - plane.D;
            return value;
        }
   
        private static System.Numerics.Vector3 ComputePointOnPlane(Camera camera, Point3D point, Plane plane)
        {
            var vA = new System.Numerics.Vector3((Single)(point.X), (Single)(point.Y), (Single)(point.Z));
            var vCA = new System.Numerics.Vector3((Single)(camera.Point.X - point.X), (Single)(camera.Point.Y - point.Y), (Single)(camera.Point.Z - point.Z));
            var hC = Math.Abs(ComputeDistance(camera.Point, camera.Plane));
            
            var hA = Math.Abs(ComputeDistance(vA, camera.Plane));
            Console.WriteLine("hC = " + hC + " hA = " + hA);
            if (hC + hA == 0)
            {
                return new System.Numerics.Vector3((int)point.X, (int)point.Y, (int)point.Z);
            }
            var mA = (hA * vCA.Length()) / (hC + hA); // length of vector CP, where P i point of break CA with plant
            var vP = System.Numerics.Vector3.Normalize(vCA);
            vP = System.Numerics.Vector3.Multiply(vP, mA);
            vP = System.Numerics.Vector3.Add(vP, vA);
            Console.WriteLine("ComputePointOnPlane x:" + vP.X + "y:" + vP.Y + "z:" + vP.Z);
            return vP;
        }

        private static System.Numerics.Vector3 Vector3(double x, double y, double z)
        {
            throw new NotImplementedException();
        }
    }
}
