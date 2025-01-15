using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Bar;
using SavedGameData;
using Technical;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using System.IO;
using Extra;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EndOfDay
{
    public class EndOfDayManager : MonoBehaviour
    {
        [Header("Canvases")]
        [SerializeField] private FadeCanvas endOfDayCanvas;
        [SerializeField] private GameObject popupPanel;
        
        [Header("Entry-related objects")]
        [SerializeField] private TextMeshProUGUI dayText;
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private TextMeshProUGUI[] entryTexts;
        [SerializeField] private TextMeshProUGUI[] amountTexts;
        [SerializeField] private TextMeshProUGUI totalAmount;
        
        [SerializeField] private Image stampImage;

        [Header("Buttons")]
        [SerializeField] private Button nextDayButton;
        [SerializeField] private Button gameOverButton;

        [Header("Timing")]
        [SerializeField] private float timeBeforeLines;
        [SerializeField] private float timeBetweenLines;
        [SerializeField] private float timeAfterLines;

        [Header("Other")]
        [SerializeField] private string[] summaryMessages;
        [SerializeField] private float dailyBalance;
        [SerializeField] private int payoffAmount;

        private string endingType;

        void Start()
        {
            EventSystemManager.OnLoadEndOfDay();
            
            endOfDayCanvas.FadeIn();
            popupPanel.SetActive(false);
            nextDayButton.gameObject.SetActive(false);
            gameOverButton.gameObject.SetActive(false);
            
            StartCoroutine(ShowPopup());
        }

        private IEnumerator ShowPopup()
        {
            // Mafia area
            bool payoffToPay = GameData.Choices["MafiaDeal"] && GameData.Choices["PayoffAccepted"] && GameData.CurrentDay == 6;
            payoffAmount = GameData.payoffAmount;
            if (GameData.Choices["MafiaDeal"]) GameData.Supplies = 20;
            
            stampImage.gameObject.SetActive(false);
            dayText.text = "Day " + GameData.CurrentDay;
            string summaryMessage = summaryMessages[GameData.CurrentDay - 1];
            messageText.text = summaryMessage;

            dailyBalance = GameData.TodayEarnings - GameData.Rent - GameData.Food - GameData.Supplies - (payoffToPay ? payoffAmount : 0);

            // Set amounts
            amountTexts[0].text = $"${GameData.TodayEarnings:N0}";
            amountTexts[1].text = $"${GameData.Savings:N0}";
            amountTexts[2].text = $"-${GameData.Rent:N0}";
            amountTexts[3].text = $"-${GameData.Food:N0}";
            amountTexts[4].text = $"-${GameData.Supplies:N0}";
            if (payoffToPay)
            {
                amountTexts[5].text = $"-${payoffAmount}";
            }
            else
            {
                amountTexts[5].text = "you shouldn't be seeing this";
            }
            totalAmount.text = $"${GameData.Savings + dailyBalance:N0}";
            
            
            // Disable *text components* of all objects
            foreach (TextMeshProUGUI entryText in entryTexts)
            {
                entryText.enabled = false;
            }
            foreach (TextMeshProUGUI amountText in amountTexts)
            {
                amountText.enabled = false;
            }
            totalAmount.enabled = false;
            
            // If there's no payoff to be paid, we disable the payoff-related objects too (not just the text components)
            if (!payoffToPay)
            {
                entryTexts[^1].gameObject.SetActive(false);
                amountTexts[^1].gameObject.SetActive(false);
            }

            popupPanel.SetActive(true);
            
            StartCoroutine(DisplayTextsOneByOne());
            
            yield return null;
        }

        public void SkipText()
        {
            timeBeforeLines = 0f;
            timeBetweenLines = 0f;
            timeAfterLines = 0f;
        }

        private IEnumerator DisplayTextsOneByOne()
        {
            yield return new WaitForSeconds(timeBeforeLines);
            CheckAchievement(); // This is here as a hard fix
            for (var i = 0; i < amountTexts.Length; i++)
            {
                entryTexts[i].enabled = true;
                amountTexts[i].enabled = true;
                yield return new WaitForSeconds(timeBetweenLines);
            }
            
            // Displays total
            // yield return new WaitForSeconds(timeBetweenLines); // doubled waiting time
            totalAmount.enabled = true;

            yield return new WaitForSeconds(timeAfterLines);
            
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

            CheckEndOfDay();
        }

        private void CheckAchievement()
        {
            // Achievement
            if (GameData.CurrentDay == 1)
            {
                EventSystemManager.OnTutorialCompleted();
            }

            if (GameData.allCustomersServed)
            {
                EventSystemManager.OnAllCustomersServed();
            }
        }

        private void CheckEndOfDay()
        {
            if (GameData.Savings + dailyBalance < 0 || (GameData.Choices["MafiaDeal"] && !GameData.Choices["PayoffAccepted"]))
            {
                if (GameData.Choices["MafiaDeal"] && !GameData.Choices["PayoffAccepted"])
                {
                    endingType = "mafia";
                }
                else 
                {
                    endingType = "bankrupt";
                }

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
            GameData.EndDay(dailyBalance);

            // UpdateGuestBook(GameData.CurrentDay);

            GameData.CurrentDay++;
            GameData.SaveToJson();
            StartCoroutine(LoadNextScene());
        }

        // private void UpdateGuestBook(int currentDay)
        // {
        //     string jsonString = File.ReadAllText(Application.streamingAssetsPath + "/GuestBookData/GuestBook.json");
        //     GuestList guestList = JsonConvert.DeserializeObject<GuestList>(jsonString);
        //     List<Guest> _guestsData = guestList.guests;
        //     switch (currentDay) 
        //     {
        //         case 1:
        //             _guestsData.Find(guest => guest.name == "Ernest Wade").isUnlocked = true;
        //             break;
        //         case 2:
        //             _guestsData.Find(guest => guest.name == "Margaret Brookside").isUnlocked = true;
        //             _guestsData.Find(guest => guest.name == "Helene Hollis").isUnlocked = true;
        //             _guestsData.Find(guest => guest.name == "Charles Doyle").isUnlocked = true;
        //             _guestsData.Find(guest => guest.name == "Elisabeth Sanford").isUnlocked = true;
        //             _guestsData.Find(guest => guest.name == "Kathryn Lesbihonest").isUnlocked = true;
        //             _guestsData.Find(guest => guest.name == "Eugene Norris").isUnlocked = true;
        //             _guestsData.Find(guest => guest.name == "Doris Norris").isUnlocked = true;
        //             break;
        //         case 3:
        //             _guestsData.Find(guest => guest.name == "Howard Preston").isUnlocked = true;
        //             _guestsData.Find(guest => guest.name == "Luke Spencer").isUnlocked = true;
        //             _guestsData.Find(guest => guest.name == "Gaston Petit").isUnlocked = true;
        //             _guestsData.Find(guest => guest.name == "Willie Truman").isUnlocked = true;
        //             break;
        //         case 4:
        //             _guestsData.Find(guest => guest.name == "Kenneth Ward").isUnlocked = true;
        //             _guestsData.Find(guest => guest.name == "Ernest Dawe").isUnlocked = true;
        //             break;
        //         case 5:
        //             _guestsData.Find(guest => guest.name == "Mafia Goon").isUnlocked = true;
        //             break;
        //     }
        //     string updatedJson = JsonConvert.SerializeObject(guestList, Formatting.Indented);
        //     File.WriteAllText(Application.streamingAssetsPath + "/GuestBookData/GuestBook.json", updatedJson);
        // }

        private IEnumerator LoadGameOverScene()
        {
            endOfDayCanvas.FadeOut();
            EventSystemManager.OnLoadLoseScreen(endingType);
            yield return new WaitForSeconds(2f);
            GameData.loseType = endingType;
            SceneManager.LoadScene("EndingScene");
        }

        private IEnumerator LoadNextScene()
        {
            endOfDayCanvas.FadeOut();
            yield return new WaitForSeconds(1f);

            if (GameData.CurrentDay >= 7)
            {
                endingType = "good";
                EventSystemManager.OnLoadWinScreen();
                yield return new WaitForSeconds(1.1f);
                GameData.loseType = endingType;
                SceneManager.LoadScene("EndingScene");
            }
            else
            {
                SceneManager.LoadScene("ShopWindow");
            }
            
            //SceneManager.LoadScene(GameData.CurrentDay >= 7 ? "VictoryScreen" : "ShopWindow");
        }
        
        public void BackToMainMenu()
        {
            endOfDayCanvas.FadeOut();
            StartCoroutine(WaitBeforeMenu());
        }
        
        private IEnumerator WaitBeforeMenu()
        {
            yield return new WaitForSeconds(1.1f);
            SceneManager.LoadScene("MainMenu");
        }
    }
}
