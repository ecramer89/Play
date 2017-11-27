using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System;


public class StoryNode  {
    public string text;
    public Action effectUponReaching = () => { };

    private Dictionary<string, StoryNode> inputBasedTransitions; //stories the player can only reach by inputting a string that matches some pattern.
    private List<StoryNode> flavorText; //stories that the player can reach whatever they input.
    private List<StoryNode> hints;

    private event Action<StoryNode> HintAddedToStory = (StoryNode hint) => { };
    private event Action<StoryNode> FlavorTextAddedToStory = (StoryNode flavorText) => { };
    private event Action<string, StoryNode> InputBasedTransitionAddedToStory = (string input, StoryNode transition) => { };
   

    public StoryNode(string text)  {
        this.text = text;
        this.inputBasedTransitions = new Dictionary<string, StoryNode>();
        this.flavorText = new List<StoryNode>();
        this.hints = new List<StoryNode>();
    }

    public StoryNode(string text, Action effectUponReaching) : this(text)
    {
        this.effectUponReaching = effectUponReaching;
    }

    /*
     * a recent replacement for loop back to this via... the idea is that I don't think it makes much sense to loop back to the exact same text.
     * what we want to loop back to is a node with different text then the original, but the same subgraph. so instead of looping back to the original node
     * we loop back to a new node which is basically a graft. 
     * 
     * */

    public void GraftLoop(string via, StoryNode graftIn)
    {
        StoryNode intermediary = new StoryNode(via);
        graftIn.TakeEntireSubgraphOf(this);
        intermediary.AddFlavorText(graftIn);
        AddFlavorText(intermediary);
    }


    /*
     * a hint is basically like a change in the text; it has the same subgraph as the node from which the hint is accessed
     * so users can access the same subgraph from the hint node as they could from the parent. the hint node also inherits the parent's hints,
     * minus itself. 
     * 
     * in the actual game, when the user fails to match a transition, hints will be delivered before free transitions.
     * 
     * */
    public void AddHint(StoryNode hint)
    {
       
        //note that hints don't take the entire hint set of this;
        //each hint should only take the hints that -would follow it- in the queue of its parent,
        //so the specific hints that any one will get depends on what's added afterwards.
        hint.TakeTransitionsOf(this);
        hint.TakeFlavorTextOf(this);
        hints.Add(hint);

        //notify subscribers who also want to take this hint
        HintAddedToStory(hint); 
    }

    public void AddInputBasedTransition(string inputPattern, StoryNode transitionTo)
    {
        inputBasedTransitions[inputPattern] = transitionTo;

        InputBasedTransitionAddedToStory(inputPattern, transitionTo);
    }

    public void AddFlavorText(StoryNode transitonTo)
    {
        flavorText.Add(transitonTo);

        FlavorTextAddedToStory(transitonTo);
    }

    public StoryNode GetTransition(string forInput)
    {
        StoryNode result = TryMatchInput(forInput);
        if (result != null) return result;

        if (hints.Count > 0)
        {
            StoryNode hint = hints.First();
            hint.hints = hints.Skip(1).Take(hints.Count - 1).ToList();
            return hint;
        }

        return GetRandomFlavorText();

    }

   
    /*
     * very similar to loopGraft, only the transition to the grafted composite is an input pattern, as opposed to a free transition as 
     * is assumed with LoopGraft. Use Graft to create additional steps in a story w/o having to actually build out new subgraphs.
     * 
     * note: all "graft" methods establish a persistent directed link between this and param CompositeStory. the param CompositeStory will 
     * get all hints/input based transitions/flavor texts that are cureently saved to this and all future ones, but this won't get the hints/flavor texts/etc that are
     * added to the grafted in story
     * */
    public void GraftStep(string viaInput, StoryNode step)
    {
        step.TakeEntireSubgraphOf(this);
        this.AddInputBasedTransition(viaInput, step);
      

    }

    /*
     *  graft in an intermediary step that the player can trigger via entering anything.
     * 
     * */
    public void GraftStep(StoryNode step)
    {
        step.TakeEntireSubgraphOf(this);
        this.AddFlavorText(step);

    }

    private void TakeEntireSubgraphOf(StoryNode of)
    {
        TakeTransitionsOf(of);
        TakeFlavorTextOf(of);
        TakeHintsOf(of);
    }

    private void TakeTransitionsOf(StoryNode of)
    {
        foreach (KeyValuePair<string, StoryNode> transition in of.inputBasedTransitions)
        {
            //important to invoke method, not directly update dictionary key, 
            //because anything nodes that were grafted into this should also receive the subgraph of of.
            this.AddInputBasedTransition(transition.Key, transition.Value);
            
        }

        of.InputBasedTransitionAddedToStory += AddInputBasedTransition;


    }

    private void TakeFlavorTextOf(StoryNode of)
    {
        foreach (StoryNode free in of.flavorText)
        {
            this.AddFlavorText(free);
        }

        of.FlavorTextAddedToStory += AddFlavorText;

    }

    private void TakeHintsOf(StoryNode of)
    {
        //take all the hints that of currently possesses
        foreach(StoryNode hint in of.hints){
            this.AddHint(hint);
        }

        //subscribe to take all hints that will be added to of in the future
        of.HintAddedToStory += AddHint;
    }

    private StoryNode TryMatchInput(string forInput)
    {

        foreach (KeyValuePair<string, StoryNode> entry in inputBasedTransitions)
        {
            
            if (Regex.IsMatch(forInput, entry.Key))
            {
                return entry.Value;
            }

        }

        return null;
    }

    private StoryNode GetRandomFlavorText()
    {   if (flavorText.Count == 0) return this;
        return flavorText[Utils.random.Next(flavorText.Count)];
    }

}


