using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;
using UnityEngine.UI;
using System.Reflection;


public class ChatToCat : StoryModel {

    [SerializeField] Sprite[] characterImages;
    private string catName = "Puzzle";


    StoryNode root = new StoryNode("...");

    //Any branch you want to reference globally (i.e., from other branches), needs to be defined and initialized here.
    StoryNode playerAngeredCat = new StoryNode("Humph. I'm surprised you have the gumption to show your face to me again.");

    StoryNode catHobbies = new StoryNode("I have several hobbies. Eating, sleeping, " +
        "staring out the window. Ask me about one of them.");

    StoryNode hobbiesWindow = new StoryNode("Not many people appreciate windows.");

    StoryNode hobbiesEating = new StoryNode("You might think that housecats have a limited variety of food, but you'd be wrong. ");

    StoryNode hobbiesSleeping = new StoryNode("Sleeping... well my interest began when I was a wee kitten. " +
         "My mother was a champion sleeper. She was my inspiration. She slept all throughout my kittenhood, and " +
         "then when I outgrew my childish energy I followed in her pawsteps. I always have such interesting dreams.");

    StoryNode riddle = new StoryNode("I told you before. Riddle was a childhood friend.");

    protected override void InitializeStoryTitle()
    {
        title = "Chat to cat";
    }


    protected override void InitRoot()
    {
      

        root.AddInputBasedTransition("(hobbies|hobby|interests|like to do|what are they)".MatchSomewhere(), catHobbies);
        root.AddInputBasedTransition("(cats suck|i hate cats)".MatchSomewhere(), new StoryNode("I'm done talking to you.", ()=>
        {
            this.storyRoot = playerAngeredCat; //next time player engages cat, he'll encounter the player angered cat root.

            View.SetStoryToRoot();

            Timer timer = TimerFactory.Instance.NewTimer();
            timer.onTimeUp = () => { View.Deactivate(); };
            timer.secondsDuration = .8f;
            timer.Begin();
            
       
        }));

    
        root.AddHint(new StoryNode("You could ask me about my hobbies..."));
        root.GraftStep("(hello|hi|greetings)".MatchSomewhere(), new StoryNode("Hello"));

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


            foreach(MethodInfo mi in this.GetType().GetMethods())
            {
                if (mi.Name.Contains("Init"))
                {
                    mi.Invoke(this, (Object[])null);
                }
            }
        }




        private void InitHobbies()
        {

            container.catHobbies.AddInputBasedTransition("window", container.hobbiesWindow);
            container.catHobbies.AddInputBasedTransition("sleep".MatchSomewhere(), container.hobbiesSleeping);
            container.catHobbies.AddInputBasedTransition("(eat|food)".MatchSomewhere(), container.hobbiesEating);

            container.catHobbies.AddHint(new StoryNode("It's somewhat rude to deny a direct request."));
        }



        private void InitPlayerAngeredCat()
        {
            StoryNode acceptApology = new StoryNode("Ok whatever.", () =>
            {
                container.storyRoot = container.root; //cat's okay now, so make the root the generic root.
            });

            container.playerAngeredCat.AddHint(new StoryNode("I'd accept an apology."));

            container.playerAngeredCat.AddInputBasedTransition("(sorry|i didn't mean it|i apologize|forgive me)".MatchSomewhere(), acceptApology);
            acceptApology.TakeEntireSubgraphOf(container.root);
        }


        private void InitWindow()
        {
            StoryNode sawSomethingInteresting = new StoryNode("There was a time... I saw something very interesting.");
            sawSomethingInteresting.GraftStep(new StoryNode(string.Format("<--{0} has a faraway look in his eyes -->", container.catName)));
            container.hobbiesWindow.GraftStep(sawSomethingInteresting);

            container.hobbiesWindow.AddHint(new StoryNode("No one ever wants to hear about the most interesting thing I've seen."));

            StoryNode sawAnotherCat = new StoryNode("I saw another cat.");
            StoryNode sawAnotherCat1 = new StoryNode("It was long and black.");
            StoryNode sawAnotherCat2 = new StoryNode("He was interesting because he didn't have a colllar. That's pretty unusual around here.");
            sawAnotherCat.AddFreeTransition(sawAnotherCat1);
            sawAnotherCat1.AddFreeTransition(sawAnotherCat2);

            container.hobbiesWindow.AddInputBasedTransition("(see|saw|what was|look)".MatchSomewhere(), sawAnotherCat);

            StoryNode catLooksAwkward = new StoryNode(string.Format("<-- {0} looks very uncomfortable -->", container.catName));
            sawAnotherCat2.AddFreeTransition(catLooksAwkward);

            catLooksAwkward.GraftStep(new StoryNode("Maybe I shouldn't have brought this up. I don't know why I did."));
            catLooksAwkward.GraftStep(new StoryNode("I feel odd talking about him."));

            StoryNode catKnowsHim = new StoryNode("Well... I knew him.");

            catLooksAwkward.AddInputBasedTransition("(wrong?|okay?|uncomfortable|what's up?)".MatchSomewhere(), catKnowsHim);

            StoryNode hisNameWasRiddle = new StoryNode("He was a childhood friend. His name is Riddle.");

            catKnowsHim.AddInputBasedTransition("(name|who|how|from where|from when|how long)".MatchSomewhere(), hisNameWasRiddle);
            catKnowsHim.AddHint(new StoryNode(string.Format("{0} looks as though he's remembering something from long ago", container.catName)));
            catKnowsHim.AddHint(new StoryNode(string.Format("<--{0} looks as though he wants to talk about the other cat-->", container.catName)));
            catKnowsHim.AddHint(new StoryNode(string.Format("<--{0} just needs an excuse to explain how he knows the other cat-->", container.catName)));

            StoryNode hobbiesWithoutWindow = new StoryNode("My other hobbies are eating and sleeping.");
            hobbiesWithoutWindow.TakeEntireSubgraphOf(container.catHobbies);

            //leaf for the window branch
            StoryNode catWantsToStopTalkingAboutRiddle = new StoryNode("Let's discuss something else.", () =>
            {
                //can't talk about window anymore, finished with that branch.
                container.catHobbies.RemoveInputBasedTransition("window");
                //go back to hobbies
                container.View.SetStory(hobbiesWithoutWindow);

                //update the text after a time
                Timer timer = TimerFactory.Instance.NewTimer();
                timer.onTimeUp = () => { container.View.DisplayTextOfCurrentStoryNode(); };
                timer.secondsDuration = 5f;
                timer.Begin();

                //add a new branch from root.
                container.root.AddInputBasedTransition("riddle".MatchSomewhere(), container.riddle);


            });

            hisNameWasRiddle.AddFreeTransition(catWantsToStopTalkingAboutRiddle);
        }


