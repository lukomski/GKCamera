using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media.Media3D;
using SN = System.Numerics;
using OpenTK.Input;

using Accord.Math;


namespace GKFOpenTk
{
    internal class Game
    {
        public GameWindow window;
        private List<Block> blocks = new List<Block>();
        private Camera camera = new Camera();

        KeyboardState keyboardState, lastKeyboardState;



        public Game(GameWindow window)
        {
            this.window = window;

            window.Load += Window_Load;
            window.RenderFrame += Window_RenderFrame;
            window.UpdateFrame += Window_UpdateFrame;
            window.Closing += Window_Closing;
            window.KeyPress += Window_KeyPress;


            // set camera
            Camera camera = new Camera();

            var point = new SN.Vector3(0, 0, 0);
            var shapes = new SN.Vector3(300, 300, 300);
            Block block = new Block(point, shapes);
            blocks.Add(new Block(point, shapes));

            List<Tuple<SN.Vector3, SN.Vector3>> l = block.GetEdges();
            foreach (var p in l)
            {

                Console.WriteLine("p: " + p.ToString());
            }       
        }

        private void Window_KeyPress(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine("Window_KeyPress");
            var step = 0.1f;
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Key.W))
            {
                var attitude = camera.GetAttitude();
               // attitude.Z += step;
                attitude += camera.MapRelativeMoveToNonRelativeMove(new SN.Vector3(0, 0, 0.03f));
                camera.SetAttitude(attitude);
                Console.WriteLine("attitude = " + camera.GetAttitude().ToString());
            } 
            else if (state.IsKeyDown(Key.S))
            {
                var attitude = camera.GetAttitude();
               // attitude.Z -= step;
                attitude -= camera.MapRelativeMoveToNonRelativeMove(new SN.Vector3(0, 0, 0.03f));
                camera.SetAttitude(attitude);
                Console.WriteLine("attitude = " + camera.GetAttitude().ToString());
            }
            else if (state.IsKeyDown(Key.A))
            {
                var attitude = camera.Attitude;
                var alfa = -0.1;
                var m = camera.GetOYRotMatrix((float)Math.Cos(alfa), (float)Math.Sin(alfa));
                var vec = SN.Vector4.Transform(new SN.Vector4(attitude, 0), m);
                var v = new SN.Vector3(vec.X, vec.Y, vec.Z);
                //var Beta = 0.01;
                //var nA = attitude;
                //nA.X = (float)Math.Cos(Beta) * attitude.X - (float)Math.Sin(Beta) * attitude.Z;
                // nA.Z = (float)Math.Sin(Beta) * attitude.X +  (float)Math.Cos(Beta) * attitude.Z;
                //attitude.X -= step/10;
               // attitude -= camera.MapRelativeMoveToNonRelativeMove(new SN.Vector3(0.03f, 0, 0));
                camera.SetAttitude(v);
                Console.WriteLine("attitude = " + camera.GetAttitude().ToString());
            }
            else if (state.IsKeyDown(Key.D))
            {
                var attitude = camera.Attitude;
                var alfa = 0.1;
                var m = camera.GetOYRotMatrix((float)Math.Cos(alfa), (float)Math.Sin(alfa));
                var vec = SN.Vector4.Transform(new SN.Vector4(attitude, 0), m);
                var v = new SN.Vector3(vec.X, vec.Y, vec.Z);
                //var Beta = 0.01;
                //var nA = attitude;
                //nA.X = (float)Math.Cos(Beta) * attitude.X - (float)Math.Sin(Beta) * attitude.Z;
                // nA.Z = (float)Math.Sin(Beta) * attitude.X +  (float)Math.Cos(Beta) * attitude.Z;
                //attitude.X -= step/10;
                // attitude -= camera.MapRelativeMoveToNonRelativeMove(new SN.Vector3(0.03f, 0, 0));
                camera.SetAttitude(v);
                Console.WriteLine("attitude = " + camera.GetAttitude().ToString());
                // var attitude = camera.GetAttitude();
                //// attitude.X += step;
                // attitude += camera.MapRelativeMoveToNonRelativeMove(new SN.Vector3(0.03f, 0, 0));
                // camera.SetAttitude(attitude);
                // Console.WriteLine("attitude = " + camera.GetAttitude().ToString());
            }


