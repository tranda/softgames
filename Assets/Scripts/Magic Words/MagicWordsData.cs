using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MagicWordsData
{
    public List<dialogueSingle> dialogue;
    public List<emojie> emojies;
    public List<avatar> avatars;
}

[Serializable]
public class dialogueSingle
{
    public string name;
    public string text;
}

[Serializable]
public class emojie
{
    public string name;
    public string url;
    
    public Sprite sprite;
}

[Serializable]
public class avatar
{
    public string name;
    public string url;
    public string position;

    public Sprite sprite;
}