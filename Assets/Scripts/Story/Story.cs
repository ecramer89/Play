using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System;

using UnityEngine;
public abstract class Story
{
    
    public string text;

   
    public Story(string text)
    {
        this.text = text;
    }

    public abstract Story GetTransition(string forInput);
    

}

public class CompositeStory : Story {
  
    private Dictionary<string, Story> inputBasedTransitions; //stories the player can only reach by inputting a string that matches some pattern.
    private List<Story> flavorText; //stories that the player can reach whatever they input.
    private List<CompositeStory> hints;

    private event Action<CompositeStory> hintAddedToStory = (CompositeStory hint) => { };
    private event Action<Story> flavorTextAddedToStory = (Story flavorText) => { };
    private event Action<string, Story> inputBasedTransitionAddedToStory = (string input, Story transition) => { };
   

    public CompositeStory(string text) : base(text) {
        this.inputBasedTransitions = new Dictionary<string, Story>();
        this.flavorText = new List<Story>();
        this.hints = new List<CompositeStory>();
    }
     
    /*
     * a recent replacement for loop back to this via... the idea is that I don't think it makes much sense to loop back to the exact same text.
     * what we want to loop back to is a node with different text then the original, but the same subgraph. so instead of looping back to the original node
     * we loop back to a new node which is basically a graft. 
     * 
     * */

    public void GraftLoop(string via, CompositeStory graftIn)
    {
        CompositeStory intermediary = new CompositeStory(via);
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
    public void AddHint(CompositeStory hint)
    {
       
        //note that hints don't take the entire hint set of this;
        //each hint should only take the hints that -would follow it- in the queue of its parent,
        //so the specific hints that any one will get depends on what's added afterwards.
        hint.TakeTransitionsOf(this);
        hint.TakeFlavorTextOf(this);
        hints.Add(hint);

        //notify subscribers who also want to take this hint
        hintAddedToStory(hint); 
    }

    public void AddInputBasedTransition(string inputPattern, Story transitionTo)
    {
        inputBasedTransitions[inputPattern] = transitionTo;

        inputBasedTransitionAddedToStory(inputPattern, transitionTo);
    }

    public void AddFlavorText(Story transitonTo)
    {
        flavorText.Add(transitonTo);

        flavorTextAddedToStory(transitonTo);
    }

    public override Story GetTransition(string forInput)
    {
        Story result = TryMatchInput(forInput);
        if (result != null) return result;

        if (hints.Count > 0)
        {
            CompositeStory hint = hints.First();
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
    public void GraftStep(string viaInput, CompositeStory step)
    {
        step.TakeEntireSubgraphOf(this);
        this.AddInputBasedTransition(viaInput, step);
      

    }

    /*
     *  graft in an intermediary step that the player can trigger via entering anything.
     * 
     * */
    public void GraftStep(CompositeStory step)
    {
        step.TakeEntireSubgraphOf(this);
        this.AddFlavorText(step);

    }

    private void TakeEntireSubgraphOf(CompositeStory of)
    {
        TakeTransitionsOf(of);
        TakeFlavorTextOf(of);
        TakeHintsOf(of);
    }

    private void TakeTransitionsOf(CompositeStory of)
    {
        foreach (KeyValuePair<string, Story> transition in of.inputBasedTransitions)
        {
            //important to invoke method, not directly update dictionary key, 
            //because anything nodes that were grafted into this should also receive the subgraph of of.
            this.AddInputBasedTransition(transition.Key, transition.Value);
            
        }

        of.inputBasedTransitionAddedToStory += AddInputBasedTransition;


    }

    private void TakeFlavorTextOf(CompositeStory of)
    {
        foreach (Story free in of.flavorText)
        {
            this.AddFlavorText(free);
        }

        of.flavorTextAddedToStory += AddFlavorText;

    }

    private void TakeHintsOf(CompositeStory of)
    {
        //take all the hints that of currently possesses
        foreach(CompositeStory hint in of.hints){
            this.AddHint(hint);
        }

        //subscribe to take all hints that will be added to of in the future
        of.hintAddedToStory += AddHint;
    }

    private Story TryMatchInput(string forInput)
    {

        foreach (KeyValuePair<string, Story> entry in inputBasedTransitions)
        {
            
            if (Regex.IsMatch(forInput, entry.Key))
            {
                return entry.Value;
            }

        }

        return null;
    }

    private Story GetRandomFlavorText()
    {   if (flavorText.Count == 0) return this;
        return flavorText[Utils.random.Next(flavorText.Count)];
    }

}
public class LeafStory : Story {
    
    public LeafStory(string text) : base(text) { }

    public override Story GetTransition(string forInput)
    {
        return this;
    }

}

