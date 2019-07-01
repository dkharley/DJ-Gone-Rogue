using UnityEngine;
using System.Collections;

public class DJ_ResetData : MonoBehaviour {


    public GameObject confirmationPopupGO;
    public GameObject yesButton;
    public GameObject cancelButton;
    public GameObject[] bg;
    public GameObject resetData;
    public GameObject mainMenu;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClick()
    {
        if (confirmationPopupGO.activeSelf)
        {
            confirmationPopupGO.SetActive(false);
            yesButton.SetActive(false);
            cancelButton.SetActive(false);
            for (int i = 0; i < bg.Length; ++i)
                bg[i].SetActive(false);
            resetData.SetActive(true);
            mainMenu.SetActive(true);
            DJ_GameInitiliazer.ResetDefaultDataValues();
        }
    }
}
