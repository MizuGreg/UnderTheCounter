using System.Collections;
using Technical;
using TMPro;
using UnityEngine;
using Bar;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Blitz
{
    public class BlitzManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup blitzCanvas;
        [SerializeField] private BlitzTimer blitzTimer;
        
        [SerializeField] private FadeCanvas warningPlaceholder;

        [SerializeField] private CanvasGroup barContainer;
        private int placedBottlesCounter;

        [SerializeField] private List<GameObject> placeholderSlots;
        

        private void Start()
        {
            warningPlaceholder.gameObject.SetActive(false);
            
            EventSystemManager.OnBlitzCallWarning += BlitzWarning;
            EventSystemManager.OnBlitzCalled += CallBlitz;
            EventSystemManager.OnBlitzTimerEnded += LossByBlitz;
            EventSystemManager.OnBottlePlaced += IncreasePlacedBottlesCounter;
            EventSystemManager.OnPanelClosed += CheckBlitzWin;
        }

        private void OnDestroy()
        {
            EventSystemManager.OnBlitzCallWarning -= BlitzWarning;
            EventSystemManager.OnBlitzCalled -= CallBlitz;
            EventSystemManager.OnBlitzTimerEnded -= LossByBlitz;
            EventSystemManager.OnBottlePlaced -= IncreasePlacedBottlesCounter;
            EventSystemManager.OnPanelClosed -= CheckBlitzWin;
        }

        private void BlitzWarning()
        {
            StartCoroutine(BlinkPlaceholderText());
        }

        private IEnumerator BlinkPlaceholderText()
        {
            warningPlaceholder.FadeIn();
            yield return new WaitForSeconds(3f);
            warningPlaceholder.FadeOut();
        }
        
        public void CallBlitz()
        {
            StartCoroutine(FadeInBlitz());
        }

        private IEnumerator FadeInBlitz()
        {
            yield return new WaitForSeconds(1f);
            placedBottlesCounter = 0;
            ShufflePlaceholders();
            blitzCanvas.GetComponent<FadeCanvas>().FadeIn();
            StartCoroutine(WaitBeforeHideMinigame());
        }

        private void ShufflePlaceholders()
        {
            string[] prefabGUIDs = AssetDatabase.FindAssets("", new[] { $"Assets/Resources/Prefabs/Blitz" });

            List<GameObject> ingredientPrefabs = new List<GameObject>();
            foreach (string guid in prefabGUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab != null)
                {
                    ingredientPrefabs.Add(prefab);
                }
            }

            Shuffle(ingredientPrefabs);
            Debug.Log(ingredientPrefabs.Count);

            for (int i = 0; i < ingredientPrefabs.Count; i++)
            {
                placeholderSlots[i].tag = ingredientPrefabs[i].GetComponent<PlaceholderScript>().ingredientType.ToString();
                placeholderSlots[i].GetComponent<Image>().sprite = ingredientPrefabs[i].GetComponent<PlaceholderScript>().sprite;
            }
        }

        private void Shuffle(List<GameObject> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int rnd = Random.Range(i, list.Count);
                var temp = list[i];
                list[i] = list[rnd];
                list[rnd] = temp;
            }
        }

        private IEnumerator WaitBeforeHideMinigame()
        {
            yield return new WaitForSeconds(1f);
            blitzTimer.StartTimer();
        }

        private void LossByBlitz()
        {
            barContainer.GetComponent<FadeCanvas>().FadeOut();
            StartCoroutine(LoadLoseScreen());
        }

        private IEnumerator LoadLoseScreen()
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("GameOverScreen");
        }

        private void IncreasePlacedBottlesCounter()
        {
            placedBottlesCounter++;
            CheckBlitzWin();
        }

        private void CheckBlitzWin()
        {
            Debug.Log(placedBottlesCounter);
            if (placedBottlesCounter == AssetDatabase.FindAssets("", new[] { $"Assets/Resources/Prefabs/Blitz" }).Length) 
            {
                blitzCanvas.GetComponent<FadeCanvas>().FadeOut();
                // panel needs to be not movable anymore
                // we need some kind of confirmation to show up for the player, then wait a bit, and then fade out
            }
        }

        private void InspectorInterrogation()
        {
            // start interrogation, then, if all questions answered correctly:
            EventSystemManager.OnBlitzEnd();
        }
    }
}
