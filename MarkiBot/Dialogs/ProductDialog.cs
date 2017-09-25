namespace MultiDialogsBot.Dialogs
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Http;
    using System.Configuration;
    using MarkiBot;

    [Serializable]
    public class ProductDialog : IDialog<int>
    {
        public async Task StartAsync(IDialogContext context)
        {            
            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            //PromptDialog.Choice(context, this.OnFirstOptionSelected, new List<string>() { "Direct Me to the product web page", "Show me the products" }, "How do you want me to give the product details?", "Not a valid option", 3); 
            PromptDialog.Choice(context, this.OnShowOptionSelected, new List<string>() { "Amplifiers", "Balun", "Bias Tees", "Couplers", "Equalizers" }, "Which Product category are you looking to buy?", "Not a valid option", 3);
        }

        private async Task OnFirstOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            string firstOptionSelected = await result;
            try
            {
                switch (firstOptionSelected)
                {
                    case "Direct Me to the product web page":
                        PromptDialog.Choice(context, this.OnWebPageOptionSelected, new List<string>() { "Amplifiers", "Balun", "Bias Tees", "Couplers", "Equalizers" }, "Which Product category are you looking to buy?", "Not a valid option", 3);
                        break;

                    case "Show me the products":
                        PromptDialog.Choice(context, this.OnShowOptionSelected, new List<string>() { "Amplifiers", "Balun", "Bias Tees", "Couplers", "Equalizers" }, "Which Product category are you looking to buy?", "Not a valid option", 3);
                        break;
                }               
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");

                context.Wait(this.MessageReceivedAsync);
            }
            finally
            {
                PromptDialog.Choice(context, this.MoreQuestionAsync, new List<string>() { "Yes", "No" }, "Is anything else I can help you out?");
                //context.Wait(this.MessageReceivedAsync);                
            }
        }

        private async Task OnWebPageOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                var resultMessage = context.MakeMessage();
                resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                resultMessage.Attachments = new List<Attachment>();

                string optionSelected = await result;
                HeroCard heroCard = null;

                switch (optionSelected)
                {
                    case "Amplifiers":
                         heroCard = new HeroCard()
                            {
                                Title = "Amplifiers",                             
                                Buttons = new List<CardAction>()
                                {
                                    new CardAction()
                                    {
                                        Title = "More details",
                                        Type = ActionTypes.OpenUrl,
                                        Value = "http://dev.markimicrowave.com/amplifiers/amplifiers-products.aspx"
                                    }
                                }
                            };  
                        break;

                    case "Balun":
                         heroCard = new HeroCard()
                        {
                            Title = "Balun",
                            Buttons = new List<CardAction>()
                                {
                                    new CardAction()
                                    {
                                        Title = "More details",
                                        Type = ActionTypes.OpenUrl,
                                        Value = "http://dev.markimicrowave.com/baluns/baluns-products.aspx"
                                    }
                                }
                        };
                        break;
                    case "Bias Tees":
                        heroCard = new HeroCard()
                        {
                            Title = "Bias Tees",
                            Buttons = new List<CardAction>()
                                {
                                    new CardAction()
                                    {
                                        Title = "More details",
                                        Type = ActionTypes.OpenUrl,
                                        Value = "http://dev.markimicrowave.com/bias-tees/bias-tees-products.aspx"
                                    }
                                }
                        };
                        break;
                    case "Couplers":
                        heroCard = new HeroCard()
                        {
                            Title = "Couplers",
                            Buttons = new List<CardAction>()
                                {
                                    new CardAction()
                                    {
                                        Title = "More details",
                                        Type = ActionTypes.OpenUrl,
                                        Value = "http://dev.markimicrowave.com/couplers/couplers-products.aspx"
                                    }
                                }
                        };
                        break;
                    case "Equalizers":
                        heroCard = new HeroCard()
                        {
                            Title = "Equalizers",
                            Buttons = new List<CardAction>()
                                {
                                    new CardAction()
                                    {
                                        Title = "More details",
                                        Type = ActionTypes.OpenUrl,
                                        Value = "http://dev.markimicrowave.com/equalizers/equalizers-products.aspx"
                                    }
                                }
                        };
                        break;
                }
                resultMessage.Attachments.Add(heroCard.ToAttachment());
                await context.PostAsync(resultMessage);
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");

                context.Wait(this.MessageReceivedAsync);
            }
            finally
            {
                PromptDialog.Choice(context, this.MoreQuestionAsync, new List<string>() { "Yes", "No" }, "Is anything else I can help you out?");                
                //context.Wait(this.MessageReceivedAsync);                
            }
        }

        private async Task OnShowOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            // Construct a base URL for Image
            // To allow it to be found wherever the application is deployed
            string strCurrentURL = ConfigurationManager.AppSettings["strCurrentURL"];
            try
            {
                var resultMessage = context.MakeMessage();
                resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                resultMessage.Attachments = new List<Attachment>();

                string optionSelected = await result;
                List<HeroCard> herocardList = null;
                ProductContentClass pc = new ProductContentClass();
               
                switch (optionSelected)
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
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");

                context.Wait(this.MessageReceivedAsync);
            }
            finally
            {
                PromptDialog.Choice(context, this.MoreQuestionAsync, new List<string>() { "Yes", "No" }, "Is anything else I can help you out?");
                //context.Wait(this.MessageReceivedAsync);                
            }
        }

        public async Task MoreQuestionAsync(IDialogContext context, IAwaitable<string> result)
        {
            string selectedOption = await result;
            try
            {
                if (selectedOption.ToString() == "Yes")
                {
                    await context.PostAsync("What can I do for you?");
                }
                else
                {
                    await context.PostAsync("Thank you. Please feel free to seek any help from me if you need. Have a nice day!!");
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                context.Done<string>(null);
            }
        }


    }
}