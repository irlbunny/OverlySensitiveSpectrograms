using System;
using System.Linq;
using UnityEngine;

namespace OverlySensitiveSpectrograms.Utilities
{
    internal static class VectorUtil
    {
        public enum CoordType { X, Y, Z, W }

        public static CoordType GetMaxCoordType(Vector4 vector)
        {
            var coords = new float[] { vector.x, vector.y, vector.z, vector.w };
            return (CoordType)Array.IndexOf(coords, coords.Max());
        }

        public static float GetCoordValue(Vector4 vector, CoordType coordType)
        {
            return coordType switch
            {
                CoordType.X => vector.x,
                CoordType.Y => vector.y,
                CoordType.Z => vector.z,
                CoordType.W => vector.w,
                _ => 0f
            };
        }

        public static void SetCoordValue(ref Vector4 vector, CoordType coordType, float value)
        {
            switch (coordType)
            {
                case CoordType.X: vector.x = value; break;
                case CoordType.Y: vector.y = value; break;
                case CoordType.Z: vector.z = value; break;
                case CoordType.W: vector.w = value; break;
            }
        }
    }
}
