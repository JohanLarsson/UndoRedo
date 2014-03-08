namespace UndoRedo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Vm _vm = new Vm();
        private readonly Dictionary<string, PropertyInfo> _propertyInfos;
        private readonly Stack<HistoryPoint> _history = new Stack<HistoryPoint>();
        public MainWindow()
        {
            InitializeComponent();
            _propertyInfos = _vm.GetType().GetProperties().ToDictionary(x => x.Name, x => x);
            foreach (var propertyInfo in _propertyInfos)
            {
                _history.Push(new HistoryPoint(_vm, propertyInfo.Value));
            }
            DataContext = _vm;
            var observable = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                x => _vm.PropertyChanged += x,
                x => _vm.PropertyChanged -= x);
            observable.Subscribe(x => _history.Push(new HistoryPoint((INotifyPropertyChanged)x.Sender, _propertyInfos[x.EventArgs.PropertyName])));
        }
        private void Undo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            HistoryPoint historyPoint = _history.Pop();
            historyPoint.Undo();
            _history.Pop();
        }
        private void Undo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _history.Count > _propertyInfos.Count;
        }
        private void Redo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void Redo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }
    }
}
