using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using System.Numerics;

namespace GKFOpenTk
{
    class Block
    {
        public Vector3 position;
        public Vector3 shapes;

        public Block(Vector3 position, Vector3 shapes)
        {
            this.position = position;
            this.shapes = shapes;
        }
        public List<Tuple<Vector3, Vector3>> GetEdges()
        {
            var edges = new List<Tuple<Vector3, Vector3>>();
            // front
            Vector3 point1 = position;
            Vector3 point2 = position;
            {        
                // front bottom
                point2.X += shapes.X;
                edges.Add(new Tuple<Vector3,Vector3>(point1, point2));

                // front left
                point1 = point2;
                point2.Y += shapes.Y;
                edges.Add(new Tuple<Vector3, Vector3>(point1, point2));

                // front top
                point1 = point2;
                point2.X -= shapes.X;
                edges.Add(new Tuple<Vector3, Vector3>(point1, point2));

                // front right
                point1 = point2;
                point2.Y -= shapes.Y;
                edges.Add(new Tuple<Vector3, Vector3>(point1, point2));
            }
            // back
            point1 = position;
            point2 = position;
            point1.Z += shapes.Z;
            point2.Z += shapes.Z;

            {
                // bottom
                point2.X += shapes.X;
                edges.Add(new Tuple<Vector3, Vector3>(point1, point2));

                // left
                point1 = point2;
                point2.Y += shapes.Y;
                edges.Add(new Tuple<Vector3, Vector3>(point1, point2));

                // top
                point1 = point2;
                point2.X -= shapes.X;
                edges.Add(new Tuple<Vector3, Vector3>(point1, point2));

                // right
                point1 = point2;
                point2.Y -= shapes.Y;
                edges.Add(new Tuple<Vector3, Vector3>(point1, point2));
            }

            // connect front and back
            {
                // top right
                point1 = position;
                point1.X += shapes.X;
                point1.Y += shapes.Y;
                point2 = point1;
                point2.Z += shapes.Z;
                edges.Add(new Tuple<Vector3, Vector3>(point1, point2));

                // top left
                point1 = position;
                point1.Y += shapes.Y;
                point2 = point1;
                point2.Z += shapes.Z;
                edges.Add(new Tuple<Vector3, Vector3>(point1, point2));

                // bottom left
                point1 = position;
                point2 = point1;
                point2.Z += shapes.Z;
                edges.Add(new Tuple<Vector3, Vector3>(point1, point2));

                // bottom right
                point1 = position;
                point1.X += shapes.X;
                point2 = point1;
                point2.Z += shapes.Z;
                edges.Add(new Tuple<Vector3, Vector3>(point1, point2));
            }

            return edges;
        }
        
    }
}
