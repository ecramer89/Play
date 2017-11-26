using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class StoryRepository {

    private static Dictionary<CharacterName, Story> storyRepository;

    public static void Init()
    {
        storyRepository = new Dictionary<CharacterName, Story>();

        GameObject.FindObjectsOfType<CharacterData>()
            .ToList()
            .ForEach(characterData =>
            {
                storyRepository[characterData.Name()] = characterData.StoryRoot();
            });

    }
    public static Story GetStory(CharacterName characterName)
    {  if (storyRepository == null) Init();
        Story story;
        if (!storyRepository.TryGetValue(characterName, out story)) throw new System.Exception(string.Format("No story found for {0}", characterName));
        return story;
    }

}
