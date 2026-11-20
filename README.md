\# Automação de Testes com Selenium e C#



Projeto desenvolvido para a disciplina de \*\*Garantia da Qualidade de Software\*\*, utilizando .NET 10, C#, Selenium WebDriver e MSTest.



\## Objetivo



Desenvolver testes automatizados para validar funcionalidades do mecanismo de pesquisa DuckDuckGo.



\## Tecnologias utilizadas



\- C#

\- .NET 10

\- Selenium WebDriver

\- Selenium.Support

\- MSTest

\- Git e GitHub



\## Cenários de teste



\### Exercício 1 — Pesquisa básica

Automatiza uma pesquisa por "Selenium WebDriver C#" e valida o título ou a URL dos resultados.



\### Exercício 2 — Autocompletar

Digita "C# unit testing", aguarda as sugestões e seleciona a terceira opção apresentada.



\### Exercício 3 — Navegação por histórico

Pesquisa "Automated Testing", acessa um resultado externo e verifica o retorno à página de pesquisa usando o histórico do navegador.



\## Como executar



\*\*Pré-requisitos:\*\* .NET 10 SDK e Google Chrome.



Na pasta principal da solução, execute:



```powershell

dotnet restore

dotnet build

dotnet test

```



\## Estrutura do projeto



\- `ConsoleSelenium` — Projeto principal.

\- `ConsoleSelenium.Tests` — Projeto de testes automatizados.

\- `BuscaDesafioTest.cs` — Classe com os três cenários de teste.



\## Autora



Michele Carvalho



Projeto acadêmico de Ciências da Computação.

