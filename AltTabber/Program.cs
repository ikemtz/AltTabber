using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Automation;

namespace AltTabber
{
    class Program
    {
        static void Main(string[] args)
        {
            while (1 == 1)
            {
                Console.Clear();
                Console.WriteLine("Tabbing through the following tabs:");

                var desk = AutomationElement.RootElement;
                var elements = desk.FindAll(TreeScope.Children, Condition.TrueCondition)
                    .Cast<AutomationElement>()
                   .Where(t =>
                        null != t
                        && t.Current.ProcessId != Process.GetCurrentProcess().Id
                        && t.Current.IsKeyboardFocusable
                        && null != t.GetCurrentPattern(WindowPattern.Pattern)
                        && ((WindowPattern)t.GetCurrentPattern(WindowPattern.Pattern)).Current.CanMaximize)
                   .ToList();
                elements.ForEach(t => Console.WriteLine("\t {0}", t.Current.Name));
                Console.WriteLine();
                elements.ForEach(t =>
                {
                    try
                    {
                        if (null != t
                        && null != t.GetCurrentPattern(WindowPattern.Pattern)
                        && null != t.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ProcessIdProperty, t.Current.ProcessId))
                        && t.Current.IsKeyboardFocusable)
                        {
                            Console.WriteLine("Activating: {0}", t.Current.Name);
                            var pattern = t.GetCurrentPattern(WindowPattern.Pattern) as WindowPattern;
                            pattern.SetWindowVisualState(WindowVisualState.Maximized);

                            t.SetFocus();
                            Thread.Sleep(30000);
                        }
                    }
                    catch (Exception) {
                        Console.WriteLine("Error Activating a window.");
                    }
                });
            }
        }
    }
}
