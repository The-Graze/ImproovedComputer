using BepInEx;
using GorillaNetworking;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GorillaNetworking.GorillaComputer;

namespace ImproovedComputer
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance;
        public GameObject HomeKey;
        private float t = 0.0f;
        public float transitionSpeed = 0.2f;
        public int num;
        Color lerpedColor;
        void Awake()
        {
            HarmonyPatches.ApplyHarmonyPatches();
            Instance = this;
        }
        void Start()
        {
            HarmonyPatches.ApplyHarmonyPatches();
            Invoke("LageChange", 2);
            Invoke("SetUp", 2);
        }
        void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
        }
        void Update()
        {
            if (!HarmonyPatches.IsPatched)
            {
                HarmonyPatches.ApplyHarmonyPatches();
            }
            t += Time.deltaTime * transitionSpeed;
            lerpedColor = Color.HSVToRGB(Mathf.PingPong(t, 1.0f), 1.0f, 1.0f);
            GorillaComputer.instance.unpressedMaterial.color = lerpedColor;
        }

        void LateChange()
        {
            GorillaComputer.instance.Awake();
            GorillaComputer.instance.unpressedMaterial.color = Color.black;
        }
        static TimeSettings StringToTime(string stwing)
        {
            if (stwing == "Normal")
            {
                return TimeSettings.Normal;
            }
            else
            {
                return TimeSettings.Static;
            }
        }
        public void InitTime()
        {
            BetterDayNightManager.instance.currentSetting = StringToTime(PlayerPrefs.GetString("TimeMode", "Normal"));
            if (StringToTime(PlayerPrefs.GetString("TimeMode", "Normal")) == TimeSettings.Static)
            {
                BetterDayNightManager.instance.currentTimeIndex = PlayerPrefs.GetInt("Time", 0);
            }
        }

        int Idex()
        {
            if (PhotonNetworkController.Instance.currentRegionIndex == 0)
            {
                return 0;
            }
            else
            {
                return PhotonNetworkController.Instance.currentRegionIndex - 1;
            }
        }
        void NewUpdateFunc()

        {
            instance.functionSelectText.Text = "ROOM   " + ((instance.currentState == ComputerState.Room) ? "<-" : "") + "\nNAME   " + ((instance.currentState == ComputerState.Name) ? "<-" : "") + "\nCOLOR  " + ((instance.currentState == ComputerState.Color) ? "<-" : "") + "\nTURN   " + ((instance.currentState == ComputerState.Turn) ? "<-" : "") + "\nMIC    " + ((instance.currentState == ComputerState.Mic) ? "<-" : "") + "\nQUEUE  " + ((instance.currentState == ComputerState.Queue) ? "<-" : "") + "\nGROUP  " + ((instance.currentState == ComputerState.Group) ? "<-" : "") + "\nVOICE  " + ((instance.currentState == ComputerState.Voice) ? "<-" : "") + "\nITEMS  " + ((instance.currentState == ComputerState.Visuals) ? "<-" : "") + "\nCREDITS" + ((instance.currentState == ComputerState.Credits) ? "<-" : "") + "\nTIME" + ((instance.currentState == ComputerState.Time) ? "<-" : "");
        }

        public void NewScreen()
        {
            if (PhotonNetworkController.Instance != null && !PhotonNetworkController.Instance.wrongVersion)
            {
                NewUpdateFunc();
                switch (GorillaComputer.instance.currentState)
                {
                    case ComputerState.Startup:
                        GorillaComputer.instance.screenText.Text = "GORILLA OS 2\n\n" + PhotonNetworkController.Instance.TotalUsers() + " TOTAL PLAYERS ONLINE\n\n" + "REGION: " + PhotonNetworkController.Instance.serverRegions[Idex()].ToUpper() + " WITH " + PhotonNetworkController.Instance.pingInRegion[Idex()].ToString() + " PING\nAND " + PhotonNetworkController.Instance.playersInRegion[Idex()].ToString() + " PLAYERS " + "\n\n" + GorillaComputer.instance.usersBanned + " USERS BANNED YESTERDAY\n\nPRESS ANY KEY TO BEGIN";
                        break;
                    case ComputerState.Color:
                        {
                            GorillaComputer.instance.screenText.Text = "USE THE OPTIONS BUTTONS TO SELECT COLOUR, THEN PRESS 0-9 TO CHANGE IT.";
                            GorillaText gorillaText6 = GorillaComputer.instance.screenText;
                            gorillaText6.Text = gorillaText6.Text + "\n\n  RED: " + Mathf.FloorToInt(GorillaComputer.instance.redValue * 9f) + ((GorillaComputer.instance.colorCursorLine == 0) ? "<--" : "");
                            GorillaText gorillaText7 = GorillaComputer.instance.screenText;
                            gorillaText7.Text = gorillaText7.Text + "\n\nGREEN: " + Mathf.FloorToInt(GorillaComputer.instance.greenValue * 9f) + ((GorillaComputer.instance.colorCursorLine == 1) ? "<--" : "");
                            GorillaText gorillaText8 = GorillaComputer.instance.screenText;
                            gorillaText8.Text = gorillaText8.Text + "\n\n BLUE: " + Mathf.FloorToInt(GorillaComputer.instance.blueValue * 9f) + ((GorillaComputer.instance.colorCursorLine == 2) ? "<--" : "");
                            break;
                        }
                    case ComputerState.Room:
                        {
                            GorillaComputer.instance.screenText.Text = "PRESS ENTER TO JOIN OR CREATE A CUSTOM ROOM. PRESS OPTION 1 TO LEAVE.\n\nCURRENT ROOM: ";
                            if (PhotonNetwork.InRoom)
                            {
                                GorillaComputer.instance.screenText.Text += PhotonNetwork.CurrentRoom.Name;
                                GorillaText gorillaText3 = GorillaComputer.instance.screenText;
                                gorillaText3.Text = gorillaText3.Text + "\n\nPLAYERS IN ROOM: " + PhotonNetwork.CurrentRoom.PlayerCount + "\n ROOM HOST: " + PhotonNetwork.MasterClient.NickName;
                            }
                            else
                            {
                                GorillaComputer.instance.screenText.Text += "-NOT IN ROOM-";
                                GorillaText gorillaText4 = GorillaComputer.instance.screenText;
                                gorillaText4.Text = gorillaText4.Text + "\n\n REGION PLAYERS ONLINE: " + PhotonNetworkController.Instance.pingInRegion[Idex()].ToString();
                            }

                            GorillaText gorillaText5 = GorillaComputer.instance.screenText;
                            gorillaText5.Text = gorillaText5.Text + "\n\nROOM TO JOIN: " + GorillaComputer.instance.roomToJoin;
                            if (GorillaComputer.instance.roomFull)
                            {
                                GorillaComputer.instance.screenText.Text += "\n\nROOM FULL. TRY AGAIN.";
                            }
                            else if (GorillaComputer.instance.roomNotAllowed)
                            {
                                GorillaComputer.instance.screenText.Text += "\n\nCANNOT JOIN ROOM TYPE FROM HERE.(TRY A DIFFRENT COMPUTER)";
                            }

                            break;
                        }
                    case ComputerState.Name:
                        {
                            GorillaComputer.instance.screenText.Text = "PRESS ENTER TO CHANGE YOUR NAME.\n\nCURRENT NAME: " + GorillaComputer.instance.savedName;
                            GorillaText gorillaText2 = GorillaComputer.instance.screenText;
                            gorillaText2.Text = gorillaText2.Text + "\n\n    NEW NAME: " + GorillaComputer.instance.currentName;
                            break;
                        }
                    case ComputerState.Turn:
                        GorillaComputer.instance.screenText.Text = "PRESS OPTION 1 TO USE SNAP TURN. PRESS OPTION 2 TO USE SMOOTH TURN. PRESS OPTION 3 TO USE NO ARTIFICIAL TURNING. PRESS THE NUMBER KEYS TO CHOOSE A TURNING SPEED.\n CURRENT TURN TYPE: " + GorillaComputer.instance.turnType + "\nCURRENT TURN SPEED: " + GorillaComputer.instance.turnValue;
                        break;
                    case ComputerState.Queue:
                        if (GorillaComputer.instance.allowedInCompetitive)
                        {
                            GorillaComputer.instance.screenText.Text = "THIS OPTION AFFECTS WHO YOU PLAY WITH. DEFAULT IS FOR ANYONE TO PLAY NORMALLY. MINIGAMES IS FOR PEOPLE LOOKING TO PLAY WITH THEIR OWN MADE UP RULES.COMPETITIVE IS FOR PLAYERS WHO WANT TO PLAY THE GAME AND TRY AS HARD AS THEY CAN. PRESS OPTION 1 FOR DEFAULT, OPTION 2 FOR MINIGAMES, OR OPTION 3 FOR COMPETITIVE.\n\nCURRENT QUEUE: " + GorillaComputer.instance.currentQueue;
                        }
                        else
                        {
                            GorillaComputer.instance.screenText.Text = "THIS OPTION AFFECTS WHO YOU PLAY WITH. DEFAULT IS FOR ANYONE TO PLAY NORMALLY. MINIGAMES IS FOR PEOPLE LOOKING TO PLAY WITH THEIR OWN MADE UP RULES.BEAT THE OBSTACLE COURSE IN CITY TO ALLOW COMPETITIVE PLAY. PRESS OPTION 1 FOR DEFAULT, OR OPTION 2 FOR MINIGAMES\n\nCURRENT QUEUE: " + GorillaComputer.instance.currentQueue;
                        }

                        break;
                    case ComputerState.Mic:
                        GorillaComputer.instance.screenText.Text = "CHOOSE ALL CHAT, PUSH TO TALK, OR PUSH TO MUTE. THE BUTTONS FOR PUSH TO TALK AND PUSH TO MUTE ARE ANY OF THE FACE BUTTONS.\nPRESS OPTION 1 TO CHOOSE ALL CHAT.\nPRESS OPTION 2 TO CHOOSE PUSH TO TALK.\nPRESS OPTION 3 TO CHOOSE PUSH TO MUTE.\n\nCURRENT MIC SETTING: " + GorillaComputer.instance.pttType;
                        break;
                    case ComputerState.Group:
                        if (GorillaComputer.instance.allowedMapsToJoin.Length == 1)
                        {
                            GorillaComputer.instance.screenText.Text = "USE THIS TO JOIN A PUBLIC ROOM WITH A GROUP OF FRIENDS. GET EVERYONE IN A PRIVATE ROOM. PRESS THE NUMBER KEYS TO SELECT THE MAP. 1 FOR " + GorillaComputer.instance.allowedMapsToJoin[Mathf.Min(GorillaComputer.instance.allowedMapsToJoin.Length - 1, GorillaComputer.instance.groupMapJoinIndex)].ToUpper() + ". WHILE EVERYONE IS SITTING NEXT TO THE COMPUTER, PRESS ENTER. YOU WILL ALL JOIN A PUBLIC ROOM TOGETHER AS LONG AS EVERYONE IS NEXT TO THE COMPUTER.\nCURRENT MAP SELECTION : " + GorillaComputer.instance.allowedMapsToJoin[Mathf.Min(GorillaComputer.instance.allowedMapsToJoin.Length - 1, GorillaComputer.instance.groupMapJoinIndex)].ToUpper();
                        }
                        else
                        {
                            GorillaComputer.instance.screenText.Text = "USE THIS TO JOIN A PUBLIC ROOM WITH A GROUP OF FRIENDS. GET EVERYONE IN A PRIVATE ROOM. PRESS THE NUMBER KEYS TO SELECT THE MAP. 1 FOR FOREST, 2 FOR CAVE, AND 3 FOR CANYON, AND 4 FOR CITY. WHILE EVERYONE IS SITTING NEXT TO THE COMPUTER, PRESS ENTER. YOU WILL ALL JOIN A PUBLIC ROOM TOGETHER AS LONG AS EVERYONE IS NEXT TO THE COMPUTER.\nCURRENT MAP SELECTION : " + GorillaComputer.instance.allowedMapsToJoin[Mathf.Min(GorillaComputer.instance.allowedMapsToJoin.Length - 1, GorillaComputer.instance.groupMapJoinIndex)].ToUpper();
                        }

                        break;
                    case ComputerState.Voice:
                        GorillaComputer.instance.screenText.Text = "USE THIS TO ENABLE OR DISABLE VOICE CHAT.\nPRESS OPTION 1 TO ENABLE VOICE CHAT.\nPRESS OPTION 2 TO DISABLE VOICE CHAT.\n\nVOICE CHAT ON: " + GorillaComputer.instance.voiceChatOn;
                        break;
                    case ComputerState.Visuals:
                        GorillaComputer.instance.screenText.Text = "UPDATE ITEMS SETTINGS. PRESS OPTION 1 TO ENABLE ITEM PARTICLES. PRESS OPTION 2 TO DISABLE ITEM PARTICLES. PRESS 1-10 TO CHANGE INSTRUMENT VOLUME FOR OTHER PLAYERS.\n\nITEM PARTICLES ON: " + (GorillaComputer.instance.disableParticles ? "FALSE" : "TRUE") + "\nINSTRUMENT VOLUME: " + Mathf.CeilToInt(GorillaComputer.instance.instrumentVolume * 50f);
                        break;
                    case ComputerState.Credits:
                        GorillaComputer.instance.screenText.Text = GorillaComputer.instance.creditsView.GetScreenText();
                        break;
                    case ComputerState.Time:
                        GorillaComputer.instance.screenText.Text = "UPDATE TIME SETTINGS. (LOCALLY ONLY). \n\nPRESS OPTION 1 FOR NORMAL MODE. \nPRESS OPTION 2 FOR STATIC MODE. \nPRESS 1-10 TO CHANGE TIME OF DAY. \n\nCURRENT MODE: " + BetterDayNightManager.instance.currentSetting.ToString().ToUpper() + ". \nTIME OF DAY: " + BetterDayNightManager.instance.dayNightLightmaps[num].name.ToUpper() + ". \n";
                        break;
                    case ComputerState.Support:
                        if (GorillaComputer.instance.displaySupport)
                        {
                            string text = "PC";
                            GorillaComputer.instance.screenText.Text = "SUPPORT\n\nPLAYERID   " + PlayFabAuthenticator.instance._playFabPlayerIdCache + "\nVERSION    " + GorillaComputer.instance.version.ToUpper() + "\nPLATFORM   " + text + "\nBUILD DATE " + GorillaComputer.instance.buildDate + "\n";
                        }
                        else
                        {
                            GorillaComputer.instance.screenText.Text = "SUPPORT\n\n";
                            GorillaComputer.instance.screenText.Text += "PRESS ENTER TO DISPLAY SUPPORT AND ACCOUNT INFORMATION\n\n\n\n";
                            GorillaComputer.instance.screenText.Text += "<color=red>DO NOT SHARE ACCOUNT INFORMATION WITH ANYONE OTHER ";
                            GorillaComputer.instance.screenText.Text += "THAN ANOTHER AXIOM SUPPORT</color>";
                        }

                        break;
                    case ComputerState.NameWarning:
                        {
                            GorillaComputer.instance.screenText.Text = "<color=red>WARNING: PLEASE CHOOSE A BETTER NAME\n\nENTERING ANOTHER BAD NAME WILL RESULT IN A BAN</color>";
                            if (GorillaComputer.instance.warningConfirmationInputString.ToLower() == "yes")
                            {
                                GorillaComputer.instance.screenText.Text += "\n\nPRESS ANY KEY TO CONTINUE";
                                break;
                            }

                            GorillaText gorillaText = GorillaComputer.instance.screenText;
                            gorillaText.Text = gorillaText.Text + "\n\nTYPE 'YES' TO CONFIRM: " + GorillaComputer.instance.warningConfirmationInputString;
                            break;
                        }
                    case ComputerState.Loading:
                        GorillaComputer.instance.screenText.Text = "LOADING";
                        GorillaComputer.instance.screenText.Text = "LOADING.";
                        GorillaComputer.instance.screenText.Text = "LOADING..";
                        GorillaComputer.instance.screenText.Text = "LOADING...";
                        break;
                }
            }

            if (PhotonNetwork.InRoom)
            {
                if (GorillaGameManager.instance != null && GorillaGameManager.instance.GetComponent<GorillaTagManager>() != null)
                {
                    if (!GorillaGameManager.instance.GetComponent<GorillaTagManager>().IsGameModeTag())
                    {
                        GorillaComputer.instance.currentGameModeText.text = "CURRENT MODE\nCASUAL";
                    }
                    else
                    {
                        GorillaComputer.instance.currentGameModeText.text = "CURRENT MODE\nINFECTION";
                    }
                }
                else if (GorillaGameManager.instance != null && GorillaGameManager.instance.GetComponent<GorillaHuntManager>() != null)
                {
                    GorillaComputer.instance.currentGameModeText.text = "CURRENT MODE\nHUNT";
                }
                else if (GorillaGameManager.instance != null && GorillaGameManager.instance.GetComponent<GorillaBattleManager>() != null)
                {
                    GorillaComputer.instance.currentGameModeText.text = "CURRENT MODE\nPAINTBRAWL";
                }
                else
                {
                    GorillaComputer.instance.currentGameModeText.text = "CURRENT MODE\nERROR";
                }
            }
            else
            {
                GorillaComputer.instance.currentGameModeText.text = "CURRENT MODE\n-NOT IN ROOM-";
            }
        }

        public void NewPress(GorillaKeyboardButton buttonPressed)
        {
            switch (GorillaComputer.instance.currentState)
            {
                case ComputerState.Startup:
                    GorillaComputer.instance.ProcessStartupState(buttonPressed);
                    break;
                case ComputerState.Color:
                    GorillaComputer.instance.ProcessColorState(buttonPressed);
                    break;
                case ComputerState.Room:
                    GorillaComputer.instance.ProcessRoomState(buttonPressed);
                    break;
                case ComputerState.Name:
                    GorillaComputer.instance.ProcessNameState(buttonPressed);
                    break;
                case ComputerState.Turn:
                    GorillaComputer.instance.ProcessTurnState(buttonPressed);
                    break;
                case ComputerState.Mic:
                    GorillaComputer.instance.ProcessMicState(buttonPressed);
                    break;
                case ComputerState.Queue:
                    GorillaComputer.instance.ProcessQueueState(buttonPressed);
                    break;
                case ComputerState.Group:
                    GorillaComputer.instance.ProcessGroupState(buttonPressed);
                    break;
                case ComputerState.Voice:
                    GorillaComputer.instance.ProcessVoiceState(buttonPressed);
                    break;
                case ComputerState.Credits:
                    GorillaComputer.instance.ProcessCreditsState(buttonPressed);
                    break;
                case ComputerState.Support:
                    GorillaComputer.instance.ProcessSupportState(buttonPressed);
                    break;
                case ComputerState.Visuals:
                    GorillaComputer.instance.ProcessVisualsState(buttonPressed);
                    break;
                case ComputerState.NameWarning:
                    GorillaComputer.instance.ProcessNameWarningState(buttonPressed);
                    break;
                case ComputerState.Time:
                    ProcessTimeState(buttonPressed);
                    break;

            }

            buttonPressed.GetComponent<MeshRenderer>().material = GorillaComputer.instance.pressedMaterial;
            buttonPressed.pressTime = Time.time;
            GorillaComputer.instance.StartCoroutine(GorillaComputer.instance.ButtonColorUpdate(buttonPressed));
        }
        void ProcessTimeState(GorillaKeyboardButton buttonPressed)
        {
            if (int.TryParse(buttonPressed.characterString, out var result))
            {
                BetterDayNightManager.instance.SetTimeOfDay(result);
                num = result;
                PlayerPrefs.SetInt("Time", result);
                PlayerPrefs.Save();
                                    NewScreen();
            }
            switch (buttonPressed.characterString)
            {
                case "option1":
                    BetterDayNightManager.instance.currentSetting = TimeSettings.Normal;
                    PlayerPrefs.SetString("TimeMode", "Normal");
                    PlayerPrefs.Save();
                    NewScreen();
                    break;
                case "option2":
                    BetterDayNightManager.instance.currentSetting = TimeSettings.Static;
                    PlayerPrefs.SetString("TimeMode", "Static");
                    PlayerPrefs.Save();
                    NewScreen();
                    break;
                case "down":
                    GorillaComputer.instance.SwitchToRoomState();
                    break;
                case "up":
                    GorillaComputer.instance.SwitchToCreditsState();
                    break;
            }
        }

        public void SetUp()
        {
            Font f = GameObject.Find("Casual Button Text").GetComponent<Text>().font;
            HomeKey = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/TreeRoomInteractables/UI/-- PhysicalComputer UI --/keyboard/Buttons/Keys").transform.GetChild(36).gameObject;
            HomeKey.SetActive(true);
            HomeKey.transform.localPosition = new Vector3(8.2018f, 0.3f, 33.7628f);
            HomeKey.name = "HomeKey";
            Destroy(HomeKey.GetComponent<GorillaKeyboardButton>());
            HomeKey.AddComponent<HomeKey>();
        }
    }
    class HomeKey : GorillaPressableButton
    {
        private void OnTriggerEnter(Collider collider)
        {
            if (!base.enabled || !(touchTime + debounceTime < Time.time) || collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() == null)
            {
                return;
            }

            touchTime = Time.time;
            GorillaTriggerColliderHandIndicator component = collider.GetComponent<GorillaTriggerColliderHandIndicator>();
            if (!(component == null))
            {
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, component.isLeftHand, 0.05f);
                GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
                if (PhotonNetwork.InRoom && GorillaTagger.Instance.myVRRig != null)
                {
                    GorillaTagger.Instance.myVRRig.RPC("PlayHandTap", RpcTarget.Others, 67, component.isLeftHand, 0.05f);
                }
            }
            GorillaComputer.instance.SwitchState(ComputerState.Startup);
        }
    }
    class HomeGiver : MonoBehaviour
    {
        bool done;
        void Start()
        {
            if (!done)
            {
                Instantiate(Plugin.Instance.HomeKey, transform.GetChild(1).GetChild(0).GetChild(0));
                done = true;
            }
        }
    }
}
