using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using System.Numerics;

namespace GKFOpenTk
{
    class Block
    {
        public Point3D position;
        public Vector3 shapes;

        public Block(Point3D position, Vector3 shapes)
        {
            this.position = position;
            this.shapes = shapes;
        }
        public List<Tuple<Point3D, Point3D>> getEdges()
        {
            var edges = new List<Tuple<Point3D, Point3D>>();
            // front
            Point3D point1 = position;
            Point3D point2 = position;
            {        
                // front bottom
                point2.X += shapes.X;
                edges.Add(new Tuple<Point3D, Point3D>(point1, point2));

                // front left
                point1 = point2;
                point2.Y += shapes.Y;
                edges.Add(new Tuple<Point3D, Point3D>(point1, point2));

                // front top
                point1 = point2;
                point2.X -= shapes.X;
                edges.Add(new Tuple<Point3D, Point3D>(point1, point2));

                // front right
                point1 = point2;
                point2.Y -= shapes.Y;
                edges.Add(new Tuple<Point3D, Point3D>(point1, point2));
            }
            // back
            point1 = position;
            point2 = position;
            point1.Z += shapes.Z;
            point2.Z += shapes.Z;

            {
                // bottom
                point2.X += shapes.X;
                edges.Add(new Tuple<Point3D, Point3D>(point1, point2));

                // left
                point1 = point2;
                point2.Y += shapes.Y;
                edges.Add(new Tuple<Point3D, Point3D>(point1, point2));

                // top
                point1 = point2;
                point2.X -= shapes.X;
                edges.Add(new Tuple<Point3D, Point3D>(point1, point2));

                // right
                point1 = point2;
                point2.Y -= shapes.Y;
                edges.Add(new Tuple<Point3D, Point3D>(point1, point2));
            }

            // connect front and back
            {
                // top right
                point1 = position;
                point1.X += shapes.X;
                point1.Y += shapes.Y;
                point2 = point1;
                point2.Z += shapes.Z;
                edges.Add(new Tuple<Point3D, Point3D>(point1, point2));

                // top left
                point1 = position;
                point1.Y += shapes.Y;
                point2 = point1;
                point2.Z += shapes.Z;
                edges.Add(new Tuple<Point3D, Point3D>(point1, point2));

                // bottom left
                point1 = position;
                point2 = point1;
                point2.Z += shapes.Z;
                edges.Add(new Tuple<Point3D, Point3D>(point1, point2));

                // bottom right
                point1 = position;
                point1.X += shapes.X;
                point2 = point1;
                point2.Z += shapes.Z;
                edges.Add(new Tuple<Point3D, Point3D>(point1, point2));
            }

            return edges;
        }
        
    }
}
