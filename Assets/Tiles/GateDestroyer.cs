using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateDestroyer : MonoBehaviour
{
    public Gate[] Gates;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        foreach (var gate in this.Gates)
        {
            gate.Open();
        }
    }
}
