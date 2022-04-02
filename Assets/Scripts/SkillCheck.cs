using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCheck : MonoBehaviour
{
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

    /// <summary>
    /// Position offset for the skill check to the target entity's transform to display to.
    /// </summary>
    struct SpriteOffset
    {
        public float x;
        public float y;
    }

    /// <summary>
    /// Position/Value of the left and right value of the correct field range for the skill check.
    /// </summary>
    struct CorrectFieldRangeValues
    {
        public float leftValue;
        public float rightValue;
    }

    bool isSkillCheckHappening = false;

    Transform target;
    Controls controls;
    EventBroadcaster eventBroadcaster;

    SpriteOffset spriteOffset;
    CorrectFieldRangeValues correctSkillCheckField;
    PlayerSkillCheckDifficulty currentSkillCheckDifficulty;

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
        this.target = null;
    }

    void Update()
    {
        FollowTarget(); //Follows player and presents skill check with offset

        #region Skill Check Presentation
        if (isSkillCheckHappening && skillCheckOfPlayer_RightSide.value < correctSkillCheckField.rightValue + playerSkillCheckPin)
            skillCheckOfPlayer_RightSide.value += 0.01f * currentSkillCheckDifficulty.skillCheckSpeed * Time.deltaTime;
        else
        {
            //playerPrompt.text = (Random.Range(5.0f, 5000.0f).ToString() + ": Failed Skill Check"); //temporary
            Parameters param = new Parameters();
            param.AddParameter<bool>(ParameterNames.SKILLCHECK_RESULT, false);
            eventBroadcaster.PostEvent(EventNames.ON_SKILL_CHECK_FINISHED, param);
            this.gameObject.SetActive(false);
        }
        #endregion

        #region Player Input for Skill Check
        if (controls.Player.Interact.WasPressedThisFrame() && (skillCheckOfPlayer_RightSide.value < correctSkillCheckField.rightValue && skillCheckOfPlayer_RightSide.value > correctSkillCheckField.leftValue))
        {
            //playerPrompt.text = (Random.Range(5.0f, 5000.0f).ToString() + ": Successful Skill Check"); //temporary
            Parameters param = new Parameters();
            param.AddParameter<bool>(ParameterNames.SKILLCHECK_RESULT, true);
            eventBroadcaster.PostEvent(EventNames.ON_SKILL_CHECK_FINISHED, param);
            this.gameObject.SetActive(false);
        } 
        else if (controls.Player.Interact.WasPressedThisFrame())
        {
            Parameters param = new Parameters();
            param.AddParameter<bool>(ParameterNames.SKILLCHECK_RESULT, false);
            eventBroadcaster.PostEvent(EventNames.ON_SKILL_CHECK_FINISHED, param);
            this.gameObject.SetActive(false);
        }
        #endregion
    }

    #region Follow Player
    private void AssignTarget(Transform targetObj)
    {
        //GameObject targetObj = GameObject.FindGameObjectWithTag(tagName);
        if (targetObj != null)
        {
            this.target = targetObj;
            this.spriteOffset.x = this.target.GetComponent<SpriteRenderer>().bounds.size.x;
            this.spriteOffset.y = this.target.GetComponent<SpriteRenderer>().bounds.size.y;
        }
    }

    private void FollowTarget()
    {
        if (this.target != null)
        {
            float xOffset = this.spriteOffset.x / 2;
            this.transform.position = new Vector2(this.target.transform.position.x + xOffset + 0.3f, this.target.transform.position.y);
        }
    }
    #endregion

    #region Skill Check
    /// <summary>
    /// Call this function to Enable the SkillCheck object and it will perform one skill check. Please supply a difficulty object that will define the complexity of the skill check.
    /// </summary>
    /// <param name="difficulty"></param>
    public void TriggerSkillCheck(PlayerSkillCheckDifficulty difficulty, Transform target)
    {
        AssignTarget(target);
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
    #endregion
}

public enum PlayerSkillCheckDifficultyModes
{
    Easy,
    Medium,
    Hard
}