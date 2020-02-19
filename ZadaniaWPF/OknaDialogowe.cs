using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using ZadaniaWPF.ViewModel;

namespace ZadaniaWPF
{
	public abstract class DialogBox : FrameworkElement, INotifyPropertyChanged
	{
		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string nazwaWłasności)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(nazwaWłasności));
		}
		#endregion

		protected Action<object> execute = null;

		public string Caption { get; set; }// = null;

		protected ICommand show;

		public virtual ICommand Show
		{
			get
			{
				if (show == null) show = new RelayCommand(execute);
				return show;
			}
		}
	}

	public class SimpleMessageDialogBox : DialogBox
	{
		public SimpleMessageDialogBox()
		{
			execute =
				o =>
				{
					MessageBox.Show((string)o, Caption);
				};
		}
	}

	public abstract class CommandDialogBox : DialogBox
	{
		public override ICommand Show
		{
			get
			{
				if (show == null) show = new RelayCommand(
					o =>
					{
						ExecuteCommand(CommandBefore, CommandParameter);
						execute(o);
						ExecuteCommand(CommandAfter, CommandParameter);
					});
				return show;
			}
		}

		public static DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(CommandDialogBox));

		public object CommandParameter
		{
			get
			{
				return GetValue(CommandParameterProperty);
			}
			set
			{
				SetValue(CommandParameterProperty, value);
			}
		}

		protected static void ExecuteCommand(ICommand command, object commandParameter)
		{
			if (command != null)
				if (command.CanExecute(commandParameter))
					command.Execute(commandParameter);
		}

		public static DependencyProperty CommandBeforeProperty = DependencyProperty.Register("CommandBefore", typeof(ICommand), typeof(CommandDialogBox));

		public ICommand CommandBefore
		{
			get
			{
				return (ICommand)GetValue(CommandBeforeProperty);
			}
			set
			{
				SetValue(CommandBeforeProperty, value);
			}
		}

		public static DependencyProperty CommandAfterProperty = DependencyProperty.Register("CommandAfter", typeof(ICommand), typeof(CommandDialogBox));

		public ICommand CommandAfter
		{
			get
			{
				return (ICommand)GetValue(CommandAfterProperty);
			}
			set
			{
				SetValue(CommandAfterProperty, value);
			}
		}
	}

	public class NotificationDialogBox : CommandDialogBox
	{
		public NotificationDialogBox()
		{
			execute =
				o =>
				{
					MessageBox.Show((string)o, Caption, MessageBoxButton.OK, MessageBoxImage.Information);
				};
		}
	}

	public class MessageDialogBox : CommandDialogBox
	{
		public MessageBoxResult? LastResult { get; protected set; }
		public MessageBoxButton Buttons { get; set; }// = MessageBoxButton.OK;
		public MessageBoxImage Icon { get; set; }// = MessageBoxImage.None;

		public bool IsLastResultYes
		{
			get
			{
				if (!LastResult.HasValue) return false;
				return LastResult.Value == MessageBoxResult.Yes;
			}
		}

		public bool IsLastResultNo
		{
			get
			{
				if (!LastResult.HasValue) return false;
				return LastResult.Value == MessageBoxResult.No;
			}
		}

		public bool IsLastResultCancel
		{
			get
			{
				if (!LastResult.HasValue) return false;
				return LastResult.Value == MessageBoxResult.Cancel;
			}
		}

		public bool IsLastResultOK
		{
			get
			{
				if (!LastResult.HasValue) return false;
				return LastResult.Value == MessageBoxResult.OK;
			}
		}

		public MessageDialogBox()
		{
            Buttons = MessageBoxButton.OK;
		    Icon = MessageBoxImage.None;

			execute = o =>
			{
				LastResult = MessageBox.Show((string)o, Caption, Buttons, Icon);
				OnPropertyChanged("LastResult");				
				switch (LastResult)
				{
					case MessageBoxResult.Yes:
						OnPropertyChanged("IsLastResultYes");
						ExecuteCommand(CommandYes, CommandParameter);
						break;
					case MessageBoxResult.No:
						OnPropertyChanged("IsLastResultNo");
						ExecuteCommand(CommandNo, CommandParameter);
						break;
					case MessageBoxResult.Cancel:
						OnPropertyChanged("IsLastResultCancel");
						ExecuteCommand(CommandCancel, CommandParameter);
						break;
					case MessageBoxResult.OK:
						OnPropertyChanged("IsLastResultOK");
						ExecuteCommand(CommandOK, CommandParameter);
						break;
				}
			};
		}

		public static DependencyProperty CommandYesProperty = DependencyProperty.Register("CommandYes", typeof(ICommand), typeof(MessageDialogBox));
		public static DependencyProperty CommandNoProperty = DependencyProperty.Register("CommandNo", typeof(ICommand), typeof(MessageDialogBox));
		public static DependencyProperty CommandCancelProperty = DependencyProperty.Register("CommandCancel", typeof(ICommand), typeof(MessageDialogBox));
		public static DependencyProperty CommandOKProperty = DependencyProperty.Register("CommandOK", typeof(ICommand), typeof(MessageDialogBox));

		public ICommand CommandYes
		{
			get
			{
				return (ICommand)GetValue(CommandYesProperty);
			}
			set
			{
				SetValue(CommandYesProperty, value);
			}
		}

		public ICommand CommandNo
		{
			get
			{
				return (ICommand)GetValue(CommandNoProperty);
			}
			set
			{
				SetValue(CommandNoProperty, value);
			}
		}

		public ICommand CommandCancel
		{
			get
			{
				return (ICommand)GetValue(CommandCancelProperty);
			}
			set
			{
				SetValue(CommandCancelProperty, value);
			}
		}

		public ICommand CommandOK
		{
			get
			{
				return (ICommand)GetValue(CommandOKProperty);
			}
			set
			{
				SetValue(CommandOKProperty, value);
			}
		}
	}

	public class ConditionalMessageDialogBox : MessageDialogBox
	{
		public static DependencyProperty IsDialogBypassedProperty = DependencyProperty.Register("IsDialogBypassed", typeof(bool), typeof(ConditionalMessageDialogBox));
		public bool IsDialogBypassed
		{
			get
			{
				return (bool)GetValue(IsDialogBypassedProperty);
			}
			set
			{
				SetValue(IsDialogBypassedProperty, value);
			}
		}

		public MessageBoxResult DialogBypassButton { get; set; }// = MessageBoxResult.None;

		public ConditionalMessageDialogBox()
		{
            DialogBypassButton = MessageBoxResult.None;

			execute = o =>
			{
				MessageBoxResult result;
				if (!IsDialogBypassed)
				{
					LastResult = MessageBox.Show((string)o, Caption, Buttons, Icon);
					OnPropertyChanged("LastResult");
					result = LastResult.Value;
				}
				else
				{
					result = DialogBypassButton;
				}
				switch (result)
				{
					case MessageBoxResult.Yes:
						if(!IsDialogBypassed) OnPropertyChanged("IsLastResultYes");
						ExecuteCommand(CommandYes, CommandParameter);
						break;
					case MessageBoxResult.No:
						if (!IsDialogBypassed) OnPropertyChanged("IsLastResultNo");
						ExecuteCommand(CommandNo, CommandParameter);
						break;
					case MessageBoxResult.Cancel:
						if (!IsDialogBypassed) OnPropertyChanged("IsLastResultCancel");
						ExecuteCommand(CommandCancel, CommandParameter);
						break;
					case MessageBoxResult.OK:
						if (!IsDialogBypassed) OnPropertyChanged("IsLastResultOK");
						ExecuteCommand(CommandOK, CommandParameter);
						break;
				}
			};
		}
	}


	[ContentProperty("WindowContent")]
	public class CustomContentDialogBox : CommandDialogBox
	{
		bool? LastResult;

		public double WindowWidth { get; set; }// = 640;
		public double WindowHeight { get; set; }// = 480;		
		public object WindowContent { get; set; }// = null;

		private static Window window = null;

		public CustomContentDialogBox()
		{			
            WindowWidth = 640;
		    WindowHeight = 480;		

			execute =
				o =>
				{
					if (window == null)
					{
						window = new Window();
						window.Width = WindowWidth;
						window.Height = WindowHeight;
						if (Caption != null) window.Title = Caption;
						window.Content = WindowContent;						
						LastResult = window.ShowDialog();
						OnPropertyChanged("LastResult");
						window = null;
						switch (LastResult)
						{
							case true:								
								ExecuteCommand(CommandTrue, CommandParameter);
								break;
							case false:								
								ExecuteCommand(CommandFalse, CommandParameter);
								break;
							case null:
								ExecuteCommand(CommandNull, CommandParameter);
								break;
						}
					}
				};
		}

		public static bool? GetCustomContentDialogResult(DependencyObject d)
		{
			return (bool?)d.GetValue(DialogResultProperty);
		}

		public static void SetCustomContentDialogResult(DependencyObject d, bool? value)
		{
			d.SetValue(DialogResultProperty, value);
		}

		public static readonly DependencyProperty DialogResultProperty =
			DependencyProperty.RegisterAttached(
				"DialogResult",
				typeof(bool?),
				typeof(CustomContentDialogBox),
				new PropertyMetadata(null, DialogResultChanged));

		private static void DialogResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			bool? dialogResult = (bool?)e.NewValue;
			if (d is Button)
			{
				Button button = d as Button;
				button.Click += (object sender, RoutedEventArgs _e) => { window.DialogResult = dialogResult; };
			}
		}

		public static DependencyProperty CommandTrueProperty = DependencyProperty.Register("CommandTrue", typeof(ICommand), typeof(CustomContentDialogBox));
		public static DependencyProperty CommandFalseProperty = DependencyProperty.Register("CommandFalse", typeof(ICommand), typeof(CustomContentDialogBox));
		public static DependencyProperty CommandNullProperty = DependencyProperty.Register("CommandNull", typeof(ICommand), typeof(CustomContentDialogBox));		

		public ICommand CommandTrue
		{
			get
			{
				return (ICommand)GetValue(CommandTrueProperty);
			}
			set
			{
				SetValue(CommandTrueProperty, value);
			}
		}

		public ICommand CommandFalse
		{
			get
			{
				return (ICommand)GetValue(CommandFalseProperty);
			}
			set
			{
				SetValue(CommandFalseProperty, value);
			}
		}

		public ICommand CommandNull
		{
			get
			{
				return (ICommand)GetValue(CommandNullProperty);
			}
			set
			{
				SetValue(CommandNullProperty, value);
			}
		}
	}
}
