using UnityEngine;

namespace Cards.View
{
    [CreateAssetMenu(fileName = "CardAnimationControllerSo", menuName = "Cards/CardAnimationControllerSo")]
    public class CardAnimationControllerSo : ScriptableObject
    {
        [Header("Bezier Control Points")]
        [SerializeField] public Vector3 startPoint;   // The starting position (Deck area)
        [SerializeField] public Vector3 controlPoint; // The Bezier curve control point
        [SerializeField] public Vector3 endPoint;     // The final hand position
        
        [Header("Card Settings")]
        [SerializeField] public float drawSpeed = 0.5f;
        [SerializeField] public float cardDrawDelay = 0.1f;
        [SerializeField] public float zRotationRange = 30;
        [SerializeField] public float maxDistance = 2.5f;
        
        public float GetRotationAngle(float t)
        {
            return Mathf.Lerp(zRotationRange, -zRotationRange, t);
        }
    }
}