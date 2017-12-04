using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;
using UnityEngine.UI;
using System.Reflection;
using System;

public class ChatToCat : StoryModel {


    private static Sprite[] catImages;
    //image array index codes
    const int ZERO = 0;
    const int ONE = 1;
    const int TWO = 2;
    const int THREE = 3;
    const int FOUR = 4;
    const int FIVE = 5;
    const int SIX = 6;
    const int SEVEN = 7;
    const int EIGHT = 8;
    const int NINE = 9;
    const int TEN = 10;
    const int ELEVEN = 11;



    private static string catName = "Puzzle";


    private static StoryNode root = new StoryNode("...");

    //Any branch you want to reference globally (i.e., from other branches), needs to be defined and initialized here.
    private static StoryNode playerAngeredCat = new StoryNode("Humph. I'm surprised you have the gumption to show your face to me again.");

    private static StoryNode catHobbies = new StoryNode("I have several hobbies. Eating, sleeping, " +
        "staring out the window. Ask me about one of them.");

    private static StoryNode hobbiesWindow = new StoryNode("Not many people appreciate windows.");

    private static StoryNode hobbiesEating = new StoryNode("You might think that housecats have a limited variety of food, but you'd be wrong. ");

    private static StoryNode hobbiesSleeping = new StoryNode("Sleeping... well my interest began when I was a wee kitten. " +
         "My mother was a champion sleeper. She was my inspiration. She slept all throughout my kittenhood, and " +
         "then when I outgrew my childish energy I followed in her pawsteps. I always have such interesting dreams.");

    private static StoryNode riddle = new StoryNode("I told you before. Riddle was a childhood friend.");



    //for persisting:
    //i think we will need to construct the 'base' models no matter what
    //the file can indicate the referential structure of the tree
    //and what you can construct is a 'template' that wont have any of the actual data
    //it will be a special "tree info" class object just containingn lists and dictis of strings
    //the strings (or ints) will be a special id that needs be consistent always
    //could just be the objects hash code i guess; override so it's based on the hash coide of the text
    //then, dump EACH individual storynode (including even those defined -inline-, not referenced outside of a branch)
    //into a set or a dict so that
    //we can retrieve the actual object data from the set at runtime given the ids in the map.

        //IMPORTANT the identifier CANNOT be base don the text because that does actually change. you will need to assign each a unique id that DOES NOT CHANGE
        //e.g. we change the text of the hobbies node after we finish the window branch

    private Action UpdateImageAction(int imageCode)
    {
       return () => View.UpdateImage(catImages[imageCode]);
    }


    public void Start()
    {
        catImages = Resources.LoadAll<Sprite>("Images/cat") as Sprite[];
    }



    protected override void InitializeStoryTitle()
    {
        title = "Chat to cat";
    }


    protected override void InitRoot()
    {

        root.effectUponReaching = () => { root.text = "Oh, you again."; };
        root.AddInputBasedTransition("(hobbies|hobby|interests|like to do|what are they)".MatchSomewhere(), catHobbies);
        root.AddInputBasedTransition("(cats suck|i hate cats)".MatchSomewhere(), new StoryNode("I'm done talking to you.", ()=>
        {
            this.storyRoot = playerAngeredCat; //next time player engages cat, he'll encounter the player angered cat root.

            View.SetStoryToRoot();

            View.UpdateImage(catImages[EIGHT]);

            Timer timer = TimerFactory.Instance.NewTimer();
            timer.onTimeUp = () => { View.Deactivate(); };
            timer.secondsDuration = .8f;
            timer.Begin();
            
       
        }));

    
        root.AddHint(new StoryNode("You could ask me about my hobbies...", UpdateImageAction(SIX)));
        root.GraftStep("(hello|hi|greetings)".MatchSomewhere(), new StoryNode("Hello", UpdateImageAction(FOUR)));

        new BranchInitializer(this);

        this.storyRoot = root;
    }



    //special class just to make the initialization of the different branches more convenient. 
    //to add data to a branch, just define a method whose name contains "Init" within this class.
    //when the class is instantiated from init root, 
    //all methods whose names contain "init" will be invoked (via reflection).
    class BranchInitializer
    {

        private ChatToCat container;

        public BranchInitializer(ChatToCat container)
        {
            this.container = container;

         

            foreach(MethodInfo mi in typeof(BranchInitializer).GetMethods(BindingFlags.Public | BindingFlags.Instance ))
            {
               
                if (mi.Name.Contains("Init"))
                {
                   
                       mi.Invoke(this,null);
                }
            }
        }


     


