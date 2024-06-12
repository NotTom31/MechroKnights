using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    public static SoundManager GetSoundManager() {return _soundManager;}
    public static void ProvideService(SoundManager soundManagerRef) {_soundManager = soundManagerRef;}

    public static MenuManager GetMenuManager() {return _menuManager;}
    public static void ProvideService(MenuManager menuManagerRef) {_menuManager = menuManagerRef;}

    public static ChangeCharacter GetChangeCharacter() {return _changeCharacter;}
    public static void ProvideService(ChangeCharacter changeCharacter) {_changeCharacter = changeCharacter;}


    private static SoundManager _soundManager;
    private static MenuManager _menuManager;
    private static ChangeCharacter _changeCharacter;
}
