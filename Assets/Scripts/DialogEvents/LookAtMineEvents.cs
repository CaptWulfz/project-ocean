using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMineEvents : MonoBehaviour
{
    bool isFirstMine = true;
    bool isFirstFakeMine = true;
    int monsterEncounterCount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagNames.MINE && isFirstMine)
        {
            isFirstMine = false;
            GameDirector.Instance.PerformDialogSequence(TopicList.ABYSS_FIRST_MINE);
        }
        else if (collision.gameObject.tag == TagNames.FAKE_MINE && isFirstFakeMine)
        {
            isFirstFakeMine = false;
            GameDirector.Instance.PerformDialogSequence(TopicList.ABYSS_PASS_THROUGH_FIRST_MIRAGE_MINE);
        }
        else if (collision.gameObject.tag == TagNames.HOSTILE && monsterEncounterCount == 0)
        {
            isFirstMine = false;
            GameDirector.Instance.PerformDialogSequence(TopicList.ABYSS_FIRST_MONSTER_ENCOUNTER);
        }
        else if (collision.gameObject.tag == TagNames.HOSTILE && monsterEncounterCount == 5)
        {
            isFirstMine = false;
            GameDirector.Instance.PerformDialogSequence(TopicList.ABYSS_ENCOUNTER_FEW_MORE_ENTITIES);
        }
        if (collision.gameObject.tag == TagNames.HOSTILE)
            monsterEncounterCount++;
    }
}
