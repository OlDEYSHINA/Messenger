using Common;
using System.Collections.Generic;

namespace Common.Network
{
    public class UsersStatusesReceivedEventArgs
    {
        public List<UserState> UsersStatuses { get; set; }
        public UsersStatusesReceivedEventArgs(List<UserState> incomeList)
        {
            UsersStatuses = incomeList;
        }
    }
}
