using UnityEngine;

namespace BezierCurve
{
    public class BezierVisualizer : MonoBehaviour
    {
        [Header("Control Points")]
        public Transform startPoint;
        public Transform controlPoint;
        public Transform endPoint;

        [Header("Curve Settings")]
        [Range(10, 100)] public int segmentCount = 50;
        public Color curveColor = Color.green;
        
        private void OnDrawGizmos()
        {
            if (startPoint is null || controlPoint is null || endPoint is null) return;

            Gizmos.color = curveColor;
            var previousPoint = startPoint.position;

            for (var i = 1; i <= segmentCount; i++)
            {
                var t = i / (float)segmentCount;
                var currentPoint = BezierUtility.GetPoint(startPoint.position, controlPoint.position, endPoint.position, t);
            
                Gizmos.DrawLine(previousPoint, currentPoint);
                previousPoint = currentPoint;
            }
        }
    }
}