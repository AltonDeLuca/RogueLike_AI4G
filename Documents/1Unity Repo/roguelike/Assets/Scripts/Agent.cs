using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public enum FSMState
    {
        Asleep,
        Awake,
    }
    public FSMState curState;
    Rigidbody rigidbody;
    Vector3 velocity;
    public GameObject enemyPos;
    Transform playerTransform;

    //current speed of agent
    private float curSpeed;

    //TAgent rotation Speed
    private float curRotSpeed;

    //Bullet object
    public GameObject Bullet;

    //agent is dead?
    private bool bDead;

    //agent health value
    public int health;

    //number of bullets
    public int bulletCount;
    protected GameObject[] pointList;

    protected float shootRate;
    protected float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        curState = FSMState.Asleep;
        curSpeed = 15.0f;
        curRotSpeed = 1.0f;
        bDead = false;
        elapsedTime = 0.0f;
        shootRate = 1.0f;
        health = 100;

        //pointList = GameObject.FindGameObjectsWithTag("wp");

        FindNextPoint();

        rigidbody = GetComponent<Rigidbody>();
        rigidbody.position = enemyPos.transform.position;

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
        if (Vector3.Distance(transform.position, playerTransform.position) <= 10.0f)
        {
            FindNextPoint();
        }
        else if (Vector3.Distance(transform.position, playerTransform.position) <= 30.0f)
        {
            print("Switched to Chase Position");
            curState = FSMState.Awake;
        }
        Quaternion targetRotation = Quaternion.LookRotation(playerTransform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        //Go Forward
        transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }

    void UpdateAwake()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) <= 30.0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(playerTransform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);

            //move Forward
            transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);

            curState = FSMState.Awake;
            
        }
        else {
            curState = FSMState.Asleep;
        }
        Quaternion thisRotation = Quaternion.LookRotation(playerTransform.position - this.transform.position);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, thisRotation,
                Time.deltaTime * curRotSpeed);

            //Shoot the bullets
            ShootBullet();
    }

    protected void FindNextPoint()
    {
        Debug.Log("Finding next point");
        int rndIndex = Random.Range(0, pointList.Length);
        float rndRadius = 10.0f;

        Vector3 rndPosition = Vector3.zero;
        playerTransform.position = pointList[rndIndex].transform.position + rndPosition;

        //Check Range
        //Prevent to decide the random point as the same as before
        if (IsInCurrentRange(playerTransform.position))
        {
            rndPosition = new Vector3(Random.Range(-rndRadius, rndRadius), 0.0f, Random.Range(-rndRadius, rndRadius));
            playerTransform.position = pointList[rndIndex].transform.position + rndPosition;
        }
    }
    
    protected bool IsInCurrentRange(Vector3 pos)
    {
        float xPos = Mathf.Abs(pos.x - transform.position.x);
        float zPos = Mathf.Abs(pos.z - transform.position.z);

        if (xPos <= 50 && zPos <= 50)
            return true;

        return false;
    }
    private void ShootBullet()
    {
        if (elapsedTime >= shootRate)
        {


            GameObject newBullet = Instantiate(Bullet,
                this.transform.position + new Vector3(1, 0, 0),
                   this.transform.rotation) as GameObject;

            Rigidbody bulletRB = newBullet.GetComponent<Rigidbody>();

            bulletRB.velocity = this.transform.forward * shootRate;

        }
    }
   
}

