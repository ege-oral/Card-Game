using UnityEngine;

namespace BezierCurve
{
    /// <summary>
    /// Utility class for calculating points on a quadratic Bezier curve.
    /// </summary>
    public static class BezierUtility
    {
        /// <summary>
        /// Computes a point on a quadratic Bezier curve.
        /// </summary>
        /// <param name="start">The starting point of the curve (P₀).</param>
        /// <param name="control">The control point that influences the curve's shape (P₁).</param>
        /// <param name="end">The ending point of the curve (P₂).</param>
        /// <param name="t">A value between 0 and 1 that determines how far along the curve the point is.</param>
        /// <returns>A Vector3 position on the Bezier curve at the given 't' value.</returns>
        public static Vector3 GetPoint(Vector3 start, Vector3 control, Vector3 end, float t)
        {
            // 't' is a normalized value (0 to 1) representing the interpolation amount along the curve.
            // 'u' is the inverse of 't' (so when 't' is 0, 'u' is 1 and vice versa).
            var u = 1 - t;

            // Square values to calculate blending factors for each point.
            var tt = t * t;  // t²
            var uu = u * u;  // (1 - t)²

            // Quadratic Bezier formula:
            // B(t) = (1 - t)² * P₀ + 2(1 - t)t * P₁ + t² * P₂
            return (uu * start) + (2 * u * t * control) + (tt * end);
        }
    }
}