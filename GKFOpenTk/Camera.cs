using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra;

namespace GKFOpenTk
{
    public class Camera
    {
        public Vector3 Position;

        public static Vector3 StandardAttitude = new Vector3(0,0,1);
        public float Distance;
        private Vector2 ScreenDimm;
        public Vector3 Rotations;

        public void SetPoint(Vector3 value)
        {
            this.Position = value;
        }  

        public Vector3 GetAttitude()
        {
            return Vector3.Normalize(Helper.MakeRotations(Rotations, StandardAttitude));
        }

        public Camera(float screenWidth, float screenHeight)
        {
            this.ScreenDimm = new Vector2(screenWidth, screenHeight);
            this.Distance = 400;
            //this.Position = new Vector3(0, 0, -700);
            this.Position = new Vector3(0, 0, -700);
            this.Rotations = new Vector3(0, 0, 0);
        }


        public Vector2 ComputePointOnPlane(Vector3 point)
        {
            var planeCenter = this.Position + this.GetAttitude() * this.Distance;
            var relativePosition = Helper.TransformPointToBegin(this.Rotations, planeCenter, point);
           // var relativePosition = Helper.TransformLayoutToBegin(this.GetAttitude(), planeCenter, point);
            //Console.WriteLine("Attitude = " + this.Attitude);
            //Console.WriteLine("planeCenter = " + planeCenter);
            //Console.WriteLine(point + " -> " + relativePosition);
            
            if (relativePosition.Z < 0)
            {
                // The point is before plane - not display 
                Console.WriteLine("ComputePointOnPlane: relativePosition.Z < 0: "  + relativePosition);
                return new Vector2(0, 0);
            }
            Vector3 eye = new Vector3(0, 0, -this.Distance);
            var crossPoint = Helper.FindCrossWithXY(eye, relativePosition);

            return new Vector2(crossPoint.X, crossPoint.Y);
        }
        public bool IsVisible(Vector3 point)
        {
            var planeCenter = this.Position + this.GetAttitude() * this.Distance;
            var relativePosition = Helper.TransformPointToBegin(Rotations, planeCenter, point);

            return relativePosition.Z >= 0;
        }
    }
}
