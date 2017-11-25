using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class CatData : CharacterData {

    public override CharacterName Name()
    {
        return CharacterName.Cat;
    }



    public override Story StoryRoot()
    {
        CompositeStory composite0a = new CompositeStory("I have several hobbies. Eating, sleeping, " +
          "staring out the window. Ask me about one of them.");
        CompositeStory composite0b = new CompositeStory("Sleeping... well my interest began when I was a wee kitten. " +
            "My mother was a champion sleeper. She was my inspiration. She slept all throughout my kittenhood, and " +
            "then when I outgrew my childish energy I followed in her pawsteps. I always have such interesting dreams.");
        CompositeStory composite0c = new CompositeStory("My mother was a nice cat. She was big and fluffy, " +
            "not at all like my father. I only saw my father for a moment just seconds after I was old " +
            "enough to open my eyes. Male cats don't stick around too long, you see. We're not generally very interested in raising kittens. " +
            "Anyway, my mother's name was Diamond.");
        CompositeStory composite0d = new CompositeStory("I suppose the most interesting dream I've ever had was one where I was a fish. " +
            "I was swimming in circles in an aquarium, around a little plastic treasure chest, just like the one in the aquarium over in the living room. " +
            "You should look at it sometime. It's very peaceful. In my dream the hand appeared and sprinkled some pellets into the aquarium. " +
            "I swam up to eat them, but when I drew close I saw that the pellets were actually eyeballs. Then I woke up.");


        composite0b.AddInputBasedTransition("(mother|mom)".MatchSomewhere(), composite0c);
        composite0b.AddInputBasedTransition("dream".MatchSomewhere(), composite0d);
        composite0b.AddHint("It's no good... you've got me thinking about my mother... and dreams.");

        composite0a.AddHint("It's somewhat rude to deny a direct request.");
        composite0a.AddInputBasedTransition("sleep".MatchSomewhere(), composite0b);


        composite0c.AddInputBasedTransition("dream".MatchSomewhere(), composite0b);
        composite0d.AddInputBasedTransition("(mother|mom)".MatchSomewhere(), composite0b);

        composite0c.LoopBackToThisVia("Sorry. Bit spaced out. Thinking of my mom does that to me.");
        composite0d.LoopBackToThisVia(string.Format("<--{0}'s just staring into space-->", (CharacterName)0));

        CompositeStory root0 = new CompositeStory("...");

        root0.AddInputBasedTransition("(hobbies|hobby|interests|like to do)".MatchSomewhere(), composite0a);
        root0.AddInputBasedTransition("(cats suck|i hate cats)".MatchSomewhere(), new LeafStory("I'm done talking to you."));
        root0.AddHint("You could ask me about my hobbies...");


        root0.Graft(new CompositeStory("Hello"), "(hello|hi|greetings)".MatchSomewhere());


        return root0;
    }
}
