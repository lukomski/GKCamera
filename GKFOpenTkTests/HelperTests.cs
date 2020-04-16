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
    }
}