        public void InitHobbies()
        {
            catHobbies.effectUponReaching = container.UpdateImageAction(SEVEN);
            catHobbies.AddInputBasedTransition("window", hobbiesWindow);
            catHobbies.AddInputBasedTransition("sleep".MatchSomewhere(), hobbiesSleeping);
            catHobbies.AddInputBasedTransition("(eat|food)".MatchSomewhere(), hobbiesEating);

           catHobbies.AddHint(new StoryNode("It's somewhat rude to deny a direct request.", container.UpdateImageAction(SIX)));
        }



        public void InitPlayerAngeredCat()
        {
            
            StoryNode acceptApology = new StoryNode("Ok whatever.", () =>
            {
                container.storyRoot = root; //cat's okay now, so make the root the generic root.
                container.View.UpdateImage(catImages[ZERO]);
            });

            playerAngeredCat.AddHint(new StoryNode("I'd accept an apology."));

            playerAngeredCat.AddInputBasedTransition("(sorry|i didn't mean it|i apologize|forgive me)".MatchSomewhere(), acceptApology);
            acceptApology.TakeEntireSubgraphOf(root);
        }


        public void InitWindow()
        {
            hobbiesWindow.effectUponReaching = container.UpdateImageAction(3);
            StoryNode sawSomethingInteresting = new StoryNode("There was a time... I saw something very interesting.", container.UpdateImageAction(ONE));
            sawSomethingInteresting.GraftStep(new StoryNode(string.Format("<--{0} has a faraway look in his eyes -->", catName), container.UpdateImageAction(FIVE)));
            hobbiesWindow.GraftStep(sawSomethingInteresting);

            hobbiesWindow.AddHint(new StoryNode("No one ever wants to hear about the most interesting thing I've seen.", container.UpdateImageAction(TWO)));

            StoryNode sawAnotherCat = new StoryNode("I saw another cat.", container.UpdateImageAction(ZERO));
            StoryNode sawAnotherCat1 = new StoryNode("It was long and black.", container.UpdateImageAction(ONE));
            StoryNode sawAnotherCat2 = new StoryNode("He was interesting because he didn't have a colllar. That's pretty unusual around here.", container.UpdateImageAction(FIVE));
            sawAnotherCat.AddFreeTransition(sawAnotherCat1);
            sawAnotherCat1.AddFreeTransition(sawAnotherCat2);

            hobbiesWindow.AddInputBasedTransition("(see|saw|what was|look)".MatchSomewhere(), sawAnotherCat);

            StoryNode catLooksAwkward = new StoryNode(string.Format("<-- {0} looks very uncomfortable -->", catName), container.UpdateImageAction(TWO));
            sawAnotherCat2.AddFreeTransition(catLooksAwkward);

            catLooksAwkward.GraftStep(new StoryNode("Maybe I shouldn't have brought this up. I don't know why I did.", container.UpdateImageAction(6)));
            catLooksAwkward.GraftStep(new StoryNode("I feel odd talking about him.", container.UpdateImageAction(9)));

            StoryNode catKnowsHim = new StoryNode("Well... I knew him.", container.UpdateImageAction(NINE));

            catLooksAwkward.AddInputBasedTransition("(wrong?|okay?|uncomfortable|what's up?)".MatchSomewhere(), catKnowsHim);

            StoryNode hisNameWasRiddle = new StoryNode("He was a childhood friend. His name is Riddle.", container.UpdateImageAction(ELEVEN));

            catKnowsHim.AddInputBasedTransition("(name|who|how|from where|from when|how long|other cat|the cat|tell me)".MatchSomewhere(), hisNameWasRiddle);
            catKnowsHim.AddHint(new StoryNode(string.Format("{0} looks as though he's remembering something from long ago", catName), container.UpdateImageAction(ZERO)));
            catKnowsHim.AddHint(new StoryNode(string.Format("<--{0} looks as though he wants to talk about the other cat-->", catName)));
            catKnowsHim.AddHint(new StoryNode(string.Format("<--{0} just needs an excuse to explain how he knows the other cat-->", catName)));

         
            //leaf for the window branch
            StoryNode catWantsToStopTalkingAboutRiddle = new StoryNode("Let's discuss something else.", () =>
            {
                //can't talk about window anymore, finished with that branch.
                catHobbies.text = "My other hobbies are eating and sleeping.";
                catHobbies.RemoveInputBasedTransition("window");
                //go back to hobbies
                container.View.SetStory(catHobbies);

                //update the text after a time
                Timer timer = TimerFactory.Instance.NewTimer();
                timer.onTimeUp = () => {

                    container.View.DisplayTextOfCurrentStoryNode();
                    container.View.UpdateImage(catImages[ZERO]);

                };
                timer.secondsDuration = 3f;
                timer.Begin();

                //add a new branch, from any other.
                root.AddInputBasedTransition("riddle".MatchSomewhere(), riddle);


                //player completed this branch, so give the player an item...
                MyEvents.PlayerGivenItem.Fire(new Item(string.Format("{0} knew a cat called Riddle", catName), string.Format("{0} seemed uncomfortable thinking about Riddle", catName)));


            });

            hisNameWasRiddle.AddFreeTransition(catWantsToStopTalkingAboutRiddle);
        }


