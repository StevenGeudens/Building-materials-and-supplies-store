using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace Kipcorn_Test.Views
{

    // Voor het runnen van deze test dient de "KlantToevoegenView" ingeladen te worden bij het starten van de applicatie
    // de wijzigingen hiervoor staan reeds in het App.xaml.cs bestand en dienen enkel uit commentaar gehaald te worden

    [TestFixture]
    public class KlantToevoegenViewTest
    {
        string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        string WpfAppId = @"C:\Thomas More\8. Projecten\2. Gev. programeertechnieken & agile & testing\2022-GPR-Agile-d2.1-KiPcOrN\Kipcorn\Kipcorn\bin\Debug\net6.0-windows\wpf.exe";

        WindowsDriver<WindowsElement> session;

        WindowsElement naam, telefoon, straat, nummer, postcode, plaats, email, btwNummer;

        [SetUp]
        public void Setup()
        {
            if (session == null)
            {
                var appiumOptions = new AppiumOptions();
                appiumOptions.AddAdditionalCapability("app", WpfAppId);
                session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);
          
                naam = session.FindElementByAccessibilityId("naam");
                telefoon = session.FindElementByAccessibilityId("telefoon");
                straat = session.FindElementByAccessibilityId("straat");
                nummer = session.FindElementByAccessibilityId("nummer");
                postcode = session.FindElementByAccessibilityId("postcode");
                plaats = session.FindElementByAccessibilityId("plaats");
                email = session.FindElementByAccessibilityId("email");
                btwNummer = session.FindElementByAccessibilityId("btwNummer");
            }
        }

        [Test]
        public void KlantToevoegen()
        {
            // Act
            naam.SendKeys("Naam Klant");
            telefoon.SendKeys("1234567890");
            straat.SendKeys("Straat");
            nummer.SendKeys("69");
            postcode.SendKeys("1234");
            plaats.SendKeys("Plaats");
            email.SendKeys("test@email.com");
            btwNummer.SendKeys("BE1234567890");

            // Assert
            Assert.AreEqual("Naam Klant", naam.Text);
            Assert.AreEqual("1234567890", telefoon.Text);
            Assert.AreEqual("Straat", straat.Text);
            Assert.AreEqual("69", nummer.Text);
            Assert.AreEqual("1234", postcode.Text);
            Assert.AreEqual("Plaats", plaats.Text);
            Assert.AreEqual("test@email.com", email.Text);
            Assert.AreEqual("BE1234567890", btwNummer.Text);
        }

        [TearDown]
        public void TearDown()
        {
            naam.Clear();
            telefoon.Clear();
            straat.Clear();
            nummer.Clear();
            postcode.Clear();
            plaats.Clear();
            email.Clear();
            btwNummer.Clear();
        }

        [OneTimeTearDown]
        public void CloseSession()
        {
            session.Close();
        }
    }
}
