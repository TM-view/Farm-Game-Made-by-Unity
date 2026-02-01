using System.Collections.Generic;
using UnityEngine;

public class StoreSetting : MonoBehaviour
{
    void Start()
    {
        if (transform.name == "PlantStore")
        {
            transform.gameObject.SetActive(true);
        }
        else
        {
            transform.gameObject.SetActive(false);
        }
        SortStore(transform.name);
    }

    public void SortStore(string StoreName)
    {
        List<Transform> children = new List<Transform>();

        foreach (Transform child in transform)
            children.Add(child);

        switch (StoreName)
        {
            case "PlantStore" : 
                                children.Sort((a, b) => a.GetComponent<PlantItem>().realPrice
                                .CompareTo(b.GetComponent<PlantItem>().realPrice));
                                break;
            case "ToolStore" : 
                                children.Sort((a, b) => a.GetComponent<ToolItem>().realPrice
                                .CompareTo(b.GetComponent<ToolItem>().realPrice));
                                break;
            case "UpgradeStore" : 
                                children.Sort((a, b) => a.GetComponent<UpgradeItem>().realPrice
                                .CompareTo(b.GetComponent<UpgradeItem>().realPrice));   
                                break;
            case "RelicStore" :     
                                children.Sort((a, b) =>
                                {
                                    if (a.name == "Reset") return -1;
                                    if (b.name == "Unlock_Relic") return 1;

                                    return a.GetComponent<RelicItem>().realCostFloat
                                        .CompareTo(b.GetComponent<RelicItem>().realCostFloat);
                                });
                                break;
        }

        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetSiblingIndex(i);
        }
    }
}
