using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WebTool
{
    public static class Selenium
    {
        public static IWebDriver CreateChromeDriver()
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            ChromeOptions options = new ChromeOptions();
            ChromeDriver driver = new ChromeDriver(service, options);

            return driver;
        }
    }
}
