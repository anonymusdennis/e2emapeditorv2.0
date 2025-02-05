using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using UnityEngine.UI;
using System.ComponentModel;
using Steamworks;


[assembly: MelonInfo(typeof(e2emapeditor2.MapEditor), "E2EMapeditor2", "2.0.0", "anonymusdennis")]
namespace e2emapeditor2
{

    public class MapEditor : MelonMod
        {
            public static MapEditor instance;

            private static KeyCode freezeToggleKey;
            private static KeyCode menuToggleKey;
            private static bool menuOpen;
            private static bool frozen;
            private static float baseTimeScale;
            private bool immortality_on = false;
            private int heat = 0;
            public override void OnEarlyInitializeMelon()
            {
                instance = this;
                menuToggleKey = KeyCode.Keypad5;
                freezeToggleKey = KeyCode.Space;
            e2emapeditor2.MapEditor.instance.LoggerInstance.Msg("WE NEED TO DO THINGS!!!!!!!!!!!!");
        }

            public override void OnLateUpdate()
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    heat++;
                    if(heat > 100) heat = 100;
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    heat--;
                    if (heat < 0) heat = 0;
                }

            if (Input.GetKeyDown(menuToggleKey))
                {
                ConfigManager.GetInstance().minigameConfig.m_Solitary_StaminaLossModifier = 0.0f;
                CraftManager instance = CraftManager.GetInstance();
                if (instance != null)
                {
                    List<CraftManager.Recipe> currentRecipes = instance.GetCurrentRecipes();
                    int count = currentRecipes.Count;
                    int num3 = 0;
                    for (int i = 0; i < count; i++)
                    {
                        CraftManager.Recipe recipe = currentRecipes[i];
                        CraftManager.GetInstance().DiscoverHiddenItem(recipe);
                    }
                }
                ToggleMenu();
                e2emapeditor2.MapEditor.instance.LoggerInstance.Msg("WE NEED TO DO THINGS!!!!!!!!!!!!");
            }

                if (Input.GetKeyDown(KeyCode.F6))
                {
                    this.immortality_on = !this.immortality_on;
                }

                if (this.immortality_on)
                {
                    Gamer[]  gamers = Gamer.GetLocalGamers();
                    foreach (var gamer in gamers)
                    {
                        if (gamer != null && gamer.IsLocal())
                        {
                            //MapEditor.instance.LoggerInstance.Msg(gamer.m_PlayerObject.m_CharacterStats.Health);
                            gamer.m_PlayerObject.m_CharacterStats.SetHealth(100);
                            gamer.m_PlayerObject.m_CharacterStats.SetEnergy(100);
                            gamer.m_PlayerObject.m_CharacterStats.SetHeat( (float) heat);
                    }
                    }
                }
            }

            public static void DrawMenuText()
            {
                GUI.Label(new Rect(20, 20, 1000, 200), "<b>Menu</b>");
            }

            private static void ToggleMenu()
            {
                menuOpen = !menuOpen;
                if (menuOpen)
                {
                    
                    instance.LoggerInstance.Msg("Opening Menu2");
                    MelonEvents.OnGUI.Subscribe(DrawMenuText, 100); // Register the 'Frozen' label
                    baseTimeScale = Time.timeScale; // Save the original time scale before freezing
                    Time.timeScale = 10;
            }
                else
                {
                    instance.LoggerInstance.Msg("Closing Menu");
                    MelonEvents.OnGUI.Unsubscribe(DrawMenuText); // Unregister the 'Frozen' label
                    Time.timeScale = baseTimeScale;
            }
            }

            private static void ToggleFreeze()
            {
                frozen = !frozen;

                if (frozen)
                {
                    instance.LoggerInstance.Msg("Freezing");

                    //MelonEvents.OnGUI.Subscribe(DrawFrozenText, 100); // Register the 'Frozen' label
                    baseTimeScale = Time.timeScale; // Save the original time scale before freezing
                    Time.timeScale = 0;
                }
                else
                {
                    instance.LoggerInstance.Msg("Unfreezing");

                    //MelonEvents.OnGUI.Unsubscribe(DrawFrozenText); // Unregister the 'Frozen' label
                    Time.timeScale = baseTimeScale; // Reset the time scale to what it was before we froze the time
                }
            }

            public override void OnDeinitializeMelon()
            {
                if (frozen)
                {
                    ToggleFreeze(); // Unfreeze the game in case the melon gets unregistered
                }
            }
        }


    [HarmonyPatch(typeof(CraftManager), nameof(CraftManager.HasItemsForRecipe), new Type[] { typeof(int) ,
        typeof(ItemContainer),
        typeof(int[]),
        typeof(bool)}, new ArgumentType[] { ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Ref, ArgumentType.Normal })]
    static class Patch
    {

        static void Postfix(bool __result)
        {

            e2emapeditor2.MapEditor.instance.LoggerInstance.Msg("WE DID THINGS!!!   (1)");
            __result = true;
        }
    }
    [HarmonyPatch(typeof(CraftManager), nameof(CraftManager.HasItemsForRecipe), new Type[] { typeof(ItemData[]) ,
        typeof(bool[]),
        typeof(ItemContainer),
        typeof(int[]),typeof(int),typeof(bool),typeof(bool)}, new ArgumentType[] { ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Ref, ArgumentType.Out, ArgumentType.Normal, ArgumentType.Normal })]
    static class Patch2
    {

        static void Postfix(bool __result)
        {

            e2emapeditor2.MapEditor.instance.LoggerInstance.Msg("WE DID THINGS!!!  (2)");
            __result = true;
        }
    }
    [HarmonyPatch(typeof(CraftManager), nameof(CraftManager.HasItemsForRecipe), new Type[] { typeof(CraftManager.Recipe) ,
        typeof(ItemContainer),
        typeof(int[]),
        typeof(bool)}, new ArgumentType[] { ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Ref, ArgumentType.Normal })]
    static class Patch3
    {

        static void Postfix(bool __result)
        {

            e2emapeditor2.MapEditor.instance.LoggerInstance.Msg("WE DID THINGS!!!  (3)");
            __result = true;
        }
    }
    [HarmonyPatch(typeof(SteamAPI), nameof(SteamAPI.RestartAppIfNecessary), new Type[] {typeof(AppId_t) })]
    static class Patch4
    {
        static void Postfix(bool __result)
        {
            e2emapeditor2.MapEditor.instance.LoggerInstance.Msg("WE DID THINGS!!!  (3)");
            __result = false;
        }
    }

}
