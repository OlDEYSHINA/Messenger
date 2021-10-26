﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    class RegistrationModel:IRegistrationModel
    {
        private string _username;
        private string _password;
        private string _eMail;

        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
            }
        }
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value; 
            }
        }
        public string EMail
        {
            get
            {
                return _eMail;
            }
            set
            {
                _eMail = value;
            }
        }
    }
}
