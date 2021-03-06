//
// This file is part of the Frenetic Game Engine, created by Frenetic LLC.
// This code is Copyright (C) Frenetic LLC under the terms of a strict license.
// See README.md or LICENSE.txt in the FreneticGameEngine source root for the contents of the license.
// If neither of these are available, assume that neither you nor anyone other than the copyright holder
// hold any right or permission to use this software until such time as the official license is identified.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEPUutilities;
using FGECore.UtilitySystems;
using FreneticUtilities.FreneticToolkit;
using FGECore.MathHelpers;

namespace FGECore.PhysicsSystem
{
    /// <summary>
    /// Utilities related to BEPU physics.
    /// </summary>
    public static class BepuUtilities
    {
        /// <summary>
        /// Converts a quaternion to a byte array.
        /// 16 bytes.
        /// </summary>
        /// <param name="quat">The quaternion.</param>
        /// <returns>The byte array.</returns>
        public static byte[] QuaternionToBytes(in BEPUutilities.Quaternion quat)
        {
            byte[] dat = new byte[4 + 4 + 4 + 4];
            QuaternionToBytes(quat, dat, 0);
            return dat;
        }

        /// <summary>
        /// Converts a quaternion to a byte array.
        /// 16 bytes.
        /// </summary>
        /// <param name="quat">The quaternion.</param>
        /// <param name="outputBytes">The output byte array.</param>
        /// <param name="offset">The starting offset in the output bytes.</param>
        /// <returns>The byte array.</returns>
        public static void QuaternionToBytes(in BEPUutilities.Quaternion quat, byte[] outputBytes, int offset)
        {
            PrimitiveConversionHelper.Float32ToBytes((float)quat.X, outputBytes, offset);
            PrimitiveConversionHelper.Float32ToBytes((float)quat.Y, outputBytes, offset + 4);
            PrimitiveConversionHelper.Float32ToBytes((float)quat.Z, outputBytes, offset + (4 + 4));
            PrimitiveConversionHelper.Float32ToBytes((float)quat.W, outputBytes, offset + (4 + 4 + 4));
        }

        /// <summary>
        /// Converts a byte array to a quaternion.
        /// </summary>
        /// <param name="dat">The byte array.</param>
        /// <param name="offset">The offset in the array.</param>
        /// <returns>The quaternion.</returns>
        public static BEPUutilities.Quaternion BytesToQuaternion(byte[] dat, int offset)
        {
            return new BEPUutilities.Quaternion(
                PrimitiveConversionHelper.BytesToFloat32(dat, offset),
                PrimitiveConversionHelper.BytesToFloat32(dat, offset + 4),
                PrimitiveConversionHelper.BytesToFloat32(dat, offset + (4 + 4)),
                PrimitiveConversionHelper.BytesToFloat32(dat, offset + (4 + 4 + 4))
                );
        }

        /// <summary>
        /// Creates a Matrix that "looks at" a target from a location, left-hand notation.
        /// </summary>
        /// <param name="start">The starting coordinate.</param>
        /// <param name="end">The end target.</param>
        /// <param name="up">The normalized up vector.</param>
        /// <returns>A matrix.</returns>
        public static Matrix LookAtLH(in Location start, in Location end, in Location up)
        {
            Location zAxis = (end - start).Normalize();
            Location xAxis = up.CrossProduct(zAxis).Normalize();
            Location yAxis = zAxis.CrossProduct(xAxis);
            return new Matrix(xAxis.X, yAxis.X, zAxis.X, 0, xAxis.Y,
                yAxis.Y, zAxis.Y, 0, xAxis.Z, yAxis.Z, zAxis.Z, 0,
                -xAxis.Dot(start), -yAxis.Dot(start), -zAxis.Dot(start), 1);
        }

        /// <summary>
        /// Converts a matrix to Euler angles.
        /// </summary>
        /// <param name="WorldTransform">The matrix.</param>
        /// <returns>The Euler angles.</returns>
        public static Location MatrixToAngles(in Matrix WorldTransform)
        {
            Location rot;
            rot.X = Math.Atan2(WorldTransform.M32, WorldTransform.M33) * 180 / Math.PI;
            rot.Y = -Math.Asin(WorldTransform.M31) * 180 / Math.PI;
            rot.Z = Math.Atan2(WorldTransform.M21, WorldTransform.M11) * 180 / Math.PI;
            return rot;
        }

        /// <summary>
        /// Converts Euler angles to a matrix.
        /// </summary>
        /// <param name="rot">The Euler angles.</param>
        /// <returns>The matrix.</returns>
        public static Matrix AnglesToMatrix(in Location rot)
        {
            // TODO: better method?
            return Matrix.CreateFromAxisAngle(new Vector3(1, 0, 0), (rot.X * MathUtilities.PI180))
                    * Matrix.CreateFromAxisAngle(new Vector3(0, 1, 0), (rot.Y * MathUtilities.PI180))
                    * Matrix.CreateFromAxisAngle(new Vector3(0, 0, 1), (rot.Z * MathUtilities.PI180));
        }

        /// <summary>
        /// Projects a vector onto another.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>The projected vector.</returns>
        public static Vector3 Project(in Vector3 a, in Vector3 b)
        {
            return b * (Vector3.Dot(a, b) / b.LengthSquared());
        }
    }
}
