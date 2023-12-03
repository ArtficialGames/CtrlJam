using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour
{
    public List<Survivor> survivors;

    public Survivor GetNextInLine(Survivor survivor)
    {
        Survivor next;

        next = survivors[survivors.IndexOf(survivor) - 1];

        return next;
    }

    public List<Survivor> GetSurvivorsBehind(Survivor survivor)
    {
        List<Survivor> survivorsBehind = new List<Survivor>();

        foreach (var s in survivors)
        {
            if(survivors.IndexOf(s) > survivors.IndexOf(survivor))
                survivorsBehind.Add(s);
        }

        return survivorsBehind;
    }

    public void Add(Survivor survivor)
    {
        survivors.Add(survivor);
    }

    public void Remove(Survivor survivor)
    {
        survivors.Remove(survivor);
        //survivors.RemoveRange(survivors.IndexOf(survivor), survivors.Count - 1);
    }
}
