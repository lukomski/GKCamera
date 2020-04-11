using Microsoft.VisualStudio.TestTools.UnitTesting;
using GKFOpenTk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace GKFOpenTk.Tests
{
    [TestClass()]
    public class Matrix3x3Tests
    {
        [TestMethod()]
        public void OperatorMultiplyTest()
        {
            Matrix3x3 m1 = new Matrix3x3
            {
                // first row
                M11 = 1,
                M12 = 0,
                M13 = 1,

                // scnd row
                M21 = 0,
                M22 = 1,
                M23 = 0,

                // third row
                M31 = 1,
                M32 = 1,
                M33 = 1
            };

            Matrix3x3 m2 = new Matrix3x3
            {
                // first row
                M11 = 1,
                M12 = 1,
                M13 = 0,

                // scnd row
                M21 = 0,
                M22 = 1,
                M23 = 1,

                // third row
                M31 = 1,
                M32 = 0,
                M33 = 1
            };

            Matrix3x3 m3 = m1 * m2;

            // first row
            Assert.AreEqual(2, m3.M11);
            Assert.AreEqual(1, m3.M12);
            Assert.AreEqual(1, m3.M13);

            // scnd row
            Assert.AreEqual(0, m3.M21);
            Assert.AreEqual(1, m3.M22);
            Assert.AreEqual(1, m3.M23);

            // third row
            Assert.AreEqual(2, m3.M31);
            Assert.AreEqual(2, m3.M32);
            Assert.AreEqual(2, m3.M33);
        }

        [TestMethod()]
        public void OperatorMultiplyWithVector()
        {
            var m = new Matrix3x3
            {
                // first row
                M11 = 1,
                M12 = 0,
                M13 = 1,

                // scnd row
                M21 = 0,
                M22 = 1,
                M23 = 0,

                // third row
                M31 = 1,
                M32 = 1,
                M33 = 1
            };

            Vector3 v = new Vector3
            {
                X = 1,
                Y = 0,
                Z = 1
            };

            Vector3 v1 = m * v;

            // first row
            Assert.AreEqual(2, v1.X);
            Assert.AreEqual(0, v1.Y);
            Assert.AreEqual(2, v1.Z);
        }

        [TestMethod()]
        public void GetDeterminationTest()
        {
            Matrix3x3 m = new Matrix3x3
            {
                // first row
                M11 = 1,
                M12 = 4,
                M13 = 2,

                // scnd row
                M21 = 2,
                M22 = -3,
                M23 = 2,

                // third row
                M31 = 1,
                M32 = 0,
                M33 = 0
            };

            var v = m.Determinate();
            Assert.AreEqual(14, v);
        }

        [TestMethod()]
        public void GetRevOXRotMatrixTest()
        {
            // set camera
            Camera camera = new Camera();
            camera.SetPoint(new Vector3(camera.Distance, 150, 150));
            camera.SetAttitude(new Vector3(-1, 0, 0));
            Console.WriteLine("Plane = " + camera.Plane.ToString());

            var xRot = camera.GetRevOXRotMatrix();
            Console.WriteLine("xRot = " + xRot.ToString());
        }

        [TestMethod()]
        public void MapPointToPlateWithNoChangesTest()
        {
            // set camera
            Camera camera = new Camera();
            camera.SetPoint(new Vector3(0, 0, -camera.Distance));
            camera.SetAttitude(new Vector3(0, 0, 1));
            Console.WriteLine("Plane = " + camera.Plane.ToString());

            var p = camera.MapPointToPlate(new Vector3(300, 300, 0));

            Console.WriteLine("p = " + p.ToString());
            Assert.AreEqual(new Vector2(300, 300), p);
        }
        [TestMethod()]
        public void MapPointOYToPlateTest()
        {
            // set camera
            Camera camera = new Camera();
            camera.SetPoint(new Vector3(camera.Distance, 0, 0));
            camera.SetAttitude(new Vector3(-1, 0, 0));
            Console.WriteLine("Plane = " + camera.Plane.ToString());

            var p = camera.MapPointToPlate(new Vector3(0, 300, 300));

            Console.WriteLine("p = " + p.ToString());
            Assert.AreEqual(new Vector2(300, 300), p);
        }

        [TestMethod()]
        public void MapPointOXToPlateTest()
        {
            // set camera
            Camera camera = new Camera();
            camera.SetPoint(new Vector3(camera.Distance, 0, 0));
            camera.SetAttitude(new Vector3(0, 1, 0));
            Console.WriteLine("Plane = " + camera.Plane.ToString());

            var p = camera.MapPointToPlate(new Vector3(0, 0, 300));

            Console.WriteLine("p = " + p.ToString());
            Assert.AreEqual(new Vector2(0, -300), p);
        }

        [TestMethod()]
        public void MapRelativeMoveToNonRelativeMoveTest()
        {
            // set camera
            Camera camera = new Camera();
            camera.SetPoint(new Vector3(camera.Distance, 0, 0));
            camera.SetAttitude(new Vector3(-1, 0, 0));

            var v = camera.MapRelativeMoveToNonRelativeMove(new Vector3(-1, 0, 0));

            Console.WriteLine("v = " + v.ToString());
            Assert.AreEqual(new Vector3(0,0,1), v);
        }
    }
}