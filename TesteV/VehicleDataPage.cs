using OpenQA.Selenium;

namespace AutoMobile
{
    public class VehicleDataPage
    {
        private readonly IWebDriver _driver;

        public VehicleDataPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void SelecionarMake(string make)
        {
            var makeDropdown = _driver.FindElement(By.Id("make"));
            makeDropdown.SendKeys(make);
        }

        public void SelecionarFuelType(string fuelType)
        {
            var fuelTypeDropdown = _driver.FindElement(By.Id("fuel"));
            fuelTypeDropdown.SendKeys(fuelType);
        }

        public void SelecionarNumberOfSeats(int numberOfSeats)
        {
            var seatsDropdown = _driver.FindElement(By.Id("numberofseats"));
            seatsDropdown.SendKeys(numberOfSeats.ToString());
        }
    }
}
