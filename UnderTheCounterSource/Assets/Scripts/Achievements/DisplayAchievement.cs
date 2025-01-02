using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Achievements
{
    public class DisplayAchievement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private float timeBeforeDisappear;

        private Animator _animator;
        private static readonly int Display = Animator.StringToHash("Display");
        
        void Start()
        {
            _animator = gameObject.GetComponent<Animator>();
        }

        public void SetPopUpValues(string t, string d)
        {
            title.text = t;
            description.text = d;
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
        
        public void ResetPopUp()
        {
            title.text = "";
            description.text = "";
        }
    }
}
