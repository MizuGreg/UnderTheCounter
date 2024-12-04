using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Bar;
using Technical;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace EndOfDay
{
    public class EndOfDayManager : MonoBehaviour
    {
        public FadeCanvas endOfDayCanvas;
        public GameObject popupPanel;
        public TextMeshProUGUI dayText;
        public TextMeshProUGUI messageText;
        public TextMeshProUGUI[] entryTexts;
        public TextMeshProUGUI[] amountTexts;
        public Image stampImage;

        public Button nextDayButton;

        public Button gameOverButton;

        public float timeBeforeLines = 1f;
        public float timeBetweenLines = 0.5f;
        public float timeAfterLines = 1f;

        public string[] summaryMessages;

        [System.Serializable]
        public struct PopupData
        {
            public int day;
            public float earnings;
            public float savings;
            public float rent;
            public float food;
            public float supplies;
        }

        public float dailyBalance;

        private PopupData popupData;

        void Start()
        {
            endOfDayCanvas.FadeIn();
            popupPanel.SetActive(false);
            nextDayButton.gameObject.SetActive(false);
            gameOverButton.gameObject.SetActive(false);

            PopulateData();
            StartCoroutine(ShowPopup());
        }

        private void PopulateData()
        {
            popupData.day = Day.CurrentDay;
            popupData.earnings = Day.TodayEarnings;
            popupData.savings = Day.Savings;
            // todo rent, food, alcohol

            popupData.rent = 100;
        }

        private IEnumerator ShowPopup()
        {
            stampImage.gameObject.SetActive(false);

            dayText.text = "Day " + popupData.day;

            string summaryMessage = summaryMessages[popupData.day-1];
            messageText.text = summaryMessage;

            foreach (TextMeshProUGUI text in amountTexts)
            {
                text.text = "";
                text.gameObject.SetActive(false);
            }
            
            yield return new WaitForSeconds(1f);

            dailyBalance = popupData.earnings - popupData.rent - popupData.food - popupData.supplies;

            amountTexts[0].text = $"<b>${popupData.earnings}</b>";
            amountTexts[1].text = $"<b>${popupData.savings}</b>";
            amountTexts[2].text = $"<b>-${popupData.rent}</b>";
            amountTexts[3].text = $"<b>-${popupData.food}</b>";
            amountTexts[4].text = $"<b>-${popupData.supplies}</b>";
            amountTexts[5].text = $"<b>${popupData.savings + dailyBalance}</b>";

            popupPanel.SetActive(true);

            StartCoroutine(DisplayTextsOneByOne());
        }

        private string GetRandomMessage()
        {
            int index = Random.Range(0, summaryMessages.Length);
            return summaryMessages[index];
        }

        private IEnumerator DisplayTextsOneByOne()
        {
            yield return new WaitForSeconds(timeBeforeLines);
            for (var i = 0; i < amountTexts.Length; i++)
            {
                entryTexts[i].gameObject.SetActive(true);
                amountTexts[i].gameObject.SetActive(true);
                yield return new WaitForSeconds(timeBetweenLines);
                if (i == amountTexts.Length - 2) // waits a little bit more before the final entry
                {
                    yield return new WaitForSeconds(timeBetweenLines);
                }
            }

            yield return new WaitForSeconds(timeAfterLines);

            stampImage.gameObject.SetActive(true);
            StartCoroutine(StampEffect());
        }

        private IEnumerator StampEffect()
        {
            stampImage.gameObject.SetActive(true);

            // Configurazioni iniziali
            Vector3 startPosition = stampImage.transform.localPosition + new Vector3(0, 50, 0); // Leggero offset verticale
            Vector3 endPosition = stampImage.transform.localPosition;

            Vector3 startScale = Vector3.one * 1.5f; // Scala iniziale più grande per enfatizzare la forza
            Vector3 impactScale = Vector3.one * 0.9f; // Leggera compressione al momento dell'impatto
            Vector3 endScale = Vector3.one; // Scala finale normale

            stampImage.transform.localPosition = startPosition; // Posizione iniziale
            stampImage.transform.localScale = startScale; // Scala iniziale

            float duration = 0.3f; // Durata del movimento principale
            float bounceDuration = 0.1f; // Durata del rimbalzo
            float elapsedTime = 0;

            // Animazione del movimento verso il foglio
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                stampImage.transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
                stampImage.transform.localScale = Vector3.Lerp(startScale, impactScale, t); // Scala verso una leggera compressione
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Imposta la posizione e la scala di impatto
            stampImage.transform.localPosition = endPosition;
            stampImage.transform.localScale = impactScale;

            // Rimbalzo verso la scala finale
            elapsedTime = 0;
            while (elapsedTime < bounceDuration)
            {
                float t = elapsedTime / bounceDuration;
                stampImage.transform.localScale = Vector3.Lerp(impactScale, endScale, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Scala finale esatta
            stampImage.transform.localScale = endScale;

            CheckEndOFDay();
        }

        private void CheckEndOFDay()
        {

            Day.Savings += dailyBalance;
            Day.TodayEarnings = 0;

            if (Day.Savings < 0)
            {
                gameOverButton.gameObject.SetActive(true);
            }
            else
            {
                nextDayButton.gameObject.SetActive(true);
            }
        }

        public void GameOver()
        {
            StartCoroutine(LoadGameOverScene());
        }

        public void NextDay()
        {
            StartCoroutine(LoadNextScene());
            Day.CurrentDay++;
        }

        private IEnumerator LoadGameOverScene()
        {
            endOfDayCanvas.FadeOut();
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("GameOverScreen");
        }

        private IEnumerator LoadNextScene()
        {
            endOfDayCanvas.FadeOut();
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("ShopWindow");
        }
    }
}
