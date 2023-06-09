using UnityEngine;

namespace OpenSkiJumping.ScriptableObjects
{
    [CreateAssetMenu]
    public class DecimalVariable : ScriptableObject
    {
        [SerializeField]
        private decimal value;
        public decimal Value
        {
            get => value;
            set => this.value = value;
        }
    }
}