        private void InitSleepingHobby()
        {
            StoryNode mother = new StoryNode("My mother was a nice cat. She was big and fluffy, " +
                "not at all like my father. I only saw my father for a moment just seconds after I was old " +
                "enough to open my eyes. Male cats don't stick around too long, you see. We're not generally very interested in raising kittens. " +
                "Anyway, my mother's name was Diamond.");

            StoryNode dreams = new StoryNode("I suppose the most interesting dream I've ever had was one where I was a fish. " +
                "I was swimming in circles in an aquarium, around a little plastic treasure chest, just like the one in the aquarium over in the living room. " +
                "You should look at it sometime. It's very peaceful. In my dream the hand appeared and sprinkled some pellets into the aquarium. " +
                "I swam up to eat them, but when I drew close I saw that the pellets were actually eyeballs. Then I woke up.");


            container.hobbiesSleeping.AddInputBasedTransition("(mother|mom)".MatchSomewhere(), mother);
            container.hobbiesSleeping.AddInputBasedTransition("dream".MatchSomewhere(), dreams);
            container.hobbiesSleeping.AddHint(new StoryNode("It's no good... you've got me thinking about my mother... and dreams."));

            mother.AddInputBasedTransition("dream".MatchSomewhere(), container.hobbiesSleeping);
            dreams.AddInputBasedTransition("(mother|mom)".MatchSomewhere(), container.hobbiesSleeping);

            mother.GraftLoop(new StoryNode("Sorry. Bit spaced out. Thinking of my mom does that to me."), new StoryNode(string.Format("<--{0} is thinking about his mother.-->", container.catName)));
            dreams.GraftLoop(new StoryNode("Dreams are pretty strange. I wonder if other animals have them."), new StoryNode(string.Format("<--{0}'s just staring into space-->", container.catName)));

        }

        private void InitEatingHobby()
        {
            StoryNode eating1 = new StoryNode("There's a lot of variation among the different food brands.");
            StoryNode eating2 = new StoryNode("There's of course different flavors of cat food, like chicken, fish, beef; then within the flavor types there's wet and dry variants.");
            StoryNode eating3 = new StoryNode("I'm no mathematician, but there's a lot of combinations of cat food.");

            container.hobbiesEating.GraftStep(eating1);
            eating1.GraftStep(eating2);
            eating2.GraftStep(eating3);

            StoryNode favoriteFoods = new StoryNode("It depends... if it's wet food, probably Friskies chicken. If it's dry food- uh, well. I kind of hate dry food.");

            container.hobbiesEating.AddHint(new StoryNode("Are you curious about my preferences?"));
            container.hobbiesEating.AddInputBasedTransition("(prefer|favorite|like|love|best)".MatchSomewhere(), favoriteFoods);

        }


        private void InitRiddle()
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

            container.riddle.AddFreeTransition(riddleNotBestFriend);
            riddleNotBestFriend.AddInputBasedTransition("(do|common|get along)".MatchSomewhere(), weHunted);
            weHunted.GraftStep("(why|what.*//?|how come)".MatchSomewhere(), huntingIsDumbWeWereHousecats);
            riddleNotBestFriend.GraftStep("(other|what.*//?|why?|friend)".MatchSomewhere(), sadThingsHappenedToOtherFriends);
            weHunted.AddInputBasedTransition("(what|hunt|catch|kill|go (for|after))".MatchSomewhere(), huntedGrasshoppers);
            huntedGrasshoppers.GraftStep("(gross|disgusting|e+w|sick)".MatchSomewhere(), new StoryNode("Yeah, I know. Look -Riddle- liked grasshoppers. Not me."));

        }

    }
}
