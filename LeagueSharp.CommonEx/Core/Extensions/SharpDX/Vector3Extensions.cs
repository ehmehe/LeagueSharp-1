﻿#region

using System.Collections.Generic;
using System.Linq;
using SharpDX;

#endregion

namespace LeagueSharp.CommonEx.Core.Extensions.SharpDX
{
    /// <summary>
    ///     SharpDX/Vector3 Extensions
    /// </summary>
    public static class Vector3Extensions
    {
        /// <summary>
        ///     Checks for if the extended Vector3 is valid.
        /// </summary>
        /// <param name="vector3">SharpDX Vector3</param>
        /// <returns>Boolean</returns>
        public static bool IsValid(this Vector3 vector3)
        {
            return vector3 != Vector3.Zero;
        }

        /// <summary>
        ///     Normalizes a Vector3.
        /// </summary>
        /// <param name="vector3">SharpDX Vector3</param>
        /// <returns>Normalized Vector3</returns>
        public static Vector3 Normalized(this Vector3 vector3)
        {
            vector3.Normalize();
            return vector3;
        }

        /// <summary>
        ///     Returns the Perpendicular Vector3 to the Extended Vector3.
        /// </summary>
        /// <param name="vector3">Extended SharpDX Vector3</param>
        /// <param name="offset">Axis Offset (0 = X, 1 = Y)</param>
        /// <returns>Perpendicular Vector3</returns>
        public static Vector3 Perpendicular(this Vector3 vector3, int offset = 0)
        {
            return (offset == 0)
                ? new Vector3(-vector3.X, vector3.Y, vector3.Z)
                : new Vector3(vector3.X, -vector3.Y, vector3.Z);
        }

        /// <summary>
        ///     Rotates the Vector3 to a set angle.
        /// </summary>
        /// <param name="vector3">Extended SharpDX Vector2</param>
        /// <param name="angle">Angle (in radians)</param>
        /// <returns>Rotated Vector3</returns>
        public static Vector3 Rotated(this Vector3 vector3, float angle)
        {
            var cos = System.Math.Cos(angle);
            var sin = System.Math.Sin(angle);

            return new Vector3(
                (float) (vector3.X*cos - vector3.Y*sin), (float) (vector3.Y*cos + vector3.X*sin), vector3.Z);
        }

        /// <summary>
        ///     Returns the polar for vector angle (in Degrees).
        /// </summary>
        /// <param name="vector3">Extended SharpDX Vector2</param>
        /// <returns>Polar for Vector Angle (Degrees)</returns>
        public static float Polar(this Vector3 vector3)
        {
            if (System.Math.Abs(vector3.X - 0) <= (float) 1e-9)
            {
                return (vector3.Y > 0) ? 90 : (vector3.Y < 0) ? 270 : 0;
            }

            var theta = (float) (System.Math.Atan((vector3.Y)/(vector3.X))*(180/System.Math.PI));
            if (vector3.X < 0)
            {
                theta += 180;
            }
            if (theta < 0)
            {
                theta += 180;
            }

            return theta;
        }

        /// <summary>
        ///     Converts the points to 2D, then returns the projection of the Vector2 on the segment.
        /// </summary>
        /// <param name="point">Point</param>
        /// <param name="segmentStart">Start of Segment</param>
        /// <param name="segmentEnd">End of Segment</param>
        /// <returns><see cref="ProjectionInfo" /> containing the projection.</returns>
        public static ProjectionInfo ProjectOn(this Vector3 point, Vector3 segmentStart, Vector3 segmentEnd)
        {
            return point.ToVector2().ProjectOn(segmentStart.ToVector2(), segmentEnd.ToVector2());
        }

        /// <summary>
        ///     Gets the total distance of a list of vectors.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Total distance of the path</returns>
        public static float GetPathLength(this List<Vector3> path)
        {
            var distance = 0f;

            for (var i = 0; i < path.Count - 1; i++)
            {
                distance += path[i].Distance(path[i + 1]);
            }

            return distance;
        }

        /// <summary>
        ///     Returns the calculated mangitude of the given Vector3.
        /// </summary>
        /// <param name="vector3">Extended SharpDX Vector3</param>
        /// <returns>Magnitude in float-units</returns>
        public static float Magnitude(this Vector3 vector3)
        {
            return (float) System.Math.Sqrt((vector3.X*vector3.X) + (vector3.Y*vector3.Y) + (vector3.Z*vector3.Z));
        }

