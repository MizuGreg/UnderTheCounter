using Technical;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using Achievements;

namespace Extra {
    public class GuestBookManager : MonoBehaviour {
        [SerializeField] private FadeCanvas guestBook;
        [SerializeField] private FadeCanvas achievementsBook;
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;
        private int currentCustomerIndex;
        private List<Guest> _guestsData;
        private Guest currentCustomer;

        [SerializeField] private Button guestsButton;
        [SerializeField] private Button achievementsButton;

        private Vector3 originalGuestsButtonPosition;
        private Vector3 originalAchievementsButtonPosition;
        private Vector3 selectedGuestsButtonPosition;
        private Vector3 selectedAchievementsButtonPosition;

        private float buttonPositionShift = -25f;

        private Canvas currentlyOpenedPage;
        
        [SerializeField] private Canvas guestsPage;
        [SerializeField] private Canvas achievementsPage;

        private void Start() {
            originalGuestsButtonPosition = guestsButton.transform.localPosition;
            originalAchievementsButtonPosition = achievementsButton.transform.localPosition;
            selectedGuestsButtonPosition = originalGuestsButtonPosition + new Vector3(buttonPositionShift, 0, 0);
            selectedAchievementsButtonPosition = originalAchievementsButtonPosition + new Vector3(buttonPositionShift, 0, 0);

            // LoadGuestBook();
        }

        public void LoadGuestBook() {
            // open guests tab by default
            OpenGuestsTab();

            leftButton.gameObject.SetActive(false);
            rightButton.gameObject.SetActive(true);

            // Load guest book data
            GuestList guestList;
            if (!PlayerPrefs.HasKey("GuestBook"))
            {
                string jsonString = Resources.Load<TextAsset>("TextAssets/GuestBookData/GuestBook").text;
                guestList = JsonConvert.DeserializeObject<GuestList>(jsonString);
                PlayerPrefs.SetString("GuestBook", jsonString);
            }
            else
            {
                guestList = JsonConvert.DeserializeObject<GuestList>(PlayerPrefs.GetString("GuestBook"));
            }

            _guestsData = guestList.guests;
            
            // Load first guest
            currentCustomerIndex = 0;
            currentCustomer = _guestsData.Find(guest => guest.index == currentCustomerIndex);
            if (currentCustomer.isUnlocked)
            {
                ShowGuest();
            }
            else
            {
                ShowLockedGuest();
            }
        }

        public void ShowGuest() {
            // hardcode it in the case of the mafia goon
            if (currentCustomer.name == "Mafia Goon")
            {
                guestBook.transform.Find("CustomerName").GetComponent<TextMeshProUGUI>().text = currentCustomer.name;
            }
            else
            {
                guestBook.transform.Find("CustomerName").GetComponent<TextMeshProUGUI>().text = currentCustomer.name.Split(' ')[0];
            }
            guestBook.transform.Find("PhotoFrame").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/GuestBook/Char unlocked/" + currentCustomer.name.Split(' ')[0]);
            guestBook.transform.Find("Name").GetChild(0).GetComponent<TextMeshProUGUI>().text = currentCustomer.name;
            guestBook.transform.Find("Age").GetChild(0).GetComponent<TextMeshProUGUI>().text = currentCustomer.age;
            guestBook.transform.Find("Height").GetChild(0).GetComponent<TextMeshProUGUI>().text = currentCustomer.height;
            guestBook.transform.Find("Status").GetChild(0).GetComponent<TextMeshProUGUI>().text = currentCustomer.status;
            guestBook.transform.Find("Job").GetChild(0).GetComponent<TextMeshProUGUI>().text = currentCustomer.job;
            guestBook.transform.Find("FavouriteDrink").GetChild(0).GetComponent<TextMeshProUGUI>().text = currentCustomer.favouriteDrink;
            guestBook.transform.Find("Description").GetChild(0).GetComponent<TextMeshProUGUI>().text = currentCustomer.description;
        }

        public void ShowLockedGuest() {
            guestBook.transform.Find("CustomerName").GetComponent<TextMeshProUGUI>().text = "???";
            guestBook.transform.Find("PhotoFrame").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/GuestBook/Char locked/Tenser ___");
            guestBook.transform.Find("PhotoFrame").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/GuestBook/Char locked/" + currentCustomer.name.Split(' ')[0] + " ___");
            guestBook.transform.Find("Name").GetChild(0).GetComponent<TextMeshProUGUI>().text = "???";
            guestBook.transform.Find("Age").GetChild(0).GetComponent<TextMeshProUGUI>().text = "???";
            guestBook.transform.Find("Height").GetChild(0).GetComponent<TextMeshProUGUI>().text = "???";
            guestBook.transform.Find("Status").GetChild(0).GetComponent<TextMeshProUGUI>().text = "???";
            guestBook.transform.Find("Job").GetChild(0).GetComponent<TextMeshProUGUI>().text = "???";
            guestBook.transform.Find("FavouriteDrink").GetChild(0).GetComponent<TextMeshProUGUI>().text = "???";
            guestBook.transform.Find("Description").GetChild(0).GetComponent<TextMeshProUGUI>().text = "???";
        }

