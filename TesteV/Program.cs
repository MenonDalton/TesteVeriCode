using System;
using System.IO;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace AutoMobile
{
    public class ValidacaoDeTelas
    {
        [Fact]
        public void Tricentis()
        {
            // Arrange: Inicializa o driver do navegador
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://sampleapp.tricentis.com/101/app.php");
            driver.Manage().Window.Maximize();

            // Inicializa as páginas para interação com o formulário
            VehicleDataPage vehicleDataPage = new VehicleDataPage(driver);
            InsurantDataPage insurantDataPage = new InsurantDataPage(driver);

            // Define variáveis para o Relatório
            var dataInicio = DateTime.Now;
            var sucesso = true;
            var detalhesExecucao = new StringBuilder();
            detalhesExecucao.AppendLine("Tela,Status,Mensagem,Data e Hora,Caminho da Captura de Tela");

            // Inicializa o gerador de Relatório HTML
            string caminhoHtml = @"C:/Users/DaltonMenon/Downloads/ScreenShot/Automobile.html";
            HtmlReportGenerator reportGenerator = new HtmlReportGenerator(caminhoHtml, dataInicio);
            reportGenerator.InitializeReport();

            try
            {
                // Função para capturar a tela
                void CapturarScreenshot(string nomeTela)
                {
                    Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                    string caminhoScreenshot = @$"C:/Users/DaltonMenon/Downloads/ScreenShot/{nomeTela}_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.png";

                    // Salva o screenshot diretamente como PNG
                    screenshot.SaveAsFile(caminhoScreenshot);

                    // Adiciona ao Relatório de execução
                    detalhesExecucao.AppendLine($"{nomeTela},Sucesso,,{DateTime.Now},{caminhoScreenshot}");
                    reportGenerator.AddTestResult(nomeTela, "Sucesso", DateTime.Now - dataInicio, caminhoScreenshot);
                }

                // Interações com as páginas
                string make = "BMW";
                vehicleDataPage.SelecionarMake(make);
                CapturarScreenshot("MAKE");

                // Assert para verificar se o Make foi selecionado corretamente
                var selectedMake = driver.FindElement(By.Id("make")).GetAttribute("value");
                Assert.Equal(make, selectedMake);

                vehicleDataPage.SelecionarFuelType("Petrol");
                CapturarScreenshot("GAS");

                // Assert para verificar se o Fuel Type foi selecionado corretamente
                var selectedFuelType = driver.FindElement(By.Id("fuel")).GetAttribute("value");
                Assert.Equal("Petrol", selectedFuelType);

                vehicleDataPage.SelecionarNumberOfSeats(3);
                CapturarScreenshot("NumberOfSeats");

                // Assert para verificar se o número de assentos foi selecionado corretamente
                var selectedSeats = driver.FindElement(By.Id("numberofseats")).GetAttribute("value");
                Assert.Equal("3", selectedSeats);

                driver.FindElement(By.Id("nextenterinsurantdata")).Click();

                insurantDataPage.PreencherFirstName("João");
                CapturarScreenshot("FirstName");

                // Assert para verificar se o nome foi preenchido corretamente
                var firstName = driver.FindElement(By.Id("firstname")).GetAttribute("value");
                Assert.Equal("João", firstName);

                insurantDataPage.PreencherLastName("Silva");
                CapturarScreenshot("LastName");

                // Assert para verificar se o sobrenome foi preenchido corretamente
                var lastName = driver.FindElement(By.Id("lastname")).GetAttribute("value");
                Assert.Equal("Silva", lastName);
            }
            catch (Exception ex)
            {
                sucesso = false;
                detalhesExecucao.AppendLine($"Erro,Falha,{ex.Message},{DateTime.Now},N/A");
                reportGenerator.AddTestResult("Erro", "Falha", DateTime.Now - dataInicio, ex.Message);
            }
            finally
            {
                // Fecha o navegador
                driver.Quit();

                // Gera o Relatório CSV
                string caminhoCsv = @"C:/Users/DaltonMenon/Downloads/ScreenShot/Automobile.csv";
                using (var writer = new StreamWriter(caminhoCsv, true))
                {
                    writer.WriteLine("Data de Inicio,Data de Fim,Resultado Final");
                    writer.WriteLine($"{dataInicio},{DateTime.Now},{(sucesso ? "Sucesso" : "Falha")}");

                    // Gera o relatório de detalhes
                    writer.WriteLine();
                    writer.WriteLine(detalhesExecucao.ToString());
                }

                // Finaliza o Relatório HTML
                reportGenerator.FinalizeReport(sucesso);
            }
        }
    }
}
