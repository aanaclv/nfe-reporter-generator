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
            
            // CORREÇÃO 1: Ajustada a barra "/" no namespace da Sefaz
            XNamespace ns = "http://www.portalfiscal.inf.br/nfe";

            XElement infNfe = doc.Descendants(ns + "infNFe").FirstOrDefault() 
                ?? throw new Exception("Tag infNFe não encontrada.");

            string number = infNfe.Element(ns + "ide")?.Element(ns + "nNF")?.Value 
                ?? throw new Exception("Número da NF não encontrado.");

            string companyName = infNfe.Element(ns + "emit")?.Element(ns + "xNome")?.Value 
                ?? throw new Exception("Emitente não encontrado.");

            string cnpj = infNfe.Element(ns + "emit")?.Element(ns + "CNPJ")?.Value 
                ?? throw new Exception("CNPJ não encontrado.");

            // CORREÇÃO 2: Ajustado de "customeName" para "customerName"
            string customerName = infNfe.Element(ns + "dest")?.Element(ns + "xNome")?.Value 
                ?? throw new Exception("Consumidor final.");

            DateTime issueDate = DateTime.Parse(infNfe.Element(ns + "ide")?.Element(ns + "dhEmi")?.Value 
                ?? throw new Exception("Data de emissão não encontrada."));

            decimal totalValue = decimal.Parse(
                infNfe.Element(ns + "total")?.Element(ns + "ICMSTot")?.Element(ns + "vNF")?.Value 
                ?? throw new Exception("Valor total não encontrado."), 
                System.Globalization.CultureInfo.InvariantCulture
            );

            return new Invoice(number, companyName, cnpj, customerName, issueDate, totalValue);
        }
    }
}