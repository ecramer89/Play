using System.Collections;
using System.Collections.Generic;

//this is basically a "join table" between character and storyEvent, as well as the table for the story event
public class StoryEventRepository  {
    private static StoryEvent playerLearnedThatCatLikesButterChicken = new StoryEvent("PlayerLearnedThatCatLikesButterChicken");


    private static Dictionary<CharacterName, HashSet<StoryEvent>> characterStoryEvents = new Dictionary<CharacterName, HashSet<StoryEvent>> {
        {
            CharacterName.Cat,
            new HashSet<StoryEvent>{
                playerLearnedThatCatLikesButterChicken
            }
        }
    };


    public static HashSet<StoryEvent> GetStoryEventsForCharacter(CharacterName characterName)
    {
        HashSet<StoryEvent> storyEvents;
        if (!characterStoryEvents.TryGetValue(characterName, out storyEvents)) throw new System.Exception(string.Format("Error! No story events found for {0}", characterName));
        return storyEvents;
    }


}
