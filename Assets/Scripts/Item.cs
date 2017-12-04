using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Item  {

    public string name;
    public string description;
    public Sprite image;

    public Item(string name, string description, Sprite image)
    {
        this.name = name;
        this.description = description;
        this.image = image;
    }


    public Item(string name, string description) : this(name, description, null) { }

}
