using System;
using UnityEngine;
using Technical;

namespace Tutorial
{
    public class EasterEgg2 : MonoBehaviour
    {
        [SerializeField] private GameObject easterEgg;
        private void Start()
        {
            EventSystemManager.OnNapoli += SpawnEasterEgg;
        }

        private void OnDestroy()
        {
            EventSystemManager.OnNapoli -= SpawnEasterEgg;
        }

        private void SpawnEasterEgg()
        {
            easterEgg.SetActive(true);
        }
    }
}