        /// <summary>
        ///     Returns the modifed Vector3 with a quick changed Z-axis value.
        /// </summary>
        /// <param name="v">Extended SharpDX Vector3</param>
        /// <param name="value">Switched Z value in float-units</param>
        /// <returns>Modified Vector3.</returns>
        public static Vector3 SetZ(this Vector3 v, float? value = null)
        {
            if (value == null)
            {
                v.Z = Game.CursorPos.Z;
            }
            else
            {
                v.Z = (float) value;
            }
            return v;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3 SwitchYz(this Vector3 v)
        {
            return new Vector3(v.X, v.Z, v.Y);
        }

        /// <summary>
        ///     Returns the total distance of a path.
        /// </summary>
        public static float PathLength(this List<Vector3> path)
        {
            var distance = 0f;
            for (var i = 0; i < path.Count - 1; i++)
            {
                distance += path[i].Distance(path[i + 1]);
            }
            return distance;
        }

        /// <summary>
        ///     Returns if the Vector3 is on the screen.
        /// </summary>
        /// <param name="vector3">Extended SharpDX Vector3</param>
        /// <returns>Is Vector3 on screen</returns>
        public static bool IsOnScreen(this Vector3 vector3)
        {
            var pos = Drawing.WorldToScreen(vector3);
            return pos.X > 0 && pos.X <= Drawing.Width && pos.Y > 0 && pos.Y <= Drawing.Height;
        }

        /// <summary>
        ///     Returns if the Vector3 position is a wall.
        /// </summary>
        /// <param name="vector3">Extended SharpDX Vector3</param>
        /// <returns>Is Vector3 position a wall position</returns>
        public static bool IsWall(this Vector3 vector3)
        {
            return NavMesh.GetCollisionFlags(vector3).HasFlag(CollisionFlags.Wall);
        }

        /// <summary>
        ///     Returns whether the given position is under a turret
        /// </summary>
        /// <param name="position">Extended SharpDX Vector3</param>
        /// <param name="enemyTurretsOnly">Include Enemy Turret Only</param>
        /// <returns>Is Position under a turret</returns>
        public static bool IsUnderTurret(this Vector3 position, bool enemyTurretsOnly)
        {
            return
                ObjectManager.Get<Obj_AI_Turret>().Any(turret => turret.IsValidTarget(950, enemyTurretsOnly, position));
        }

        #region AngleBetween

        /// <summary>
        ///     Returns the angle between two vectors.
        /// </summary>
        /// <param name="vector3">Extended SharpDX Vector3</param>
        /// <param name="toVector3">SharpDX Vector3</param>
        /// <returns>Angle between two vectors in float-units</returns>
        public static float AngleBetween(this Vector3 vector3, Vector3 toVector3)
        {
            var magnitudeA =
                System.Math.Sqrt((vector3.X*vector3.X) + (vector3.Y*vector3.Y) + (vector3.Z*vector3.Z));
            var magnitudeB =
                System.Math.Sqrt(
                    (toVector3.X*toVector3.X) + (toVector3.Y*toVector3.Y) + (toVector3.Z*toVector3.Z));

            var dotProduct = (vector3.X*toVector3.X) + (vector3.Y*toVector3.Y) + (vector3.Z + toVector3.Z);

            return (float) System.Math.Cos(dotProduct/magnitudeA*magnitudeB);
        }

        /// <summary>
        ///     Returns the angle between two vectors.
        /// </summary>
        /// <param name="vector3">Extended SharpDX Vector3</param>
        /// <param name="toVector2">SharpDX Vector2</param>
        /// <returns>Angle between two vectors in float-units</returns>
        public static float AngleBetween(this Vector3 vector3, Vector2 toVector2)
        {
            return AngleBetween(vector3, toVector2.ToVector3());
        }

        /// <summary>
        ///     Returns the angle between two vectors.
        /// </summary>
        /// <param name="vector3">Extended SharpDX Vector3</param>
        /// <param name="toVector4">SharpDX Vector4</param>
        /// <returns>Angle between two vectors in float-units</returns>
        public static float AngleBetween(this Vector3 vector3, Vector4 toVector4)
        {
            return AngleBetween(vector3, toVector4.ToVector3());
        }

        #endregion

        #region IsOrthogonal

        /// <summary>
        ///     Returns if the angle is orthogonal.
        /// </summary>
        /// <param name="vector3">Extended SharpDX Vector3</param>
        /// <param name="toVector3">SharpDX Vector3</param>
        /// <returns>Returns if the angle is orthogonal</returns>
        public static bool IsOrthogonal(Vector3 vector3, Vector3 toVector3)
        {
            return System.Math.Abs((vector3.X*toVector3.X) + (vector3.Y*toVector3.Y)) < float.Epsilon;
        }

        /// <summary>
        ///     Returns if the angle is orthogonal.
        /// </summary>
        /// <param name="vector3">Extended SharpDX Vector3</param>
        /// <param name="toVector2">SharpDX Vector2</param>
        /// <returns>Returns if the angle is orthogonal</returns>
        public static bool IsOrthogonal(Vector3 vector3, Vector2 toVector2)
        {
            return IsOrthogonal(vector3, toVector2.ToVector3());
        }

        /// <summary>
        ///     Returns if the angle is orthogonal.
        /// </summary>
        /// <param name="vector3">Extended SharpDX Vector3</param>
        /// <param name="toVector4">SharpDX Vector2</param>
        /// <returns>Returns if the angle is orthogonal</returns>
        public static bool IsOrthogonal(Vector3 vector3, Vector4 toVector4)
        {
            return IsOrthogonal(vector3, toVector4.ToVector3());
        }

        #endregion

        #region Closest

        /// <summary>
        ///     Seeks for the closest Vector3 to the extended Vector3.
        /// </summary>
        /// <param name="vector3">Extended SharpDX Vector3</param>
        /// <param name="array">Vector3 Collection</param>
        /// <returns>Closest Vector3</returns>
        public static Vector3 Closest(this Vector3 vector3, IEnumerable<Vector3> array)
        {
            var result = Vector3.Zero;
            var distance = float.MaxValue;

            foreach (var vector in array)
            {
                var temporaryDistance = vector3.Distance(vector);
                if (distance < temporaryDistance)
                {
                    distance = temporaryDistance;
                    result = vector;
                }
            }

            return result;
        }

        /// <summary>
        ///     Seeks for the closest Vector2 to the extended Vector3.
        /// </summary>
        /// <param name="vector3">Extended SharpDX Vector3</param>
        /// <param name="array">Vector2 Collection</param>
        /// <returns>Closest Vector2</returns>
        public static Vector2 Closest(this Vector3 vector3, IEnumerable<Vector2> array)
        {
            var result = Vector2.Zero;
            var distance = float.MaxValue;

            foreach (var vector in array)
            {
                var temporaryDistance = vector3.Distance(vector);
                if (distance < temporaryDistance)
                {
                    distance = temporaryDistance;
                    result = vector;
                }
            }

            return result;
        }

        /// <summary>
        ///     Seeks for the closest Vector4 to the extended Vector3.
        /// </summary>
        /// <param name="vector3">Extended SharpDX Vector3</param>
        /// <param name="array">Vector4 Collection</param>
        /// <returns>Closest Vector4</returns>
        public static Vector4 Closest(this Vector3 vector3, IEnumerable<Vector4> array)
        {
            var result = Vector4.Zero;
            var distance = float.MaxValue;

            foreach (var vector in array)
            {
                var temporaryDistance = vector3.Distance(vector);
                if (distance < temporaryDistance)
                {
                    distance = temporaryDistance;
                    result = vector;
                }
            }

            return result;
        }

        #endregion

        #region Extend

        /// <summary>
        ///     Extends a Vector3 to another Vector3.
        /// </summary>
        /// <param name="vector3">Extended SharpDX Vector3 (From)</param>
        /// <param name="toVector3">SharpDX Vector3 (To)</param>
        /// <param name="distance">Distance (float units)</param>
        /// <returns>Extended Vector3</returns>
        public static Vector3 Extend(this Vector3 vector3, Vector3 toVector3, float distance)
        {
            return vector3 + distance*(toVector3 - vector3).Normalized();
        }

        /// <summary>
        ///     Extends a Vector3 to a Vector2.
        /// </summary>
        /// <param name="vector3">Extended SharpDX Vector3 (From)</param>
        /// <param name="toVector2">SharpDX Vector2 (To)</param>
        /// <param name="distance">Distance (float units)</param>
        /// <returns>Extended Vector3</returns>
        public static Vector3 Extend(this Vector3 vector3, Vector2 toVector2, float distance)
        {
            return vector3 + distance*(toVector2.ToVector3(vector3.Z) - vector3).Normalized();
        }

        /// <summary>
        ///     Extends a Vector3 to a Vector4.
        /// </summary>
        /// <param name="vector3">Extended SharpDX Vector3 (From)</param>
        /// <param name="toVector4">SharpDX Vector4 (To)</param>
        /// <param name="distance">Distance (float units)</param>
        /// <returns>Extended Vector3</returns>
        public static Vector3 Extend(this Vector3 vector3, Vector4 toVector4, float distance)
        {
            return vector3 + distance*(toVector4.ToVector3() - vector3).Normalized();
        }

        #endregion

        #region ToVector

        /// <summary>
        ///     Transforms an extended Vector3 into a Vector2.
        /// </summary>
        /// <param name="vector3">SharpDX Vector3</param>
        /// <returns>Vector2</returns>
        public static Vector2 ToVector2(this Vector3 vector3)
        {
            return new Vector2(vector3.X, vector3.Y);
        }

        /// <summary>
        ///     Transforms an extended Vector3 into a Vector4.
        /// </summary>
        /// <param name="vector3">SharpDX Vector3</param>
        /// <param name="w">Float W-axis (default = 0f)</param>
        /// <returns>Vector4</returns>
        public static Vector4 ToVector4(this Vector3 vector3, float w = 1f)
        {
            return new Vector4(vector3, w);
        }

        /// <summary>
        ///     Transforms an extended Vector3 List into a Vector2 List.
        /// </summary>
        /// <returns>Vector2 List</returns>
        public static List<Vector2> ToVector2(this List<Vector3> path)
        {
            return path.Select(point => point.ToVector2()).ToList();
        }

        /// <summary>
        ///     Transforms an extended Vector3 List into a Vector4 List.
        /// </summary>
        /// <returns>Vector4 List</returns>
        public static List<Vector4> ToVector4(this List<Vector3> path)
        {
            return path.Select(point => point.ToVector4()).ToList();
        }

        #endregion

        #region Distance

        /// <summary>
        ///     Calculates the distance between the extended Vector3 and a Vector3.
        /// </summary>
        /// <param name="vector3">SharpDX Vector3 (From)</param>
        /// <param name="toVector3">SharpDX Vector3 (To)</param>
        /// <returns>Float Units</returns>
        public static float Distance(this Vector3 vector3, Vector3 toVector3)
        {
            return Vector3.Distance(vector3, toVector3);
        }

        /// <summary>
        ///     Calculates the distance between the extended Vector3 and a Vector3.
        /// </summary>
        /// <param name="vector3">SharpDX Vector3 (From)</param>
        /// <param name="toVector2">SharpDX Vector2 (To)</param>
        /// <returns>Float Units</returns>
        public static float Distance(this Vector3 vector3, Vector2 toVector2)
        {
            return Vector3.Distance(vector3, toVector2.ToVector3());
        }

        /// <summary>
        ///     Calculates the distance between the extended Vector3 and a Vector4.
        /// </summary>
        /// <param name="vector3">SharpDX Vector3 (From)</param>
        /// <param name="toVector4">SharpDX Vector4 (To)</param>
        /// <returns>Float Units</returns>
        public static float Distance(this Vector3 vector3, Vector4 toVector4)
        {
            return Vector3.Distance(vector3, toVector4.ToVector3());
        }

        #endregion

        #region Distance Squared

        /// <summary>
        ///     Calculates the squared distance between two vectors.
        /// </summary>
        /// <param name="vector3">Extended SharpDX Vector3</param>
        /// <param name="toVector3">SharpDX Vector3</param>
        /// <returns>The squared distance between the two vectors.</returns>
        public static float DistanceSquared(this Vector3 vector3, Vector3 toVector3)
        {
            return Vector3.DistanceSquared(vector3, toVector3);
        }

        /// <summary>
        ///     Calculates the squared distance between two vectors.
        /// </summary>
        /// <param name="vector3">Extended SharpDX Vector3</param>
        /// <param name="toVector2">SharpDX Vector2</param>
        /// <returns>The squared distance between the two vectors.</returns>
        public static float DistanceSquared(this Vector3 vector3, Vector2 toVector2)
        {
            return Vector3.DistanceSquared(vector3, toVector2.ToVector3());
        }

        /// <summary>
        ///     Calculates the squared distance between two vectors.
        /// </summary>
        /// <param name="vector3">Extended SharpDX Vector3</param>
        /// <param name="toVector4">SharpDX Vector4</param>
        /// <returns>The squared distance between the two vectors.</returns>
        public static float DistanceSquared(this Vector3 vector3, Vector4 toVector4)
        {
            return Vector3.DistanceSquared(vector3, toVector4.ToVector3());
        }

        #endregion
    }
}