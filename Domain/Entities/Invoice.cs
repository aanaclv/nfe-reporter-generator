using System;

namespace Domain.Entities;

public class Invoice
{
    public string Number { get; }
    public string CompanyName { get; }
    public string Cnpj { get; }
    public string CustomerName { get; }
    public DateTime IssueDate { get; }
    public decimal TotalValue { get; }
    public string Observacao { get; private set; } // <--- Certifique-se de que esta linha existe aqui

    // Construtor recebendo a observação com valor padrão "OK"
    public Invoice(string number, string companyName, string cnpj, string customerName, DateTime issueDate, decimal totalValue, string observacao = "OK")
    {
        ValidateDomain(number, companyName, totalValue);

        Number = number;
        CompanyName = companyName;
        Cnpj = cnpj;
        CustomerName = customerName;
        IssueDate = issueDate;
        TotalValue = totalValue;
        Observacao = observacao; // <--- E que está sendo atribuída aqui
    }

    private void ValidateDomain(string number, string companyName, decimal totalValue)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("Número da NF obrigatório!");

        if (string.IsNullOrWhiteSpace(companyName))
            throw new ArgumentException("Emitente obrigatório!");

        if (totalValue <= 0)
            throw new ArgumentException("Valor da NF inválido!");
    }
}