using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class StoryController : MonoBehaviour
{
    public StoryID storyID;

    StoryManager storyManager;
    BoxCollider boxCollider;

    void Awake()
    {
        storyManager = GameObject.Find("StoryManager").GetComponent<StoryManager>();
        boxCollider = GetComponent<BoxCollider>();
    }

    void Start()
    {
        boxCollider.isTrigger = true;
    }

    public void SendStoryMsg()
    {
        storyManager.StoryShow(storyID);
    }
}
