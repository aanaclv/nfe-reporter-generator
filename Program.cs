using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Domain.Entities;
using Infraestructure; 

Invoice singleInvoice = XmlReaderService.LoadInvoice("Files/nfe.xml");

Console.WriteLine($"Empresa: {singleInvoice.CompanyName}");
Console.WriteLine($"Valor: {singleInvoice.TotalValue}\n");

List<Invoice> invoices = new List<Invoice> { singleInvoice };

string json = JsonSerializer.Serialize(
    invoices,
    new JsonSerializerOptions
    {
        WriteIndented = true
    });

Console.WriteLine("=== JSON GERADO ===");
Console.WriteLine(json);
    
File.WriteAllText("invoice.json", json);

string jsonRead = File.ReadAllText("invoice.json");

List<Invoice>? recoveredInvoices = JsonSerializer.Deserialize<List<Invoice>>(jsonRead);

if (recoveredInvoices != null && recoveredInvoices.Count > 0)
{
    Console.WriteLine("\n=== RELATÓRIO ESTATÍSTICO (LINQ) ===");

    decimal totalValue = recoveredInvoices.Sum(i => i.TotalValue);
    
    int totalInvoices = recoveredInvoices.Count();
    
    var biggestInvoice = recoveredInvoices
        .OrderByDescending(i => i.TotalValue)
        .FirstOrDefault();

    Console.WriteLine($"Total Faturado: R$ {totalValue}");
    Console.WriteLine($"Quantidade de Notas: {totalInvoices}");
    
    if (biggestInvoice != null)
    {
        Console.WriteLine($"Maior Nota: {biggestInvoice.CompanyName} (R$ {biggestInvoice.TotalValue})");
    }
}