using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData {
    //manage which plants on the plantbox
    public string myAccount;
    public List<string> myFriend = new List<string>();
    public List<int> id = new List<int>();
    public List<int> plant = new List<int>();
    //manage how many flower player have
    public int[] warehouse = new int[7];
    public int[] token = new int[3];
    public List<string> timeData = new List<string>();

    public void PlantBox(int id, int plant)
    {
        this.id.Add(id);
        this.plant.Add(plant);
    }
}
