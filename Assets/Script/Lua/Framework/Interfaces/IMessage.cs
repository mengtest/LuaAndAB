using System;

public interface IMessage
{
    /// <summary>
    /// NotifyCommand
    /// </summary>
    string name { get; }

    object body { get; set; }

    string type { get; set; }

    string ToString();
}
