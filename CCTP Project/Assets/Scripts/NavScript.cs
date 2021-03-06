using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//Code written by Tom Smith - Thomas19.Smith@live.uwe.ac.uk

public class NavScript : MonoBehaviour
{
    public Vector3 moveTargetTransform;

    public AgentStats myStats;

    public NavMeshAgent navMeshAgent;
    public GameObject gameManager;
    public bool arrived = false;
    TimeDateScript td;

    WorkTravelNode work_travel_node;
    WarnNode warn_node;
    TryHelpNode try_help_node;
    RunHomeNode run_home_node;
    CheckPanicNode check_panic_node;
    InjuredNode injured_node;
    InBuildingNode in_building_node;
    HomeTravelNode home_travel_node;
    DecideHelpNode decide_help_node;
    TryAttackNode try_attack_node;
    DecideAttackNode decide_attack_node;

    Selector travelling_node;
    Sequence helping_injured_node;
    Selector flee_node;
    Sequence panic_node;
    Selector inactive_node;
    Sequence attacking_node;

    Selector top_node;

    // Start is called before the first frame update
    void Awake()
    {
        myStats = this.gameObject.GetComponent<AgentStats>();
        gameManager = GameObject.FindWithTag("GameManager");
        td = gameManager.GetComponent<TimeDateScript>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        FindTarget();
        CreateBehaviourTree();
    }


    void CreateBehaviourTree() 
    {
        try_attack_node = new TryAttackNode(myStats);
        decide_attack_node = new DecideAttackNode(myStats);
        work_travel_node = new WorkTravelNode(myStats);
        warn_node = new WarnNode(myStats, this.gameObject);
        try_help_node = new TryHelpNode(myStats);
        run_home_node = new RunHomeNode(myStats, this.gameObject);
        check_panic_node = new CheckPanicNode(myStats);
        injured_node = new InjuredNode(myStats);
        in_building_node = new InBuildingNode(myStats);
        home_travel_node = new HomeTravelNode(myStats);
        decide_help_node = new DecideHelpNode(myStats);

        attacking_node = new Sequence(new List<Node> { decide_attack_node, try_attack_node });
        travelling_node = new Selector(new List<Node> { work_travel_node, home_travel_node });
        helping_injured_node = new Sequence(new List<Node> { decide_help_node, try_help_node });
        flee_node = new Selector(new List<Node> { warn_node, run_home_node });
        panic_node = new Sequence(new List<Node> { check_panic_node, flee_node });
        inactive_node = new Selector(new List<Node> { injured_node, in_building_node });

        top_node = new Selector(new List<Node> { inactive_node, panic_node, helping_injured_node, travelling_node, attacking_node });
    }

   

