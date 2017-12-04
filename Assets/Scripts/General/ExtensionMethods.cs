using UnityEngine;
using System.Collections.Generic;

namespace ExtensionMethods
{
    public static class Extensions
    {
        public static GameObject FindChildWithName(this GameObject root, string name)
        {
            Queue<GameObject> toVisit = new Queue<GameObject>();
            toVisit.Enqueue(root);
            while(toVisit.Count > 0)
            {
                GameObject nxt = toVisit.Dequeue();
                if (nxt.name == name) return nxt;
                for (int i = 0; i < nxt.transform.childCount; i++)
                {
                    toVisit.Enqueue(nxt.transform.GetChild(i).gameObject);
                }
            }

            return null;
        }

        public static string MatchSomewhere(this string subexpression)
        {
            return string.Format(".*{0}.*", subexpression);
        }

        
    }
}