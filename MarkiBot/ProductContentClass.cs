using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MarkiBot
{
    public class ProductContentClass
    {
        // Construct a base URL for Image
        // To allow it to be found wherever the application is deployed
        string strCurrentURL = ConfigurationManager.AppSettings["strCurrentURL"];
        HeroCard heroCard = null;
        List<HeroCard> heroCardList = new List<HeroCard>();

        public List<HeroCard> GenerateAmplifiersContent()
        {
            // Full URL to the image
            string lodriversurfacemounts = String.Format(@"{0}/{1}", strCurrentURL, "Images/amp_lo_driver_surface_mounts_adm-0026-5929sm_crop.png");
            // Create a CardImage and add our image
            List<CardImage> cardImages1 = new List<CardImage>();
            cardImages1.Add(new CardImage(url: lodriversurfacemounts));
            // Create a CardAction to make the HeroCard clickable
            // Note this does not work in some Skype clients
            //CardAction btnAiHelpWebsite = new CardAction()
            //{
            //    Type = "openUrl",
            //    Title = "AiHelpWebsite.com",
            //    Value = "http://AiHelpWebsite.com"
            //};
            // Finally create the Hero Card
            // adding the image and the CardAction
            heroCard = new HeroCard()
            {
                Title = "LO Driver Surface Mount",
                Subtitle = "Surface mount LO driver amplifiers are designed to function as the final output stage of the LO signal chain. Marki LO amplifiers provide a high power, fast rise time signal for driving a mixer’s LO port. Surface mount LO driver amplifiers  simplify system design by providing sufficient LO power to achieve the desired mixer spurious suppression, IP3, and other critical non-linear specifications. All ADM series amplifiers are available in a standard QFN package with broadband die versions available upon request.",
                Images = cardImages1
            };
            heroCardList.Add(heroCard);

            string lodrivermodules = String.Format(@"{0}/{1}", strCurrentURL, "Images/amp_lo_driver_module_adm1-0026pa.png");
            List<CardImage> cardImages2 = new List<CardImage>();
            cardImages2.Add(new CardImage(url: lodrivermodules));
            heroCard = new HeroCard()
            {
                Title = "LO Driver Modules",
                Subtitle = "Connectorized LO driver amplifier modules are designed to function as the final output stage of the LO signal chain in a coax connectorized package built ready for installation. Marki LO amplifiers provide a high power, fast rise time signal for driving a mixer’s LO port while providing a good impedance match. Connectorized LO driver amplifiers provide power up through millimeter wave frequencies and are unconditionally stable. LO driver amplifiers provide sufficient LO power to achieve the desired mixer spurious suppression, IP3, and other critical non-linear specifications. All ADM series modules are built using wirebonded amplifier onto a PWB with the bare die versions being available upon request.",
                Images = cardImages2
            };
            heroCardList.Add(heroCard);

            string legacy = String.Format(@"{0}/{1}", strCurrentURL, "Images/amplifiers-legacy-hero-image.png");
            List<CardImage> cardImages3 = new List<CardImage>();
            cardImages3.Add(new CardImage(url: legacy));
            heroCard = new HeroCard()
            {
                Title = "Legacy",
                Subtitle = "Legacy amplifiers are not recommended for new designs due to better alternatives being available. See the LO Driver Module tab for recommended products and replacement amplifiers.",
                Images = cardImages3
            };
            heroCardList.Add(heroCard);

            return heroCardList;
        }

        public List<HeroCard> GenerateBalunContent()
        {
            string testandmeasurement = String.Format(@"{0}/{1}", strCurrentURL, "Images/Baluns/preview-full-Baluns-Test-Measurement-Hero-Image-BAL-0067-300x169.png");
            List<CardImage> testandmeasurementcardImages1 = new List<CardImage>();
            testandmeasurementcardImages1.Add(new CardImage(url: testandmeasurement));
            heroCard = new HeroCard()
            {
                Title = "Test & Measurement",
                Subtitle = "Test and measurement baluns are connectorized modules that can be used in laboratory settings or built into temporary or permanent test sets. They are hand tuned to offer superior amplitude balance, phase balance, and common mode rejection ratio up to 67 GHz. Important specifications to consider when selecting a test and measurement balun include frequency coverage, excess insertion loss, and isolation between balanced ports. Data applications require a balun with frequency coverage to low frequencies, typically below 1 MHz. For converting a differential signal to a single ended signal it is best to use a balun with isolation, which is functionally equivalent to a 180˚ hybrid. For more information on how these specifications can affect system performance, consult our balun primer and tech notes.",
                Images = testandmeasurementcardImages1
            };
            heroCardList.Add(heroCard);

            string surfacemounts = String.Format(@"{0}/{1}", strCurrentURL, "Images/Baluns/preview-full-Baluns-Surface-Mounts-Hero-Image-BAL-0006SMG.png");
            List<CardImage> surfacemountsImages1 = new List<CardImage>();
            surfacemountsImages1.Add(new CardImage(url: surfacemounts));
            heroCard = new HeroCard()
            {
                Title = "Surface Mounts",
                Subtitle = "Our surface mount baluns offer the best phase balance, amplitude balance, and common mode rejection over the widest bandwidths of any product commercially available. These surface mount baluns are commonly used to interface to analog to digital converters, digital to analog converters, and used in differential cable test sets.Unlike competing transformer baluns, our broadband baluns use a unique architecture combined with sophisticated and proprietary assembly techniques to provide the highest performance balun on the market.For applications above 1 GHz Marki also offers lower cost, capacitively - coupled baluns.",
                Images = surfacemountsImages1
            };
            heroCardList.Add(heroCard);

            string inverters = String.Format(@"{0}/{1}", strCurrentURL, "Images/Baluns/preview-full-Baluns-Inverters-Hero-Image-INV-0040.png");
            List<CardImage> inverterscardImages1 = new List<CardImage>();
            inverterscardImages1.Add(new CardImage(url: inverters));
            heroCard = new HeroCard()
            {
                Title = "Inverters",
                Subtitle = "Pulse inverters are available as connectorized modules up to 65 GHz operation. These laboratory devices use both magnetic and capacitive coupling to create an inverted or negative version of a voltage signal. In the frequency domain it introduces a broadband 180˚ phase shift to the input signal, while maintaining a flat group delay to ensure signal integrity.",
                Images = inverterscardImages1
            };
            heroCardList.Add(heroCard);

            return heroCardList;
        }
    }
}