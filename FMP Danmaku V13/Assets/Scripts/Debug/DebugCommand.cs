using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCommandBase
{
    private string _cmdId;
    private string _cmdDesc;
    private string _cmdFormat;
    
    public string commandId { get { return _cmdId; } }
    public string commandDescription { get { return _cmdDesc; } }
    public string commandFormat { get { return _cmdFormat; } }

    public DebugCommandBase(string id, string description, string format)
    {
        _cmdId = id;
        _cmdDesc = description;
        _cmdFormat = format;
    }
}

public class DebugCommand : DebugCommandBase
{
    private Action command;
    
    public DebugCommand(string id, string description, string format, Action command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke()
    {
        command.Invoke();
    }
}

public class DebugCommand<T1> : DebugCommandBase
{
    private Action<T1> command;
    
    public DebugCommand(string id, string description, string format, Action<T1> command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke(T1 value)
    {
        command.Invoke(value);
    }
}


