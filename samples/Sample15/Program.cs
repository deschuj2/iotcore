namespace Sample15;

using System;
using ifm.IoTCore.ElementManager.Contracts.Elements;
using ifm.IoTCore.ElementManager.Contracts.Elements.Tree;
using ifm.IoTCore.Factory;

internal class Program
{
    static void Main()
    {
        try
        {
            var ioTCore = IoTCoreFactory.Create("MyIoTCore");

            // Register treechanged event handler
            ioTCore.Root.TreeChanged += HandleTreeChangedEvent;

            // Create an element and do not raise a treechanged event
            var struct1 = ioTCore.ElementManager.CreateStructureElement(ioTCore.Root, 
                "struct1");

            // Raise a treechanged event on demand
            ioTCore.ElementManager.RaiseTreeChanged(TreeChangedActions.ChildAdded, 
                ioTCore.Root, 
                struct1);

            // Remove an element and raise a treechanged event
            ioTCore.ElementManager.RemoveElement(ioTCore.Root, 
                struct1, 
                raiseTreeChanged: true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Console.ReadLine();
    }

    private static void HandleTreeChangedEvent(object sender, EventArgs e)
    {
        var treeChangedEventArgs = (TreeChangedEventArgs<IBaseElement>)e;

        // Handle event
        switch (treeChangedEventArgs.Action)
        {
            case TreeChangedActions.ChildAdded:
                Console.WriteLine("Element added");
                break;
            case TreeChangedActions.ChildRemoved:
                Console.WriteLine("Element removed");
                break;
            case TreeChangedActions.TreeChanged:
                Console.WriteLine("Tree changed");
                break;
        }
    }
}