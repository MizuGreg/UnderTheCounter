using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Technical
{
    public class RaycastThroughInvisible : MonoBehaviour
    {
        public void OnEnable()
        {
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.01f;
        }

        public void OnDisable()
        {
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.0f;
        }
    }
}
