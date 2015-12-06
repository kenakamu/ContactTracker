/*================================================================================================================================

  This Sample Code is provided for the purpose of illustration only and is not intended to be used in a production environment.  

  THIS SAMPLE CODE AND ANY RELATED INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, 
  INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.  

  We grant You a nonexclusive, royalty-free right to use and modify the Sample Code and to reproduce and distribute the object 
  code form of the Sample Code, provided that You agree: (i) to not use Our name, logo, or trademarks to market Your software 
  product in which the Sample Code is embedded; (ii) to include a valid copyright notice on Your software product in which the 
  Sample Code is embedded; and (iii) to indemnify, hold harmless, and defend Us and Our suppliers from and against any claims 
  or lawsuits, including attorneys’ fees, that arise or result from the use or distribution of the Sample Code.

 =================================================================================================================================*/

using ContactTracker.Common;
using System;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace ContactTracker.Commands
{
    /// <summary>
    /// Button Click Event
    /// </summary>
    public class ClickCommand
    {
        public ClickCommand()
        {
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand),
            typeof(ClickCommand), new PropertyMetadata(null, OnCommandPropertyChanged));

        public static void SetCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(CommandProperty);
        }

        private static void OnCommandPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var control = d as Button;
            if (control != null)
                control.Click += OnClick;
        }

        private static void OnClick(object sender, RoutedEventArgs e)
        {
            var control = sender as Button;
            var command = GetCommand(control);

            if (command != null && command.CanExecute(e))
                command.Execute(e);
        }
    }

    /// <summary>
    /// ListView ItemClick Event
    /// </summary>
    public class ItemClickCommand
    {
        public ItemClickCommand()
        {
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand),
            typeof(ItemClickCommand), new PropertyMetadata(null, OnCommandPropertyChanged));

        public static void SetCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(CommandProperty);
        }

        private static void OnCommandPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var control = d as ListViewBase;
            if (control != null)
                control.ItemClick += OnItemClick;
        }

        private static void OnItemClick(object sender, ItemClickEventArgs e)
        {
            var control = sender as ListViewBase;
            var command = GetCommand(control);

            if (command != null && command.CanExecute(e.ClickedItem))
                command.Execute(e.ClickedItem);
        }
    }

    /// <summary>
    /// ListView SelectionChanged Event
    /// </summary>
    public class ListSelectionChangedCommand
    {
        public ListSelectionChangedCommand()
        {
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand),
            typeof(ListSelectionChangedCommand), new PropertyMetadata(null, OnCommandPropertyChanged));

        public static void SetCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(CommandProperty);
        }

        private static void OnCommandPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var control = d as ListView;
            if (control != null)
                control.SelectionChanged += OnSelectionChanged;
        }

        private static void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var control = sender as ListView;
            var command = GetCommand(control);

            if (command != null && command.CanExecute(e.AddedItems))
                command.Execute(e);
        }
    }

    /// <summary>
    /// ComboBox SelectionChanged Event
    /// </summary>
    public class SelectionChangedCommand
    {
        public SelectionChangedCommand()
        {
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand),
            typeof(SelectionChangedCommand), new PropertyMetadata(null, OnCommandPropertyChanged));

        public static void SetCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(CommandProperty);
        }

        private static void OnCommandPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var control = d as ComboBox;
            if (control != null)
                control.SelectionChanged += OnSelectionChanged;
        }

        private static void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var control = sender as ComboBox;
            var command = GetCommand(control);

            if (command != null && command.CanExecute(e.AddedItems))
                command.Execute(e);
        }
    }

    /// <summary>
    /// TextBox TextChanged Event
    /// </summary>
    public class TextChangedCommand
    {
        public TextChangedCommand()
        {
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand),
            typeof(TextChangedCommand), new PropertyMetadata(null, OnCommandPropertyChanged));

        public static void SetCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(CommandProperty);
        }

        private static void OnCommandPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var control = d as TextBox;
            if (control != null)
                control.TextChanged += OnTextChanged;
        }

        private static void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var control = sender as TextBox;
            var command = GetCommand(control);

            if (command != null && command.CanExecute(e))
                command.Execute(e);
        }
    }

    /// <summary>
    /// TextBox KeyDown Event
    /// </summary>
    public class KeyDownCommand
    {
        public KeyDownCommand()
        {
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand),
            typeof(KeyDownCommand), new PropertyMetadata(null, OnCommandPropertyChanged));

        public static void SetCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(CommandProperty);
        }

        private static void OnCommandPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var control = d as TextBox;
            if (control != null)
                control.KeyDown += OnKeyDown;
        }

        private static void OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            var control = sender as TextBox;
            var command = GetCommand(control);

            if (command != null && command.CanExecute(e))
                command.Execute(e);
        }
    }

    /// <summary>
    /// TextBox LostFocus Event
    /// </summary>
    public class LostFocusCommand
    {
        public LostFocusCommand()
        {
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand),
            typeof(LostFocusCommand), new PropertyMetadata(null, OnCommandPropertyChanged));

        public static void SetCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(CommandProperty);
        }

        private static void OnCommandPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var control = d as TextBox;
            if (control != null)
                control.LostFocus += OnLostFocus;
        }

        private static void OnLostFocus(object sender, RoutedEventArgs e)
        {
            var control = sender as TextBox;
            var command = GetCommand(control);

            if (command != null && command.CanExecute(e))
                command.Execute(e);
        }
    }

    /// <summary>
    /// Image ImagedTapped Event
    /// </summary>
    public class ImageTappedCommand
    {
        public ImageTappedCommand()
        {
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand),
            typeof(ImageTappedCommand), new PropertyMetadata(null, OnCommandPropertyChanged));

        public static void SetCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(CommandProperty);
        }

        private static void OnCommandPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var control = d as Image;
            if (control != null)
                control.Tapped += OnImageTapped;
        }

        private static void OnImageTapped(object sender, TappedRoutedEventArgs e)
        {
            var control = sender as Image;
            var command = GetCommand(control);

            if (command != null && command.CanExecute(e))
                command.Execute(e);
        }
    }

    /// <summary>
    /// CheckBox Checked Event
    /// </summary>
    public class CheckedCommand
    {
        public CheckedCommand()
        {
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand),
            typeof(CheckedCommand), new PropertyMetadata(null, OnCommandPropertyChanged));

        public static void SetCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(CommandProperty);
        }

        private static void OnCommandPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var control = d as CheckBox;
            if (control != null)
                control.Checked += OnChecked;
        }

        private static void OnChecked(object sender, RoutedEventArgs e)
        {
            var control = sender as CheckBox;
            var command = GetCommand(control);

            if (command != null && command.CanExecute(e))
                command.Execute(e);
        }
    }

    /// <summary>
    /// CheckBox UnChecked Event
    /// </summary>
    public class UnCheckedCommand
    {
        public UnCheckedCommand()
        {
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand),
            typeof(UnCheckedCommand), new PropertyMetadata(null, OnCommandPropertyChanged));

        public static void SetCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(CommandProperty);
        }

        private static void OnCommandPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var control = d as CheckBox;
            if (control != null)
                control.Unchecked += OnUnChecked;
        }

        private static void OnUnChecked(object sender, RoutedEventArgs e)
        {
            var control = sender as CheckBox;
            var command = GetCommand(control);

            if (command != null && command.CanExecute(e))
                command.Execute(e);
        }
    }

    /// <summary>
    /// TextBlock Tapped Event
    /// </summary>
    public class TextBlockTappedCommand
    {
        public TextBlockTappedCommand()
        {
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand),
            typeof(TextBlockTappedCommand), new PropertyMetadata(null, OnCommandPropertyChanged));

        public static void SetCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(CommandProperty);
        }

        private static void OnCommandPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var control = d as TextBlock;
            if (control != null)
                control.Tapped += OnTapped;
        }

        private static void OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var control = sender as TextBlock;
            var command = GetCommand(control);

            if (command != null && command.CanExecute(e))
                command.Execute(e);
        }
    }

    /// <summary>
    /// Actions for field click event
    /// </summary>
    public class ActionCommands
    {
        static public ICommand AddressTo
        {
            get
            {
                return new RelayCommandEx<string>(async (param) =>
                {
                    if (!String.IsNullOrEmpty(param))
                    {
                        await Launcher.LaunchUriAsync(new Uri("bingmaps:?where=" + param));
                    }
                });
            }
        }

        static public ICommand MailTo
        {
            get
            {
                return new RelayCommandEx<string>(async (param) =>
                {
                    if (!String.IsNullOrEmpty(param))
                    {
                        await Launcher.LaunchUriAsync(new Uri(String.Format("mailto:{0}", param)));
                    }
                });
            }
        }

        static public ICommand CallTo
        {
            get
            {
                return new RelayCommandEx<string>(async (param) =>
                {
                    if (!String.IsNullOrEmpty(param))
                    {
                        await Launcher.LaunchUriAsync(new Uri(String.Format("CallTo:{0}", param)));
                    }
                });
            }
        }

        static public ICommand WebTo
        {
            get
            {
                return new RelayCommandEx<string>(async (param) =>
                {
                    if (!String.IsNullOrEmpty(param))
                    {
                        await Launcher.LaunchUriAsync(new Uri(param));
                    }
                });
            }
        }
    }
}
