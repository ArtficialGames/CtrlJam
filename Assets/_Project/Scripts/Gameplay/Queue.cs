using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] GameObject line;

    public List<Survivor> survivors;
    public bool undetachable;
    public int maxSurvivors = 8;

    private void Awake()
    {
        GetComponent<Leader>().hud.UpdateSurvivorsCount(survivors.Count - 1, maxSurvivors);
        lineRenderer = Instantiate(line, Vector3.zero, Quaternion.identity).GetComponent<LineRenderer>();
    }

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
        GetComponent<Leader>().hud.UpdateSurvivorsCount(survivors.Count - 1, maxSurvivors);
    }

    public void Remove(Survivor survivor)
    {
        survivors.Remove(survivor);
        GetComponent<Leader>().hud.UpdateSurvivorsCount(survivors.Count - 1, maxSurvivors);
    }

    private void Update()
    {
        UpdateLine();
    }

    void UpdateLine()
    {
        lineRenderer.positionCount = survivors.Count;

        for (int i = 0; i < survivors.Count; i++)
        {
            lineRenderer.SetPosition(i, survivors[i].transform.position + Vector3.up * 0.5f);
        }
    }
}
