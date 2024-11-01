using System.Collections.Generic;
using UnityEngine;

namespace CocktailCreation
{
    public class CocktailManager : MonoBehaviour
    {
        [SerializeField] private GameObject sbagliato;
        
        public void MakeCocktail(List<IngredientType> list)
        {
            if (list.Contains(IngredientType.Prosecco) && list.Contains(IngredientType.Campari) &&
                list.Contains(IngredientType.Vermouth) && list.Count == 3)
            {
                // TODO: think about instantiating cocktails from Resources
                sbagliato.SetActive(true);
            }
        }
    }
}
