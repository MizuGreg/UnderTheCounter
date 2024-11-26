using Technical;
using UnityEngine;
using UnityEngine.UI;

public class BlitzTimer : MonoBehaviour
{
    public Image timerBarImage;  // Riferimento all'immagine della barra di progresso
    public float timerDuration = 10f; // Durata totale del timer in secondi
    private float timeRemaining;
    private bool isTimerRunning;
    
    public void StartTimer()
    {
        timeRemaining = timerDuration; // Imposta il tempo iniziale al valore massimo
        isTimerRunning = true;
        timerBarImage.fillAmount = 1f;  // Imposta la barra come completamente piena
    }

    void Update()
    {
        if (isTimerRunning)
        {
            // Riduci il tempo rimanente
            timeRemaining -= Time.deltaTime;

            // Calcola il riempimento della barra come frazione del tempo rimanente rispetto al totale
            float fillAmount = Mathf.Clamp01(timeRemaining / timerDuration);

            // Aggiorna la barra
            timerBarImage.fillAmount = fillAmount;

            // Quando il timer si esaurisce
            if (timeRemaining <= 0f)
            {
                isTimerRunning = false;
                // Puoi aggiungere logica extra qui, ad esempio fine partita o azione specifica
                Debug.Log("Tempo scaduto!");
                EventSystemManager.OnBlitzTimerEnded();
            }
        }
    }
}
