using System.Collections;
using Bar;
using Technical;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShopWindow
{
    public class ShopWindowManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup shopWindowCanvas;
        [SerializeField] private TextMeshProUGUI dayText;
        [SerializeField] private TextMeshProUGUI savingsText;
        
        public struct Poster
        {
            public string PosterName;
            public int price;
        }

        private void Start()
        {
            shopWindowCanvas.GetComponent<FadeCanvas>().FadeIn();
            dayText.text = $"DAY {Day.CurrentDay}";
            savingsText.text = $"${Day.Savings}";
        }

        public void NextScene()
        {
            StartCoroutine(FadeThenNextScene());

        }

        private IEnumerator FadeThenNextScene()
        {
            shopWindowCanvas.GetComponent<FadeCanvas>().FadeOut();
            yield return new WaitForSeconds(1.1f);
            SceneManager.LoadScene("Scenes/TutorialDay1");
        }
    }
}