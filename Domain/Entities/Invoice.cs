using System;

namespace Domain.Entities;

public class Invoice
{
    public string InvoiceNumber { get; private set; }
    public string CompanyName { get; private set; }
    public string Cnpj { get; private set; }
    public string CustomerName { get; private set; }
    public decimal TotalValue { get; private set; }

    // Construtor que recebe os parâmetros e chama a validação do domínio
    public Invoice(string invoiceNumber, string companyName, string cnpj, string customerName, decimal totalValue)
    {
        ValidateDomain(invoiceNumber, companyName, totalValue);

        InvoiceNumber = invoiceNumber;
        CompanyName = companyName;
        Cnpj = cnpj;
        CustomerName = customerName;
        TotalValue = totalValue;
    }

    // Método de validação das regras de negócio
    private void ValidateDomain(string invoiceNumber, string companyName, decimal totalValue)
    {
        if (string.IsNullOrWhiteSpace(invoiceNumber))
            throw new ArgumentException("Número da nota obrigatório!");

        if (string.IsNullOrWhiteSpace(companyName))
            throw new ArgumentException("Empresa obrigatória!");

        if (totalValue <= 0)
            throw new ArgumentException("Valor deve ser maior que zero!");
    }
}