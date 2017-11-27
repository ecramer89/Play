using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class ChatToCat : StoryModel {


    private string catName = "Puzzle";


    StoryNode genericRoot;
    StoryNode playerAngeredCatRoot;


    protected override void InitializeStoryTitle()
    {
        title = "Chat to cat";
    }


    protected override void InitializeStoryRoot()
    {
      
        genericRoot = new StoryNode("...");

        playerAngeredCatRoot = new StoryNode("Humph. I'm surprised you have the gumption to show your face to me again.");
        StoryNode acceptApology = new StoryNode("Ok whatever.", () => {
            this.storyRoot = genericRoot; //cat's okay now, so make the root the generic root.
        });
        playerAngeredCatRoot.AddHint(new StoryNode("I'd accept an apology."));
       
        playerAngeredCatRoot.AddInputBasedTransition("(sorry|i didn't mean it|i apologize|forgive me)".MatchSomewhere(), acceptApology);
        acceptApology.TakeEntireSubgraphOf(genericRoot);

        StoryNode hobbies = new StoryNode("I have several hobbies. Eating, sleeping, " +
        "staring out the window. Ask me about one of them.");

  
        genericRoot.AddInputBasedTransition("(hobbies|hobby|interests|like to do|what are they)".MatchSomewhere(), hobbies);
        genericRoot.AddInputBasedTransition("(cats suck|i hate cats)".MatchSomewhere(), new StoryNode("I'm done talking to you.", ()=>
        {
            this.storyRoot = playerAngeredCatRoot; //next time player engages cat, he'll encounter the player angered cat root.

            View.SetStoryToRoot();

            Timer timer = TimerFactory.Instance.NewTimer();
            timer.onTimeUp = () => { View.Deactivate(); };
            timer.secondsDuration = .8f;
            timer.Begin();
            
       
        }));


        genericRoot.AddHint(new StoryNode("You could ask me about my hobbies..."));

        
        genericRoot.GraftStep("(hello|hi|greetings)".MatchSomewhere(), new StoryNode("Hello"));


        StoryNode sleeping = new StoryNode("Sleeping... well my interest began when I was a wee kitten. " +
            "My mother was a champion sleeper. She was my inspiration. She slept all throughout my kittenhood, and " +
            "then when I outgrew my childish energy I followed in her pawsteps. I always have such interesting dreams.");


        StoryNode eating = new StoryNode("You might think that housecats have a limited variety of food, but you'd be wrong. ");

        StoryNode eating1 = new StoryNode("There's a lot of variation among the different food brands.");
        StoryNode eating2 = new StoryNode("There's of course different flavors of cat food, like chicken, fish, beef; then within the flavor types there's wet and dry variants.");
        StoryNode eating3 = new StoryNode("I'm no mathematician, but there's a lot of combinations of cat food.");
    
        eating.GraftStep(eating1);
        eating1.GraftStep(eating2);
        eating2.GraftStep(eating3);

        StoryNode favoriteFoods = new StoryNode("It depends... if it's wet food, probably Friskies chicken. If it's dry food- uh, well. I kind of hate dry food.");

        eating.AddHint(new StoryNode("Are you curious about my preferences?"));
        eating.AddInputBasedTransition("(prefer|favorite|like|love|best)".MatchSomewhere(), favoriteFoods);


        StoryNode window = new StoryNode("Not many people appreciate windows.");
        StoryNode sawSomethingInteresting = new StoryNode("There was a time... I saw something very interesting.");
        sawSomethingInteresting.GraftStep(new StoryNode(string.Format("<--{0} has a faraway look in his eyes -->", catName)));
        window.GraftStep(sawSomethingInteresting);
   
        window.AddHint(new StoryNode("Would you like to hear about the most interesting thing I ever saw through a window?"));

        StoryNode sawAnotherCat = new StoryNode("I saw another cat.");
        StoryNode sawAnotherCat1 = new StoryNode("It was long and black.");
        StoryNode sawAnotherCat2 = new StoryNode("He was interesting because he didn't have a colllar. That's pretty unusual around here.");
        sawAnotherCat.AddFreeTransition(sawAnotherCat1);
        sawAnotherCat1.AddFreeTransition(sawAnotherCat2);
   
        window.AddInputBasedTransition("(see|saw|what was|look)".MatchSomewhere(), sawAnotherCat);

        StoryNode catLooksAwkward = new StoryNode(string.Format("<-- {0} looks very uncomfortable -->", catName));
        sawAnotherCat2.AddFreeTransition(catLooksAwkward);

        catLooksAwkward.GraftStep(new StoryNode("Maybe I shouldn't have brought this up. I don't know why I did."));
        catLooksAwkward.GraftStep(new StoryNode("I feel odd talking about him."));

        StoryNode catKnowsHim = new StoryNode("Well... I knew him.");

        catLooksAwkward.AddInputBasedTransition("(wrong?|okay?|uncomfortable|what's up?)".MatchSomewhere(), catKnowsHim);

        StoryNode hisNameWasRiddle = new StoryNode("He was a childhood friend. His name is Riddle.");

        catKnowsHim.AddInputBasedTransition("(name|who|how|from where|from when|how long)".MatchSomewhere(), hisNameWasRiddle);

        
        StoryNode hobbiesWithoutWindow = new StoryNode("My other hobbies are eating and sleeping.");
        hobbiesWithoutWindow.TakeEntireSubgraphOf(hobbies);
        
        StoryNode catWantsToStopTalkingAboutRiddle = new StoryNode("Let's discuss something else.", ()=> {
            hobbies.RemoveInputBasedTransition("window");
            View.SetStory(hobbiesWithoutWindow);
        });

        hisNameWasRiddle.AddFreeTransition(catWantsToStopTalkingAboutRiddle);

        hobbies.AddInputBasedTransition("window", window);
        hobbies.AddInputBasedTransition("sleep".MatchSomewhere(), sleeping);
        hobbies.AddInputBasedTransition("(eat|food)".MatchSomewhere(), eating);

        hobbies.AddHint(new StoryNode("It's somewhat rude to deny a direct request."));

        StoryNode mother = new StoryNode("My mother was a nice cat. She was big and fluffy, " +
            "not at all like my father. I only saw my father for a moment just seconds after I was old " +
            "enough to open my eyes. Male cats don't stick around too long, you see. We're not generally very interested in raising kittens. " +
            "Anyway, my mother's name was Diamond.");
        StoryNode dreams = new StoryNode("I suppose the most interesting dream I've ever had was one where I was a fish. " +
            "I was swimming in circles in an aquarium, around a little plastic treasure chest, just like the one in the aquarium over in the living room. " +
            "You should look at it sometime. It's very peaceful. In my dream the hand appeared and sprinkled some pellets into the aquarium. " +
            "I swam up to eat them, but when I drew close I saw that the pellets were actually eyeballs. Then I woke up.");


        sleeping.AddInputBasedTransition("(mother|mom)".MatchSomewhere(), mother);
        sleeping.AddInputBasedTransition("dream".MatchSomewhere(), dreams);
        sleeping.AddHint(new StoryNode("It's no good... you've got me thinking about my mother... and dreams."));

        mother.AddInputBasedTransition("dream".MatchSomewhere(), sleeping);
        dreams.AddInputBasedTransition("(mother|mom)".MatchSomewhere(), sleeping);

        mother.GraftLoop(new StoryNode("Sorry. Bit spaced out. Thinking of my mom does that to me."), new StoryNode(string.Format("<--{0} is thinking about his mother.-->", catName)));
        dreams.GraftLoop(new StoryNode("Dreams are pretty strange. I wonder if other animals have them."), new StoryNode(string.Format("<--{0}'s just staring into space-->", catName)));

        this.storyRoot = genericRoot;
    }
}
