using System.Collections.Generic;
using UnityEngine;

namespace CocktailCreation
{
    [CreateAssetMenu(fileName = "new Recipe", menuName = "Recipe")]
    public class Recipe : ScriptableObject
    {
        public List<IngredientType> ingredients;
        public GameObject cocktailPrefab;
    }
}
