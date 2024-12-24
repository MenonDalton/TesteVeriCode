using System;
using System.IO;

namespace AutoMobile
{
    public class HtmlReportGenerator
    {
        private readonly string _reportPath;
        private readonly DateTime _startTime;

        public HtmlReportGenerator(string reportPath, DateTime startTime)
        {
            _reportPath = reportPath;
            _startTime = startTime;
        }

        public void InitializeReport()
        {
            using (StreamWriter sw = File.CreateText(_reportPath))
            {
                sw.WriteLine("<html><head><title>Relatorio de Teste</title>");
                sw.WriteLine("<style>body{font-family:Arial,sans-serif;}table{width:100%;border-collapse:collapse;}th,td{border:1px solid #ddd;padding:8px;}th{background-color:#f2f2f2;text-align:left;}tr:nth-child(even){background-color:#f9f9f9;}tr:hover{background-color:#f1f1f1;}</style>");
                sw.WriteLine("</head><body>");
                sw.WriteLine($"<h1>Relatorio de Teste - {DateTime.Now:dd/MM/yyyy HH:mm:ss}</h1>");
                sw.WriteLine("<table>");
                sw.WriteLine("<tr><th>Nome do Teste</th><th>Status</th><th>Tempo de Execução</th><th>Captura de Tela</th></tr>");
            }
        }

        public void AddTestResult(string testName, string testStatus, TimeSpan executionTime, string screenshotPath)
        {
            using (StreamWriter sw = File.AppendText(_reportPath))
            {
                string screenshotHtml = string.IsNullOrEmpty(screenshotPath) ? "Sem Captura" : $"<a href='{screenshotPath}' target='_blank'>Ver Captura</a>";
                sw.WriteLine($"<tr><td>{testName}</td><td>{testStatus}</td><td>{executionTime.TotalSeconds:F2}s</td><td>{screenshotHtml}</td></tr>");
            }
        }

        public void FinalizeReport(bool sucesso)
        {
            using (StreamWriter sw = File.AppendText(_reportPath))
            {
                sw.WriteLine("</table>");
                sw.WriteLine($"<p>Status Geral: <strong>{(sucesso ? "Sucesso" : "Falha")}</strong></p>");
                sw.WriteLine($"<p>Inicio: {_startTime:dd/MM/yyyy HH:mm:ss}</p>");
                sw.WriteLine($"<p>Termino: {DateTime.Now:dd/MM/yyyy HH:mm:ss}</p>");
                sw.WriteLine("</body></html>");
            }
        }
    }
}
