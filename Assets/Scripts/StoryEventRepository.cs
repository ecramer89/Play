using System.Collections;
using System.Collections.Generic;

//this is basically a "join table" between character and storyEvent, as well as the table for the story event
public class StoryEventRepository  {
    private static StoryEvent playerLearnedThatCatLikesButterChicken = new StoryEvent("PlayerLearnedThatCatLikesButterChicken");


    private static Dictionary<CharacterName, HashSet<StoryEvent>> characterStoryEvents = new Dictionary<CharacterName, HashSet<StoryEvent>>();


    public static HashSet<StoryEvent> GetStoryEventsForCharacter(CharacterName characterName)
    {
        return null;
    }


}
