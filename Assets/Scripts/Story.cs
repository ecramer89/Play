using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

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
   

    public CompositeStory(string text) : base(text) {
        this.inputBasedTransitions = new Dictionary<string, Story>();
        this.flavorText = new List<Story>();
        this.hints = new List<CompositeStory>();
    }
     

    public void LoopBackToThisVia(string viaText)
    {
        CompositeStory intermediary = new CompositeStory(viaText);
        intermediary.AddFreeTransition(this);
        AddFreeTransition(intermediary);
    }


    /*
     * a hint is basically like a change in the text; it has the same subgraph as the node from which the hint is accessed
     * so users can access the same subgraph from the hint node as they could from the parent. the hint node also inherits the parent's hints,
     * minus itself. 
     * 
     * */
    public void AddHint(string hintText)
    {
        CompositeStory hint = new CompositeStory(hintText);
        hint.TakeTransitionsOf(this);
        hint.TakeFlavorTextOf(this);
        hints.Add(hint);
    }


    public void Graft(CompositeStory story, string inputPattern)
    {
        story.TakeTransitionsOf(this);
        story.TakeFlavorTextOf(this);
        story.TakeHintsOf(this);

        this.inputBasedTransitions.Add(inputPattern, story);

    }

    private void TakeTransitionsOf(CompositeStory of)
    {
        foreach (KeyValuePair<string, Story> transition in of.inputBasedTransitions)
        {
            this.inputBasedTransitions[transition.Key] = transition.Value;
        }
        
    }

    private void TakeFlavorTextOf(CompositeStory of)
    {
        foreach (Story free in of.flavorText)
        {
            this.flavorText.Add(free);
        }
    }

    private void TakeHintsOf(CompositeStory of)
    {
        foreach(CompositeStory hint in of.hints){
            this.hints.Add(hint);
        }
    }



    public void AddInputBasedTransition(string inputPattern, Story transitionTo)
    {
        inputBasedTransitions.Add(inputPattern, transitionTo);
    }

    public void AddFreeTransition(Story transitonTo)
    {
        flavorText.Add(transitonTo);
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

    private Story TryMatchInput(string forInput)
    {

        foreach (KeyValuePair<string, Story> entry in inputBasedTransitions)
        {
            Debug.Log(entry.Key + " " + forInput);
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

