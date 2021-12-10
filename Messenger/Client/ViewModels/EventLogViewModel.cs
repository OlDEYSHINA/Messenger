using Client.View;
using Common.Network;
using Common.Network._EventArgs_;
using Common.Network.Messages;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Client.ViewModels
{
    public class EventLogViewModel : BindableBase
    {
        private DateTime _firstDate;
        private DateTime _secondDate;
        private string _errorString;
        private string _searchString;
        private List<EventNote> _incomeEventLog;
        private List<EventNote> incomeEventLog
        {
            get
            {
                return _incomeEventLog;
            }
            set
            {
                _incomeEventLog = value;
                SearchInEventLog();
            }
        }

        private ObservableCollection<EventNote> _eventLog;
        private ITransport _transport;
        private EventLogWindow _eventLogWindow;
        public DelegateCommand Request { get; }
        public DelegateCommand Search { get; }
        public ObservableCollection<EventNote> EventLog
        {
            get
            {
                return _eventLog;
            }
            set
            {
                SetProperty(ref _eventLog, value);
            }
        }

        public string SearchString
        {
            get
            {
                return _searchString;
            }
            set
            {
                SetProperty(ref _searchString, value);
            }
        }

        public DateTime FirstDate
        {
            get
            {
                return _firstDate;
            }
            set
            {
                SetProperty(ref _firstDate, value);
            }
        }
        public DateTime SecondDate
        {
            get
            {
                return _secondDate;
            }
            set
            {
                SetProperty(ref _secondDate, value);
            }
        }

        public string ErrorString
        {
            get
            {
                return _errorString;
            }
            set
            {
                SetProperty(ref _errorString, value);
            }
        }

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

        public void EventLogResponse(object sender, EventLogResponseEventArgs e)
        {
            _incomeEventLog = e.EventLog;
        }

        public void SearchInEventLog()
        {
            if (string.IsNullOrEmpty(_searchString))
            {
                EventLog.Clear();
                foreach (var eventNote in _incomeEventLog)
                {
                    EventLog.Add(eventNote);
                }
            }
            else
            {
                var finded = _incomeEventLog.FindAll(o => o.EventText.Contains(_searchString) & o.Login.Contains(_searchString));
                EventLog.Clear();
                foreach (var item in finded)
                {
                    EventLog.Add(item);
                }
            }
        }

        public void EventBaseCall()
        {
            if (FirstDate != DateTime.MinValue & SecondDate != DateTime.MinValue)
            {
                _transport?.EventRequest(FirstDate, SecondDate);
            }
            else
            {
                ErrorString = "Заполните даты запроса";
            }
        }
        public void OpenWindow()
        {
            _eventLogWindow.Show();
            // _eventLogWindow.Activate();
        }
    }
}
