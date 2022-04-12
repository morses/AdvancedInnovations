using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;
using System.Collections.ObjectModel;

namespace KorbinSpecFlowProject.PageObjects
{
    public class HomePage : Page
    {
        private IWebElement Title => _browserInteractions.WaitAndReturnElement(By.Id("title"));
        private IWebElement WelcomeText => _browserInteractions.WaitAndReturnElement(By.Id("loggedin-welcome"));
        private IEnumerable<IWebElement> AppleButtons => _browserInteractions.WaitAndReturnElements(By.CssSelector("#listOfApples button"));

        public HomePage(IBrowserInteractions browserInteractions)
            : base(browserInteractions)
        {
            PageName = Common.HomePageName;
        }

        public string GetTitle => Title.Text;
        public string GetWelcomeText => WelcomeText.Text;

        public string GetAppleButtonText(int index) => AppleButtons.ElementAt(index).Text;

        public IEnumerable<string> GetAppleButtonTexts() => AppleButtons.Select(x => x.Text);

        public void ClickAppleButton(int index)
        {
            AppleButtons.ElementAt(index).Click();
        }

        public void SaveAllCookies()
        {
            ReadOnlyCollection<Cookie> cookies = _browserInteractions.GetCookies();
            FileUtils.SerializeCookiesToFile(Common.CookieFile, cookies);
        }

        public void LoadAllCookies()
        {
            List<Cookie> cookies = FileUtils.DeserializeCookiesFromFile(Common.CookieFile);
            foreach (Cookie cookie in cookies)
            {
                _browserInteractions.AddCookie(cookie);
            }
            _browserInteractions.RefreshPage();
        }

        public void DeleteCookies()
        {
            _browserInteractions.DeleteAllCookies();
        }

    }
}