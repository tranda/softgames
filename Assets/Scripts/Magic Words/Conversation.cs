using UnityEngine;

public class Conversation : MonoBehaviour
{
    [SerializeField] private MessageFull messageFullLeft;
    [SerializeField] private MessageFull messageFullRight;


    private MagicWordsData magicWordsData;
    private int currentConversationIndex = 0;



    void Awake()
    {
        messageFullLeft.gameObject.SetActive(false);
        messageFullRight.gameObject.SetActive(false);
    }

    public void Init(MagicWordsData magicWordsData)
    {
        this.magicWordsData = magicWordsData;
    }

    public void DisplayConversation()
    {
        var dialogue = magicWordsData.dialogue[currentConversationIndex];
        displayMessage(dialogue);
    }

    public void NextConversation()
    {
        if (currentConversationIndex >= magicWordsData.dialogue.Count - 1)
        {
            Debug.Log("End of conversation");
            return;
        }
        currentConversationIndex++;
        DisplayConversation();
    }

    public void Reset()
    {
        currentConversationIndex = 0;
        DisplayConversation();  
    }

    private void displayMessage(dialogueSingle dialogue)
    {
        string name = dialogue.name;
        string text = dialogue.text;

        avatar avatar = magicWordsData.avatars.Find(a => a.name == name);
        if (avatar != null)
        {
            if (avatar.position == "left")
            {
                messageFullLeft.gameObject.SetActive(true);
                messageFullRight.gameObject.SetActive(false);
                messageFullLeft.SetName(name);
                messageFullLeft.SetText(text);
                messageFullLeft.SetAvatar(avatar.sprite);
            }
            else if (avatar.position == "right")
            {
                messageFullRight.gameObject.SetActive(true);
                messageFullLeft.gameObject.SetActive(false);
                messageFullRight.SetName(name);
                messageFullRight.SetText(text);
                messageFullRight.SetAvatar(avatar.sprite);
            }
        }

    }


}