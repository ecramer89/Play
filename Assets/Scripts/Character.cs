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
        story = StoryRepository.GetStory(data.Name());


        textBox = this.gameObject.FindChildWithName("TextBox");
        uIText = textBox.GetComponentInChildren<Text>();
        uIText.text = story.text;
       

        toggleActiveButton = this.gameObject.FindChildWithName("ToggleActiveButton");
        toggleActiveButton.GetComponent<Button>().onClick.AddListener(ToggleActive);
        toggleActiveButtonText = toggleActiveButton.GetComponentInChildren<Text>();

      
        Deactivate();

    }


    private void AdvanceStory(string playerInput)
    {
        
        this.story = this.story.GetTransition(playerInput);
     
        this.uIText.text = story.text;
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

    private void Deactivate() {
        //unsubscribe from changes in player input
        MyEvents.PlayerEnteredNewInput.UnSubscribe(this);
        this.textBox.SetActive(false);
        toggleActiveButtonText.text = String.Format("Talk to {0}", data.Name());
    }

    private void Activate() {
        //reset the character's story to the root, if we deactivated upon reaching a leaf
    
        if (story is LeafStory)
        { 
            story = StoryRepository.GetStory(data.Name());
            this.uIText.text = story.text;
        }

        MyEvents.PlayerEnteredNewInput.Subscribe(this, AdvanceStory);
        this.textBox.SetActive(true);
        toggleActiveButtonText.text = String.Format("Stop talking to {0}", data.Name());

    }





}

