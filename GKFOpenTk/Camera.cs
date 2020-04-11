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

        public Plane Plane;    
        public Vector3 Attitude;
        public float Distance;
        private Vector2 ScenePoint;

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
            this.Distance = 400;
            this.Position = new Vector3(0, 0, -700);
            this.Attitude = Vector3.Normalize(new Vector3(0, 0, 1.0f));
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
            // Console.WriteLine("Aprim = " + Aprim);

            Vector3 p = Position + Vector3.Multiply(Distance, Attitude);
            Aprim -= p;
            //
            var oXRot = Matrix4x4.Transpose(this.GetOXRotMatrix());
            var oYRot = Matrix4x4.Transpose(this.GetOYRotMatrix());
            var transl = Matrix4x4.Transpose(this.GetEyeTranslMatrix());

            var m = transl * (oXRot * oYRot);

            var vec = Vector4.Transform(new Vector4(Aprim, 0), m);
            //Console.WriteLine("m2 = " + vec);
            var result = new Vector2(vec.X, vec.Y);
            return result;
        }
        public bool isVisible(Vector3 a)
        {
            Vector3 CA = new Vector3((Position.X - a.X), (Position.Y - a.Y), (Position.Z - a.Z));

            var u = CA;
            var v = Attitude;

            var u2d = new Vector2(u.Y, u.Z);
            var v2d = new Vector2(v.Y, v.Z);

            float cosEspilon = (u2d.X * v2d.X + u2d.Y * v2d.Y) / (u2d.Length() * v2d.Length());
            float sinEpsilon = (u2d.X * v2d.Y - u2d.Y * v2d.X) / (u2d.Length() * v2d.Length());
            return false;
        }

        public float Cos(Vector2 u, Vector2 v)
        {
            return (u.X * v.X + u.Y * v.Y) / (u.Length() * v.Length());

        }
        public float Sin(Vector2 u, Vector2 v)
        {
            return (u.X * v.Y - u.Y * v.X) / (u.Length() * v.Length());
        }

        public Matrix4x4 GetOXRotMatrix() {
            var Ca = Attitude;
            // translate by OX
            float cosAlfa = (-Ca.Z) / (float)Math.Sqrt(Ca.Y * Ca.Y + Ca.Z * Ca.Z);
            float sinAlfa = (-Ca.Y) / (float)Math.Sqrt(Ca.Y * Ca.Y + Ca.Z * Ca.Z);

            var u = Attitude;
            var v = new Vector3(0, 0, 1);

            var u2d = new Vector2(u.Y, u.Z);
         //   Console.WriteLine("u2d = " + u2d);
            var v2d = new Vector2(v.Y, v.Z);
          //  Console.WriteLine("v2d = " + v2d);
            if (u2d.Length() == 0)
            // nothing to change
            {
                return new Matrix4x4
                {
                    M11 = 1,
                    M22 = 1,
                    M33 = 1,
                    M44 = 1
                };
            }

            float cosEspilon = (u2d.X * v2d.X + u2d.Y * v2d.Y) / (u2d.Length() * v2d.Length());
            float sinEpsilon = (u2d.X * v2d.Y - u2d.Y * v2d.X) / (u2d.Length() * v2d.Length());
            cosAlfa = cosEspilon;
            sinAlfa = sinEpsilon;

            var mOXRot = new Matrix4x4 {
                // first row
                M11 = 1,
                M12 = 0,
                M13 = 0,
                M14 = 0,

                // scnd row
                M21 = 0,
                M22 = cosAlfa,
                M23 = -sinAlfa,
                M24 = 0,

                // third row
                M31 = 0,
                M32 = sinAlfa,
                M33 = cosAlfa,
                M34 = 0,

                // fourth row
                M41 = 0,
                M42 = 0,
                M43 = 0,
                M44 = 1
            };
            return mOXRot;
         }

        
        public Matrix4x4 GetRevOXRotMatrix()
        {
            var Ca = Attitude;

            Console.WriteLine("Ca.Y * Ca.Y + Ca.Z * Ca.Z = " + ((float)Math.Sqrt(Ca.Y * Ca.Y + Ca.Z * Ca.Z)).ToString());
            // translate by OX
            
            float cosAlfa = (-Ca.Z) / (float)Math.Sqrt(Ca.Y * Ca.Y + Ca.Z * Ca.Z);
            float sinAlfa = (-Ca.Y) / (float)Math.Sqrt(Ca.Y * Ca.Y + Ca.Z * Ca.Z);

            var u = Attitude;
            var v = new Vector3(0, 0, 1);

            float cosEspilon = (u.X * v.X + u.Y + v.Y + u.Z + v.Z) / (u.Length() + v.Length());
            float sinEpsilon = (float)Math.Sqrt(1 - cosEspilon * cosEspilon);
            cosAlfa = cosEspilon;
            sinAlfa = sinEpsilon;


            var mOXRot = new Matrix4x4
            {
                // first row
                M11 = 1,
                M12 = 0,
                M13 = 0,
                M14 = 0,

                // scnd row
                M21 = 0,
                M22 = cosAlfa,
                M23 = sinAlfa,
                M24 = 0,

                // third row
                M31 = 0,
                M32 = -sinAlfa,
                M33 = cosAlfa,
                M34 = 0,

                // fourth row
                M41 = 0,
                M42 = 0,
                M43 = 0,
                M44 = 1
            };
            return mOXRot;
        }

        public Matrix4x4 GetOYRotMatrix() {
            var Ca = Attitude;
            // translate by OY
            float cosBeta = (-Ca.Z) / (float)Math.Sqrt(Ca.X * Ca.X + Ca.Z * Ca.Z);
            float sinBeta = (-Ca.X) / (float)Math.Sqrt(Ca.X * Ca.X + Ca.Z * Ca.Z);

            var u = Attitude;
            var v = new Vector3(0, 0, 1);

            var u2d = new Vector2(u.X, u.Z);
            var v2d = new Vector2(v.X, v.Z);
            if (u2d.Length() == 0)
                // nothing to change
            {
                return new Matrix4x4
                {
                    M11 = 1,
                    M22 = 1,
                    M33 = 1,
                    M44 = 1
                };
            }


            float cosEspilon = (u2d.X * v2d.X + u2d.Y * v2d.Y) / (u2d.Length() * v2d.Length());
            float sinEpsilon = (u2d.Y * v2d.X - u2d.X * v2d.Y) / (u2d.Length() * v2d.Length());
            cosBeta = cosEspilon;
            sinBeta = sinEpsilon;

            var mOYRot = new Matrix4x4 {
                // first row
                M11 = cosBeta,
                M12 = 0,
                M13 = sinBeta,
                M14 = 0,

                // scnd row
                M21 = 0,
                M22 = 1,
                M23 = 0,
                M24 = 0,

                // third row
                M31 = -sinBeta,
                M32 = 0,
                M33 = cosBeta,
                M34 = 0,

                // fourth row
                M41 = 0,
                M42 = 0,
                M43 = 0,
                M44 = 1
            };
            return mOYRot;
        }

        public Matrix4x4 GetOYRotMatrix(float cosAlfa, float sinAlfa)
        {
            return new Matrix4x4
            {
                // first row
                M11 = cosAlfa,
                M12 = 0,
                M13 = sinAlfa,
                M14 = 0,

                // scnd row
                M21 = 0,
                M22 = 1,
                M23 = 0,
                M24 = 0,

                // third row
                M31 = -sinAlfa,
                M32 = 0,
                M33 = cosAlfa,
                M34 = 0,

                // fourth row
                M41 = 0,
                M42 = 0,
                M43 = 0,
                M44 = 1
            };
        }
        public Matrix4x4 GetRevOYRotMatrix()
        {
            var Ca = Attitude;
            // translate by OY
            float cosBeta = (-Ca.Z) / (float)Math.Sqrt(Ca.X * Ca.X + Ca.Z * Ca.Z);
            float sinBeta = (-Ca.X) / (float)Math.Sqrt(Ca.X * Ca.X + Ca.Z * Ca.Z);

            var u = new Vector3(0, 0, 1);
            var v = Attitude;

            var u2d = new Vector2(u.X, u.Z);
            var v2d = new Vector2(v.X, v.Z);
            Console.WriteLine("v2d=" + v2d.X + " " + v2d.Y);
            if (u2d.Length() == 0)
            // nothing to change
            {
                return new Matrix4x4
                {
                    M11 = 1,
                    M22 = 1,
                    M33 = 1,
                    M44 = 1
                };
            }


            float cosEspilon = (u2d.X * v2d.X + u2d.Y * v2d.Y) / (u2d.Length() * v2d.Length());
            float sinEpsilon = (u2d.Y * v2d.X - u2d.X * v2d.Y) / (u2d.Length() * v2d.Length());
            cosBeta = cosEspilon;
            sinBeta = sinEpsilon;

            var mOYRot = new Matrix4x4
            {
                // first row
                M11 = cosBeta,
                M12 = 0,
                M13 = -sinBeta,
                M14 = 0,

                // scnd row
                M21 = 0,
                M22 = 1,
                M23 = 0,
                M24 = 0,

                // third row
                M31 = sinBeta,
                M32 = 0,
                M33 = cosBeta,
                M34 = 0,

                // fourth row
                M41 = 0,
                M42 = 0,
                M43 = 0,
                M44 = 1
            };
            return mOYRot;
        }

        public Matrix4x4 GetTranslMatrix()
        {
            Vector3 p = Vector3.Multiply(Distance, Attitude);

            return new Matrix4x4 {
                M11 = 1,
                M22 = 1,
                M33 = 1,
                M44 = 1,
                M14 = -p.X,
                M24 = -p.Y,
                M34 = -p.Z
            };
        }
        public Matrix4x4 GetEyeTranslMatrix()
        {
            return new Matrix4x4
            {
                M11 = 1,
                M22 = 1,
                M33 = 1,
                M44 = 1,
                M41 = -Position.X,
                M42 = -Position.Y,
                M43 = -Position.Z
            };
        }
        public Matrix4x4 GetRevEyeTranslMatrix()
        {
            return new Matrix4x4
            {
                M11 = 1,
                M22 = 1,
                M33 = 1,
                M44 = 1,
                M41 = Position.X,
                M42 = Position.Y,
                M43 = Position.Z
            };
        }
        public Vector3 MapRelativeMoveToNonRelativeMove(Vector3 v) {
            var revOXRot = Matrix4x4.Transpose(this.GetOXRotMatrix());
            var revOYRot = Matrix4x4.Transpose(this.GetRevOYRotMatrix());
            var transl = Matrix4x4.Transpose(this.GetRevEyeTranslMatrix());
            Console.WriteLine("oy: " + this.GetRevOYRotMatrix());

            var m = revOYRot;

            var vec = Vector4.Transform(new Vector4(v,0), m);
            //var vec = v;  
            

      //      Console.WriteLine("TMP = " + tmpRes);
            var result = new Vector3(vec.X, vec.Y, vec.Z);
         //   Console.WriteLine("m = \n" + new Vector4(v, 0));
            return result;
        }

        public Vector2 MapPointToPlate(Vector3 v)
        {
            var oXRot = Matrix4x4.Transpose(this.GetOXRotMatrix());
            var oYRot = Matrix4x4.Transpose(this.GetRevOYRotMatrix());
            var transl = Matrix4x4.Transpose(this.GetTranslMatrix());

            var m = transl * (oXRot * oYRot);

            var vec = Vector4.Transform(new Vector4(v, 0), m);
            var result = new Vector2(vec.X, vec.Y);
            return result;
        }
    }
}
