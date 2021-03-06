using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    [SerializeField]
    private string title;

    [SerializeField]
    private string description;

    [SerializeField]
    private CollectObjective[] collectObjectives;
    [SerializeField]
    private KillObjective[] killObjectives;
    [SerializeField]
    private KillObjective[] reachObjectives;

    public QuestScript MyQuestScript {
        get;
        set;
    }
    public QuestGiver MyQuestGiver { get; set; }


    public string MyTitle
    {
        get
        {
            return title;
        }
        set
        {
            title = value;
        }
    }

    public string MyDescription
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
        }
    }

    public CollectObjective[] MyCollectObjectives
    {
        get
        {
            return collectObjectives;
        }
    }


    public bool IsComplete
    {
        get
        {

            foreach (Objective o in collectObjectives)
            {
                if (!o.IsComplete)
                {
                    return false;
                }
            }

            foreach (Objective o in reachObjectives)
            {
                if (!o.IsComplete)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public KillObjective[] MyKillObjectives
    {
        get
        {
            return killObjectives;
        }
    }
}

[System.Serializable]
public abstract class Objective
{
    [SerializeField]
    private int amount;

    private int currentAmount;

    [SerializeField]
    private string type;

    public int MyAmount
    {
        get
        {
            return amount;
        }
    }

    public int MyCurrentAmount
    {
        get
        {
            return currentAmount;
        }

        set
        {
            currentAmount = value;
        }
    }

    public string MyType
    {
        get
        {
            return type;
        }
    }

    public bool IsComplete
    {
        get
        {
            return MyCurrentAmount >= MyAmount;
        }
    }
}

[System.Serializable]
public class CollectObjective : Objective
{
}


[System.Serializable]
public class KillObjective : Objective
{

}

[System.Serializable]
public class ReachObjective : Objective
{

}