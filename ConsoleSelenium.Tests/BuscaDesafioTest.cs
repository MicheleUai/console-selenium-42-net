
using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace ConsoleSelenium.Tests;

[TestClass]
public class BuscaDesafioTest
{
    // EXERCICIO 1 - PESQUISA BASICA

    [TestMethod]
    public void PesquisaBasica_DeveMostrarResultados()
    {
        IWebDriver navegador = new ChromeDriver();

        try
        {
            navegador.Navigate().GoToUrl("https://duckduckgo.com");

            IWebElement campoBusca =
                navegador.FindElement(By.Name("q"));

            campoBusca.SendKeys("Selenium WebDriver C#");
            campoBusca.SendKeys(Keys.Enter);

            WebDriverWait espera = new WebDriverWait(
                navegador, TimeSpan.FromSeconds(15)
            );

            espera.Until(driver => driver.Url.Contains("q="));

            string url = Uri.UnescapeDataString(navegador.Url)
                .Replace("+", " ");

            string titulo = navegador.Title;

            bool pesquisaCorreta =
                url.Contains("Selenium WebDriver C#",
                    StringComparison.OrdinalIgnoreCase)
                || titulo.Contains("Selenium WebDriver C#",
                    StringComparison.OrdinalIgnoreCase);

            Assert.IsTrue(
                pesquisaCorreta,
                "O termo pesquisado nao apareceu no titulo ou na URL."
            );
        }
        finally
        {
            navegador.Quit();
        }
    }

    // EXERCICIO 2 - AUTOCOMPLETAR

    [TestMethod]
    public void Autocompletar_DevePesquisarTerceiraSugestao()
    {
        IWebDriver navegador = new ChromeDriver();

        try
        {
            navegador.Navigate().GoToUrl("https://duckduckgo.com");

            IWebElement campoBusca =
                navegador.FindElement(By.Name("q"));

            campoBusca.SendKeys("C# unit testing");

            WebDriverWait espera = new WebDriverWait(
                navegador, TimeSpan.FromSeconds(15)
            );

            By seletor = By.CssSelector(
                ".search-suggestions-list li, " +
                ".sbox-suggestions li, " +
                "[role='option']"
            );

            var sugestoes = espera.Until(driver =>
            {
                var visiveis = driver.FindElements(seletor)
                    .Where(elemento =>
                        elemento.Displayed &&
                        elemento.Enabled &&
                        !string.IsNullOrWhiteSpace(elemento.Text))
                    .ToList();

                return visiveis.Count >= 3 ? visiveis : null;
            });

            IWebElement terceiraSugestao = sugestoes[2];

            string termoEscolhido =
                terceiraSugestao.Text.Trim();

            Console.WriteLine(
                "Terceira sugestao: " + termoEscolhido
            );

            string urlAnterior = navegador.Url;

            terceiraSugestao.Click();

            espera.Until(driver =>
                driver.Url != urlAnterior &&
                driver.Url.Contains("q=")
            );

            string urlFinal =
                Uri.UnescapeDataString(navegador.Url)
                    .Replace("+", " ");

            bool pesquisaCorreta =
                urlFinal.Contains(
                    termoEscolhido,
                    StringComparison.OrdinalIgnoreCase)
                || navegador.Title.Contains(
                    termoEscolhido,
                    StringComparison.OrdinalIgnoreCase);

            Assert.IsTrue(
                pesquisaCorreta,
                "A pesquisa nao corresponde a terceira sugestao."
            );
        }
        finally
        {
            navegador.Quit();
        }
    }
// EXERCICIO 3 - HISTORICO DO NAVEGADOR

    [TestMethod]
    public void NavegacaoHistorico_DeveRetornarResultados()
    {
        IWebDriver navegador = new ChromeDriver();

        try
        {
            // 1. Abrir o DuckDuckGo
            navegador.Navigate().GoToUrl(
                "https://duckduckgo.com/?kn=-1"
            );

            // 2. Realizar a pesquisa
            IWebElement campoBusca =
                navegador.FindElement(By.Name("q"));

            campoBusca.SendKeys("Automated Testing");
            campoBusca.SendKeys(Keys.Enter);

            // 3. Aguardar a pagina de resultados
            WebDriverWait espera = new WebDriverWait(
                navegador, TimeSpan.FromSeconds(15)
            );

            espera.Until(driver =>
                driver.Url.Contains("q=")
            );

            string urlResultados = navegador.Url;

            // 4. Localizar um resultado organico
            By seletor = By.CssSelector(
                "a[data-testid='result-title-a'], a.result__a"
            );

            IWebElement primeiroResultado =
                espera.Until(driver =>
                    driver.FindElements(seletor)
                        .FirstOrDefault(elemento =>
                            elemento.Displayed &&
                            elemento.Enabled)
                );

            // 5. Abrir o resultado na mesma aba
            ((IJavaScriptExecutor)navegador)
                .ExecuteScript(
                    "arguments[0].removeAttribute('target');",
                    primeiroResultado
                );

            primeiroResultado.Click();

            // 6. Aguardar a pagina externa
            espera.Until(driver =>
                driver.Url != urlResultados &&
                !new Uri(driver.Url).Host.EndsWith(
                    "duckduckgo.com",
                    StringComparison.OrdinalIgnoreCase
                )
            );

            // 7. Verificar a pagina externa
            Assert.IsFalse(
                new Uri(navegador.Url).Host.EndsWith(
                    "duckduckgo.com",
                    StringComparison.OrdinalIgnoreCase
                ),
                "O site externo nao foi aberto."
            );

            // 8. Voltar pelo historico
            navegador.Navigate().Back();

            // 9. Aguardar o retorno aos resultados
            espera.Until(driver =>
                driver.Url.Contains("duckduckgo.com") &&
                driver.Url.Contains("q=")
            );

            // 10. Verificar que a pesquisa foi restaurada
            string urlRetorno =
                Uri.UnescapeDataString(navegador.Url)
                    .Replace("+", " ");

            Assert.IsTrue(
                urlRetorno.Contains(
                    "Automated Testing",
                    StringComparison.OrdinalIgnoreCase
                ),
                "A pagina de pesquisa nao foi restaurada."
            );
        }
        finally
        {
            navegador.Quit();
        }
    } // Fecha o terceiro metodo

} // Fecha a classe BuscaDesafioTest
