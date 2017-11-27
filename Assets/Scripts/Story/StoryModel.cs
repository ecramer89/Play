
using UnityEngine;
[RequireComponent(typeof(StoryView))]
public abstract class StoryModel : MonoBehaviour {

    protected StoryTitle title;
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

    public StoryTitle GetTitle()
    {  if (title == null) InitializeStoryTitle();
        return title;
    }

     public StoryNode GetBeginning()
    {   if (storyRoot == null) InitializeStoryRoot();
        return storyRoot;
    }

    protected abstract void InitializeStoryRoot();

    protected abstract void InitializeStoryTitle();
}
