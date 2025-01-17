

using System;
using System.Collections.Generic;

namespace IntroductionScreen
{
    [Serializable]
    public class Slide
    {
        public string sprite;
        public string caption;
        
        public override string ToString()
        {
            try
            {
                return $"sprite: {sprite}, caption: {caption}";
            }
            catch (Exception e)
            {
                return $"Error :( Exception in Slide.ToString(): {e}";
            }
        }
    }

    [Serializable]
    public class SlideList
    {
        public List<Slide> slides;
    }
}