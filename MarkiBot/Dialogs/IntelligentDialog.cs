using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using MultiDialogsBot.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MarkiBot.Dialogs
{
    [Serializable]
    [LuisModel(Constants.LUIS_EMPLOYEE_HELPER_APP_ID, Constants.LUIS_SUBSCRIPTION_KEY)]
    public class IntelligentDialog : LuisDialog<object>
    {
        private string userName;
        private DateTime msgReceivedDate;
        public IntelligentDialog(Activity activity)
        {
            userName = activity.From.Name;
            msgReceivedDate = activity.Timestamp?? DateTime.Now;
        }
        [LuisIntent("")]
        [LuisIntent("none")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult luisResult)
        {
            await context.PostAsync("Sorry I could not understand your request, By the way I can help you out to search any products from Marki Microwave");
            var dialog = new RootDialog();
            PromptDialog.Choice(context, dialog.NextQuestionAsync, new List<string>() { "Yes", "No" }, "Please let me know if you want me to help you out to search any product.");
            context.Wait(this.MessageReceived);
        }
        [LuisIntent("Greet.Welcome")]
        public async Task GreetWelcome(IDialogContext context, LuisResult luisResult)
        {
            string response = string.Empty;
            if (this.msgReceivedDate.ToString("tt") == "AM")
            {
                response = $"Good morning, {userName}. :)";
            }
            else
            {
                response = $"Hey {userName}. :)";
            }
            await context.PostAsync(response);
            context.Wait(this.MessageReceived);
        }
        [LuisIntent("Greet.Farewell")]
        public async Task GreetFarewell(IDialogContext context, LuisResult luisResult)
        {
            string response = string.Empty;
            if (this.msgReceivedDate.ToString("tt") == "AM")
            {
                response = $"Good bye, {userName}.. Have a nice day. :)";
            }
            else
            {
                response = $"b'bye {userName}, Take care.";
            }
            await context.PostAsync(response);
            context.Wait(this.MessageReceived);
        }
        [LuisIntent("Search")]
        public async Task SearchProduct(IDialogContext context, LuisResult luisResult)
        {            
            //var dialog = new RootDialog();
            //await context.Forward(new ProductDialog(), dialog.ResumeAfterProductDialog, null, CancellationToken.None);
            foreach (EntityRecommendation curEntity in luisResult.Entities)
            {
                //await dialog.RedirectToproductContentGenerator(context, curEntity.Entity);
                
                var resultMessage = context.MakeMessage();
                resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                resultMessage.Attachments = new List<Attachment>();

                List<HeroCard> herocardList = null;
                ProductContentClass pc = new ProductContentClass();

                switch (curEntity.Entity.ToLower().ToString())
                {
                    case "amplifier":
                        herocardList = pc.GenerateAmplifiersContent();
                        break;
                    case "balun":
                        herocardList = pc.GenerateBalunContent();
                        break;
                    case "bias tees":

                        break;
                    case "coupler":

                        break;
                    case "equalizer":

                        break;
                    default:
                        herocardList = null;
                        break;
                }
                if (herocardList != null)
                {
                    await context.PostAsync("Collecting informstions on " + curEntity.Entity);
                    foreach (HeroCard hc in herocardList)
                    {
                        resultMessage.Attachments.Add(hc.ToAttachment());
                    }
                    await context.PostAsync(resultMessage);
                    context.Wait(this.MessageReceived);
                }
                else
                    continue;
            }
        }
    }
}