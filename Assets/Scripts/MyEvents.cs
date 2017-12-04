using System.Collections;
using System.Collections.Generic;

public class MyEvents  {
    public static UnaryParameterizedEvent<string> PlayerEnteredNewInput = new UnaryParameterizedEvent<string>("PlayerEneteredNewInput");

    public static UnaryParameterizedEvent<string> StoryInitiated = new UnaryParameterizedEvent<string>("StoryInitiated");

    public static UnaryParameterizedEvent<string> StorySuspended = new UnaryParameterizedEvent<string>("StorySuspended");

    public static UnaryParameterizedEvent<Item> PlayerGivenItem = new UnaryParameterizedEvent<Item>("PlayerGivenItem");




    //state update events. subscribe to these if you care about the result of a trigger on the game state
    public static ParameterlessEvent PlayerInventoryUpdated = new ParameterlessEvent("PlayerInventoryUpdated");



}
