using System;
using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;

[RequireComponent(typeof(CharacterData))]
public class Character : MonoBehaviour, ISubscriber {

    private CharacterData data;

    private GameObject textBox;
    private Text uIText;

    private GameObject toggleActiveButton;
    private Text toggleActiveButtonText;

    private Story story;

    private bool active; 

    public bool Active
    {
        get { return active; } 
    }

    public void Start()
    {
        data = GetComponent<CharacterData>();

        textBox = this.gameObject.FindChildWithName("TextBox");
        uIText = textBox.GetComponentInChildren<Text>();
        
      
        toggleActiveButton = this.gameObject.FindChildWithName("ToggleActiveButton");
        toggleActiveButton.GetComponent<Button>().onClick.AddListener(ToggleActive);
        toggleActiveButtonText = toggleActiveButton.GetComponentInChildren<Text>();

        SetStoryToRoot();
        Deactivate();

    }


    public void SetStoryToRoot()
    {
        story = StoryRepository.GetStory(data.Name());
        
    }


    private void AdvanceStory(string playerInput)
    {
        
        this.story = this.story.GetTransition(playerInput);

        this.uIText.text = story.text;

        story.effectUponReaching();
    }



    public void ToggleActive()
    {
        active = !active;
        if (active)
        {
            Activate();
        }
        else
        {
            Deactivate();
        }

    }

    public void Deactivate() {
        //unsubscribe from changes in player input
        MyEvents.PlayerEnteredNewInput.UnSubscribe(this);
        this.textBox.SetActive(false);
        toggleActiveButtonText.text = String.Format("Talk to {0}", data.Name());
    }

    public void Activate() {
      
        MyEvents.PlayerEnteredNewInput.Subscribe(this, AdvanceStory);
        this.textBox.SetActive(true);
        uIText.text = story.text;
        toggleActiveButtonText.text = String.Format("Stop talking to {0}", data.Name());
    }





}