        public void InitSleepingHobby()
        {
            StoryNode mother = new StoryNode("My mother was a nice cat. She was big and fluffy, " +
                "not at all like my father. I only saw my father for a moment just seconds after I was old " +
                "enough to open my eyes. Male cats don't stick around too long, you see. We're not generally very interested in raising kittens. " +
                "Anyway, my mother's name was Diamond.", container.UpdateImageAction(FOUR));

            StoryNode dreams = new StoryNode("I suppose the most interesting dream I've ever had was one where I was a fish. " +
                "I was swimming in circles in an aquarium, around a little plastic treasure chest, just like the one in the aquarium over in the living room. " +
                "You should look at it sometime. It's very peaceful. In my dream the hand appeared and sprinkled some pellets into the aquarium. " +
                "I swam up to eat them, but when I drew close I saw that the pellets were actually eyeballs. Then I woke up.", ()=>{
                    container.View.UpdateImage(catImages[FIVE]);

                });


            hobbiesSleeping.AddInputBasedTransition("(mother|mom)".MatchSomewhere(), mother);
            hobbiesSleeping.AddInputBasedTransition("dream".MatchSomewhere(), dreams);
            hobbiesSleeping.AddHint(new StoryNode("It's no good... you've got me thinking about my mother... and dreams.", container.UpdateImageAction(TEN))
            );

            mother.AddInputBasedTransition("dream".MatchSomewhere(), hobbiesSleeping);
            dreams.AddInputBasedTransition("(mother|mom)".MatchSomewhere(), hobbiesSleeping);

            mother.GraftLoop(new StoryNode("Sorry. Bit spaced out. Thinking of my mom does that to me."), new StoryNode(string.Format("<--{0} is thinking about his mother.-->", catName)));
            dreams.GraftLoop(new StoryNode("Dreams are pretty strange. I wonder if other animals have them."), new StoryNode(string.Format("<--{0}'s just staring into space-->", catName)));

        }

        public void InitEatingHobby()
        {
            StoryNode eating1 = new StoryNode("There's a lot of variation among the different food brands.", container.UpdateImageAction(FOUR));
            StoryNode eating2 = new StoryNode("There's of course different flavors of cat food, like chicken, fish, beef; then within the flavor types there's wet and dry variants.");
            StoryNode eating3 = new StoryNode("I'm no mathematician, but there's a lot of combinations of cat food.");

            hobbiesEating.GraftStep(eating1);
            eating1.GraftStep(eating2);
            eating2.GraftStep(eating3);

            StoryNode favoriteFoods = new StoryNode("It depends... if it's wet food, probably Friskies chicken. " +
                "If it's dry food- uh, well. I kind of hate dry food.", container.UpdateImageAction(TEN));

            hobbiesEating.AddHint(new StoryNode("Are you curious about my preferences?"));
            hobbiesEating.AddInputBasedTransition("(prefer|favorite|like|love|best)".MatchSomewhere(), favoriteFoods);

        }


        public void InitRiddle()
        {

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

            riddle.AddFreeTransition(riddleNotBestFriend);
            riddleNotBestFriend.AddInputBasedTransition("(do|common|get along)".MatchSomewhere(), weHunted);
            weHunted.GraftStep("(why|what.*//?|how come)".MatchSomewhere(), huntingIsDumbWeWereHousecats);
            riddleNotBestFriend.GraftStep("(other|what.*//?|why?|friend)".MatchSomewhere(), sadThingsHappenedToOtherFriends);
            weHunted.AddInputBasedTransition("(what|hunt|catch|kill|go (for|after))".MatchSomewhere(), huntedGrasshoppers);
            huntedGrasshoppers.GraftStep("(gross|disgusting|e+w|sick)".MatchSomewhere(), new StoryNode("Yeah, I know. Look -Riddle- liked grasshoppers. Not me."));

        }

    }
}
