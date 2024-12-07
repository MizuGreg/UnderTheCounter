using System.Collections;
using Bar;
using Technical;
using TMPro;
using Tutorial;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace ShopWindow
{
    public class ShopWindowManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasContainer;
        [SerializeField] private TextMeshProUGUI dayText;
        [SerializeField] private TextMeshProUGUI savingsText;
        private TutorialManager2 tutorialManager2;
        
        [SerializeField] public int forceDay = 2;

        private void Start()
        {
            EventSystemManager.OnLoadShopWindow();
            
            canvasContainer.GetComponent<FadeCanvas>().FadeIn();
            tutorialManager2 = GetComponent<TutorialManager2>();
            
            if (forceDay != 0) Day.CurrentDay = forceDay;
            
            dayText.text = $"DAY {Day.CurrentDay}";
            savingsText.text = $"${Day.Savings:N0}";
            if (Day.CurrentDay == 2) StartCoroutine(WaitAndStartTutorial());
        }

        private IEnumerator WaitAndStartTutorial()
        {
            yield return new WaitForSeconds(1f);
            tutorialManager2.StartTutorial();
        }

        public void NextScene()
        {
            StartCoroutine(FadeThenNextScene());

        }

        private IEnumerator FadeThenNextScene()
        {
            canvasContainer.GetComponent<FadeCanvas>().FadeOut();
            yield return new WaitForSeconds(1.1f);
            SceneManager.LoadScene("Scenes/BarView");
        }
        
        public void BackToMainMenu()
        {
            canvasContainer.GetComponent<FadeCanvas>().FadeOut();
            StartCoroutine(WaitBeforeMenu());
        }
        
        private IEnumerator WaitBeforeMenu()
        {
            yield return new WaitForSeconds(1.1f);
            SceneManager.LoadScene("MainMenu");
        }

        public void AddPoster(int posterID)
        {
            if (!Day.IsPosterActive(posterID))
            {
                Day.CurrentPosters.Add(new Poster(posterID, null, null, 0, null, null, null));
            }
            
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