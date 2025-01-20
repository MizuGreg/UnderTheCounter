using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Technical;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SavedGameData;
using Bar;

namespace Endings
{
    public class FinalDayManager : MonoBehaviour
    {
        public CanvasGroup barContainer;
        private static readonly int IsOpen = Animator.StringToHash("IsOpen");
        private DialogueManager _dialogueManager;
        [SerializeField] private Animator animator;
        private List<Customer> _dailyCustomers;
         private Customer _currentCustomer;
        private string _customerName;
        private Image _currentImage;
        [SerializeField] private CanvasGroup customerCanvas;

        private void Start() 
        {
            EventSystemManager.OnErnestReveal += RevealErnest;

            barContainer.GetComponent<FadeCanvas>().FadeIn();
            EventSystemManager.OnLoadBarView();
            StartCoroutine(WaitAndStartDay());
        }

        private IEnumerator WaitAndStartDay()
        {
            yield return new WaitForSeconds(1f);
            EventSystemManager.OnCustomerEnter();

            _dialogueManager = FindObjectOfType<DialogueManager>();
            _currentImage = customerCanvas.transform.Find("CustomerSprite").gameObject.GetComponent<Image>();
            LoadDialogues();
        }

        private void OnDestroy()
        {
            EventSystemManager.OnErnestReveal -= RevealErnest;
        }

        private void LoadDialogues() 
        {
            string jsonString = File.ReadAllText(Application.streamingAssetsPath + "/EndingsData/FinalDay.json");
            _dailyCustomers = JsonConvert.DeserializeObject<List<Customer>>(jsonString);
            StartCoroutine(ShowCustomer());
        }

        private IEnumerator ShowCustomer()
        {
            yield return new WaitForSeconds(1f);
            _currentCustomer = _dailyCustomers[0];
            _customerName = "B.U. member";
            _currentImage.sprite = Resources.Load("Sprites/Characters/" + _currentCustomer.sprite, typeof(Sprite)) as Sprite;
            customerCanvas.GetComponent<FadeCanvas>().FadeIn();

            yield return new WaitForSeconds(1f);
            DialogueType dialogueType = DialogueType.NoDrink;
            _dialogueManager.StartDialogue(new Dialogue(_customerName, _currentCustomer.lines["greet"]), dialogueType);
            _dailyCustomers.RemoveAt(0);
        }

        private void RevealErnest()
        {
            StartCoroutine(ShowErnest());
        }

        private IEnumerator ShowErnest()
        {
            if (_dailyCustomers.Count == 0)
            {
                animator.SetBool(IsOpen, false);

                yield return new WaitForSeconds(1f);
                EventSystemManager.OnCustomerLeaveSound();
                customerCanvas.GetComponent<FadeCanvas>().FadeOut();

                barContainer.GetComponent<FadeCanvas>().FadeOut();

                EventSystemManager.OnLoadWinScreen();
                yield return new WaitForSeconds(2f);
                GameData.loseType = "good";
                SceneManager.LoadScene("EndingScene");
            }
            else
            {
                yield return new WaitForSeconds(1f);
                _currentCustomer = _dailyCustomers[0];
                _customerName = _currentCustomer.sprite.ToString();
                _currentImage.sprite = Resources.Load("Sprites/Characters/" + _currentCustomer.sprite, typeof(Sprite)) as Sprite;

                yield return new WaitForSeconds(1f);
                DialogueType dialogueType = DialogueType.NoDrink;
                _dialogueManager.StartDialogue(new Dialogue(_customerName, _currentCustomer.lines["greet"]), dialogueType);

                // BUG: first line doesnt show up correctly
                _dailyCustomers.RemoveAt(0);
            }
        }

    }
}