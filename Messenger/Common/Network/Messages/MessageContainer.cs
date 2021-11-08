// ---------------------------------------------------------------------------------------------------------------------------------------------------
// Copyright ElcomPlus LLC. All rights reserved.
// Author: Пальников М. С.
// ---------------------------------------------------------------------------------------------------------------------------------------------------

namespace Common.Network.Messages
{
    public class MessageContainer
    {
        #region Properties

        public string Identifier { get; set; }

        public object Payload { get; set; }

        #endregion Properties
    }
}