        public void NextGuest() {
            currentCustomerIndex++;
            if (currentCustomerIndex == _guestsData.Count - 1)
            {
                rightButton.gameObject.SetActive(false);
            }
            else if (currentCustomerIndex != 0)
            {
                leftButton.gameObject.SetActive(true);
            }
            currentCustomer = _guestsData.Find(guest => guest.index == currentCustomerIndex);
            if (currentCustomer.isUnlocked)
            {
                ShowGuest();
            }
            else
            {
                ShowLockedGuest();
            }
        }

        public void PreviousGuest() {
            currentCustomerIndex--;
            if (currentCustomerIndex == 0)
            {
                leftButton.gameObject.SetActive(false);
            }
            else if (currentCustomerIndex != _guestsData.Count - 1)
            {
                rightButton.gameObject.SetActive(true);
            }
            currentCustomer = _guestsData.Find(guest => guest.index == currentCustomerIndex);
            if (currentCustomer.isUnlocked)
            {
                ShowGuest();
            }
            else
            {
                ShowLockedGuest();
            }
        }

        public void OpenGuestsTab()
        {
            if (currentlyOpenedPage == guestsPage) return;
            currentlyOpenedPage = guestsPage;
            
            guestsPage.gameObject.SetActive(true);
            achievementsPage.gameObject.SetActive(false);

            // leftButton.gameObject.SetActive(false);
            // rightButton.gameObject.SetActive(true);

            guestsButton.GetComponent<RectTransform>().localPosition = selectedGuestsButtonPosition;
            achievementsButton.GetComponent<RectTransform>().localPosition = originalAchievementsButtonPosition;
        }

        public void OpenAchievementsTab()
        {
            if (currentlyOpenedPage == achievementsPage) return;
            currentlyOpenedPage = achievementsPage;

            LoadAchievements();
            
            guestsPage.gameObject.SetActive(false);
            achievementsPage.gameObject.SetActive(true);
            
            guestsButton.GetComponent<RectTransform>().localPosition = originalGuestsButtonPosition;
            achievementsButton.GetComponent<RectTransform>().localPosition = selectedAchievementsButtonPosition;
        }

        public void LoadAchievements()
        {
            List<Achievement> achievements;
            if (!PlayerPrefs.HasKey("Achievements"))
            {
                // Read Achievements JSON and create achievements list
                string jsonString = Resources.Load<TextAsset>("TextAssets/AchievementData/Achievements").text;
                achievements = JsonConvert.DeserializeObject<AchievementList>(jsonString).achievements;
                PlayerPrefs.SetString("Achievements", jsonString);
            }
            else
            {
                string jsonString = PlayerPrefs.GetString("Achievements");
                achievements = JsonConvert.DeserializeObject<AchievementList>(jsonString).achievements;
            }
            
            foreach (var achievement in achievements)
            {
                GameObject targetObject = FindChildByName(achievementsPage.gameObject, achievement.title);
                Image targetImage = targetObject.GetComponent<Image>();
                if (achievement.isUnlocked)
                {
                    Color newColor = targetImage.color;
                    newColor.a = 1f;
                    targetImage.color = newColor;
                }
            }
        }

        private GameObject FindChildByName(GameObject parent, string childName)
        {
        foreach (Transform child in parent.transform)
        {
            if (child.name == childName)
                return child.gameObject;

            GameObject result = FindChildByName(child.gameObject, childName);
            if (result != null)
                return result;
        }
        return null;
        }

        public void ResetGuests()
        {
            string jsonString = PlayerPrefs.GetString("GuestBook");
            GuestList guestList = JsonConvert.DeserializeObject<GuestList>(jsonString);
            _guestsData = guestList.guests;
            foreach (var guest in _guestsData)
            {
                guest.isUnlocked = false;
            }
            string updatedJson = JsonConvert.SerializeObject(guestList, Formatting.Indented);
            PlayerPrefs.SetString("GuestBook", updatedJson);
        }

    }
}