using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace ChatRoom
{
    public class ContentNotificationsHub : Hub
    {
        public string SignalRNotificationGroup = "portal_{0}_notification_group";
       
        
        public void Send(string name, string message)
        {
            Clients.All.receieveNotification(new ContentNotificationsClientDto
            {
                Guid = Guid.NewGuid(),
                Message = message,
                State = NotificationState.Success,
                PortalId = 978,
                NodeId = 15
            });


            //Clients.Groups(groups).receieveNotification(new ContentNotificationsClientDto
            //{
            //    Message = model.Message,
            //    Guid = model.Guid,
            //    State = model.State,
            //    PortalId = model.PortalId,
            //    NodeId = model.NodeId
            //});


            //// Call the broadcastMessage method to update clients.
            //Clients.All.broadcastMessage(name, message);
        }


        private void JoinGroup()
        {
           var portalId = 978;
            // Not awaiting Groups.Add https://docs.microsoft.com/en-us/aspnet/signalr/overview/guide-to-the-api/working-with-groups "in this example, the await keyword is not applied before Groups.Add because a message is not immediately sent to members of the group"
            Groups.Add(Context.ConnectionId, string.Format(SignalRNotificationGroup, portalId));
            //if (Context.User.IsSystemAdministrator())
            //{
            //    Groups.Add(Context.ConnectionId, string.Format(Constants.SignalRSysAdminNotificationGroup));
            //}
        }


        private async Task ResendMessages()
        {
            //var ongoingGuids = await _cache.SetMembersAsync<Guid>(Constants.CacheKeyForSignalROngoingMessagesSet);
            //var ongoingGuidsList = ongoingGuids as IList<Guid> ?? ongoingGuids.ToList();
            //if (!ongoingGuidsList.Any())
            //{
            //    return;
            //}
            //var ongoingKeys = ongoingGuidsList.Select(guid => string.Format(Constants.CacheKeyForSignalROngoingMessages, guid));
            //var ongoing = await _cache.GetAsync<ContentNotificationsDto>(ongoingKeys.ToArray());
            //var ongoingList = ongoing as IList<ContentNotificationsDto> ?? ongoing.ToList();
            //if (!ongoingList.Any())
            //{
            //    return;
            //}
            //var portalId = Context.User.GetCurrentPortalId();
            //var isSystemAdmin = Context.User.IsSystemAdministrator();
            //foreach (var data in ongoingList.Where(data => isSystemAdmin || data.PortalId == portalId))
            //{
            //await Clients.Caller.receieveNotification(new ContentNotificationsClientDto
            //{
            //    Guid = data.Guid,
            //    Message = data.Message,
            //    State = data.State,
            //    PortalId = data.PortalId,
            //    NodeId = data.NodeId
            //});
            //   }
            await Clients.Caller.receieveNotification(new ContentNotificationsClientDto
            {
                Guid = Guid.NewGuid(),
                Message = "Message",
                State = NotificationState.Success,
                PortalId = 978,
                NodeId = 15
            });
        }

        //automatically join groups when client first connects
        public override async Task OnConnected()
        {
            try
            {
                JoinGroup();
                       await ResendMessages();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            await base.OnConnected();
        }

        //rejoin groups if client disconnects and then reconnects
        public override async Task OnReconnected()
        {
            try
            {
                JoinGroup();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            await base.OnReconnected();
        }
    }
    public class ContentNotificationsClientDto
    {
        public Guid Guid { get; set; }
        public string Message { get; set; }
        public NotificationState State { get; set; }
        public int PortalId { get; set; }
        public int NodeId { get; set; }
    }

    public enum NotificationState
    {
        Queued = 1,
        Processing,
        Success,
        Error,
        NotFound,
        NoAccessToPortal,
        NoAccess,
        NoTranslations,
        Conflict
    }
}