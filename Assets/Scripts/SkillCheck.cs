using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCheck : MonoBehaviour
{
    [Header("Temporary")]
    [SerializeField] Text playerPrompt;

    [Header("Skill Check Properties")]
    [SerializeField] float skillCheckCorrectFieldPercentage = 0.10f; //Out of 100%, 0.10f = 10%
    [SerializeField] float skillCheckMinimumLeftHandValue = 0.30f; //Skill check can start from 0.3f until 1f
    [SerializeField] float playerSkillCheckPin = 0.05f;

    [Header("Skill Check Components")]
    [SerializeField] Slider skillCheckToHit_LeftSide;
    [SerializeField] Slider skillCheckToHit_RightSide;
    [SerializeField] Slider skillCheckOfPlayer_RightSide;
    [SerializeField] Slider skillCheckOfPlayer_LeftCorrectSide;
    [SerializeField] Slider skillCheckOfPlayer_LeftWrongSide;

    /// <summary>
    /// This struct holds the definition of the skill check properties.
    /// PlayerSkillCheckDifficultyModes can either be Easy, Medium, or Hard
    /// SkillCheckSpeed defines how fast the player pin traverse the skill check
    /// </summary>
    public struct PlayerSkillCheckDifficulty
    {
        public PlayerSkillCheckDifficultyModes modes;
        public float skillCheckSpeed;
        public bool rotateSkillCheckRandom;
    }

    struct CorrectFieldRangeValues
    {
        public float leftValue;
        public float rightValue;
    }

    Controls controls;
    EventBroadcaster eventBroadcaster;
    
    CorrectFieldRangeValues correctSkillCheckField;
    PlayerSkillCheckDifficulty currentSkillCheckDifficulty;

    bool isSkillCheckHappening = false;

    void OnEnable()
    {
        correctSkillCheckField = GenerateCorrectFieldRangeValues();
        controls = InputManager.Instance.GetControls();
        eventBroadcaster = EventBroadcaster.Instance;
        controls.Player.Enable();
    }

    void OnDisable()
    {
        isSkillCheckHappening = false;
        skillCheckToHit_LeftSide.value = 0f;
        skillCheckToHit_RightSide.value = 0f;
        skillCheckOfPlayer_RightSide.value = 0f;
        skillCheckOfPlayer_LeftCorrectSide.value = 0f;
        skillCheckOfPlayer_LeftWrongSide.value = 0f;
    }

    void Update()
    {
        if (isSkillCheckHappening && skillCheckOfPlayer_RightSide.value < correctSkillCheckField.rightValue + playerSkillCheckPin)
            skillCheckOfPlayer_RightSide.value += 0.01f * currentSkillCheckDifficulty.skillCheckSpeed * Time.deltaTime;
        else
        {
            playerPrompt.text = (Random.Range(5.0f, 5000.0f).ToString() + ": Failed Skill Check"); //temporary
            Parameters param = new Parameters();
            param.AddParameter<bool>(EventNames.EVENT_SKILLCHECK_RESULT, false);
            eventBroadcaster.PostEvent(EventNames.EVENT_SKILLCHECK_FAIL, param);
            this.gameObject.SetActive(false);
        }

        if (controls.Player.Interact.WasPressedThisFrame() && (skillCheckOfPlayer_RightSide.value < correctSkillCheckField.rightValue && skillCheckOfPlayer_RightSide.value > correctSkillCheckField.leftValue))
        {
            playerPrompt.text = (Random.Range(5.0f, 5000.0f).ToString() + ": Successful Skill Check"); //temporary
            Parameters param = new Parameters();
            param.AddParameter<bool>(EventNames.EVENT_SKILLCHECK_RESULT, true);
            eventBroadcaster.PostEvent(EventNames.EVENT_SKILLCHECK_FAIL, param);
            this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Call this function to Enable the SkillCheck object and it will perform one skill check. Please supply a difficulty object that will define the complexity of the skill check.
    /// </summary>
    /// <param name="difficulty"></param>
    public void TriggerSkillCheck(PlayerSkillCheckDifficulty difficulty)
    {
        this.gameObject.SetActive(true);

        if (difficulty.rotateSkillCheckRandom)
            RotateToRandom();

        currentSkillCheckDifficulty = difficulty;

        CreateCorrectSkillCheckField();

        isSkillCheckHappening = true;
    }

    public void FollowPlayerPinSkillCheck()
    {
        if (skillCheckOfPlayer_LeftWrongSide.value < correctSkillCheckField.leftValue)
            skillCheckOfPlayer_LeftWrongSide.value = skillCheckOfPlayer_RightSide.value - playerSkillCheckPin;
        skillCheckOfPlayer_LeftCorrectSide.value = skillCheckOfPlayer_RightSide.value - playerSkillCheckPin;
    }

    void RotateToRandom()
    {
        float randomRotationValue = Random.Range(1.0f, 360.0f);
        this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, randomRotationValue);

        float randomClockRotationValue = Random.Range(1.0f, 10.0f);

        Component[] images = this.GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            if (randomClockRotationValue < 5.0f)
                image.fillClockwise = true;
            else
                image.fillClockwise = false;
        }
    }

    void CreateCorrectSkillCheckField()
    {
        skillCheckToHit_LeftSide.value = correctSkillCheckField.leftValue;
        skillCheckToHit_RightSide.value = correctSkillCheckField.rightValue;
    }

    CorrectFieldRangeValues GenerateCorrectFieldRangeValues()
    {
        CorrectFieldRangeValues correctFieldRangeValues = new CorrectFieldRangeValues();
        correctFieldRangeValues.leftValue = Random.Range(skillCheckMinimumLeftHandValue, 1f - skillCheckCorrectFieldPercentage - playerSkillCheckPin);
        correctFieldRangeValues.rightValue = correctFieldRangeValues.leftValue + skillCheckCorrectFieldPercentage;
        return correctFieldRangeValues;
    }
}

public enum PlayerSkillCheckDifficultyModes
{
    Easy,
    Medium,
    Hard
}