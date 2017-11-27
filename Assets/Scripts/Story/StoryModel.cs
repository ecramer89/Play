
using UnityEngine;
[RequireComponent(typeof(StoryView))]
public abstract class StoryModel : MonoBehaviour {

    protected string title;
    protected StoryNode storyRoot;
    private StoryView view;

    protected StoryView View
    {
        get
        {
            if (view == null)
            {
                view = GetComponent<StoryView>();
            }
            return view;
        }
    }

    public string GetTitle()
    {  if (title == null) InitializeStoryTitle();
        return title;
    }

     public StoryNode StoryRoot()
    {   if (storyRoot == null) InitializeStoryRoot();
        return storyRoot;
    }

    protected abstract void InitializeStoryRoot();

    protected abstract void InitializeStoryTitle();
}
