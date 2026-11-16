using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Domain.Entities;
using Infraestructure; 

Invoice invoice = NfeXmlReader.LoadInvoice("Files/nfe.xml");

Console.WriteLine($"NF: {invoice.Number}");
Console.WriteLine($"Emitente: {invoice.CompanyName}");
Console.WriteLine($"Data: {invoice.IssueDate.ToShortDateString()}");
Console.WriteLine($"Valor: R$ {invoice.TotalValue}");
Console.WriteLine($"Status / Observação: {invoice.Observacao}\n");


List<Invoice> invoices = new List<Invoice> { invoice };


// SERIALIZAÇÃO
string json = JsonSerializer.Serialize(
    invoices,
    new JsonSerializerOptions
    {
        WriteIndented = true
    });
    
File.WriteAllText("invoice.json", json);
Console.WriteLine("[SUCESSO] Arquivo 'invoice.json' gerado no disco.");


// DESSERIALIZAÇÃO
string jsonRead = File.ReadAllText("invoice.json");

List<Invoice>? recoveredInvoices = JsonSerializer.Deserialize<List<Invoice>>(jsonRead);


// APLICANDO LINQ
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
        Console.WriteLine($"Observação da Maior Nota: {biggestInvoice.Observacao}");
    }
}