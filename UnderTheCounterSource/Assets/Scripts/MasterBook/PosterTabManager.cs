using System.Collections.Generic;
using System.Linq;
using SavedGameData;
using ShopWindow;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MasterBook
{
    public class PosterTabManager : MonoBehaviour
    {
        [SerializeField] private GameObject poster1Object;
        [SerializeField] private GameObject poster2Object;
        [SerializeField] private GameObject ownedPostersObject;
        
        [SerializeField] private Sprite hiddenPosterSprite;
        
        public void PopulatePosters()
        {
            /*
            Poster poster1 = GameData.Posters.Find(p => p.hanged == 1) as Poster;
            PopulateObjectWithPosterInfo(poster1Object, poster1);
            
            Poster poster2 = GameData.Posters.Find(p => p.hanged == 2) as Poster;
            PopulateObjectWithPosterInfo(poster2Object, poster2);

            PopulateUnlockedPosters();
            */
            
            List<Poster> dummyList = new List<Poster>();
            dummyList.Add(new Poster(0, null, "biafgbiyaf", -1, "hfhsdiof", "hifhifu", "iusdhbuifds", 0, true));
            dummyList.Add(new Poster(2, null, "biafgbiyaf", -1, "hfhsdiof", "hifhifu", "iusdhbuifds", 1, true));
            
            Poster poster1 = dummyList.Find(p => p.hanged == 1);
            PopulateObjectWithPosterInfo(poster1Object, poster1);
            
            Poster poster2 = dummyList.Find(p => p.hanged == 2);
            PopulateObjectWithPosterInfo(poster2Object, poster2);

            PopulateUnlockedPosters();
        }

        public void PopulateObjectWithPosterInfo(GameObject posterObject, Poster poster)
        {
            if (poster != null)
            {
                posterObject.transform.Find("Image").GetComponent<Image>().sprite = poster.image;
                posterObject.transform.Find("PosterName").GetComponent<TextMeshProUGUI>().text = poster.name;
                posterObject.transform.Find("Buff").GetComponent<TextMeshProUGUI>().text = poster.buff;
                posterObject.transform.Find("Nerf").GetComponent<TextMeshProUGUI>().text = poster.nerf;
            }
            else
            {
                posterObject.transform.Find("Image").GetComponent<Image>().sprite = hiddenPosterSprite;
                posterObject.transform.Find("PosterName").GetComponent<TextMeshProUGUI>().text = "NO POSTER HANGED";
                posterObject.transform.Find("Buff").GetComponent<TextMeshProUGUI>().text = "---";
                posterObject.transform.Find("Nerf").GetComponent<TextMeshProUGUI>().text = "---";
            }
        }

        public void PopulateUnlockedPosters()
        {
            /*
            int count = 0;
            foreach (Poster poster in GameData.Posters)
            {
                if (poster.visible)
                {
                    ownedPostersObject.transform.Find($"Poster{poster.posterID}").GetComponent<Image>().sprite = poster.image;
                    count++;
                }
                else
                {
                    ownedPostersObject.transform.Find($"Poster{poster.posterID}").GetComponent<Image>().sprite = hiddenPosterSprite;
                }
            }
            ownedPostersObject.transform.Find("OwnedCount").GetComponent<TextMeshProUGUI>().text = count.ToString();
            */
            
            List<Poster> dummyList = new List<Poster>();
            dummyList.Add(new Poster(0, null, "biafgbiyaf", -1, "hfhsdiof", "hifhifu", "iusdhbuifds", 0, true));
            dummyList.Add(new Poster(2, null, "biafgbiyaf", -1, "hfhsdiof", "hifhifu", "iusdhbuifds", 1, true));
            
            int count = 0;
            foreach (Poster poster in dummyList)
            {
                if (poster.visible)
                {
                    ownedPostersObject.transform.Find($"Poster{poster.posterID}").GetComponent<Image>().sprite = poster.image;
                    count++;
                }
                else
                {
                    ownedPostersObject.transform.Find($"Poster{poster.posterID}").GetComponent<Image>().sprite = hiddenPosterSprite;
                }
            }
            ownedPostersObject.transform.Find("OwnedCount").GetComponent<TextMeshProUGUI>().text = count.ToString();
        }
    }
}