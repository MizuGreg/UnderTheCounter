using UnityEngine;

namespace CocktailCreation
{
    [CreateAssetMenu(fileName = "new Garnish", menuName = "Garnish")]
    public class Garnish : ScriptableObject
    {
        public GarnishType type;
        public Sprite sprite;
    }
}
