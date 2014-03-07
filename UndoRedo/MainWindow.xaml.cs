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
            DataContext = _vm;
            var observable = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                x => _vm.PropertyChanged += x,
                x => _vm.PropertyChanged -= x);
            observable.Subscribe(x => _history.Push(new HistoryPoint(x.Sender,_propertyInfos[x.EventArgs.PropertyName])))
        }

    }
}
