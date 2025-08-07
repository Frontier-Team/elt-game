using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class ComputerUI : MonoBehaviour
{
    [SerializeField]
    private UIDocument uiDocument;

    // UI Components 
    private VisualElement computerWindow, lockScreen, startMenu;

    // buttons
    private VisualElement startMenuButton, closeButton, loginButton, logoutButton, exitButton, shutDownButton;

    // flags
    private bool isComputerOpen = false;
    private bool isStartMenuOpen = false;
    private bool isLockScreenOpen = true;

    private void OnEnable()
    {
        // subscribe to events
        UIManager.OnPCOpened += OpenPC;
        UIManager.OnPCClosed += ClosePC;
    }

    private void OnDisable()
    {
        // unsubscribe from events
        UIManager.OnPCOpened -= OpenPC;
        UIManager.OnPCClosed -= ClosePC;
    }

    private void Awake()
    {
        if (uiDocument != null)
        {
            // INITIALISE //
            // components
            computerWindow = uiDocument.rootVisualElement.Q<VisualElement>("PCWrapper");
            lockScreen = uiDocument.rootVisualElement.Q<VisualElement>("LockScreen");
            startMenu = uiDocument.rootVisualElement.Q<VisualElement>("StartMenu");
            // buttons
            startMenuButton = uiDocument.rootVisualElement.Q<VisualElement>("StartMenuButton");
            closeButton = uiDocument.rootVisualElement.Q<VisualElement>("CloseButton");
            loginButton = uiDocument.rootVisualElement.Q<VisualElement>("LoginButton");
            logoutButton = uiDocument.rootVisualElement.Q<VisualElement>("LogoutButton");
            exitButton = uiDocument.rootVisualElement.Q<VisualElement>("ExitButton");
            shutDownButton = uiDocument.rootVisualElement.Q<VisualElement>("ShutDownButton");
            // events
            startMenuButton.RegisterCallback<ClickEvent>(ev => ToggleStartMenu());
            closeButton.RegisterCallback<ClickEvent>(ev => ClosePC());
            loginButton.RegisterCallback<ClickEvent>(ev => LockScreenOff());
            logoutButton.RegisterCallback<ClickEvent>(ev => LockScreenOn());
            exitButton.RegisterCallback<ClickEvent>(ev => ClosePC());
            shutDownButton.RegisterCallback<ClickEvent>(ev => ClosePC());

            // hide on start
            computerWindow.style.display = DisplayStyle.None;
        }   
    }

    private void OpenPC()
    {
        computerWindow.style.display = DisplayStyle.Flex;
        isComputerOpen = true;
    }

    private void ClosePC()
    {
        computerWindow.style.display = DisplayStyle.None;
        isComputerOpen = false;
        CloseStartMenu();
    }

    private void LockScreenOn()
    {
        lockScreen.style.translate = new StyleTranslate(new Translate(0, 0));
        isLockScreenOpen = false;
        CloseStartMenu();
    }

    private void LockScreenOff()
    {
        lockScreen.style.translate = new StyleTranslate(new Translate(0, -1000));
        isLockScreenOpen = true;
    }

    private void ToggleStartMenu()
    {
        if (isStartMenuOpen)
            CloseStartMenu();
        else
            OpenStartMenu();
    }

    private void OpenStartMenu()
    {
        startMenu.style.display = DisplayStyle.Flex;
        isStartMenuOpen = true;
    }

    private void CloseStartMenu()
    {
        startMenu.style.display = DisplayStyle.None;
        isStartMenuOpen = false;
    }
}