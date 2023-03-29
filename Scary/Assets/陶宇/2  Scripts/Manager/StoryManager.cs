using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    [SerializeField] Text story_text;
    [SerializeField] Image story_image;

    public bool b_CanShowStory;
    public bool b_ShowingStory;
    public bool[] b_StoryFragment;

    void Start()
    {
        Init();
        ClearStory();
    }

    void Update()
    {
        //if (b_ShowingStory && Input.GetKeyDown(KeyCode.E))
    }

    void Init()
    {
        story_text = GameObject.Find("Canvas/StoryText").GetComponent<Text>();
        story_image = GameObject.Find("Canvas/StoryImage").GetComponent<Image>();
    }

    public void StoryShow(StoryID _storyID)
    {
        if (b_CanShowStory)
        {
            story_image.enabled = true;

            switch (_storyID)
            {
                case StoryID.S1_0_0:
                    if (b_StoryFragment[0] == false)
                        story_text.text = GlobalDeclare.StoryMessage[0];
                    else if (b_StoryFragment[1] == false)
                        story_text.text = GlobalDeclare.StoryMessage[1];
                    break;
            }
        }
    }

    public void ClearStory()
    {
        story_text.text = "";
        story_image.enabled = false;
    }
}
