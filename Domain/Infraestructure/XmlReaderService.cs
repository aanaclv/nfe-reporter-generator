using System.Xml.Linq;
using Domain.Entities;

namespace Infraestructure
{
    public static class XmlReaderService 
    {
        public static Invoice LoadInvoice(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);

            var invoice = new Invoice(
                doc.Root?.Element("InvoiceNumber")?.Value ?? string.Empty,
                doc.Root?.Element("Emitter")?.Element("Name")?.Value ?? string.Empty,
                doc.Root?.Element("Emitter")?.Element("Cnpj")?.Value ?? string.Empty,
                doc.Root?.Element("Customer")?.Element("Name")?.Value ?? string.Empty,
                decimal.Parse(doc.Root?.Element("TotalValue")?.Value ?? "0")
            );

            return invoice;
        }
    }
}