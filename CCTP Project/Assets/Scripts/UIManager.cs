using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject emotion_object;
    public SpriteRenderer sprite;
    public float awake_timer = 5;
    public Sprite def;
    public Sprite happy;
    public Sprite attack;
    public Sprite help;
    public Sprite injured;
    public Sprite panic;
    public Sprite stressed;
    private Sprite current_sprite;
    Vector3 scale;

    // Start is called before the first frame update
    void Start()
    {
        scale = emotion_object.transform.localScale;
        sprite = emotion_object.GetComponent<SpriteRenderer>();
        current_sprite = sprite.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (sprite.sprite == happy)
        {
            emotion_object.transform.localScale = scale * 1.5f;
        }
        else 
        {
            emotion_object.transform.localScale = scale;
        }
        if (awake_timer > 0)
        {
            sprite.enabled = true;
            awake_timer -= Time.deltaTime;
        }
        else 
        {
            sprite.enabled = false;        
        }
        /*if (current_sprite != sprite.sprite) 
        {
            awake_timer = 5;
            current_sprite = sprite.sprite;
        }*/
    }
}
