using OpenQA.Selenium;

namespace AutoMobile
{
    public class InsurantDataPage
    {
        private readonly IWebDriver _driver;

        public InsurantDataPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void PreencherFirstName(string firstName)
        {
            var firstNameInput = _driver.FindElement(By.Id("firstname"));
            firstNameInput.SendKeys(firstName);
        }

        public void PreencherLastName(string lastName)
        {
            var lastNameInput = _driver.FindElement(By.Id("lastname"));
            lastNameInput.SendKeys(lastName);
        }
    }
}
