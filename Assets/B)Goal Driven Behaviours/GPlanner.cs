using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NNode
{
    public NNode parent;
    public float cost;
    public Dictionary<string, int> state;
    public GAction action;

    public NNode(NNode parent, float cost, Dictionary<string, int> allstates, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allstates);
        this.action = action;
    }
}

public class GPlanner 
{
    public Queue<GAction> plan (List<GAction> actions, Dictionary<string, int> goal, WorldStates states)
    {
        List<GAction> usableActions = new List<GAction>();
        foreach (GAction a in actions)
        {
            if (a.IsAchievable())
                usableActions.Add(a);
        }

        List<NNode> leaves = new List<NNode>();
        NNode start = new NNode(null, 0, GWorld.Instance.GetWorld().GetStates(), null);

        bool success = BuildGraph(start, leaves, usableActions, goal);

        if (!success)
        {
            Debug.Log("NO PLAN");
            return null;
        }

        NNode cheapest = null;
        foreach(NNode leaf in leaves)
        {
            if (cheapest == null)
                cheapest = leaf;
            else
            {
                if (leaf.cost < cheapest.cost)
                    cheapest = leaf;
            }
        }

        List<GAction> result = new List<GAction>();
        NNode n = cheapest;
        while ( n!= null)
        {
            if (n.action != null)
            {
                result.Insert(0, n.action);
            }
        }

        Queue<GAction> queue = new Queue<GAction>();
        foreach(GAction a in result)
        {
            queue.Enqueue(a);
        }

        Debug.Log("The Plan is: ");
        foreach(GAction a in queue)
        {
            Debug.Log("Q: " + a.actionName);
        }

        return queue;
    }
    
}
