using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class CatData : CharacterData {




    protected override void InitializeCharacterName()
    {
        characterName = new CharacterName("Cat");
    }


    protected override void InitializeStoryRoot()
    {
      
        Story root = new Story("...");

        Story hobbies = new Story("I have several hobbies. Eating, sleeping, " +
        "staring out the window. Ask me about one of them.");

  
        root.AddInputBasedTransition("(hobbies|hobby|interests|like to do)".MatchSomewhere(), hobbies);
        root.AddInputBasedTransition("(cats suck|i hate cats)".MatchSomewhere(), new Story("I'm done talking to you.", ()=>
        {
            Debug.Log("do something interested and probably deactivate since this is a leaf"); //consider implemneting factry pattern to create different kinds of storis
        }));


        root.AddHint(new Story("You could ask me about my hobbies..."));
        root.GraftStep("(hello|hi|greetings)".MatchSomewhere(), new Story("Hello"));

      
        Story sleeping = new Story("Sleeping... well my interest began when I was a wee kitten. " +
            "My mother was a champion sleeper. She was my inspiration. She slept all throughout my kittenhood, and " +
            "then when I outgrew my childish energy I followed in her pawsteps. I always have such interesting dreams.");


        Story eating = new Story("You might think that housecats have a limited variety of food, but you'd be wrong. ");

        Story eating1 = new Story("There's a lot of variation among the different food brands.");
        Story eating2 = new Story("There's of course different flavors of cat food, like chicken, fish, beef; then within the flavor types there's wet and dry variants.");
        Story eating3 = new Story("I'm no mathematician, but there's a lot of combinations of cat food.");
    
        eating.GraftStep(eating1);
        eating1.GraftStep(eating2);
        eating2.GraftStep(eating3);

        Story favoriteFoods = new Story("It depends... if it's wet food, probably Friskies chicken. If it's dry food- uh, well. I kind of hate dry food.");

        eating.AddHint(new Story("Are you curious about my preferences?"));
        eating.AddInputBasedTransition("(prefer|favorite|like|love|best)".MatchSomewhere(), favoriteFoods);


        hobbies.AddInputBasedTransition("sleep".MatchSomewhere(), sleeping);
        hobbies.AddInputBasedTransition("(eat|food)".MatchSomewhere(), eating);

        hobbies.AddHint(new Story("It's somewhat rude to deny a direct request."));

        Story mother = new Story("My mother was a nice cat. She was big and fluffy, " +
            "not at all like my father. I only saw my father for a moment just seconds after I was old " +
            "enough to open my eyes. Male cats don't stick around too long, you see. We're not generally very interested in raising kittens. " +
            "Anyway, my mother's name was Diamond.");
        Story dreams = new Story("I suppose the most interesting dream I've ever had was one where I was a fish. " +
            "I was swimming in circles in an aquarium, around a little plastic treasure chest, just like the one in the aquarium over in the living room. " +
            "You should look at it sometime. It's very peaceful. In my dream the hand appeared and sprinkled some pellets into the aquarium. " +
            "I swam up to eat them, but when I drew close I saw that the pellets were actually eyeballs. Then I woke up.");


        sleeping.AddInputBasedTransition("(mother|mom)".MatchSomewhere(), mother);
        sleeping.AddInputBasedTransition("dream".MatchSomewhere(), dreams);
        sleeping.AddHint(new Story("It's no good... you've got me thinking about my mother... and dreams."));

        mother.AddInputBasedTransition("dream".MatchSomewhere(), sleeping);
        dreams.AddInputBasedTransition("(mother|mom)".MatchSomewhere(), sleeping);

        mother.GraftLoop("Sorry. Bit spaced out. Thinking of my mom does that to me.", new Story(string.Format("<--{0} is thinking about his mother.-->", characterName)));
        dreams.GraftLoop("Dreams are pretty strange. I wonder if other animals have them.", new Story(string.Format("<--{0}'s just staring into space-->", characterName)));

        this.storyRoot = root;
    }
}
