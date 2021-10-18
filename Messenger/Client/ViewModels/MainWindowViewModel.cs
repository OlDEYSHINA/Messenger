﻿using Prism.Commands;
using Prism.Mvvm;
using System.Threading;
using System.Windows;

namespace Client.ViewModels
{
    class MainWindowViewModel : BindableBase
    {

        object _currentContentVM;
        public object CurrentContentVM
        {
            get
            {
                return _currentContentVM;
            }
            set
            {
                SetProperty(ref _currentContentVM, value);
            }
        }
        private string _outputView;
        public string OutputView
        {
            get
            {
                return _outputView;
            }
            set
            {
                SetProperty(ref _outputView, value);
            }
        }
        /// <summary>
        /// Выбор отображения в главном окне
        /// </summary>
        public enum ViewType
        {
            Login,
            Registration,
            Chat
        }

        public void ChangeView(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Login:
                    {
                        OutputView = "LoginView";
                        break;
                    }
                case ViewType.Chat:
                    {
                        CurrentContentVM = chatViewModel;
                        break;
                    }
            }
        }
        ChatViewModel chatViewModel = new ChatViewModel();
        LoginVM loginVM;
        public MainWindowViewModel()
        {
            loginVM = new LoginVM(this);
           CurrentContentVM = loginVM;
            //MainWindowViewModel & Aboba = (*MainWindowViewModel);
        }
    }
}