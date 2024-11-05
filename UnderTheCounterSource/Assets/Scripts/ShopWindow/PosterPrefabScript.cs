using UnityEngine;
using UnityEngine.UI;

public class PosterPrefabScript : MonoBehaviour
{
    public Image posterImage; // Reference to the image component for the poster
    public Text posterNameText; // Reference to the text component for the poster name
    public Text posterPriceText; // Reference to the text component for the poster price
    public Image posterIcon; // Reference to the image component for the icon

    // Method to set poster data
    public void SetPosterData(Sprite image, string name, string price, Sprite icon)
    {
        posterImage.sprite = image;
        posterNameText.text = name;
        posterPriceText.text = price;
        posterIcon.sprite = icon;
    }
}