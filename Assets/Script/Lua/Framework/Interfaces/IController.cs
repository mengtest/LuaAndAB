using System;

namespace LuaFramework
{
    public interface IController
    {
        //
        void RegisterCommand(string messageName, Type commandType);

        //
        void RemoveCommand(string messageName);

        //
        void RegisterViewCommand(IView view, string[] commands);

        //
        void RemoveViewCommand(IView view, string[] commands);

        //
        void ExecuteCommand(IMessage message);

        //
        bool HasCommand(string messageName);
    }
}
