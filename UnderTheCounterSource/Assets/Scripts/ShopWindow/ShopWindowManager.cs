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
            SceneManager.LoadScene("Scenes/BarView");
        }

        public void AddPoster(int posterID)
        {
            if (!Day.IsPosterActive(posterID))
            {
                Day.CurrentPosters.Add(new Poster(posterID, null, null, 0, null, null, null));
            }

            debugPrint();
        }

        public void debugPrint()
        {
            foreach (Poster poster in Day.CurrentPosters)
            {
                print("Poster hung: " + poster.posterID);
            }
        }

        public void AddPoster(Poster poster)
        {
            if (!Day.IsPosterActive(poster.posterID))
            {
                Day.CurrentPosters.Add(poster);
            }
        }

        public void RemovePoster(int posterID)
        {
            foreach (Poster poster in Day.CurrentPosters)
            {
                if (poster.posterID == posterID)
                {
                    Day.CurrentPosters.Remove(poster);
                    return;
                }
            }
            debugPrint();
        }

        public void RemovePoster(Poster poster)
        {
            if (Day.IsPosterActive(poster.posterID))
            {
                Day.CurrentPosters.Remove(poster);
            }
        }
    }
}