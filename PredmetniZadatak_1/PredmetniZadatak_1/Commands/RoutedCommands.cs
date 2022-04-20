using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PredmetniZadatak_1.Commands
{
    public static class RoutedCommands
    {
        public static readonly RoutedUICommand DrawEllipseCommand = new RoutedUICommand("DrawEllipseCommand", "DrawEllipseCommand", typeof(RoutedCommands));
        public static readonly RoutedUICommand DrawPolygonCommand = new RoutedUICommand("DrawPolygonCommand", "DrawPolygonCommand", typeof(RoutedCommands));
        public static readonly RoutedUICommand AddTextCommand = new RoutedUICommand("AddTextCommand", "AddTextCommand", typeof(RoutedCommands));

        public static readonly RoutedUICommand UndoCommand = new RoutedUICommand("UndoCommand", "UndoCommand", typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Z, ModifierKeys.Control)
            });

        public static readonly RoutedUICommand RedoCommand = new RoutedUICommand("RedoCommand", "RedoCommand", typeof(RoutedCommands),
            new InputGestureCollection()
            {
                        new KeyGesture(Key.Y, ModifierKeys.Control)
            });
    }
}
