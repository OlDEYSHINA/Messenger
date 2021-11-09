using Common;
using System.Collections.Generic;

namespace Common.Network
{
    public class UsersStatusesRequestEventArgs
    {
        public List<UserStatus> UsersStatuses { get; set; }
        public UsersStatusesRequestEventArgs(List<UserStatus> incomeList)
        {
            UsersStatuses = incomeList;
        }
    }
}
