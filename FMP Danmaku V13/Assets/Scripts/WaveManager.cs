using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }
    
    
    public GameObject[] allWaves;
    
    public int currentWave;

    public float[] waveStartTimes;
    
    public PlayableDirector director;
    // Start is called before the first frame update
    
    
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
    
    void Start()
    {
        currentWave = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWave < allWaves.Length)
        {
            if (allWaves[currentWave].transform.childCount == 0)    //only active children?
            {
                Debug.Log("<color=green>WAVE CLEARED!</color>");
                CallNextWave();
            }
        }
        
        
    }

    public void CallNextWave()
    {
        foreach (Transform child in allWaves[currentWave].transform)
        {
            child.gameObject.SetActive(false);
        }

        if (currentWave >= allWaves.Length)
        {
            Debug.LogWarning("No more waves.");
        }
        else
        {
            currentWave++;
            director.time = waveStartTimes[currentWave];
        }
        
    }

    public void JumpToTimestamp(float time)
    {
        director.time = time;
    }
}
