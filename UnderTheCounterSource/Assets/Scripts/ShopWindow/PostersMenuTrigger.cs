using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class PosterMenuTrigger : MonoBehaviour
{
    public Button openButton;
    public Animator animator; 
    private const string IsOpen = "IsOpen";
    private void Start()
    {
        openButton.onClick.AddListener(ToggleMenu);
        animator.SetBool(IsOpen, false);
        animator.speed = 0.5f;
    }
    private bool isToggling = false;
    public void ToggleMenu()
    {
        // Avoid multiple calls to the coroutine
        if (isToggling) return;
        isToggling = true;
        StartCoroutine(ResetToggling());
        animator.SetBool("IsOpen", !animator.GetBool("IsOpen"));
    }
    private IEnumerator ResetToggling()
    {
        yield return new WaitForSeconds(0.1f);
        isToggling = false;
    }
}