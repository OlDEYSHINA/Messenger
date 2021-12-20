namespace Client.ViewModels
{
    using Common.Network;
    using Common.Network.Messages;
    using Prism.Commands;
    using Prism.Mvvm;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
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
            set
            {
                if (string.IsNullOrEmpty(value) || value?.Length < 30)
                {
                    SetProperty(ref _searchString, value);
                }
            }
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
            if (_incomeEventLog == null)
            {
                return;
            }

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
                List<EventNote> eventNotes = _incomeEventLog.FindAll(o => o.EventText.Contains(_searchString) || o.Login.Contains(_searchString));

                if (EventLog != null)
                {
                    Application.Current.Dispatcher.Invoke(() => EventLog.Clear());
                }

                foreach (EventNote item in eventNotes)
                {
                    Application.Current.Dispatcher.Invoke(() => EventLog?.Add(item));
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
        }

        #endregion
    }
}
