// ---------------------------------------------------------------------------------------------------------------------------------------------------
// Copyright ElcomPlus LLC. All rights reserved.
// Author: Пальников М. С.
// ---------------------------------------------------------------------------------------------------------------------------------------------------

namespace Common.Network.Packets
{
    public enum PacketId : byte
    {
        ConnectionRequest = 0x01,
        ConnectionResponse = 0x02,
        MessageRequest = 0x03,
        MessageBroadcast = 0x04,
        UserStatusBroadcast = 0x05,
        UserStatusChangeBroadcast = 0x06,
    }
}
