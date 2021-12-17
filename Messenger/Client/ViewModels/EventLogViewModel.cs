namespace Client.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;

    using Common.Network;
    using Common.Network.Messages;

    using Prism.Commands;
    using Prism.Mvvm;

    using View;

    public class EventLogViewModel : BindableBase
    {
        #region Fields

        private DateTime _firstDate;
        private DateTime _secondDate;
        private string _errorString;
        private string _searchString;
        private List<EventNote> _incomeEventLog;

        private ObservableCollection<EventNote> _eventLog;
        private readonly ITransport _transport;
        private readonly EventLogWindow _eventLogWindow;

        #endregion

        #region Properties

        public DelegateCommand Request { get; }

        public DelegateCommand Search { get; }

        public ObservableCollection<EventNote> EventLog
        {
            get => _eventLog;
            set => SetProperty(ref _eventLog, value);
        }

        public string SearchString
        {
            get => _searchString;
            set => SetProperty(ref _searchString, value);
        }

        public DateTime FirstDate
        {
            get => _firstDate;
            set => SetProperty(ref _firstDate, value);
        }

        public DateTime SecondDate
        {
            get => _secondDate;
            set => SetProperty(ref _secondDate, value);
        }

        public string ErrorString
        {
            get => _errorString;
            set => SetProperty(ref _errorString, value);
        }

        private List<EventNote> incomeEventLog
        {
            get => _incomeEventLog;
            set
            {
                _incomeEventLog = value;
                SearchInEventLog();
            }
        }

        #endregion

        #region Constructors

        public EventLogViewModel(ITransport transport)
        {
            _transport = transport;
            _transport.EventLogResponse += EventLogResponse;
            _eventLogWindow = new EventLogWindow(this);
            FirstDate = DateTime.Now;
            SecondDate = DateTime.Now;
            Request = new DelegateCommand(EventBaseCall, () => true);
            Search = new DelegateCommand(SearchInEventLog, () => true);
            EventLog = new ObservableCollection<EventNote>();
        }

        #endregion

        #region Methods

        public void EventLogResponse(object sender, EventLogResponseEventArgs e)
        {
            _incomeEventLog = e.EventLog;
            SearchInEventLog();
        }

        public void SearchInEventLog()
        {
            if (string.IsNullOrEmpty(_searchString))
            {
                if (EventLog != null)
                {
                    Application.Current.Dispatcher.Invoke(() => EventLog.Clear());
                }

                foreach (EventNote eventNote in _incomeEventLog)
                {
                    Application.Current.Dispatcher.Invoke(() => EventLog.Add(eventNote));
                }
            }
            else
            {
                List<EventNote> finded = _incomeEventLog.FindAll(o => o.EventText.Contains(_searchString) || o.Login.Contains(_searchString));

                if (EventLog != null)
                {
                    EventLog.Clear();
                }

                foreach (EventNote item in finded)
                {
                    EventLog.Add(item);
                }
            }
        }

        public void EventBaseCall()
        {
            DateTime firstDate = FirstDate.Date;
            DateTime secondDate = SecondDate.Date;
            secondDate = secondDate.AddDays(1);

            if ((firstDate != DateTime.MinValue) & (secondDate != DateTime.MinValue) && firstDate < secondDate)
            {
                _transport?.EventRequest(firstDate, secondDate);
            }
            else
            {
                ErrorString = "Некорретный выбор временного промежутка";
            }
        }

        public void OpenWindow()
        {
            _eventLogWindow.Show();
            // _eventLogWindow.Activate();
        }

        #endregion
    }
}
