using UnityEngine;

namespace ExtensionMethods
{
    public static class Extensions
    {
        public static GameObject FindChildWithName(this GameObject go, string name)
        {
            for(int i=0;i<go.transform.childCount; i++)
            {
                GameObject next = go.transform.GetChild(i).gameObject;
                if (next.name == name) return next;
            }

            return null;
        }

        public static string MatchSomewhere(this string subexpression)
        {
            return string.Format(".*{0}.*", subexpression);
        }

        
    }
}