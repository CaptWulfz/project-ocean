using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventDialogManager : Singleton<EventDialogManager>
{
    [SerializeField] EventDialogGroup eventDialogGroup; //Custom Asset for Dialog Group
    [SerializeField] GameObject eventDialogBox; //Prefab of Dialog Option

    [SerializeField] Text dialogText; //Text for dialog
    [SerializeField] GameObject dialogOptionArea; //Panel to act as parent of dialog options.

    void Awake()
    {
        GameObjectPool.Instance.Initialize(5, eventDialogBox);
    }

    void Start()
    {
        GenerateDialogSequence(eventDialogGroup.EventDialogs[0]);
    }

    public void GenerateDialogSequence(EventDialog eventDialog)
    {
        dialogText.text = eventDialog.EventDialogText;

        if (eventDialog.EventDialogPlayerResponses != null)
        {
            //foreach(GameObject objectToDequeue in )
            //This is if there's still a continuation of the dialog/there needs to be a player response
            foreach (EventDialog playerResponse in eventDialog.EventDialogPlayerResponses)
            {
                GameObject newObject = GameObjectPool.Instance.GetObject();
                newObject.transform.SetParent(dialogOptionArea.transform, false);

                newObject.GetComponentInChildren<Text>().text = playerResponse.EventDialogText;
                newObject.GetComponent<EventDialogBox>().SetDialogBoxProperties(this.gameObject, playerResponse.EventDialogPlayerResponses[0]);

                newObject.SetActive(true);
            }
            dialogOptionArea.SetActive(true);
        }
        else if (eventDialog.EventDialogEffect != DialogEffects.NONE)
        {
            dialogOptionArea.SetActive(false);
            PerformDialogEffect(eventDialog);
        }
        else
            dialogOptionArea.SetActive(false);
    }

    void PerformDialogEffect(EventDialog dialog)
    {
        switch (dialog.EventDialogEffect)
        {
            case DialogEffects.PANIC:
            {
                Parameters param = new Parameters();
                param.AddParameter<float>("currPanicValue", dialog.EventDialogPanicDamage);
                EventBroadcaster.Instance.PostEvent(EventNames.ON_PANIC_MODIFIED, param);
            }
            break;

            case DialogEffects.DOUBT:
            {
                Parameters param = new Parameters();
                param.AddParameter<DoubtEffects>("doubtEffect", dialog.DoubtEffect);
                EventBroadcaster.Instance.PostEvent(EventNames.EVENT_DOUBT_EFFECT, param);
            }
            break;

            default:
            break;
        }
    }
}

public class EventDialogBoxPool : GameObjectPool
{

}


//Object Pooling
public class GameObjectPool : Singleton<GameObjectPool>
{
    Queue<GameObject> objects;

    public void Initialize(int initialObjectCount, GameObject gameObject = null)
    {
        if (objects == null && initialObjectCount > 0 && gameObject != null)
        {
            objects = new Queue<GameObject>();
            for (int count = 0; count < initialObjectCount; count++)
            {
                GameObject newGameObject = Instantiate(gameObject);
                newGameObject.SetActive(false);
                objects.Enqueue(newGameObject);
            }
        }
    }
    public GameObject GetObject()
    {
        if (objects.Count > 0)
            return objects.Dequeue();
        else
            return null;
    }

    public void ReturnObject(GameObject objectToReturn)
    {
        objectToReturn.SetActive(false);
        objects.Enqueue(objectToReturn);
    }
}
