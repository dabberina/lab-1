using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameRestart()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);   // must come first, see below

            GoombaStomp stomp = child.GetComponent<GoombaStomp>();
            if (stomp != null)
                stomp.ResetStomp();

            child.GetComponent<EnemyMovement>().GameRestart();
        }
    }
}
