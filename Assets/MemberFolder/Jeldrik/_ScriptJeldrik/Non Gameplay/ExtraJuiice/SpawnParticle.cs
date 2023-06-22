using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticle : MonoBehaviour
{

    private SparkPooling _sparkPooling;

    // Start is called before the first frame update
    void Start()
    {
        _sparkPooling = GameObject.FindGameObjectWithTag("JuiceManager").GetComponent<SparkPooling>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnSpark()
    {
       _sparkPooling.SpawnSpark(Camera.main.ScreenToWorldPoint(Input.mousePosition), 30);
    }
}
