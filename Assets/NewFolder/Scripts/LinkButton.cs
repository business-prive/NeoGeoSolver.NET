using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkButton : MonoBehaviour
{
    public GameObject panel;
    public float timeDeley;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TimerPanel());
     
        
 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TimerPanel()
    {
        yield return new WaitForSeconds(timeDeley);
        panel.SetActive(false);
    }
}
