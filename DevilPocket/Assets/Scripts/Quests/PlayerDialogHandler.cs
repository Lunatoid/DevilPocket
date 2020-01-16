using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerDialogHandler : MonoBehaviour {

    List<string> queuedDialog = new List<string>();

    class ChoiceInfo {
        public bool makingChoice = false;
        public List<string> choices = new List<string>();
        public int selectedIndex = 0;
    }
    bool alreadyMovedAxis = false; // Used to disable the key repeat on the axis
    bool firstFrameInteractDelay = false; // If you talk to an NPC on the same frame as having a choice it will skip the choice
    bool finishedAllText = false;

    ChoiceInfo choiceInfo = new ChoiceInfo();

    public TextMeshProUGUI textbox;
    public Image textboxBg;
    public TextMeshProUGUI choiceBox;
    public Image choiceBg;

    // The last gameObject that added dialog.
    // Used for #function's SendMessage
    GameObject npc;

    Color textboxColor;
    Color choiceColor;

    // How many seconds it takes for the next letter to appear
    const float TYPEWRITER_LETTER_TIME = 0.05f;
    float typewriterTimer = 0.0f;

    // Deadzone for moving up and down choices
    const float AXIS_CHOICE_DEADZONE = 0.0f;

    FirstPersonController fps;

    // Start is called before the first frame update
    public void Init() {
        textboxColor = textboxBg.color;
        choiceColor = choiceBg.color;

        textboxBg.color = Color.clear;
        choiceBg.color = Color.clear;

        fps = GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update() {
        if (queuedDialog.Count > 0) {
            fps.LockMovement();
        } else if (fps.IsMovementLocked()) {
            fps.UnlockMovement();
        }

        if (choiceInfo.makingChoice) {
            HandleChoice();
        } else {
            HandleText();
        }

        if (firstFrameInteractDelay) {
            firstFrameInteractDelay = false;
        }
    }

    void ParseTextCommands() {
        if (queuedDialog.Count == 0 || queuedDialog[0].Length == 0) return;

        if (queuedDialog[0][0] == '#') {
            // Split the first line into the command and arguments

            // The length of the command string is either till the first \n or the end of the string if there is no newline
            int cmdLineLength = (queuedDialog[0].IndexOf('\n') == -1) ? queuedDialog[0].Length : queuedDialog[0].IndexOf('\n');
            string cmdLine = queuedDialog[0].Substring(0, cmdLineLength);
            List<string> args = new List<string>(cmdLine.Split(' '));
            string cmd = args[0].Substring(1); // The first argument is the command, remove the starting '#'
            args.RemoveAt(0);

            // Remove the line from the dialog
            queuedDialog[0] = queuedDialog[0].Remove(0, cmdLineLength);
            if (queuedDialog[0].Length > 0 && queuedDialog[0][0] == '\n') {
                queuedDialog[0] = queuedDialog[0].Substring(1);
            }

            // Debug.Log($"Parsing text command {cmd}");

            switch (cmd) {
                case "choice":
                    choiceInfo.makingChoice = true;
                    choiceInfo.choices = args;
                    choiceInfo.selectedIndex = 0;

                    choiceBg.color = choiceColor;

                    textbox.text = queuedDialog[0];
                    break;

                case "ifchoice":
                    if (args.Count == 1) {
                        // Remove it if the choice is not the current choice
                        if (args[0] != choiceInfo.choices[choiceInfo.selectedIndex]) {
                            queuedDialog.RemoveAt(0);
                            ParseTextCommands();
                        }
                    }
                    break;

                case "function":
                    if (args.Count >= 1) {
                        string funcName = args[0];
                        args.RemoveAt(0);
                        npc.SendMessage(funcName, args);
                    }
                    break;

                default:
                    Debug.LogError($"Unknown text command '{cmd}'");
                    break;
            }
        }
    }

    public void PushDialog(GameObject npc, string[] dialog) {
        // Only push the dialog if there is no queued dialog and we're not on the same frame as the final message
        if (queuedDialog.Count == 0 && !finishedAllText) {
            this.npc = npc;

            queuedDialog.AddRange(dialog);
            ParseTextCommands();

            textboxBg.color = textboxColor;
            firstFrameInteractDelay = true;
        }

        finishedAllText = false;
    }

    void HandleChoice() {
        choiceBox.text = "";
        for (int i = 0; i < choiceInfo.choices.Count; ++i) {
            if (i == choiceInfo.selectedIndex) {
                choiceBox.text += $"<align=\"center\"><color=\"yellow\">{choiceInfo.choices[i]}</color></align>\n";
            } else {
                choiceBox.text += $"<align=\"center\">{choiceInfo.choices[i]}</align>\n";
            }
        }

        float axis = CrossPlatformInputManager.GetAxis("Vertical");
        // Debug.Log($"Axis {axis}");
        if (!alreadyMovedAxis && axis > AXIS_CHOICE_DEADZONE) {
            alreadyMovedAxis = true;
            if (choiceInfo.selectedIndex == 0) {
                choiceInfo.selectedIndex = choiceInfo.choices.Count - 1;
            } else {
                --choiceInfo.selectedIndex;
            }
        } else if (!alreadyMovedAxis && axis < AXIS_CHOICE_DEADZONE) {
            alreadyMovedAxis = true;
            if (choiceInfo.selectedIndex == choiceInfo.choices.Count - 1) {
                choiceInfo.selectedIndex = 0;
            } else {
                ++choiceInfo.selectedIndex;
            }
        } else if (axis == 0.0f) {
            alreadyMovedAxis = false;
        }

        if (CrossPlatformInputManager.GetButtonDown("Interact") && !firstFrameInteractDelay) {
            choiceInfo.makingChoice = false;

            choiceBox.text = "";
            choiceBg.color = Color.clear;

            NextText();
        }
    }

    void HandleText() {
        if (queuedDialog.Count > 0) {
            // Keep popping the first character and adding it to the textbox
            if (queuedDialog[0].Length > 0) {
                if (typewriterTimer > TYPEWRITER_LETTER_TIME) {
                    typewriterTimer = 0.0f;

                    textbox.text += queuedDialog[0][0];
                    queuedDialog[0] = queuedDialog[0].Substring(1);
                }

                typewriterTimer += Time.deltaTime;

                if (CrossPlatformInputManager.GetButtonDown("Interact") && !firstFrameInteractDelay) {
                    typewriterTimer = 0.0f;
                    textbox.text += queuedDialog[0];
                    queuedDialog[0] = "";
                }

            } else {
                if (CrossPlatformInputManager.GetButtonDown("Interact") && !firstFrameInteractDelay) {
                    NextText();
                }
            }
        } else if (textboxBg.color.a != 0.0f) {
            textboxBg.color = Color.clear;
        }
    }

    void NextText() {
        typewriterTimer = 0.0f;
        textbox.text = "";
        queuedDialog.RemoveAt(0);
        ParseTextCommands();

        if (queuedDialog.Count == 0) {
            finishedAllText = true;
        }
    }
}
