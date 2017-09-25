using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Configuration;

namespace MarkiBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {            

            if (activity.Type == ActivityTypes.Message)
            {
                //Implementation of typing indication
                //ConnectorClient connector = new ConnectorClient(new System.Uri(activity.ServiceUrl));
                //Activity isTypingReply = activity.CreateReply("Shuttlebot is typing...");
                //isTypingReply.Type = ActivityTypes.Typing;
                //await connector.Conversations.ReplyToActivityAsync(isTypingReply);

                await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Construct a base URL for Image
                // To allow it to be found wherever the application is deployed
                //string strCurrentURL = this.Url.Request.RequestUri.AbsoluteUri.Replace(@"api/messages", "");
                //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); // Add an Application Setting.
                //config.AppSettings.Settings.Add("strCurrentURL", strCurrentURL);
                //// Save the changes in App.config file.
                //config.Save(ConfigurationSaveMode.Modified);
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
                // Handle knowing tha the user is typing
                ConnectorClient connector = new ConnectorClient(new System.Uri(message.ServiceUrl));
                Activity reply = message.CreateReply("You are typing");
                connector.Conversations.ReplyToActivityAsync(reply);
            }
            else if (message.Type == ActivityTypes.Ping)
            {
                ConnectorClient connector = new ConnectorClient(new System.Uri(message.ServiceUrl));
                Activity reply = message.CreateReply("Hello PING. Please reply");
                connector.Conversations.ReplyToActivityAsync(reply);
            }

            return null;
        }
    }
}