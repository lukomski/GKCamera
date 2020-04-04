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
    class Camera
    {
        public Vector3 Position;

        private Plane Plane;    
        private Vector3 Attitude;
        public float Distance;
        private Vector2 ScenePoint;
        private Vector2 SceneShape;

        public Vector2 GetScenePoint() {
            return ScenePoint;
        }
        public void SetScenePoint(Vector2 value) {
            ScenePoint = value;
        }

        public void SetPoint(Vector3 value)
        {
            this.Position = value;
            CalculatePlane();
        }  

        public Vector3 GetAttitude()
        {
            return Attitude;
        }

        public void SetAttitude(Vector3 value)
        {
            this.Attitude = Vector3.Normalize(value); // allways should be normalized
            CalculatePlane();
        }

        public Camera()
        {
            this.Distance = 300;
            this.Position = new Vector3(0, 0, -400);
            this.Attitude = new Vector3(0, 0, 1.0f);
            this.ScenePoint = new Vector2(0,0);
            CalculatePlane();
        }

        private void CalculatePlane()
        {
            Console.WriteLine("CalculatePlane");
            // set Point owned by the plane
            Vector3 p = Position + Vector3.Multiply(Distance, Attitude);

            // calculate cooficitent d for plane
            float dp = - p.X * Attitude.X - p.Y * Attitude.Y - p.Z * Attitude.Z;

            this.Plane = new Plane(Attitude.X, Attitude.Y, Attitude.Z, dp);
        }

         public Vector2 ComputePointOnPlane(Vector3 point)
        {
            Vector3 A = point; // real point position
            Vector3 C = Position;
            Vector3 Ca = GetAttitude();
            // vector from point of view (camera) to just considering point
            Vector3 CA = new Vector3((C.X - A.X), (C.Y - A.Y), (C.Z - A.Z));

            // find coofictent t that: A' = C + t * CA, where A' is point A casted to the plane
            float t = (-C.X * Ca.X - C.Y * Ca.Y - C.Z * Ca.Z - Plane.D) / (CA.X * Ca.X + CA.Y * Ca.Y + CA.Z * Ca.Z);

            // just calculate A' = C + t * CA
            Vector3 Aprim = C + Vector3.Multiply(t, CA);

            // the center of the plane
            Vector3 Pctr = C + Vector3.Multiply(Distance, Ca);

            // move plane to set center i (OX, OY, OZ) = (0, 0, 0)
            Vector3 Am = Aprim - Pctr;

            // static vector of direction
            Vector3 Z = new Vector3(0,0,1);

            // translate by OX
            float cosAlfa = (-Ca.Z) / (float)Math.Sqrt(Ca.Y * Ca.Y + Ca.Z * Ca.Z);
            float sinAlfa = (-Ca.Y) / (float)Math.Sqrt(Ca.Y * Ca.Y + Ca.Z * Ca.Z);

            var mOXRot = new Matrix3x3 {
                // first row
                M11 = 1,
                M12 = 0,
                M13 = 0,

                // scnd row
                M21 = 0,
                M22 = cosAlfa,
                M23 = -sinAlfa,

                // third row
                M31 = 0,
                M32 = sinAlfa,
                M33 = cosAlfa
            };

            // translate by OY
            float cosBeta = (-Ca.Z) / (float)Math.Sqrt(Ca.X * Ca.X + Ca.Z * Ca.Z);
            float sinBeta = (-Ca.X) / (float)Math.Sqrt(Ca.X * Ca.X + Ca.Z * Ca.Z);

            var mOYRot = new Matrix3x3 {
                // first row
                M11 = cosBeta,
                M12 = 0,
                M13 = sinBeta,

                // scnd row
                M21 = 0,
                M22 = 1,
                M23 = 0,

                // third row
                M31 = -sinBeta,
                M32 = 0,
                M33 = cosBeta
            };

            Vector3 v = (mOXRot * mOYRot) * Am;

            return new Vector2 {
                X = v.X,
                Y = v.Y
            };
        }

        
    }
}
