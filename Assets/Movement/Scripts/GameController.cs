using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    // Start is called before the first frame update
    void Start()
    {
        // initialize player's position
        player1.transform.position = new Vector3(5,5,5);
        player2.transform.position = new Vector3(10,5,5);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
