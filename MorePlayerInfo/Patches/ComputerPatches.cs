using GorillaNetworking;
using HarmonyLib;
using Photon.Pun;
using static GorillaNetworking.GorillaComputer;
using UnityEngine;
using OVR.OpenVR;

namespace ImproovedComputer.Patches
{
    [HarmonyPatch(typeof(GorillaComputer))]
    [HarmonyPatch("UpdateScreen", MethodType.Normal)]
    internal static class ComputerPatch0
    {
        private static bool Prefix(GorillaComputer __instance)
        {
            Plugin.Instance.NewScreen();
            return false;
        }
    }

    [HarmonyPatch(typeof(GorillaComputer))]
    [HarmonyPatch("PressButton", MethodType.Normal)]
    internal static class ComputerPatch1
    {
        private static bool Prefix(GorillaComputer __instance, GorillaKeyboardButton buttonPressed)
        {
            Plugin.Instance.NewPress(buttonPressed);
            return false;
        }
    }

    [HarmonyPatch(typeof(GorillaComputer))]
    [HarmonyPatch("SwitchToSupportState", MethodType.Normal)]
    internal static class ComputerPatch2
    {
        private static bool Prefix(GorillaComputer __instance)
        {
            __instance.SwitchState(ComputerState.Time);
            return false;
        }
    }

    [HarmonyPatch(typeof(GorillaComputer))]
    [HarmonyPatch("InitializeTimeState", MethodType.Normal)]
    internal static class ComputerPatch3
    {
        private static bool Prefix(GorillaComputer __instance)
        {
            Plugin.Instance.InitTime();
            return false;
        }
    }

    [HarmonyPatch(typeof(GorillaComputer))]
    [HarmonyPatch("ProcessQueueState", MethodType.Normal)]
    internal static class ComputerPatch4
    {
        private static bool Prefix(GorillaComputer __instance, GorillaKeyboardButton buttonPressed)
        {
            switch (buttonPressed.characterString)
            {
                case "option3":
                    if (__instance.allowedInCompetitive)
                    {
                        __instance.currentQueue = "COMPETITIVE";
                        PlayerPrefs.SetString("currentQueue", __instance.currentQueue);
                        PlayerPrefs.Save();
                    }

                    break;
                case "option2":
                    __instance.currentQueue = "MINIGAMES";
                    PlayerPrefs.SetString("currentQueue", __instance.currentQueue);
                    PlayerPrefs.Save();
                    break;
                case "option1":
                    __instance.currentQueue = "DEFAULT";
                    PlayerPrefs.SetString("currentQueue", __instance.currentQueue);
                    PlayerPrefs.Save();
                    break;
                case "G":
                    __instance.currentQueue = "GRAZE";
                    PlayerPrefs.SetString("currentQueue", __instance.currentQueue);
                    PlayerPrefs.Save();
                    break;
                case "down":
                    __instance.SwitchToGroupState();
                    break;
                case "up":
                    __instance.SwitchToMicState();
                    break;
            }

            __instance.UpdateScreen();
            return false;
        }
    }

    [HarmonyPatch(typeof(GorillaComputerTerminal))]
    [HarmonyPatch("LateUpdate", MethodType.Normal)]
    internal static class ComputerPatch5
    {

        private static void Postfix(GorillaComputerTerminal __instance)
        {
            if(__instance.GetComponent<HomeGiver>() == null)
            {
                __instance.gameObject.AddComponent<HomeGiver>();
            }
        }
    }
}