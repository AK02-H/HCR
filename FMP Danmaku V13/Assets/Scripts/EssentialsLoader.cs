using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour
{
    public static EssentialsLoader Instance { get; private set; }
    
    //Should never be destroyed
    
    [Header("Gameplay Essentials")]
    public GameObject gameManager;
    public GameObject player;
    public GameObject waveManager;
    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetupGameplay()    //take argument for level?
    {
        if (GameManager.Instance == null)
        {
            Instantiate(gameManager);
        }

        if (PlayerController.instance == null)
        {
            Instantiate(player);
            //Setup player intro
        }

        if (WaveManager.Instance == null)
        {
            Instantiate(waveManager);
            //setup levels
        }
        
        //setup level, play music and start waves with a delay
    }
}
