using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Bar;
using UnityEngine.SceneManagement;

namespace EndOfDay
{
    public class EndOfDayManager : MonoBehaviour
    {
        public GameObject popupPanel;
        public TextMeshProUGUI dayText;
        public TextMeshProUGUI messageText;
        public TextMeshProUGUI[] incomeOutcomeTexts;
        public Image stampImage;

        public Button nextDayButton;

        public float timeBeforeLines = 1f;
        public float timeBetweenLines = 0.5f;
        public float timeAfterLines = 1f;

        public string[] randomMessages = 
        {
            "Random message 1",
            "Random message 2",
            "Random message 3",
            "Random message 4",
            "Random message 5"
        };

        [System.Serializable]
        public class PopupData
        {
            public int day;
            public float earnings;
            public float savings;
            public float rent;
            public float food;
            public float alcohol;
        }

        public PopupData popupData;

        void Start()
        {
            popupPanel.SetActive(false);
            nextDayButton.gameObject.SetActive(false);

            populateData();
            ShowPopupFromData();
        }

        private void populateData()
        {
            popupData.day = Day.CurrentDay;
            popupData.earnings = Day.TodayEarnings;
            popupData.savings = Day.Savings;
            // todo rent, food, alcohol
        }

        public void ShowPopupFromData()
        {
            ShowPopup(popupData.day, popupData.earnings, popupData.savings, popupData.rent, popupData.food, popupData.alcohol);
        }

        public void ShowPopup(int day, float earnings, float savings, float rent, float food, float alcohol)
        {
            stampImage.gameObject.SetActive(false);

            dayText.text = "Day " + day;

            string randomMessage = GetRandomMessage();
            messageText.text = randomMessage;

            foreach (TextMeshProUGUI text in incomeOutcomeTexts)
            {
                text.text = "";
                text.gameObject.SetActive(false);
            }

            incomeOutcomeTexts[0].text = "Earnings: <b>$" + earnings + "</b>";
            incomeOutcomeTexts[1].text = "Savings: <b>$" + savings + "</b>";
            incomeOutcomeTexts[2].text = "Rent: <b>-$" + rent + "</b>";
            incomeOutcomeTexts[3].text = "Food: <b>-$" + food + "</b>";
            incomeOutcomeTexts[4].text = "Alcohol: <b>-$" + alcohol + "</b>";
            incomeOutcomeTexts[5].text = "Total: <b>$" + (earnings + savings - rent - food - alcohol) + "</b>";

            popupPanel.SetActive(true);

            StartCoroutine(DisplayTextsOneByOne());
        }

        private string GetRandomMessage()
        {
            int index = Random.Range(0, randomMessages.Length);
            return randomMessages[index];
        }

        private IEnumerator DisplayTextsOneByOne()
        {
            yield return new WaitForSeconds(3f);
            foreach (TextMeshProUGUI text in incomeOutcomeTexts)
            {
                text.gameObject.SetActive(true);
                yield return new WaitForSeconds(timeBetweenLines);
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

            float duration = 0.2f; // Durata del movimento principale
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

            nextDayButton.gameObject.SetActive(true);
        }

        public void NextDay()
        {
            popupPanel.SetActive(false);
            Day.Savings += Day.TodayEarnings;
            Day.TodayEarnings = 0;
            Day.CurrentDay++;
        
            SceneManager.LoadScene("ShopWindow");
        }
    }
}
