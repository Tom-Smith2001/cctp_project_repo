using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//Code written by Tom Smith - Thomas19.Smith@live.uwe.ac.uk

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject emotion_object; //object holding sprite renderer
    public SpriteRenderer sprite;                       //emoji sprite for state clarity
    public float awake_timer = 5; //time emoji stays active
    public Sprite def;              //
    public Sprite happy;            //
    public Sprite attack;           //
    public Sprite help;             // Sprites used for emotion display
    public Sprite injured;          //
    public Sprite panic;            //
    public Sprite stressed;         //
    private Sprite current_sprite;  //
    Vector3 scale;

    // Start is called before the first frame update
    void Start()
    {
        scale = emotion_object.transform.localScale; //sprites are different sizes so need to be scaled dynamically
        sprite = emotion_object.GetComponent<SpriteRenderer>();
        current_sprite = sprite.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (sprite.sprite == happy)
        {
            emotion_object.transform.localScale = scale * 1.5f; //sprites are different sizes so need to be scaled dynamically
        }
        else 
        {
            emotion_object.transform.localScale = scale; //sprites are different sizes so need to be scaled dynamically
        }
        //only show the sprite while the awake timer is active
        if (awake_timer > 0)
        {
            sprite.enabled = true;
            awake_timer -= Time.deltaTime;
        }
        else 
        {
            sprite.enabled = false;        
        }
    }
}
