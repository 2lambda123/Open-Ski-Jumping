using UnityEngine;

namespace OpenSkiJumping.ScriptableObjects.Variables
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Variables/StringVariable")]
    public class StringVariable : ScriptableObject
    {
        [SerializeField]
        private string value = "";
        public string Value
        {
            get => value;
            set
            {
                this.value = value;
            } 
        }
    }
}