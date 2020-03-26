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
        public Plane Plane { get; }
        public Vector3 Point { get; }

        private Vector3 attitude;

        public Vector3 GetAttitude()
        {
            return attitude;
        }

        public void SetAttitude(Vector3 value)
        {
            attitude = value;
        }

        public Camera()
        {
            this.Plane = new Plane(0, 0, 1, 0);
            this.Point = new Vector3(50, 0, -300);
        }

        
    }
}