    // Update is called once per frame
    void Update()
    {
        int green = (int)(255 - myStats.currentPanic * 10);
        int red = 255;
        int blue = 0;
        if (myStats.injured)
        {
            if (gameObject.GetComponent<UIManager>().sprite.sprite != gameObject.GetComponent<UIManager>().injured)
            {
                gameObject.GetComponent<UIManager>().sprite.sprite = gameObject.GetComponent<UIManager>().injured;
                gameObject.GetComponent<UIManager>().awake_timer = 5;
            }
            
            green = 0;
            red = 255;
            blue = 0;
        }
        else if (myStats.helping)
        {
            if (gameObject.GetComponent<UIManager>().sprite.sprite != gameObject.GetComponent<UIManager>().help)
            {
                gameObject.GetComponent<UIManager>().sprite.sprite = gameObject.GetComponent<UIManager>().help;
                gameObject.GetComponent<UIManager>().awake_timer = 5;
            }
            blue = 255;
            red = 0;
            green = 0;
        }
        else if (myStats.panicked)
        {
            if (gameObject.GetComponent<UIManager>().sprite.sprite != gameObject.GetComponent<UIManager>().panic)
            {
                gameObject.GetComponent<UIManager>().sprite.sprite = gameObject.GetComponent<UIManager>().panic;
                gameObject.GetComponent<UIManager>().awake_timer = 5;
            }
            blue = 0;
            red = 255;
            green = (int)(255 - myStats.currentPanic * 10);
        }
        else if (myStats.attacking)
        {
            if (gameObject.GetComponent<UIManager>().sprite.sprite != gameObject.GetComponent<UIManager>().attack)
            {
                gameObject.GetComponent<UIManager>().sprite.sprite = gameObject.GetComponent<UIManager>().attack;
                gameObject.GetComponent<UIManager>().awake_timer = 5;
            }
            blue = 255;
            red = 255;
            green = 0;
        }
        else if (myStats.stressed)
        {
            if (gameObject.GetComponent<UIManager>().sprite.sprite != gameObject.GetComponent<UIManager>().stressed)
            {
                gameObject.GetComponent<UIManager>().sprite.sprite = gameObject.GetComponent<UIManager>().stressed;
                gameObject.GetComponent<UIManager>().awake_timer = 5;
            }
            blue = 0;
            red = 255;
            green = (int)(255 - myStats.currentPanic * 10);
        }
        else if (myStats.happy)
        {
            if (gameObject.GetComponent<UIManager>().sprite.sprite != gameObject.GetComponent<UIManager>().happy)
            {
                gameObject.GetComponent<UIManager>().sprite.sprite = gameObject.GetComponent<UIManager>().happy;
                gameObject.GetComponent<UIManager>().awake_timer = 5;
            }
            blue = 0;
            red = 255;
            green = (int)(255 - myStats.currentPanic * 10);
        }
        else if (myStats.travelling)
        {
            if (gameObject.GetComponent<UIManager>().sprite.sprite != gameObject.GetComponent<UIManager>().def)
            {
                gameObject.GetComponent<UIManager>().sprite.sprite = gameObject.GetComponent<UIManager>().def;
                gameObject.GetComponent<UIManager>().awake_timer = 5;
            }
            blue = 0;
            red = 255;
            green = (int)(255 - myStats.currentPanic * 10);
        }
        gameObject.GetComponent<Renderer>().material.color = new Color32((byte)red, (byte)green, (byte)blue, 1);
        
        if (myStats.wandering && !navMeshAgent.enabled)
        {
            navMeshAgent.enabled = true;
        }

        updateSpeed();

        CheckWork();
        CheckSleep();
        if (top_node.Eval() != state.failed)
        {
            myStats.wandering = false;
            StopCoroutine(RandomWait(1, 10)); 
        }
        else
        {
            myStats.wandering = true;
            if (myStats.happy)
            {
                if (gameObject.GetComponent<UIManager>().sprite.sprite != gameObject.GetComponent<UIManager>().happy)
                {
                    gameObject.GetComponent<UIManager>().sprite.sprite = gameObject.GetComponent<UIManager>().happy;
                    gameObject.GetComponent<UIManager>().awake_timer = 5;
                }
                blue = 0;
                red = 255;
                green = (int)(255 - myStats.currentPanic * 10);
            }
            else
            {
                if (gameObject.GetComponent<UIManager>().sprite.sprite != gameObject.GetComponent<UIManager>().def)
                {
                    gameObject.GetComponent<UIManager>().sprite.sprite = gameObject.GetComponent<UIManager>().def;
                    gameObject.GetComponent<UIManager>().awake_timer = 5;
                }
                blue = 0;
                red = 255;
                green = (int)(255 - myStats.currentPanic * 10);
            }
            if (myStats.stressed)
            {
                if (gameObject.GetComponent<UIManager>().sprite.sprite != gameObject.GetComponent<UIManager>().stressed)
                {
                    gameObject.GetComponent<UIManager>().sprite.sprite = gameObject.GetComponent<UIManager>().stressed;
                    gameObject.GetComponent<UIManager>().awake_timer = 5;
                }
                blue = 0;
                red = 255;
                green = (int)(255 - myStats.currentPanic * 10);
            }
            Vector3 lookTarget = new Vector3(moveTargetTransform.x, 1, moveTargetTransform.z);
            //transform.LookAt(lookTarget);
            navMeshAgent.destination = moveTargetTransform;
            if (Vector3.Distance(navMeshAgent.destination, this.transform.position) <= 4 && !arrived && myStats.wandering)
            {
                /*if (myStats.helping) 
                {
                    StartCoroutine(RandomWait((20 - myStats.helpTarget.GetComponent<AgentStats>().fortitude),20));
                }*/
                StartCoroutine(RandomWait(1, 10));
            }
        }
        
        
        

    }

    public void updateSpeed() 
    {
        if (myStats.running)
        {
            navMeshAgent.speed = (2 + myStats.speed) * td.timescale;
        }
        else
        {
            navMeshAgent.speed = 2 * td.timescale;
        }
    }

    private void FindTarget()
    {
        arrived = false;
        Transform chosenTarget = gameManager.GetComponent<AgentGen>().spawnPoints[Random.Range(0, gameManager.GetComponent<AgentGen>().spawnPoints.Length)].transform;
        moveTargetTransform = new Vector3(chosenTarget.position.x + (float)(Random.Range(-100,100)/1000f), 1, Random.Range(-24, 76));
        NavMeshHit hit;
        if (NavMesh.SamplePosition(moveTargetTransform, out hit, 10.0f, NavMesh.AllAreas))
        {
            moveTargetTransform = hit.position;
        }
        //Debug.Log("New target destination at " + moveTargetTransform.position);
    }

    IEnumerator RandomWait(int min, int max)
    {
        arrived = true;
        //Debug.Log("Waiting");
        float waitTime = Random.Range(min, max) / td.timescale;
        //Debug.Log(waitTime);
        yield return new WaitForSecondsRealtime(waitTime);
        //Debug.Log("Finished Waiting");
        FindTarget();

    }

    private void CheckWork() 
    {
        if (myStats.work_days.Contains(td.day)) 
        {
            if (!myStats.night_shift && td.hour >= 9 && td.hour < 17)
            {
                myStats.due_work = true;
            }
            else if (myStats.night_shift && td.hour >= 16 && td.hour < 24)
            {
                myStats.due_work = true;
            }
            else 
            {
                myStats.due_work = false;            
            }
        }
        else
        {
            myStats.due_work = false;
        }
    }

    private void CheckSleep()
    {       
        if (!myStats.night_shift && (td.hour >= 23 || td.hour < 7))
        {
            myStats.due_home = true;
        }
        else if (myStats.night_shift && td.hour >= 3 && td.hour < 11)
        {
            myStats.due_home = true;
        }
        else
        {
            myStats.due_home = false;
        }
    }


}
