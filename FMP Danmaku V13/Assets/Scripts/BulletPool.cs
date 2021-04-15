using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool instance;
    
    [System.Serializable]
    public class Pool            //Obsolete?
    {
        public string tag;
        public int size;

        public Pool(String tg, int s)
        {
            tag = tg;
            size = s;
        }
    }

    public List<Pool> poolTracker;    //only used for tracking pool in inspector
    
    public Dictionary<String, Queue<GameObject>> poolDictionary;        //switch to string identifier?

    public Bounds boundaries;    //switch to use?
    
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<String, Queue<GameObject>>();

        foreach (var pool in poolTracker)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            /*
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);*/
        }
    }

    private void Update()
    {
        
        poolTracker.Clear();
        foreach (var VARIABLE in poolDictionary)
        {
            poolTracker.Add(new Pool(VARIABLE.Key, VARIABLE.Value.Count));
        }
    }

    public GameObject SpawnFromPool(GameObject type, Vector2 position, Quaternion rot, bool autoParent = true)
    {
        string bulletTag = type.GetComponent<Bullet>().tagId;
        //add defensive code later

        GameObject obj;
        
        if (poolDictionary.ContainsKey(bulletTag))
        {
            if (poolDictionary[bulletTag].Count > 0)
            {
                //If the queue contains the bullet needed to spawn, fetch it from pool
                
                obj = poolDictionary[bulletTag].Dequeue();

                obj.SetActive(true);
                obj.transform.position = position;
                obj.transform.rotation = rot;

                
                
                //poolDictionary[tag].Enqueue(obj);
            
                
            }
            else
            {
                //If bullet doesn't exist in pool, create a new instance of it
                obj = Instantiate(type);
                obj.transform.position = position;
                obj.transform.rotation = rot;
                
                
            }
            
        }
        else
        {
            Debug.LogWarning("Warning: tag not found in pool dictionary");
            
            Queue<GameObject> objectPool = new Queue<GameObject>();
            poolDictionary.Add(bulletTag, objectPool);
            
            obj = Instantiate(type);
            obj.transform.position = position;
            obj.transform.rotation = rot;
                
            
            
            //return null;
        }
        
        if (autoParent)
        {
            obj.transform.parent = transform;
        }
        
        obj.GetComponent<IPooledObject>().Reset();
        //Add defensive code?

        return obj;
        
    }

    public void AddBulletToPool(GameObject bullet)
    {
        string bulletTag = bullet.GetComponent<Bullet>().tagId;
        //Debug.Log("Returning to pool");
        if (poolDictionary.ContainsKey(bulletTag))
        {
            poolDictionary[bulletTag].Enqueue(bullet);
        }
        else
        {
            Debug.LogWarning("Warning: tag not found in pool dictionary");
            
            Queue<GameObject> objectPool = new Queue<GameObject>();
            poolDictionary.Add(bulletTag, objectPool);
            
            poolDictionary[bulletTag].Enqueue(bullet);
        }
    }
}
