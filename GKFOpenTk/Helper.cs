﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GKFOpenTk
{
    public static class Helper
    {
        public static Vector3 MakeNonRelativeVecFromRelative(Vector3 rotations, Vector3 vector)
        {
            var rot = rotations;
            var v = Helper.MakeRotations(rotations, vector);
            return v;
        }
        public static Vector3 MakeRotations(Vector3 rotations, Vector3 vector)
        {
            //
            // OX - clock-reverse direction
            // OY - clock direction
            // OZ - clock-reverse direction
            //

            var sinAlfa = (float) Math.Sin(rotations.X);
            var cosAlfa = (float) Math.Cos(rotations.X);
            var mX = Helper.RotXMatrix(sinAlfa, cosAlfa);
            var mXT = Matrix4x4.Transpose(mX);

            var sinBeta = (float)Math.Sin(rotations.Y);
            var cosBeta = (float)Math.Cos(rotations.Y);
            var mY = Helper.RotYMatrix(sinBeta, cosBeta);
            var mYT = Matrix4x4.Transpose(mY);

            var sinGama = (float)Math.Sin(rotations.Z);
            var cosGama = (float)Math.Cos(rotations.Z);
            var mZ = Helper.RotZMatrix(sinGama, cosGama);
            var mZT = Matrix4x4.Transpose(mZ);

            var m = mYT * mXT * mZT;

            var v4 = Vector4.Transform(new Vector4(vector, 1), m);
            var v = new Vector3(v4.X, v4.Y, v4.Z);
            return v;
        }
        public static Vector3 RotAroundY(Vector3 vector, float sinAngle, float cosAngle)
        {
            var m = Helper.RotYMatrix(sinAngle, cosAngle);
            var mT = Matrix4x4.Transpose(m);
            var v4 = Vector4.Transform(new Vector4(vector, 1), mT);
            var v = new Vector3(v4.X, v4.Y, v4.Z);
            return v;
        }
        public static Vector3 RotAroundX(Vector3 vector, float sinAngle, float cosAngle)
        {
            var m = Helper.RotXMatrix(sinAngle, cosAngle);
            var mT = Matrix4x4.Transpose(m);
            var v4 = Vector4.Transform(new Vector4(vector, 1), mT);
            var v = new Vector3(v4.X, v4.Y, v4.Z);
            return v;
        }
        public static Vector3 FindCrossWithXY(Vector3 eye, Vector3 point)
        {
            var CA = new Vector3((point.X  - eye.X), (point.Y - eye.Y), (point.Z - eye.Z));
            float t = (-eye.Z) / (CA.Z);

            var Aprim = eye + Vector3.Multiply(t, CA);
            return Aprim;
        }
        public static Vector3 TransformRelativeMoveToNonRelative(Vector3 toAttitude, Vector3 move)
        {
            var fromAttitude = new Vector3(0, 0, 1);

            var sinAroundX = Helper.SinAroundX(fromAttitude, toAttitude);
            var cosAroundX = Helper.CosAroundX(fromAttitude, toAttitude);
            var oxRot = Helper.RotXMatrix(sinAroundX, cosAroundX);
            var oxTRot = Matrix4x4.Transpose(oxRot);
            Console.WriteLine("oxRot = " + oxRot);

            var sinAroundY = Helper.SinAroundY(fromAttitude, toAttitude);
            var cosAroundY = Helper.CosAroundY(fromAttitude, toAttitude);
            var oyRot = Helper.RotYMatrix(sinAroundY, cosAroundY);
            var oyTRot = Matrix4x4.Transpose(oyRot);
            Console.WriteLine("oyRot = " + oyRot);

            var sinAroundZ = Helper.SinAroundZ(fromAttitude, toAttitude);
            var cosAroundZ = Helper.CosAroundZ(fromAttitude, toAttitude);
            var ozRot = Helper.RotZMatrix(sinAroundZ, cosAroundZ);
            var ozTRot = Matrix4x4.Transpose(ozRot);
            Console.WriteLine("ozRot = " + ozRot);

            var mod = oxTRot * oyTRot * ozTRot;
            var v4 = Vector4.Transform(new Vector4(move, 1), mod);

            return new Vector3(v4.X, v4.Y, v4.Z);
        }
        public static Vector3 TransformPointToBegin(Vector3 rotations, Vector3 center, Vector3 point)
        {

            var p = point - center;
            p = Helper.MakeRotations(rotations * (-1), p);
            return p;
        }
        public static Vector3 TransformLayoutToBegin(Vector3 fromAtittude, Vector3 fromCenter, Vector3 point)
        {
            var toAttitude = new Vector3(0, 0, 1); // static vector

            var sinAroundX = Helper.SinAroundX(fromAtittude, toAttitude);
            var cosAroundX = Helper.CosAroundX(fromAtittude, toAttitude);
            var oxRot = Helper.RotXMatrix(sinAroundX, cosAroundX);
            var oxTRot = Matrix4x4.Transpose(oxRot);
          //  Console.WriteLine("oxRot = " + oxRot);

            var sinAroundY = Helper.SinAroundY(fromAtittude, toAttitude);
          //  Console.WriteLine("sinAroundY = " + sinAroundY);
            var cosAroundY = Helper.CosAroundY(fromAtittude, toAttitude);
          //  Console.WriteLine("cosAroundY = " + cosAroundY);
            var oyRot = Helper.RotYMatrix(sinAroundY, cosAroundY);
            var oyTRot = Matrix4x4.Transpose(oyRot);
          //  Console.WriteLine("oyRot = " + oyRot);

            var sinAroundZ = Helper.SinAroundZ(fromAtittude, toAttitude);
            var cosAroundZ = Helper.CosAroundZ(fromAtittude, toAttitude);
            var ozRot = Helper.RotZMatrix(sinAroundZ, cosAroundZ);
            var ozTRot = Matrix4x4.Transpose(ozRot);
          //  Console.WriteLine("ozRot = " + ozRot);

            var translate = Helper.TranslateMatrix(fromCenter);
            var translateT = Matrix4x4.Transpose(translate);
          //  Console.WriteLine("translate = " + translate);

            var mod = translateT * (oyTRot * ozTRot); // dont transform by OX, becouse it flip up-down
            var v4 = Vector4.Transform(new Vector4(point, 1), mod);

            return new Vector3(v4.X, v4.Y, v4.Z);
        }
        static Matrix4x4 TranslateMatrix(Vector3 point)
        {
            return new Matrix4x4
            {
                M11 = 1,
                M22 = 1,
                M33 = 1,
                M44 = 1,
                M14 = -point.X,
                M24 = -point.Y,
                M34 = -point.Z
            };
        }
        public static Matrix4x4 RotXMatrix(float sinAlfa, float cosAlfa)
        {
            return new Matrix4x4
            {
                // first row
                M11 = 1,

                // scnd row
                M22 = cosAlfa,
                M23 = sinAlfa,

                // third row
                M32 = -sinAlfa,
                M33 = cosAlfa,

                // fourth row
                M44 = 1
            };
        }
        public static Matrix4x4 RotYMatrix(float sinAlfa, float cosAlfa)
        {
            return new Matrix4x4
            {
                // first row
                M11 = cosAlfa,
                M13 = sinAlfa,

                // scnd row
                M22 = 1,

                // third row
                M31 = -sinAlfa,
                M33 = cosAlfa,

                // fourth row
                M44 = 1
            };
        }
        static Matrix4x4 RotZMatrix(float sinAlfa, float cosAlfa)
        {
            return new Matrix4x4
            {
                // first row
                M11 = cosAlfa,
                M12 = -sinAlfa,

                // scnd row
                M21 = sinAlfa,
                M22 = cosAlfa,
                
                M33 = 1,

                // fourth row
                M44 = 1
            };
        }
        static float SinAroundX(Vector3 from, Vector3 to)
        {
            var u = new Vector2(from.Y, from.Z);
            var v = new Vector2(to.Y, to.Z);

            if (u.Length() == 0 || v.Length() == 0)
            {
                return 0;
            }
            return Sin(u, v);
        }
        public static float SinAroundY(Vector3 from, Vector3 to)
        {
            var v = new Vector2(from.X, from.Z);
            var u = new Vector2(to.X, to.Z);

            if (u.Length() == 0 || v.Length() == 0)
            {
                return 0;
            }

            return Sin(u, v);
        }
        static float SinAroundZ(Vector3 from, Vector3 to)
        {
            var u = new Vector2(from.X, from.Y);
            var v = new Vector2(to.X, to.Y);

            if (u.Length() == 0 || v.Length() == 0)
            {
                return 0;
            }

            return Sin(u, v);
        }
        static float CosAroundX(Vector3 from, Vector3 to)
        {
            var u = new Vector2(from.Y, from.Z);
            var v = new Vector2(to.Y, to.Z);

            if (u.Length() == 0 || v.Length() == 0)
            {
                return 1;
            }

            return Cos(u, v);
        }
        public static float CosAroundY(Vector3 from, Vector3 to) 
        {
            var u = new Vector2(from.X, from.Z);
            var v = new Vector2(to.X, to.Z);

            if (u.Length() == 0 || v.Length() == 0)
            {
                return 1;
            }

            return Cos(u, v);
        }
        static float CosAroundZ(Vector3 from, Vector3 to)
        {
            var u = new Vector2(from.X, from.Y);
            var v = new Vector2(to.X, to.Y);

            if (u.Length() == 0 || v.Length() == 0)
            {
                return 1;
            }

            return Cos(u, v);
        }
        static public float Cos(Vector2 u, Vector2 v)
        {
            return (u.X * v.X + u.Y * v.Y) / (u.Length() * v.Length());

        }
        static public float Sin(Vector2 u, Vector2 v)
        {
            return (u.X * v.Y - u.Y * v.X) / (u.Length() * v.Length());
        }
    }
}
