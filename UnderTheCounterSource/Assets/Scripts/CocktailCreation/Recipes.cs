using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CocktailCreation
{
    [CreateAssetMenu(fileName = "new Recipes", menuName = "Recipes")]
    public class Recipes : ScriptableObject, IEnumerable
    {
        public List<Recipe> recipes;
        
        public IEnumerator GetEnumerator()
        {
            return recipes.GetEnumerator();
        }
    }
}
