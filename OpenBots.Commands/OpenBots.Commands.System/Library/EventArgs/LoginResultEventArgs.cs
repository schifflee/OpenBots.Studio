﻿using OpenBots.Commands.System.Library.Enums;

namespace OpenBots.Commands.System.Library.EventArgs
{
    public class LoginResultEventArgs
    {
        public LoginResultCode Result;
        public string MachineName { get; set; }
        public string AdditionalDetail { get; set; }

        public LoginResultEventArgs(string userName, LoginResultCode result, string additionalDetail)
        {
            MachineName = userName;
            Result = result;
            AdditionalDetail = additionalDetail;
        }
    }
}
