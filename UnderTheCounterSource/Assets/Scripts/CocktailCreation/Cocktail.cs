using System;
using UnityEngine;

namespace CocktailCreation
{
    
    [Serializable]
    public class Cocktail
    {
        public CocktailType type;
        public bool isWatered;
        public string description;
        
        public Cocktail(CocktailType type, bool isWatered)
        {
            this.type = type;
            this.isWatered = isWatered;
            
            switch (type) {
                case CocktailType.Ripple:
                    this.description = "An all-time classic: light, refreshing and a little bit zesty, perfect for those moments when you need take a break. It's a good excuse to enjoy the simplicity of the moment, and is especially loved by young people.";
                    break;
                case CocktailType.Everest:
                    this.description = "Conquer the peak of refreshment with Everest! This bold and refreshing cocktail is perfect for those who seek an intense and balanced taste adorned with the aromas of the Caledon Ridge. So take a sip, and let it take you to the summit.";
                    break;
                case CocktailType.SpringBee:
                    this.description = "Spring Bee is the best drink to lift you up when times get tough! Simple, made with few ingredients and with the iconic honey rim, this sweet and tangy cocktail will turn any day around and put a smile on your face.";
                    break;
                case CocktailType.Parti:
                    this.description = "Raise a glass to good times with Parti, a vibrant mix garnished with a stuffed olive, made for unforgettable moments. Bold, smooth, it evokes elegance. With this in hand, you won't just join the party: you'll own it.";
                    break;
                case CocktailType.Magazine:
                    this.description = "Step into the spotlight with Magazine: a sophisticated blend of ingredients perfectly rafted for those who love a cocktail with style and substance. With every sip, you'll feel like the headline for your own story.";
                    break;
                case CocktailType.Palmera:
                    this.description = "This exotic cocktail originates from South America, but it's well known around the world. Relax in its flowery aroma and spiced taste, and maybe you'll take an imaginary world trip too! Who knows?";
                    break;
                case CocktailType.Barber:
                    this.description = "The Barber got its name because it's said to be the favorite drink of barbers. The reason is clear: the neat glass, the sharp flavor and the icy feeling of mint will leave you feeling like you just got a fresh cut and a nice shave.";
                    break;
                default:
                    this.description = "Terrible cocktail!";
                    break;
            }
        }
    }
}