using UnityEngine;
using System.Collections;

public class DJ_TutorialFontScript : MonoBehaviour
{

    public Color color = new Color(0.8f, 0.8f, 0, 1.0f);
    public float scroll = 0.5f; // scrolling velocity
    public float duration = 0.75f; // time to die
    public bool die = false;
    public float timer = 5f;
    public float alpha;


    void Start()
    {
        //switch(DJ_PlayerManager.player.GetComponent<DJ_DamageNumbersCreator>().damageNumberType)
        alpha = 1f;        // have it scale with screen height.
        GetComponent<GUIText>().fontSize = Screen.width / 20;
        die = false;
        
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer >0)
        {
        }
        else
        {
            //transform.position = new Vector3(transform.position.x, transform.position.y, 0) + new Vector3(0, scroll * Time.deltaTime, 0);
            alpha -= 0.05f;//Time.deltaTime / duration;
            duration -= 0.05f;//Time.deltaTime / duration;
            GetComponent<GUIText>().material.color = new Color(GetComponent<GUIText>().material.color.r, GetComponent<GUIText>().material.color.g, GetComponent<GUIText>().material.color.b, alpha);
            if (alpha < 0)
            {
                Destroy(gameObject); // text vanished - destroy itself
            }
        }

    }


}
