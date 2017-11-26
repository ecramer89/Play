
using UnityEngine;
[RequireComponent(typeof(Character))]
public abstract class CharacterData : MonoBehaviour {

    protected CharacterName characterName;
    protected Story storyRoot;
    private Character characterComponent;

    protected Character CharacterComponent
    {
        get
        {
            if (characterComponent == null)
            {
                characterComponent = GetComponent<Character>();
            }
            return characterComponent;
        }
    }

    public CharacterName Name()
    {  if (characterName == null) InitializeCharacterName();
        return characterName;
    }

     public Story StoryRoot()
    {   if (storyRoot == null) InitializeStoryRoot();
        return storyRoot;
    }

    protected abstract void InitializeStoryRoot();

    protected abstract void InitializeCharacterName();
}
