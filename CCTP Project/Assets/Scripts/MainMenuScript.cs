using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//Code written by Tom Smith - Thomas19.Smith@live.uwe.ac.uk

public class MainMenuScript : MonoBehaviour
{
    public Slider slider;
    public Text count;
    // Start is called before the first frame update
    void Start()
    {
        slider.value = Mathf.RoundToInt(slider.value);
        AgentGen.numOfAgents = (int)slider.value;
    }

    // Update is called once per frame
    void Update()
    {
        //slider.value = Mathf.RoundToInt(slider.value);
        slider.value = (int)slider.value;
        count.text = slider.value.ToString();        
    }

    public void SetAgents() 
    {
        AgentGen.numOfAgents = (int)slider.value;            
    }

    public void ChaosMode() 
    {
        TimeDateScript.chaos = true;
        SceneManager.LoadScene(1);
    }

    public void NormalMode()
    {
        TimeDateScript.chaos = false;
        SceneManager.LoadScene(1);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