            // POS STRAIGHT/BACK/LEFT/RIGHT/UP/DOWN
            else if (state.IsKeyDown(Key.L))
            {
                var point = camera.Position;
                point += camera.MapRelativeMoveToNonRelativeMove(new SN.Vector3(5, 0, 0));
                camera.SetPoint(point);
            } 
            else if (state.IsKeyDown(Key.J))
            {
                var point = camera.Position;
                point -= camera.MapRelativeMoveToNonRelativeMove(new SN.Vector3(5, 0, 0));
                Console.WriteLine("left vector = " + camera.MapRelativeMoveToNonRelativeMove(new SN.Vector3(1, 0, 0)));
                camera.SetPoint(point);
                Console.WriteLine("point = " + point);
            } 
            else if (state.IsKeyDown(Key.I))
            {
                var point = camera.Position;
                point += camera.MapRelativeMoveToNonRelativeMove(new SN.Vector3(0, 0, 5));
                camera.SetPoint(point);
                Console.WriteLine("point = " + point);
            } 
            else if (state.IsKeyDown(Key.K))
            {
                var point = camera.Position;
                point -= camera.MapRelativeMoveToNonRelativeMove(new SN.Vector3(0, 0, 5));
                camera.SetPoint(point);
            }
            else if (state.IsKeyDown(Key.O)) {
                var point = camera.Position;
                point += camera.MapRelativeMoveToNonRelativeMove(new SN.Vector3(0, 5, 0));
                camera.SetPoint(point);
            }
            else if (state.IsKeyDown(Key.U)) {
                var point = camera.Position;
                point -= camera.MapRelativeMoveToNonRelativeMove(new SN.Vector3(0, 5, 0));
                camera.SetPoint(point);
            }
            else {
                Console.WriteLine("unhadled key " + state.GetType());
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
            // Get current state
            keyboardState = OpenTK.Input.Keyboard.GetState();

            // Check Key Presses
            if (KeyPress(Key.Right))
            {
                // move position camera right
                //  var p = this.camera.Position;
                camera.MapRelativeMoveToNonRelativeMove(new SN.Vector3(0, 0, 0));

                // Store current state for next comparison;
                lastKeyboardState = keyboardState;
            }
        }
        public bool KeyPress(Key key)
        {
            return (keyboardState [key] && (keyboardState [key] != lastKeyboardState [key]) );
        }

        private void Window_RenderFrame(object sender, FrameEventArgs e)
        {
            GL.ClearColor(Color.FromArgb(5, 5, 25));
            GL.Clear(ClearBufferMask.ColorBufferBit);
            //this.DrawLine(new System.Numerics.Vector2(-300, 300), new SN.Vector2(1 * window.Width / 2, -1 * window.Height/2), Color.Purple);

            foreach (Block block in blocks)
            {

                for (int i = 0; i <  block.GetEdges().Count; i++) 
                {
                    var p = block.GetEdges()[i];
                    var a = p.Item1;
                    var b = p.Item2;

                    var pA = camera.ComputePointOnPlane(a);
                    var pB = camera.ComputePointOnPlane(b);


                    //var pA = camera.MapPointToPlate(a);
                    //var pB = camera.MapPointToPlate(b);


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

                    this.DrawLine(pA, pB, color);

                }         
            }


            this.drawoXYZ();

            GL.Flush();
            window.SwapBuffers();
        }

        private void drawoXYZ()
        {
            SN.Vector3 oz = new SN.Vector3(0, 0, 600);
            SN.Vector3 oy = new SN.Vector3(0, 600, 0);
            SN.Vector3 ox = new SN.Vector3(600, 0, 0);

            List <SN.Vector3> l = new List<SN.Vector3>();
            l.Add(ox);
            l.Add(oy);
            l.Add(oz);

            foreach (SN.Vector3 axe in l)
            {
                SN.Vector2 p = camera.ComputePointOnPlane(axe);
                SN.Vector2 oo = camera.ComputePointOnPlane(new SN.Vector3(0, 0, 0));
                Color c = Color.Red;
                if (axe.Y>0) {
                    c = Color.Yellow;
                } 
                else if (axe.Z >0) {
                    c = Color.Green;
                }

                this.DrawLine(oo, p, c);
               // this.DrawLine(oo,new SN.Vector2(300,0), Color.Pink);
            }

        }

        private void DrawLine(SN.Vector2 a, SN.Vector2 b, Color color)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Disable(EnableCap.DepthTest);
            GL.LineWidth(3);// not working
            GL.Color3(color);
            GL.Vertex2(camera.GetScenePoint().X + a.X/(window.Width/2), camera.GetScenePoint().Y + a.Y/(window.Height/2));
            GL.Vertex2(camera.GetScenePoint().X + b.X/(window.Width/2), camera.GetScenePoint().Y + b.Y/(window.Height/2));
            GL.End();
        }
    }
}
