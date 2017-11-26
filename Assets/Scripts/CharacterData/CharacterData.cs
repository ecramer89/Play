
using UnityEngine;
[RequireComponent(typeof(Character))]
public abstract class CharacterData : MonoBehaviour {

    protected CharacterName characterName;
    protected Story storyRoot;
    protected Character character;

    protected Character Character
    {
        get
        {
            if (character == null)
            {
                character = GetComponent<Character>();
            }
            return character;
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
