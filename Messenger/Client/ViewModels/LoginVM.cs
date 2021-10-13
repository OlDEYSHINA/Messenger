using Client.Models;
using Prism.Mvvm;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    class LoginVM : BindableBase
    {
        private string _username;
        private bool _correctUsername=false;

        private string _password;
        private bool _correctPassword=false;


        public string UsernameLogin
        {
            get
            {
                return _username;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _correctUsername = false;
                }
                else
                {
                    SetProperty(ref _username, value);
                    _correctUsername = true;
                }
            }
                
        }
       
        public string PasswordLogin
        {
            get
            {
                return _password;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _correctPassword = false;
                }
                else
                {
                    SetProperty(ref _password, value);
                    _correctPassword = true;
                }
            }
        }
        public DelegateCommand SendCommand { get; }


        public void ConfirmLogin()
        {
            if(_correctUsername & _correctPassword)
            {
                MessageBox.Show("Luck");
            }
        }


        // public event PropertyChangedEventHandler PropertyChanged;
        //protected virtual void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
        public LoginVM()
        {
            var login = new LoginModel {Username="Amogus", Password="Aboba" };
            SendCommand = new DelegateCommand(ConfirmLogin,()=> true);
        }
        
    }
}
