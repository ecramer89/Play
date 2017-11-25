using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterData : MonoBehaviour {

     public abstract CharacterName Name();

     public abstract Story StoryRoot();
}
