using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    [SerializeField] private bool showConsole;
    [SerializeField] private bool showHelp;
    Vector2 scroll;
    private string input;


    public static DebugCommand<int> SET_LIFE;

    public static DebugCommand HELP;

    public List<object> commandList;


    private void Awake()
    {
        SET_LIFE = new DebugCommand<int>("set_life", "Sets the amount of player lives to <life count>.", "set_life <life count>", (x) =>
        {
            PlayerController.instance.SetLifeCount(x);
        });
        
        HELP = new DebugCommand("help", "Shows list of commands", "help", () =>
        {
            showHelp = !showHelp;
        });
        commandList = new List<object>
        {
            SET_LIFE,
            HELP,
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            OnToggleDebug();
        }

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            OnReturn();
        }
    }

    public void OnToggleDebug()
    {
        showConsole = !showConsole;
    }

    public void OnReturn()
    {
        if (showConsole)
        {
            Debug.Log("On return called");
            HandleInput();
            input = "";
        }
        
        
    }



    private void OnGUI()
    {
        if(!showConsole){return;}

        float y = 0f;

        if (showHelp)
        {
            GUI.Box(new Rect(0, y, Screen.width, 100), "");
            
            Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);

            scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);

            for (int i = 0; i < commandList.Count; i++)
            {
                DebugCommandBase command = commandList[i] as DebugCommandBase;
                
                string label = $"{command.commandFormat} - {command.commandDescription}";
                
                Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
                
                GUI.Label(labelRect, label);
            }
            
            GUI.EndScrollView();

            y += 100;
        }
        
        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = Vector4.zero;
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);

        /*
        if (GUI.Button(new Rect(100, y, Screen.width / 2, 30), "Enter"))
        {
            Debug.Log("GUI BUTTON PRESSED");
            OnReturn();
        }*/
        
        
        
        
    }

    private void HandleInput()
    {
        string[] properties = input.Split(' ');
        
        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if (input.Contains(commandBase.commandId))
            {
                if (commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }
                else if(commandList[i] as DebugCommand<int> != null)
                {
                    (commandList[i] as DebugCommand<int>).Invoke(int.Parse(properties[1]));
                }
            }
            
        }
    }
    
}


