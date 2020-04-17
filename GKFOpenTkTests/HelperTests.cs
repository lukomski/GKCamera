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
    public class HelperTests
    {
        [TestMethod()]
        public void TransformLayoutToBeginTest()
        {
            // without rotation needed
            var fromAtittude = new Vector3(0, 0, 1);
            var fromCenter = new Vector3(1, 0, 0);
            var point = new Vector3(1, 0, 0);
            var v = Helper.TransformLayoutToBegin(fromAtittude, fromCenter, point);
            Assert.AreEqual(new Vector3(0, 0, 0), v);
        }

        [TestMethod()]
        public void TransformLayoutToBeginTest1()
        {
            // 270 degrees
            var fromAtittude = new Vector3(1, 0, 0);
            var fromCenter = new Vector3(0, 0, 0);
            var point = new Vector3(0, 0, -1);
            var v = Helper.TransformLayoutToBegin(fromAtittude, fromCenter, point);
            Assert.AreEqual(new Vector3(1, 0, 0), v);
        }

        [TestMethod()]
        public void TransformLayoutToBeginTest2()
        {
            // 90 degrees
            var fromAtittude = new Vector3(-1, 0, 0);
            var fromCenter = new Vector3(0, 0, 0);
            var point = new Vector3(0, 0, 1);
            var v = Helper.TransformLayoutToBegin(fromAtittude, fromCenter, point);
            Assert.AreEqual(new Vector3(1, 0, 0), v);
        }

        [TestMethod()]
        public void TransformLayoutToBeginTest3()
        {
            // 180 deg
            var fromAtittude = new Vector3(0, 0, -1);
            var fromCenter = new Vector3(0, 0, 0);
            var point = new Vector3(1, 0, 0);
            var v = Helper.TransformLayoutToBegin(fromAtittude, fromCenter, point);
            Assert.AreEqual(new Vector3(-1, 0, 0), v);
        }

        [TestMethod()]
        public void TransformLayoutToBeginTest4()
        {
            // 180 deg
            var fromAtittude = new Vector3(0, 0, -1);
            var fromCenter = new Vector3(0, 0, 300);
            var point = new Vector3(1, 0, 0);
            var v = Helper.TransformLayoutToBegin(fromAtittude, fromCenter, point);
            Assert.AreEqual(new Vector3(-1, 0, 300), v);
        }

        [TestMethod()]
        public void TransformRelativeMoveToNonRelativeTest()
        {
            // 270 degrees
            Vector3 toAttitude = new Vector3(-1, 0, 0);
            Vector3 move = new Vector3(1, 0, 0);
            var v = Helper.TransformRelativeMoveToNonRelative(toAttitude, move);
            Assert.AreEqual(new Vector3(0, 0, 1), v);
        }

        [TestMethod()]
        public void TransformRelativeMoveToNonRelativeTest1()
        {
            // 90 degrees
            Vector3 toAttitude = new Vector3(1, 0, 0);
            Vector3 move = new Vector3(1, 0, 0);
            var v = Helper.TransformRelativeMoveToNonRelative(toAttitude, move);
            Assert.AreEqual(new Vector3(0, 0, -1), v);
        }

        [TestMethod()]
        public void CosTest()
        {
            Vector2 u = new Vector2(0, 1);
            Vector2 v = new Vector2(0, 1);

            var w = Helper.Cos(u, v);
            Assert.AreEqual(1, w);
        }

        [TestMethod()]
        public void SinTest()
        {
            Vector2 u = new Vector2(0, 1);
            Vector2 v = new Vector2(0, 1);

            var w = Helper.Sin(u, v);
            Assert.AreEqual(0, w);
        }

        [TestMethod()]
        public void CosTest1()
        {
            // -90 degrees
            Vector2 u = new Vector2(-1, 0);
            Vector2 v = new Vector2(0, 1);

            var w = Helper.Cos(u, v);
            Assert.AreEqual(0, w);
        }

        [TestMethod()]
        public void SinTest1()
        {
            // -90 degrees
            Vector2 u = new Vector2(-1, 0);
            Vector2 v = new Vector2(0, 1);

            var w = Helper.Sin(u, v);
            Assert.AreEqual(-1, w);
        }

        [TestMethod()]
        public void CosTest2()
        {
            // 90 degrees
            Vector2 u = new Vector2(1, 0);
            Vector2 v = new Vector2(0, 1);

            var w = Helper.Cos(u, v);
            Assert.AreEqual(0, w);
        }

        [TestMethod()]
        public void SinTest2()
        {
            // 90 degrees
            Vector2 u = new Vector2(1, 0);
            Vector2 v = new Vector2(0, 1);

            var w = Helper.Sin(u, v);
            Assert.AreEqual(1, w);
        }

        [TestMethod()]
        public void CosTest3()
        {
            // 180 degrees
            Vector2 u = new Vector2(0, -1);
            Vector2 v = new Vector2(0, 1);

            var w = Helper.Cos(u, v);
            Assert.AreEqual(-1, w);
        }

        [TestMethod()]
        public void SinTest3()
        {
            // 180 degrees
            Vector2 u = new Vector2(0, -1);
            Vector2 v = new Vector2(0, 1);

            var w = Helper.Sin(u, v);
            Assert.AreEqual(0, w);
        }

        [TestMethod()]
        public void FindCrossWithXYTest()
        {
            var eye = new Vector3(0, 0, -300);
            var point = new Vector3(1, 0, 0);
            var v = Helper.FindCrossWithXY(eye, point);
            Assert.AreEqual(new Vector3(1, 0, 0), v);
        }
        [TestMethod()]
        public void FindCrossWithXYTest1()
        {
            var eye = new Vector3(0, 0, -20);
            var point = new Vector3(40, 0, 20);
            var v = Helper.FindCrossWithXY(eye, point);
            Assert.AreEqual(new Vector3(20, 0, 0), v);
        }

        [TestMethod()]
        public void RotAroundYTest()
        {
            // angle clock direction
            var vector = new Vector3(-1, 0, 0);
            var angle = Math.PI / 2; // 90 degrees
            var sinAngle = (float)Math.Sin(angle);
            var cosAngle = (float)Math.Cos(angle);
            var v = Helper.RotAroundY(vector, sinAngle, cosAngle);

            var expected = new Vector3(0, 0, 1);
            double delta = 0.001;
            Assert.AreEqual(expected.X, v.X, delta);
            Assert.AreEqual(expected.Y, v.Y, delta);
            Assert.AreEqual(expected.Z, v.Z, delta);
        }

        [TestMethod()]
        public void RotAroundYTest1()
        {
            // angle clock direction
            var vector = new Vector3(1, 0, 0);
            var angle = -Math.PI / 2; // 90 degrees
            var sinAngle = (float)Math.Sin(angle);
            var cosAngle = (float)Math.Cos(angle);
            var v = Helper.RotAroundY(vector, sinAngle, cosAngle);

            var expected = new Vector3(0, 0, 1);
            double delta = 0.001;
            Assert.AreEqual(expected.X, v.X, delta);
            Assert.AreEqual(expected.Y, v.Y, delta);
            Assert.AreEqual(expected.Z, v.Z, delta);
        }

        [TestMethod()]
        public void RotAroundYTest2()
        {
            // angle clock direction
            var vector = new Vector3(1, 0, 0);
            var angle = Math.PI; // 180 degrees
            var sinAngle = (float)Math.Sin(angle);
            var cosAngle = (float)Math.Cos(angle);
            var v = Helper.RotAroundY(vector, sinAngle, cosAngle);

            var expected = new Vector3(-1, 0, 0);
            double delta = 0.001;
            Assert.AreEqual(expected.X, v.X, delta);
            Assert.AreEqual(expected.Y, v.Y, delta);
            Assert.AreEqual(expected.Z, v.Z, delta);
        }

        [TestMethod()]
        public void RotAroundXTest()
        {
            // angle REVERSE clock direction
            var vector = new Vector3(2, 1, 0);
            var angle = Math.PI / 2; // 90 degrees
            var sinAngle = (float)Math.Sin(angle);
            var cosAngle = (float)Math.Cos(angle);
            var v = Helper.RotAroundX(vector, sinAngle, cosAngle);
            Console.WriteLine("v = " + v);

            var expected = new Vector3(2, 0, -1);
            double delta = 0.001;
            Assert.AreEqual(expected.X, v.X, delta);
            Assert.AreEqual(expected.Y, v.Y, delta);
            Assert.AreEqual(expected.Z, v.Z, delta);
        }

        [TestMethod()]
        public void RotAroundXTest1()
        {
            // angle REVERSE clock direction
            var vector = new Vector3(2, 1, 0);
            var angle = -Math.PI / 2; // -90 degrees
            var sinAngle = (float)Math.Sin(angle);
            var cosAngle = (float)Math.Cos(angle);
            var v = Helper.RotAroundX(vector, sinAngle, cosAngle);
            Console.WriteLine("v = " + v);

            var expected = new Vector3(2, 0, 1);
            double delta = 0.001;
            Assert.AreEqual(expected.X, v.X, delta);
            Assert.AreEqual(expected.Y, v.Y, delta);
            Assert.AreEqual(expected.Z, v.Z, delta);
        }

        [TestMethod()]
        public void MakeRotationsTest()
        {
            Vector3 rotations = new Vector3(0, 0, 0);
            Vector3 vector = new Vector3(0, 0, 1);
            var v = Helper.MakeRotations(rotations, vector);

            var expected = new Vector3(0, 0, 1);
            double delta = 0.001;
            Assert.AreEqual(expected.X, v.X, delta);
            Assert.AreEqual(expected.Y, v.Y, delta);
            Assert.AreEqual(expected.Z, v.Z, delta);
        }

        [TestMethod()]
        public void MakeRotationsTest1()
        {
            float deg = (float)Math.PI / 2; // 90 degrees
            Vector3 rotations = new Vector3(0, deg, 0);
            Vector3 vector = new Vector3(0, 0, 1);
            var v = Helper.MakeRotations(rotations, vector);
            Console.WriteLine("v = " + v);

            var expected = new Vector3(1, 0, 0);
            Console.WriteLine("expected = " + expected);
            double delta = 0.001;
            Assert.AreEqual(expected.X, v.X, delta);
            Assert.AreEqual(expected.Y, v.Y, delta);
            Assert.AreEqual(expected.Z, v.Z, delta);
        }

        [TestMethod()]
        public void MakeRotationsTest2()
        {
            float deg = -(float)Math.PI / 2; // -90 degrees
            Vector3 rotations = new Vector3(0, 0, deg);
            Vector3 vector = new Vector3(0, 1, 0);
            var v = Helper.MakeRotations(rotations, vector);
            Console.WriteLine("v = " + v);

            var expected = new Vector3(1, 0, 0);
            Console.WriteLine("expected = " + expected);
            double delta = 0.001;
            Assert.AreEqual(expected.X, v.X, delta);
            Assert.AreEqual(expected.Y, v.Y, delta);
            Assert.AreEqual(expected.Z, v.Z, delta);
        }

        [TestMethod()]
        public void MakeRotationsTest3()
        {
            float deg = (float)Math.PI * 2 * 3 / 4; // 270 degrees
            Vector3 rotations = new Vector3(deg, 0, 0);
            Vector3 vector = new Vector3(0, 0, -1);
            var v = Helper.MakeRotations(rotations, vector);
            Console.WriteLine("v = " + v);

            var expected = new Vector3(0, 1, 0);
            Console.WriteLine("expected = " + expected);
            double delta = 0.001;
            Assert.AreEqual(expected.X, v.X, delta);
            Assert.AreEqual(expected.Y, v.Y, delta);
            Assert.AreEqual(expected.Z, v.Z, delta);
        }

        [TestMethod()]
        public void MakeRotationsTest4()
        {
            float deg = -(float)Math.PI / 2; // -90 degrees
            Vector3 rotations = new Vector3(deg, 0, 0);
            Console.WriteLine("rotations = " + rotations);

            var attitude = Helper.MakeRotations(rotations, Camera.StandardAttitude);
            Console.WriteLine("attitude =" + attitude);

            Vector3 vector = new Vector3(0, 0, 1);
            var v = Helper.MakeRotations(rotations * (-1), vector);
            Console.WriteLine("v = " + v);

            var expected = new Vector3(0, 1, 0);
            Console.WriteLine("expected = " + expected);
            double delta = 0.001;
            Assert.AreEqual(expected.X, v.X, delta);
            Assert.AreEqual(expected.Y, v.Y, delta);
            Assert.AreEqual(expected.Z, v.Z, delta);
        }

        [TestMethod()]
        public void TransformPointToBeginTest()
        {
            Vector3 rotations = new Vector3(0, 0, 0);
            Vector3 center = new Vector3(1, 0, 0);
            Vector3 point = new Vector3(0, 0, 0);

            var v = Helper.TransformPointToBegin(rotations, center, point);
            Console.WriteLine("v = " + v);

            var expected = new Vector3(-1, 0, 0);
            Console.WriteLine("expected = " + expected);
            double delta = 0.001;
            Assert.AreEqual(expected.X, v.X, delta);
            Assert.AreEqual(expected.Y, v.Y, delta);
            Assert.AreEqual(expected.Z, v.Z, delta);
        }

        [TestMethod()]
        public void TransformPointToBeginTest1()
        {
            var deg = (float)Math.PI / 2; // 90 degrees
            Vector3 rotations = new Vector3(0, 0, deg);
            Vector3 center = new Vector3(1, 0, 0);
            Vector3 point = new Vector3(0, 0, 0);

            var v = Helper.TransformPointToBegin(rotations, center, point);
            Console.WriteLine("v = " + v);

            var expected = new Vector3(0, 1, 0);
            Console.WriteLine("expected = " + expected);
            double delta = 0.001;
            Assert.AreEqual(expected.X, v.X, delta);
            Assert.AreEqual(expected.Y, v.Y, delta);
            Assert.AreEqual(expected.Z, v.Z, delta);
        }

        [TestMethod()]
        public void MakeNonRelativeVecFromRelativeTest()
        {
            Vector3 rotations = new Vector3(0, 0, 0);
            Vector3 vector = new Vector3(0, 0, 1);

            var v = Helper.MakeNonRelativeVecFromRelative(rotations, vector);
            Console.WriteLine("v = " + v);

            var expected = new Vector3(0, 0, 1);
            Console.WriteLine("expected = " + expected);
            double delta = 0.001;
            Assert.AreEqual(expected.X, v.X, delta);
            Assert.AreEqual(expected.Y, v.Y, delta);
            Assert.AreEqual(expected.Z, v.Z, delta);
        }

        [TestMethod()]
        public void MakeNonRelativeVecFromRelativeTest1()
        {
            var deg = (float)Math.PI / 2; // 90 degrees
            Vector3 rotations = new Vector3(0, deg, 0);

            var attitude = Helper.MakeRotations(rotations, Camera.StandardAttitude);
            Console.WriteLine("attitude = " + attitude);

            Vector3 vector = new Vector3(0, 0, 1); //straight 

            var v = Helper.MakeNonRelativeVecFromRelative(rotations, vector);
            Console.WriteLine("v = " + v);

            var expected = new Vector3(1, 0, 0);
            Console.WriteLine("expected = " + expected);
            double delta = 0.001;
            Assert.AreEqual(expected.X, v.X, delta);
            Assert.AreEqual(expected.Y, v.Y, delta);
            Assert.AreEqual(expected.Z, v.Z, delta);
            //Assert.Fail();
        }
    }
}