using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    Rigidbody rigidbody;
    Vector3 velocity;
    public GameObject enemyPos;
    Transform playerTransform;
    public enum FSMState
    {
        Asleep,
        Awake,
    }
    public FSMState curState;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.position = enemyPos.transform.position;

        //Get the target enemy(Player)
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;

        // Get the rigidbody
        rigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        switch (curState)
        {
            case FSMState.Asleep: UpdateAsleep(); break;
            case FSMState.Awake: UpdateAwake(); break;
        }   
    }
    void UpdateAsleep()
    {
        if (Vector3.Distance(playerTransform.position, transform.position) <= 30.0f)
        {
            curState = Awake;
        }
    }

    void UpdateAwake()
    {
        if (Vector3.Distance(playerTransform.position, transform.position) >= 30.0f)
        {
            curState = Asleep;
        }

    }

}