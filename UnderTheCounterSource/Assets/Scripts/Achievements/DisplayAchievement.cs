using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Achievements
{
    public class DisplayAchievement : MonoBehaviour
    {
        [SerializeField] private float timeBeforeDisappear;

        private Animator _animator;
        private static readonly int Display = Animator.StringToHash("Display");
        private Image _image;
        
        void Start()
        {
            _animator = gameObject.GetComponent<Animator>();
            _image = gameObject.GetComponent<Image>();
        }

        public void SetPopUpValues(string path)
        {
            _image.sprite = Resources.Load<Sprite>($"Sprites/Achievements/{path}");
            DisplayPopUp();
        }

        public void DisplayPopUp()
        {
            _animator.SetBool(Display, true);
            StartCoroutine(WaitBeforeDisappear());
        }
        
        private IEnumerator WaitBeforeDisappear()
        {
            yield return new WaitForSeconds(timeBeforeDisappear);
            _animator.SetBool(Display, false);
        }
    }
}
