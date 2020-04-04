using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace GKFOpenTk
{
    public class Matrix3x3
    {
        public float M11; // M + row + column
        public float M12;
        public float M13;

        public float M21;
        public float M22;
        public float M23;

        public float M31;
        public float M32;
        public float M33;

        public static Matrix3x3 operator * (Matrix3x3 a, Matrix3x3 b)
        {
            return new Matrix3x3
            {
                // first row
                M11 = a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31,
                M12 = a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32,
                M13 = a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33,

                // scnd row
                M21 = a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31,
                M22 = a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32,
                M23 = a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33,

                // third row
                M31 = a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31,
                M32 = a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32,
                M33 = a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33,
            };
        }

        public static Vector3 operator * (Matrix3x3 m, Vector3 v)
        {
            return new Vector3
            {
                X = m.M11 * v.X + m.M12 * v.Y + m.M13 * v.Z,
                Y = m.M21 * v.X + m.M22 * v.Y + m.M23 * v.Z,
                Z = m.M31 * v.X + m.M32 * v.Y + m.M33 * v.Z
            };
        }

    }
}
