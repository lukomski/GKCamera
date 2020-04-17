using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media.Media3D;
using SN = System.Numerics;
using OpenTK.Input;
using System.IO;

using Accord.Math;


namespace GKFOpenTk
{
    internal class Game
    {
        public GameWindow window;
        private List<Block> blocks = new List<Block>();
        private Camera camera;

        KeyboardState keyboardState, lastKeyboardState;

        public Game(GameWindow window)
        {
            this.window = window;
            // set camera
            this.camera = new Camera(window.Width, window.Height);

            window.Load += Window_Load;
            window.RenderFrame += Window_RenderFrame;
            window.UpdateFrame += Window_UpdateFrame;
            window.Closing += Window_Closing;
            window.KeyPress += Window_KeyPress;

            this.ReadFile("../../blocks.txt");   
        }
        private void ReadFile(string filename)
        {
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(filename))
                {
                    // Read the stream to a string, and write the string to the console.
                    //String line = sr.ReadToEnd();
                    while (sr.Peek() >= 0)
                    {
                        //Console.WriteLine(sr.ReadLine());
                        var line = sr.ReadLine();
                        var words = line.Split(' ');
                        
                        var point = new SN.Vector3(Int32.Parse(words[0]), Int32.Parse(words[1]), Int32.Parse(words[2]));
                        var shapes = new SN.Vector3(Int32.Parse(words[3]), Int32.Parse(words[4]), Int32.Parse(words[5]));
                        var block = new Block(point, shapes);
                        this.blocks.Add(block);

                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        private void Window_KeyPress(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine("Window_KeyPress");
            var step = 0.1f;
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Key.W))
            {
                camera.Rotations.X += (float)Math.PI / 2 / 10;
                Console.WriteLine("Rotations = " + camera.Rotations);
            } 
            else if (state.IsKeyDown(Key.S))
            {
                camera.Rotations.X -= (float)Math.PI / 2 / 10;
                Console.WriteLine("Rotations = " + camera.Rotations);
            }
            else if (state.IsKeyDown(Key.A))
            {
                camera.Rotations.Y -= (float)Math.PI / 2 / 10;
                Console.WriteLine("Rotations = " + camera.Rotations);
            }
            else if (state.IsKeyDown(Key.D))
            {
                camera.Rotations.Y += (float)Math.PI / 2 / 10;
                Console.WriteLine("Rotations = " + camera.Rotations);
            }
            else if (state.IsKeyDown(Key.E))
            {
                camera.Rotations.Z += (float)Math.PI / 2 / 10;
                Console.WriteLine("Rotations = " + camera.Rotations);   
            }
            else if (state.IsKeyDown(Key.Q))
            {
                camera.Rotations.Z -= (float)Math.PI / 2 / 10;
            }


            // POS STRAIGHT/BACK/LEFT/RIGHT/UP/DOWN
            else if (state.IsKeyDown(Key.L))
            {
                var point = camera.Position;
                var move = Helper.MakeRotations(camera.Rotations, new SN.Vector3(5, 0, 0));
                //var move = Helper.TransformRelativeMoveToNonRelative(camera.GetAttitude(), new SN.Vector3(5, 0, 0));
                point += move;
                camera.SetPoint(point);
                Console.WriteLine("move = " + move);
            } 
            else if (state.IsKeyDown(Key.J))
            {
                var point = camera.Position;
                var move = -Helper.MakeRotations(camera.Rotations, new SN.Vector3(5, 0, 0));
                point += move;
                Console.WriteLine("move = " + move);
                camera.SetPoint(point);
            } 
            else if (state.IsKeyDown(Key.I))
            {
                var point = camera.Position;
                var move = Helper.MakeRotations(camera.Rotations, new SN.Vector3(0, 0, 5));
                point += move;
                camera.SetPoint(point);
                Console.WriteLine("move = " + move);
            } 
            else if (state.IsKeyDown(Key.K))
            {
                var point = camera.Position;
                var move = -Helper.MakeRotations(camera.Rotations, new SN.Vector3(0, 0, 5));
                point += move;
                camera.SetPoint(point);
                Console.WriteLine("move = " + move);
            }
            else if (state.IsKeyDown(Key.O)) {
                var point = camera.Position;
                var move = Helper.MakeRotations(camera.Rotations, new SN.Vector3(0, 5, 0));
                point += move;
                camera.SetPoint(point);
                Console.WriteLine("move = " + move);
            }
            else if (state.IsKeyDown(Key.U)) {
                var point = camera.Position;
                var move = -Helper.MakeRotations(camera.Rotations, new SN.Vector3(0, 5, 0));
                point += move;
                camera.SetPoint(point);
                Console.WriteLine("move = " + move);
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
            // Get current state
            keyboardState = OpenTK.Input.Keyboard.GetState();

            // Check Key Presses
            if (KeyPress(Key.Right))
            {
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

            foreach (Block block in blocks)
            {

                for (int i = 0; i <  block.GetEdges().Count; i++) 
                {
                    var p = block.GetEdges()[i];
                    var a = p.Item1;
                    var b = p.Item2;
                    if (!camera.IsVisible(a) || !camera.IsVisible(b))
                    {
                        continue;
                    }

                    var pA = camera.ComputePointOnPlane(a);
                    var pB = camera.ComputePointOnPlane(b);


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


            //this.drawoXYZ();

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
                var p3d = camera.ComputePointOnPlane(axe);
                var oo3d = camera.ComputePointOnPlane(new SN.Vector3(0, 0, 0));
                SN.Vector2 p = new SN.Vector2(p3d.X, p3d.Y);
                SN.Vector2 oo = new SN.Vector2(oo3d.X, oo3d.Y);
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
            GL.Vertex2(a.X/(window.Width/2), a.Y/(window.Height/2));
            GL.Vertex2(b.X/(window.Width/2), b.Y/(window.Height/2));
            GL.End();
        }
    }
}
