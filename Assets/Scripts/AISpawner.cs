using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    public int AICount = 5;
    public GameObject AIObject;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < AICount; i++) {
            var pos = GetPositionForAI();
            var ai = Instantiate(AIObject, pos, Quaternion.identity);
            ai.transform.LookAt(Vector3.zero);
        }
    }

    Vector3 GetPositionForAI()
    {
        while (true)
        {
            var pos = new Vector3(Random.Range(-3.0f, 18.0f), 1, Random.Range(-18.0f, 3.0f));
            var colliders = Physics.OverlapBox(pos, new Vector3(1, 0.5f, 1));
            if (colliders.Length == 0) return pos;
        } 
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
