using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AgentStats : MonoBehaviour
{
    public GameObject game_manager;

    [Header("Characteristics")]
    public string name;
    [Range(0, 20)] public float composure;  // how likely they are to panic, and how quickly they can calm down, composure also affects how easily other agents can gauge this agents current state, and how volatile an agent's hapiness is.
    [Range(0, 20)] public float observation;  // how much they notice around them.
    [Range(0, 20)] public float fortitude;  // how likely they are to get injured, fortitude also affects how quickly other agents can help if this agent is injured.
    [Range(0, 20)] public float bravery;  // how likely they are to help others.
    [Range(0, 20)] public float temperament; // how friendly this agent is to other agents, a higher temperament allows more 'friends', while a lower temperament allows more 'enemies'. Temperament also affects how easily an agent becomes stressed.
    [Range(0, 2)] public float speed;  // how fast they can run.
    public float base_composure;   
    public float base_observation;  
    public float base_fortitude;  
    public float base_bravery; 
    public float base_speed;  

    [Header("State Info")]
    [Range(0, 20)] public float currentPanic = 0;
    [Range(0, 20)] public int happiness = 0;
    [Range(0, 20)] public int stress = 0;
    public bool happy = false;
    public bool stressed = false;
    public bool running = false;
    public bool panicked = false;
    public bool injured = false;
    public bool helping = false;
    public bool helped = false;
    public bool wandering = true;
    public bool travelling = false;
    public bool due_work = false;
    public bool due_home = false;
    public bool at_work = false;
    public bool at_home = false;
    public bool attacking = false;


    [Header("Relationships")]
    public List<GameObject> friends;
    public int max_friends;
    public List<GameObject> enemies;
    public int max_enemies;
    public GameObject helpTarget;
    public GameObject attack_target;

    
    [Header("Routine Info")]    
    public List<TimeDateScript.WeekDay> work_days;
    public bool night_shift;
    public GameObject home;
    public GameObject work_place;
    private GameObject[] houses;
    private GameObject[] businesses;

    [Header("Wider Knowledge")]
    public List<GameObject> known_events;
    public GameObject current_warning;


    public bool generated = false;

    public enum FirstName 
    {
            Antonio,
            Adrian,
            Abbie,
            Agatha,
            Bert,
            Bob,
            Becky,
            Brienne,
            Collin,
            Cecil,
            Christine,
            Carol,
            Derek,
            Dennis,
            Daniella,
            Daphne,
            Ethan,
            Ezekiel,
            Evelynn,
            Edna,
            Fergus,
            Frank,
            Francesca,
            Fiona,
            Greg,
            Gareth,
            Gertrude,
            Gabriella,
            Harvey,
            Hank,
            Hazel,
            Hermione,
            Ian,
            Irwin,
            Isabelle,
            Imogen,
            Jeff,
            Jamal,
            Jess,
            Judith,
            Kyle,
            Keith,
            Kelly,
            Karen,
            Lester,
            Lee,
            Louise,
            Lucy,
            Martin,
            Montgomery,
            Maisie,
            Martha,
            Neil,
            Nathan,
            Nora,
            Nancy,
            Oliver,
            Oscar,
            Ophelia,
            Olivia,
            Perry,
            Peter,
            Penelope,
            Phoebe,
            Quimby,
            Quincy,
            Qiyana,
            Queena,
            Ross,
            Roman,
            Rachel,
            Roxanne,
            Sam,
            Steve,
            Sandra,
            Sharon,
            Tom,
            Tyson,
            Tara,
            Tabitha,
            Ulric,
            Ulysses,
            Uthgerd,
            Ursula,
            Victor,
            Vinnie,
            Vanessa,
            Vicky,
            Wallace,
            Will,
            Wendy,
            Wanda,
            Xander,
            Xavier,
            Xena,
            Xara,
            Yanni,
            Youssef,
            Yasmin,
            Yennefer,
            Zachary,
            Zlatan,
            Zara,
            Zoe
    }

    public enum Surname 
    {
        Aarons,
        Buchanan,
        Cheng,
        Durkan,
        Evans,
        Fitzpatrick,
        Gray,
        Harrison,
        Izaacs,
        Johnson,
        Knowles,
        LeClerc,
        Martins,
        Newman,
        Owenson,
        Park,
        Quinn,
        Reznov,
        Sneijder,
        Thomas,
        Usman,
        Volkov,
        Wallace,
        Xavi,
        Yendell,
        Zelensky
    }

    // Start is called before the first frame update
    void Start()
    {
        generated = false;
        game_manager = GameObject.FindGameObjectWithTag("GameManager");
        GenerateStats();
        base_bravery = bravery;
        base_composure = composure;
        base_fortitude = fortitude;
        base_observation = observation;
        base_speed = speed;
        GenerateWork();
        StartCoroutine(GenerateRelationships());
        StartCoroutine(InjuryCheck());
        StartCoroutine(StressCheck());
        StartCoroutine(HappinessCheck());

    }

    IEnumerator GenerateRelationships()
    {
        yield return new WaitForSeconds(1f);
        //Find the 3 agents with the MOST similar and LEAST similar starting stats and assign them as the initial friends or enemies.
        for (int i = 0; i < 3; i++)
        {
            foreach (GameObject agent in GameObject.FindGameObjectsWithTag("Agent"))
            {
                
                if (agent != this.gameObject && agent.GetComponent<AgentStats>().generated && !friends.Contains(agent) && !enemies.Contains(agent))
                {
                    if (friends.Count == i )
                    {
                        friends.Insert(i, agent);
                    }
                    if (enemies.Count == i)
                    {
                        enemies.Insert(i, agent);
                    }

                    if (CompareStats(agent) > CompareStats(enemies[i]))
                    {
                        enemies.RemoveAt(i);
                        enemies.Insert(i, agent);
                    }
                    if (CompareStats(agent) < CompareStats(friends[i]))
                    {
                        friends.RemoveAt(i);
                        friends.Insert(i, agent);
                    }
                }
            }

        }
    }
       

    float CompareStats(GameObject other) 
    {
        AgentStats other_stats = other.GetComponent<AgentStats>();
        int comp = (int)Mathf.Abs(composure - other_stats.composure);
        int brav = (int)Mathf.Abs(bravery - other_stats.bravery);
        int temp = (int)Mathf.Abs(temperament - other_stats.temperament);

        

        return (comp +  brav + temp) / 3;
    }

    private void Update()
    {
        if (friends.Count > max_friends) 
        {
            friends.RemoveAt(Random.Range(0, max_friends - 1));      
        }
        if (enemies.Count > max_enemies)
        {
            enemies.RemoveAt(Random.Range(0, max_enemies - 1));
        }
        if (happiness > 20 - temperament && !happy) 
        {
            happy = true;
            HappyStats();                                
        }
        else if (happiness < 20 - temperament && happy)
        {
            happy = false;
            ResetStats();
        }
        if (stress > temperament && !stressed)
        {
            stressed = true;
            StressedStats();
        }
        else if (stress < temperament && stressed) 
        {
            stressed = false;
            ResetStats();
        }
    }

    public void GenerateStats()
    {
        //Name name = (Name)Random.Range(0, 49);
        this.transform.name = name.ToString();
        composure = GenStat();
        observation = GenStat();
        fortitude = GenStat();
        bravery = GenStat();
        temperament = GenStat();
        speed = (float)(GenStat() / 10.0f);

        if (temperament < 10) 
        {
            max_enemies = (int)(3 + (10 - temperament));
            max_friends = 3;
        }
        else
        {
            max_friends = (int)(3 + (temperament - 10));
            max_enemies = 3;
        }
        stress = 0;
        happiness = (int)temperament;
        generated = true;
    }

    private int GenStat() 
    {
        int x = 0;
        int y = Random.Range(0, 550);
        if (y < 5) 
        {
            x = 1;
        }
        else if (y < 15)
        {
            x = 2;
        }
        else if (y < 30)
        {
            x = 3;
        }
        else if (y < 50)
        {
            x = 4;
        }
        else if (y < 75)
        {
            x = 5;
        }
        else if (y < 105)
        {
            x = 6;
        }
        else if (y < 140)
        {
            x = 7;
        }
        else if (y < 180)
        {
            x = 8;
        }
        else if (y < 225)
        {
            x = 9;
        }
        else if (y < 275)
        {
            x = 10;
        }
        else if (y < 325)
        {
            x = 11;
        }
        else if (y < 370)
        {
            x = 12;
        }
        else if (y < 410)
        {
            x = 13;
        }
        else if (y < 445)
        {
            x = 14;
        }
        else if (y < 475)
        {
            x = 15;
        }
        else if (y < 500)
        {
            x = 16;
        }
        else if (y < 520)
        {
            x = 17;
        }
        else if (y < 535)
        {
            x = 18;
        }
        else if (y < 545)
        {
            x = 19;
        }
        else if (y < 550)
        {
            x = 20;
        }
        return x;
    }

    private void GenerateWork()
    {
        businesses = GameObject.FindGameObjectsWithTag("Work");
        work_place = businesses[Random.Range(0, businesses.Length)];
        work_place.GetComponent<WorkScript>().employees.Add(this.gameObject);

        houses = GameObject.FindGameObjectsWithTag("House");
        home = houses[Random.Range(0, houses.Length)];
        home.GetComponent<HouseScript>().tennants.Add(this.gameObject);

        while (work_days.Count < 5)
        {

            TimeDateScript.WeekDay day = (TimeDateScript.WeekDay)Random.Range(0, 7);
            if (!work_days.Contains(day))
            {
                work_days.Add(day);
            }
            else
            {
                day = (TimeDateScript.WeekDay)Random.Range(0, 7);
                if (!work_days.Contains(day))
                {
                    work_days.Add(day);
                }
            }
        }
        if (Random.Range(0, 100) < 50)
        {
            night_shift = false;
        }
        else
        { 
            night_shift = true;
        }
    }

    public void ResetStats() 
    {
        bravery = base_bravery;
        composure = base_composure;
        fortitude = base_fortitude;
        observation = base_observation;
        speed = base_speed;
        if (happy) 
        {
            HappyStats();        
        }
        if (stressed) 
        {
            StressedStats();        
        }
    }

    public void HappyStats()
    {
        bravery = (int)(bravery * 1.1);
        composure = (int)(composure * 1.5);
        fortitude = (int)(fortitude * 1.1);
        observation = (int)(observation * 1.25);
        temperament = (int)(temperament * 1.5);
    }

    public void StressedStats()
    {
        bravery = (int)(bravery * 0.75); 
        composure = (int)(composure * 0.5);
        fortitude = (int)(fortitude * 0.9);
        observation = (int)(observation * 0.75);
        temperament = (int)(temperament * 0.75);
        speed = (speed * 1.25f);
    }

    public void Heal(GameObject agent) 
    {
        StartCoroutine(HealAgent(agent));          
    }

    public void StopHeal(GameObject agent)
    {
        StopCoroutine(HealAgent(agent));
    }

    IEnumerator HealAgent(GameObject injured_agent)
    {
        injured_agent.GetComponent<AgentStats>().helped = true;
        Debug.Log(gameObject.name + " is helping " + injured_agent.name);
        yield return new WaitForSeconds(Random.Range((20 - injured_agent.GetComponent<AgentStats>().fortitude), 20));
        injured_agent.GetComponent<AgentStats>().currentPanic /= 10;
        injured_agent.GetComponent<AgentStats>().injured = false;
        injured_agent.GetComponent<AgentStats>().helped = false;
        wandering = true;
        helping = false;
        Debug.Log(gameObject.name + " has successfully helped " + injured_agent.name);
    }

    IEnumerator InjuryCheck() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(2f / game_manager.GetComponent<TimeDateScript>().timescale);
            foreach (Collider e in Physics.OverlapSphere(this.transform.position, 20f, LayerMask.NameToLayer("Event"))) 
            {
                float distance = Vector3.Distance(this.transform.position, e.transform.position);
                if (Random.Range(0, distance * 10) < 20 - fortitude) 
                {
                    injured = true;
                }           
            }
        }            
    }
    IEnumerator StressCheck()
    {
        
        while (true)
        {
            if (stress > 20)
                stress = 20;
            else if (stress < 0)
                stress = 0;
            if (at_work)
            {
                yield return new WaitForSeconds((temperament / game_manager.GetComponent<TimeDateScript>().timescale) * 10);
                if (Random.Range(0, 20) > temperament)
                {
                    stress += 1;
                }
            }
            else if (at_home)
            {
                yield return new WaitForSeconds((20 - temperament) / game_manager.GetComponent<TimeDateScript>().timescale);
                if (Random.Range(0, 20) < temperament)
                {
                    stress -= 1;
                }
            }
            else 
            {
                yield return new WaitForSeconds(temperament / game_manager.GetComponent<TimeDateScript>().timescale);
            }

            
        }
    }

    IEnumerator HappinessCheck()
    {
        while (true)
        {

            if (happiness > 20)
                happiness = 20;
            else if (happiness < 0)
                happiness = 0;
            if (!at_work)
            {
                yield return new WaitForSeconds(composure / game_manager.GetComponent<TimeDateScript>().timescale);
                int num = Random.Range(0, 5000);
                if (num < temperament)
                {
                    happiness += 1;

                   // Debug.Log("Happiness up!");
                }
                else if (num < temperament * 2)
                {
                    happiness -= 1;

                   // Debug.Log("Hapiness down.");
                }
                else 
                {
                    //Debug.Log("No change in hapiness.");           
                }
            }
            else
            {
                yield return new WaitForSeconds(temperament / game_manager.GetComponent<TimeDateScript>().timescale);
            }
        }
    }

}
