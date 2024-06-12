using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music
{
    public string name {  get; set; }
    public Album album { get; set; }

    public Music(string name,Album album)
    {
        this.name = name;
        this.album = album;
    }
}

public class Album
{
    public string name { get; set; }
    public List<Music> included { get; set; }

    public Album(string name)
    {
        this.name = name;
        included = new List<Music>();
    }
}