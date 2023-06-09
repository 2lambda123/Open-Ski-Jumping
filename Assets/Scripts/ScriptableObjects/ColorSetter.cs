using UnityEngine;
using UnityEngine.UI;

namespace OpenSkiJumping.ScriptableObjects
{
    [RequireComponent(typeof(Image))]
    public class ColorSetter : MonoBehaviour
    {
        private Image image;
        private Color color;

        private void Start()
        {
            image = GetComponent<Image>();
        }
    }
}