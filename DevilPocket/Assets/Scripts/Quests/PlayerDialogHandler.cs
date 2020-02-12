using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    ChoiceInfo choiceInfo = new ChoiceInfo();

    public TextMeshProUGUI textbox;
    public GameObject textboxBg;
    public TextMeshProUGUI choiceBox;
    public GameObject choiceBg;

    // The last gameObject that added dialog.
    // Used for #function's SendMessage
    GameObject npc;

    // How many seconds it takes for the next letter to appear
    const float TYPEWRITER_LETTER_TIME = 0.05f;
    float typewriterTimer = 0.0f;

    // Deadzone for moving up and down choices
    const float AXIS_CHOICE_DEADZONE = 0.0f;

    // Whether to bypass needing to press the Interact button
    bool noConfirm = false;

    FirstPersonController fps;
    public AudioSource sfxSource;
    public AudioClip selectClip;
    public AudioClip confirmClip;

    public void Start() {
        SceneManager.activeSceneChanged += DisableOnSceneChange;
    }

    private void DisableOnSceneChange(Scene arg0, Scene arg1) {
        if (arg1.name == "MenuScene") return;
        enabled = false;
    }

    // Start is called before the first frame update
    public void Init() {
        textboxBg.SetActive(false);
        choiceBg.SetActive(false);
        fps = GetComponent<FirstPersonController>();
    }

    public void ClearDialog() {
        queuedDialog.Clear();
        textboxBg.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (queuedDialog.Count > 0) {
            fps.LockMovement();
            Time.timeScale = 0.0f;
        } else if (fps.IsMovementLocked()) {
            fps.UnlockMovement();
            Time.timeScale = 1.0f;
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
                // #choice OptionA OptionB ...
                case "choice":
                    choiceInfo.makingChoice = true;
                    choiceInfo.choices = args;
                    choiceInfo.selectedIndex = 0;

                    choiceBg.SetActive(true);

                    textbox.text = queuedDialog[0];
                    break;

                // #ifchoice Yes
                case "ifchoice":
                    if (args.Count == 1) {
                        // Remove it if the choice is not the current choice
                        if (args[0] != choiceInfo.choices[choiceInfo.selectedIndex]) {
                            queuedDialog.RemoveAt(0);
                            ParseTextCommands();
                        }
                    }
                    break;

                // #function FunctionName arg1 arg2 ...
                case "function":
                    if (args.Count >= 1) {
                        string funcName = args[0];
                        args.RemoveAt(0);
                        npc.SendMessage(funcName, args);
                    }
                    break;

                // #noconfirm
                case "noconfirm":
                    noConfirm = true;
                    break;

                // #end
                case "end":
                    // Remove everything except the current message
                    queuedDialog.RemoveRange(1, queuedDialog.Count - 1);
                    break;

                default:
                    Debug.LogError($"Unknown text command '{cmd}'");
                    break;
            }
        }

        // If we have multiple text commands, keep parsing them
        if (queuedDialog.Count > 0 && queuedDialog[0].Length > 0 && queuedDialog[0][0] == '#') {
            ParseTextCommands();
        }
    }

    public void PushDialog(GameObject npc, string[] dialog) {
        // Only push the dialog if there is no queued dialog and we're not on the same frame as the final message
        if (queuedDialog.Count == 0) {
            this.npc = npc;

            queuedDialog.AddRange(dialog);
            ParseTextCommands();

            textboxBg.SetActive(true);
            firstFrameInteractDelay = true;
        }
    }

    void HandleChoice() {
        choiceBox.text = "";
        for (int i = 0; i < choiceInfo.choices.Count; ++i) {
            if (i == choiceInfo.selectedIndex) {
                choiceBox.text += $"<align=\"center\"><u>{choiceInfo.choices[i]}</u></align>\n";
            } else {
                choiceBox.text += $"<align=\"center\">{choiceInfo.choices[i]}</align>\n";
            }
        }

        float axis = CrossPlatformInputManager.GetAxisRaw("Vertical");
        // Debug.Log($"Axis {axis}");
        if (!alreadyMovedAxis && axis > AXIS_CHOICE_DEADZONE) {
            alreadyMovedAxis = true;
            sfxSource.clip = selectClip;
            sfxSource.Play();
            if (choiceInfo.selectedIndex == 0) {
                choiceInfo.selectedIndex = choiceInfo.choices.Count - 1;
            } else {
                --choiceInfo.selectedIndex;
            }
        } else if (!alreadyMovedAxis && axis < AXIS_CHOICE_DEADZONE) {
            alreadyMovedAxis = true;
            sfxSource.clip = selectClip;
            sfxSource.Play();
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

            sfxSource.clip = confirmClip;
            sfxSource.Play();

            choiceBox.text = "";
            choiceBg.SetActive(false);

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

                typewriterTimer += Time.unscaledDeltaTime;

                if (CrossPlatformInputManager.GetButtonDown("Interact") && !firstFrameInteractDelay) {
                    typewriterTimer = 0.0f;
                    textbox.text += queuedDialog[0];
                    queuedDialog[0] = "";
                }

            } else {
                if ((CrossPlatformInputManager.GetButtonDown("Interact") || noConfirm) && !firstFrameInteractDelay) {
                    noConfirm = false;
                    NextText();
                }
            }
        } else if (textboxBg.activeSelf) {
            textboxBg.SetActive(false);
        }
    }

    void NextText() {
        typewriterTimer = 0.0f;
        textbox.text = "";
        queuedDialog.RemoveAt(0);
        ParseTextCommands();
    }
}
