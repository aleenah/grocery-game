using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AINavigator : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent agent;
    GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = player.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Lose");
        }
    }
}
