using System.Collections;
using Technical;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShopWindow
{
    public class ShopWindowManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup shopWindowCanvas;
        
        public struct Poster
        {
            public string PosterName;
            public int price;
        }

        private void Start()
        {
            shopWindowCanvas.GetComponent<FadeCanvas>().FadeIn();
        }

        public void NextScene()
        {
            StartCoroutine(FadeThenNextScene());

        }

        private IEnumerator FadeThenNextScene()
        {
            shopWindowCanvas.GetComponent<FadeCanvas>().FadeOut();
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("Scenes/TutorialDay1");
        }
    }
}