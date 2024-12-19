using System.Collections;
using System.Collections.Generic;
using Bar;
using UnityEngine;

public class OverlayClick : MonoBehaviour
{
    [SerializeField] private RecipeBookManager recipeBookManager;
    
    public void OnClick()
    {
        print("click");
        recipeBookManager.CloseRecipeBook();
    }
}
