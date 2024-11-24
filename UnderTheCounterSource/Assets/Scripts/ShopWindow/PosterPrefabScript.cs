using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PosterPrefabScript : MonoBehaviour
{
    public Image posterImage; // Reference to the image component for the poster
    public Text posterNameText; // Reference to the text component for the poster name
    public Text posterPriceText; // Reference to the text component for the poster price
    public Image posterIcon; // Reference to the image component for the icon
    public float posterBuff; // Reference to the poster buff percentage
    public float posterNerf; // Reference to the poster nerf percentage
    public Text posterDescription; // Reference to the text for the poster description

    // Method to set poster data
    public void SetPosterData(Sprite image, string name, string price, Sprite icon, float buff, float nerf, string description)
    {
        posterImage.sprite = image;
        posterNameText.text = name;
        posterPriceText.text = price;
        posterIcon.sprite = icon;
        posterBuff = buff;
        posterNerf = nerf;
        posterDescription.text = description;
    }
    
    //Hide or Show Poster Price for when in placeholder or when in menu
    public void TogglePosterDetails(bool show)
    {
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            // Check if the GameObject has the "PosterDetail" tag
            if (child.CompareTag("PosterDetail"))
            {
                // Enable or disable the entire GameObject
                child.gameObject.SetActive(show);
            }
        }
    }
}