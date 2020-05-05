using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    static DontDestroyOnLoad Instance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Awake()
    {
        if (Instance) // PRevents creation of camera clones when loading new level
        {
            Destroy(gameObject);
        }
        else
        {

            DontDestroyOnLoad(gameObject);
            Instance = this;

        }
    }
}
