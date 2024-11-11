using System;

namespace Bar
{
    [Serializable]
    public enum Ingredient
    {
        Verlan,
        CaledonRidge,
        Ferrucci,
        Gryte,
        ShaddockJuice,
        Water
    }

    [Serializable]
    public struct Cocktail
    {
        public CocktailType type;
        public bool wateredDown;

        public Cocktail(CocktailType type, bool wateredDown)
        {
            this.type = type;
            this.wateredDown = wateredDown;
        }
    }

    [Serializable]
    public enum CocktailType
    {
        Ripple,
        Everest,
        SpringBee,
        Parti,
        Magazine,
        Mistake
    }
}