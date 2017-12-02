using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class ChatToCat : StoryModel {


    private string catName = "Puzzle";


    StoryNode root;
    StoryNode playerAngeredCatRoot;


    protected override void InitializeStoryTitle()
    {
        title = "Chat to cat";
    }


    protected override void InitializeStoryRoot()
    {
      
        root = new StoryNode("...");

        StoryNode riddleBranch = RiddleBranch();

        playerAngeredCatRoot = new StoryNode("Humph. I'm surprised you have the gumption to show your face to me again.");
        StoryNode acceptApology = new StoryNode("Ok whatever.", () => {
            this.storyRoot = root; //cat's okay now, so make the root the generic root.
        });
        playerAngeredCatRoot.AddHint(new StoryNode("I'd accept an apology."));
       
        playerAngeredCatRoot.AddInputBasedTransition("(sorry|i didn't mean it|i apologize|forgive me)".MatchSomewhere(), acceptApology);
        acceptApology.TakeEntireSubgraphOf(root);

        StoryNode hobbies = new StoryNode("I have several hobbies. Eating, sleeping, " +
        "staring out the window. Ask me about one of them.");

  
        root.AddInputBasedTransition("(hobbies|hobby|interests|like to do|what are they)".MatchSomewhere(), hobbies);
        root.AddInputBasedTransition("(cats suck|i hate cats)".MatchSomewhere(), new StoryNode("I'm done talking to you.", ()=>
        {
            this.storyRoot = playerAngeredCatRoot; //next time player engages cat, he'll encounter the player angered cat root.

            View.SetStoryToRoot();

            Timer timer = TimerFactory.Instance.NewTimer();
            timer.onTimeUp = () => { View.Deactivate(); };
            timer.secondsDuration = .8f;
            timer.Begin();
            
       
        }));


        root.AddHint(new StoryNode("You could ask me about my hobbies..."));

        
        root.GraftStep("(hello|hi|greetings)".MatchSomewhere(), new StoryNode("Hello"));


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
   
        window.AddHint(new StoryNode("No one ever wants to hear about the most interesting thing I've seen."));

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
        catKnowsHim.AddHint(new StoryNode(string.Format("{0} looks as though he's remembering something from long ago", catName)));
        catKnowsHim.AddHint(new StoryNode(string.Format("<--{0} looks as though he wants to talk about the other cat-->", catName)));
        catKnowsHim.AddHint(new StoryNode(string.Format("<--{0} just needs an excuse to explain how he knows the other cat-->", catName)));
        
        StoryNode hobbiesWithoutWindow = new StoryNode("My other hobbies are eating and sleeping.");
        hobbiesWithoutWindow.TakeEntireSubgraphOf(hobbies);
        
        //leaf for the window branch
        StoryNode catWantsToStopTalkingAboutRiddle = new StoryNode("Let's discuss something else.", ()=> {
            //can't talk about window anymore, finished with that branch.
            hobbies.RemoveInputBasedTransition("window");
            //go back to hobbies
            View.SetStory(hobbiesWithoutWindow);

            //update the text after a time
            Timer timer = TimerFactory.Instance.NewTimer();
            timer.onTimeUp = () => { View.DisplayTextOfCurrentStoryNode(); };
            timer.secondsDuration = 5f;
            timer.Begin();

            //add a new branch from root.
            root.AddInputBasedTransition("riddle".MatchSomewhere(), riddleBranch);


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

        this.storyRoot = root;
    }



    /*just abstracting different branches into their own functions. keeps things cleaner
     * the riddle branch is only accessible after the character gets through the window branch of the hobbies.
     
         */
    private StoryNode RiddleBranch()
    {
        StoryNode root = new StoryNode("I told you before. Riddle was a childhood friend.");
        StoryNode riddleNotBestFriend = new StoryNode("Riddle was not my best friend, by any stretch of the imagination. But after awhile he became my only friend, and that's close enough.");
        StoryNode sadThingsHappenedToOtherFriends = new StoryNode("Sad things happened to my other friends. I grew up between a road and a park. Cars and coyotes. Lots of cats died.");
        StoryNode weHunted = new StoryNode("Mostly, Riddle and I hunted. Riddle liked hunting better than I did. I didn't see a point.");
        StoryNode huntingIsDumbWeWereHousecats = new StoryNode("Both of us were housecats. We were being fed every day. What need was there to hunt?");
        StoryNode huntedGrasshoppers = new StoryNode("We mostly hunted grasshoppers. There weren't a lot of mice, surprisingly. Maybe the coyotes scared them away.");
        StoryNode kazAvoidedBirds = new StoryNode("I avoided birds. The woman I live with hit me if I ever killed a bird.");
        StoryNode riddleLikedGrassHoppers = new StoryNode("Riddle swore that grasshoppers tasted better than cat food.");
        StoryNode riddleWasObnoxious = new StoryNode("I considered Riddle a bit obnoxious. He was very proud of himself, and I didn't think he was justified in that.");
        StoryNode riddleDidntLikeMeEither = new StoryNode("I don't think Riddle liked me very much, either.");
        StoryNode riddleWantedCompany = new StoryNode("I think Riddle wanted company. Everyone wants company sometimes, especially when things are sad.");
        StoryNode dontHateRiddle = new StoryNode("I don't hate Riddle. But I don't have any interest in speaking to him. I don't think he's interested in speaking to me, either.");
        StoryNode riddleHangsAroundWindow = new StoryNode("Riddle hangs around the window sometimes.");

        root.AddFreeTransition(riddleNotBestFriend);
        riddleNotBestFriend.AddInputBasedTransition("(do|common|get along)".MatchSomewhere(), weHunted);
        weHunted.GraftStep("(why|what.*//?|how come)".MatchSomewhere(), huntingIsDumbWeWereHousecats);
        riddleNotBestFriend.GraftStep("(other|what.*//?|why?|friend)".MatchSomewhere(), sadThingsHappenedToOtherFriends);
        weHunted.AddInputBasedTransition("(what|hunt|catch|kill|go (for|after))".MatchSomewhere(), huntedGrasshoppers);
        huntedGrasshoppers.GraftStep("(gross|disgusting|e+w|sick)".MatchSomewhere(), new StoryNode("Yeah, I know. Look -Riddle- liked grasshoppers. Not me."));



        return root;
    }
}
