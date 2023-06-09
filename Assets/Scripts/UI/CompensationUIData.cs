using UnityEngine;

namespace OpenSkiJumping.UI
{
    [CreateAssetMenu(menuName = "ScriptableObjects/CompensationUIData")]
    public class CompensationUIData : ScriptableObject
    {
        public Color[] textColors = new Color[3];
        public Color[] backgroundColors = new Color[3];
    }
}
