using System;

public interface IEventCommand {
    Action OnFinished {get; set;}
    void Execute();
}