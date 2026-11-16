using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Domain.Entities;
using Infraestructure; 

// 1. Definimos a pasta onde estão os seus arquivos XML
string folderPath = "Files";

// 2. Buscamos todos os arquivos com extensão .xml dentro dessa pasta
string[] xmlFiles = Directory.GetFiles(folderPath, "*.xml");

List<Invoice> invoices = new List<Invoice>();

Console.WriteLine($"=== PROCESSANDO XMLs (Total encontrados: {xmlFiles.Length}) ===\n");

// 3. Percorremos cada arquivo encontrado usando um loop foreach
foreach (var file in xmlFiles)
{
    try
    {
        // Carrega a nota usando o seu leitor resiliente
        Invoice invoice = NfeXmlReader.LoadInvoice(file);
        invoices.Add(invoice);

        Console.WriteLine($"[OK] NF: {invoice.Number} | {invoice.CompanyName} | R$ {invoice.TotalValue} | Obs: {invoice.Observacao}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERRO] Não foi possível ler o arquivo {file}. Motivo: {ex.Message}");
    }
}

Console.WriteLine("\n----------------------------------------");

// Se conseguimos carregar ao menos uma nota, seguimos com o fluxo
if (invoices.Count > 0)
{
    // ==========================================
    // SERIALIZAÇÃO (Salvando a lista completa)
    // ==========================================
    string json = JsonSerializer.Serialize(
        invoices,
        new JsonSerializerOptions
        {
            WriteIndented = true
        });
        
    File.WriteAllText("invoice.json", json);
    Console.WriteLine("[SUCESSO] Arquivo 'invoice.json' gerado com todas as notas no disco.");

    // ==========================================
    // DESSERIALIZAÇÃO
    // ==========================================
    string jsonRead = File.ReadAllText("invoice.json");
    List<Invoice>? recoveredInvoices = JsonSerializer.Deserialize<List<Invoice>>(jsonRead);

    // ==========================================
    // APLICANDO LINQ (Com várias notas)
    // ==========================================
    if (recoveredInvoices != null && recoveredInvoices.Count > 0)
    {
        Console.WriteLine("\n=== RELATÓRIO ESTATÍSTICO GERAL (LINQ) ===");

        decimal totalValue = recoveredInvoices.Sum(i => i.TotalValue);
        int totalInvoices = recoveredInvoices.Count();
        
        var biggestInvoice = recoveredInvoices
            .OrderByDescending(i => i.TotalValue)
            .FirstOrDefault();

        Console.WriteLine($"Total Faturado Geral: R$ {totalValue}");
        Console.WriteLine($"Quantidade Total de Notas: {totalInvoices}");
        
        if (biggestInvoice != null)
        {
            Console.WriteLine($"Maior Nota do Lote: {biggestInvoice.CompanyName} (R$ {biggestInvoice.TotalValue})");
            Console.WriteLine($"Observação da Maior Nota: {biggestInvoice.Observacao}");
        }
    }
}
else
{
    Console.WriteLine("[AVISO] Nenhuma nota válida foi processada.");
}