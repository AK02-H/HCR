using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float masterSpeed;

    public ParallaxLayer[] bgLayers;

    public GameObject[] bgElements;
    
    public float[] layerSpeeds;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // foreach (var bg in bgLayers)
        // {
        //     bg.part1.transform.position 
        // }

        
        for (int i = 0; i < bgLayers.Length; i++)
        {
            ParallaxLayer temp = bgLayers[i];
            
            //sort sprite layers too
            
            temp.part1.transform.position += Vector3.down * masterSpeed * layerSpeeds[i] * Time.deltaTime;
            if (temp.part1.transform.position.y <= temp.minY)
            {
                //temp.part1.GetComponent<SpriteRenderer>().bounds
                //temp.part1.transform.position = new Vector2(temp.part1.transform.position.x, temp.part1.GetComponent<SpriteRenderer>().bounds.size.y) + temp.respawnOffset;
                temp.part1.transform.position = temp.respawnPos + temp.respawnOffset;
                SwapBgLayerParts(temp);
            }
            
            temp.part2.transform.position += Vector3.down * masterSpeed * layerSpeeds[i] * Time.deltaTime; //do this in fixed update?
            if (temp.part2.transform.position.y <= temp.minY)
            {
                temp.part2.transform.position = temp.respawnPos + temp.respawnOffset;
                SwapBgLayerParts(temp);
            }
        }


        
    }
    
    private void SwapBgLayerParts(ParallaxLayer paraLayer)
    {
        int order1 = paraLayer.part1.GetComponent<SpriteRenderer>().sortingOrder;
        int order2 = paraLayer.part2.GetComponent<SpriteRenderer>().sortingOrder;
        paraLayer.part1.GetComponent<SpriteRenderer>().sortingOrder = order2;
        paraLayer.part2.GetComponent<SpriteRenderer>().sortingOrder = order1;
    }
}

[System.Serializable]
public class ParallaxLayer
{
    public GameObject part1, part2;
    public float minY;
    public Vector2 respawnPos;
    public Vector2 respawnOffset;
}
