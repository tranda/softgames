using UnityEngine;
using TMPro;  // If using TextMeshPro

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;  // Drag your UI text here
    [SerializeField] private float updateInterval = 0.5f;
    
    private float accum = 0f;
    private int frames = 0;
    private float timeLeft;
    
    private static FPSCounter instance;


     void Awake()
    {
        // Singleton pattern to keep only one instance across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    void Start()
    {
        timeLeft = updateInterval;
    }
    
    void Update()
    {
        timeLeft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        frames++;
        
        if (timeLeft <= 0f)
        {
            float fps = accum / frames;
            fpsText.text = $"FPS: {fps:F2}";
            
            timeLeft = updateInterval;
            accum = 0f;
            frames = 0;
        }
    }
}