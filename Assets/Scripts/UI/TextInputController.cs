using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;

public class TextInputController : MonoBehaviour, ISubscriber {

    private GameObject playerInputFieldGO;
    private InputField playerInputField;

    float timeOfLastInputChange;
    

    public void Start()
    {
        
        playerInputFieldGO = gameObject.FindChildWithName("InputField");
        playerInputField = playerInputFieldGO.GetComponent<InputField>();

        MyEvents.StoryInitiated.Subscribe(this, (string title) => {

            timeOfLastInputChange = 0;

            playerInputField.text = "...";

            playerInputFieldGO.SetActive(true);

            //StartCoroutine(FlickerInputText());
        });


        MyEvents.StorySuspended.Subscribe(this, (string title) => {
            playerInputFieldGO.SetActive(false);
        });

        playerInputFieldGO.SetActive(false);
    }



    public void Update()
    {
        if(Time.deltaTime * (Time.time - timeOfLastInputChange) > 1.5)
        {
           // StartCoroutine(FlickerInputText());
        }
    }

    public void HandlePlayerInputChanged()
    {
        timeOfLastInputChange = Time.time;
        StopCoroutine(FlickerInputText());
    }

    public void HandlePlayerInputEntered()
    {
        
        MyEvents.PlayerEnteredNewInput.Fire(playerInputFieldGO.GetComponent<InputField>().text);

        playerInputField.text = "...";

    }

    IEnumerator FlickerInputText()
    {
        int mod = 0;

        GameObject overlay = playerInputFieldGO.FindChildWithName("Overlay");

        while (true)
        {
            overlay.SetActive(mod % 2 == 0);

            mod = 1 - mod;

            yield return new WaitForSeconds(1+Utils.random.Next(1));
        }
    }
}
