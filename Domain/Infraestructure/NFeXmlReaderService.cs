using System;
using System.Linq;
using System.Xml.Linq;
using Domain.Entities;

namespace Infraestructure
{
    public static class NfeXmlReader 
    {
        public static Invoice LoadInvoice(string xmlPath)
        {
            XDocument doc = XDocument.Load(xmlPath);
            XNamespace ns = "http://www.portalfiscal.inf.br/nfe";

            XElement infNfe = doc.Descendants(ns + "infNFe").FirstOrDefault() 
                ?? throw new Exception("Tag infNFe não encontrada no XML.");

            string observacao = "OK";

            // 1. Número da Nota (Campo crítico)
            string number = string.Empty;
            try 
            {
                number = infNfe.Element(ns + "ide")?.Element(ns + "nNF")?.Value 
                         ?? throw new Exception("Tag nNF ausente.");
            } 
            catch (Exception ex) 
            {
                number = "S/N";
                observacao = $"Aviso: Número da NF com problema ({ex.Message}).";
            }

            // 2. Emitente
            string companyName = infNfe.Element(ns + "emit")?.Element(ns + "xNome")?.Value ?? "Emitente Desconhecido";
            string cnpj = infNfe.Element(ns + "emit")?.Element(ns + "CNPJ")?.Value ?? "00.000.000/0000-00";

            // 3. Destinatário
            string customerName = infNfe.Element(ns + "dest")?.Element(ns + "xNome")?.Value ?? "Consumidor Final";

            // 4. Data de Emissão
            DateTime issueDate = DateTime.Now;
            try 
            {
                string dateStr = infNfe.Element(ns + "ide")?.Element(ns + "dhEmi")?.Value;
                if (!string.IsNullOrEmpty(dateStr))
                {
                    issueDate = DateTime.Parse(dateStr);
                }
            } 
            catch 
            {
                if (observacao == "OK") observacao = "Aviso: Data de emissão inválida ou ausente, usando data atual.";
            }

            // 5. Valor Total
            decimal totalValue = 0;
            try 
            {
                string valStr = infNfe.Element(ns + "total")?.Element(ns + "ICMSTot")?.Element(ns + "vNF")?.Value;
                if (!string.IsNullOrEmpty(valStr))
                {
                    totalValue = decimal.Parse(valStr, System.Globalization.CultureInfo.InvariantCulture);
                }
            } 
            catch 
            {
                if (observacao == "OK") observacao = "Aviso: Valor total inválido ou ausente.";
            }

            // Retorna a entidade populada, passando a observação capturada durante a leitura
            return new Invoice(number, companyName, cnpj, customerName, issueDate, totalValue, observacao);
        }
    }
}