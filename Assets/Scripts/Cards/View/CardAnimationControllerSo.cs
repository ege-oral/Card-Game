using UnityEngine;

namespace Cards.View
{
    [CreateAssetMenu(fileName = "CardAnimationControllerSo", menuName = "Cards/CardAnimationControllerSo")]
    public class CardAnimationControllerSo : ScriptableObject
    {
        [Header("Bezier Control Points")]
        [SerializeField] public Transform startPoint;   // The starting position (Deck area)
        [SerializeField] public Transform controlPoint; // The Bezier curve control point
        [SerializeField] public Transform endPoint;     // The final hand position
        
        [Header("Card Settings")]
        [SerializeField] public float drawSpeed = 0.5f;
        [SerializeField] public float zRotationRange = 30;
    }
}