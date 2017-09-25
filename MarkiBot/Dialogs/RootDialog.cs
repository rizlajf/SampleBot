using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using MultiDialogsBot.Dialogs;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Bot.Builder.FormFlow;

namespace MarkiBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            //return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            if (activity.Text.ToLower().Contains("hi") || activity.Text.ToLower().Contains("hello"))
            {
                await context.PostAsync("Hello how can I help you?");
            }
            else
            {
                if (activity.Text.ToLower().Contains("show") || activity.Text.ToLower().Contains("product") || activity.Text.ToLower().Contains("find"))
                {
                    await context.PostAsync("Sure.");
                    await context.Forward(new ProductDialog(), this.ResumeAfterProductDialog, activity, CancellationToken.None);
                }
                else if (activity.Text.ToLower().Contains("Thank"))
                {
                    await context.PostAsync("You are wellcome.");
                }
                else if (activity.Text.ToLower().Contains("hey"))
                {
                    await context.PostAsync("Yes. what do you want me to do for you?");
                }
                else if (activity.Text.ToLower().Contains("amplifiers"))
                {
                    await RedirectToproductContentGenerator(context, "Amplifiers");                    
                }
                else if (activity.Text.ToLower().Contains("balun"))
                {
                    await RedirectToproductContentGenerator(context, "Balun");
                }
                else
                {
                    await context.PostAsync("Sorry I could not understand your request, By the way I can help you out to search any products from Marki Microwave");
                    PromptDialog.Choice(context, this.NextQuestionAsync, new List<string>() { "Yes", "No" }, "Please let me know if you want me to help you out to search any product.");
                    //List<string> questions = new List<string>();
                    //questions.Add("Yes"); // Added yes option to prompt
                    //questions.Add("No"); // Added no option to prompt
                    //string QuestionPrompt = "Please let me know if you want me to help you out to search any product.";
                    //PromptOptions<string> options = new PromptOptions<string>(QuestionPrompt, "", "", questions, 1); // Overrided the PromptOptions Constructor.
                    //PromptDialog.Choice<string>(context, NextQuestionAsync, options);
                }

            }
            //context.Wait(MessageReceivedAsync);
        }

        public async Task NextQuestionAsync(IDialogContext context, IAwaitable<string> result)
        {
            string optionSelected = await result;
            try
            {
                if (optionSelected.ToString() == "Yes")
                {
                    await context.Forward(new ProductDialog(), this.ResumeAfterProductDialog, null, CancellationToken.None);
                    //context.Call(new ProductDialog(), this.ResumeAfterProductDialog);
                    //context.Wait(NoneIntent);
                }
                else
                {
                    await context.PostAsync("You Said No");
                    await context.PostAsync("Thank you. Please feel free to seek any help from me if you need. Have a nice day!!");
                    context.Done<string>(null);
                }
            }
            catch (Exception e)
            {

            }
        }

        private async Task ResumeAfterProductDialog(IDialogContext context, IAwaitable<int> result)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task RedirectToproductContentGenerator(IDialogContext context, string product)
        {
            string category = product;
            var resultMessage = context.MakeMessage();
            resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            resultMessage.Attachments = new List<Attachment>();

            List<HeroCard> herocardList = null;
            ProductContentClass pc = new ProductContentClass();

            switch (product.ToString())
            {
                case "Amplifiers":
                    herocardList = pc.GenerateAmplifiersContent();
                    break;
                case "Balun":
                    herocardList = pc.GenerateBalunContent();
                    break;
                case "Bias Tees":

                    break;
                case "Couplers":

                    break;
                case "Equalizers":

                    break;
            }
            foreach (HeroCard hc in herocardList)
            {
                resultMessage.Attachments.Add(hc.ToAttachment());
            }
            await context.PostAsync(resultMessage);
            context.Wait(this.MessageReceivedAsync);
        }

    }
}