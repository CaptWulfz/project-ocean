using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prototype_PlayerMovement : MonoBehaviour
{
    [Header("Test Skill Check")]
    [SerializeField] SkillCheck skillCheck;

    [Header("Player Physics")]
    [SerializeField] float playerSpeed;

    [Header("Player UI")]
    [SerializeField] Text playerPromptTextUI;
    [SerializeField] List<string> playerPrompts;
    [SerializeField] Slider playerOxygen;
    [SerializeField] Slider playerPanicMeter;

    Rigidbody2D playerRigidBody;
    Controls controls;
    void Start()
    {
        playerRigidBody = this.GetComponent<Rigidbody2D>(); //Get the rigidbody2d of the GameObject where this script is currently attached.
        controls = new Controls();
        controls.Player.Enable();

        playerOxygen.value = 1.0f;
        playerPanicMeter.value = 0.0f;

        SkillCheck.PlayerSkillCheckDifficulty difficulty = new SkillCheck.PlayerSkillCheckDifficulty();
        difficulty.modes = PlayerSkillCheckDifficultyModes.Easy;
        difficulty.skillCheckSpeed = 35f;
        difficulty.rotateSkillCheckRandom = true;


        skillCheck.TriggerSkillCheck(difficulty);
    }

    void Update()
    {
        playerRigidBody.velocity = controls.Player.Movement.ReadValue<Vector2>() * playerSpeed;

        if (controls.Player.Interact.WasPressedThisFrame())
        {
            UpdatePlayerPrompt("Player: " + playerPrompts[playerPrompts.Count - 1]);
        }

        UpdatePlayerOxygen(playerOxygen.value - 0.05f * Time.deltaTime);
    }

    public void UpdatePlayerPrompt(string text)
    {
        playerPromptTextUI.text = text;
    }

    public void UpdatePlayerOxygen(float value)
    {
        playerOxygen.value = value;
    }

    public void IncreasePlayerPanic(float value)
    {
        playerPanicMeter.value += value;
        this.transform.position = new Vector3(this.transform.position.x - 2.0f, this.transform.position.y, 0.0f);
    }
}
