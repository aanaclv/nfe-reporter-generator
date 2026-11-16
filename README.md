# ✧⋆⁺˖⁎ ୧ ⭒ ୨ ⁎˖⁺⋆✧ NFe Report & Reader Generator (.NET Core)

Application developed in **C# (.NET Core)** using **Clean Architecture** principles and a rich domain model, designed to perform structured reading, business rule validation, JSON serialization, and statistical queries via **LINQ** on Electronic Invoice (NF-e) XML files.

---

## ꕥ Main Features

- **Resilient XML Reading (Sefaz):** Extraction of data from NF-e XMLs using `LINQ to XML` (`XDocument`) with support for official namespaces and localized exception handling.
- **Fault Tolerance and Observations:** If any optional field fails or is corrupted, the application preserves the intact data and logs a detailed alert in the `Observacao` field.
- **Rich Domain:** Encapsulated `Invoice` entity with consistency validations inside its constructor (`ValidateDomain`).
- **Serialization and Deserialization:** Automatic conversion of objects into structured and indented JSON using `System.Text.Json`.
- **Statistical Reports via LINQ:** Batch processing of invoices with calculations for total revenue, document counts, and identification of the largest invoice in the batch (`Sum`, `Count`, `OrderByDescending`).

---

## ꕥ Technologies Used

- **C# 10+ / .NET**
- **LINQ to XML (`System.Xml.Linq`)**
- **JSON Serialization (`System.Text.Json`)**
- **Clean Architecture & Domain-Driven Design (DDD) Principles**

---

## ꕥ Project Structure

```text
nfe-reporter-generator/
│
├── Domain/
│   └── Entities/
│       └── Invoice.cs        # Domain entity, encapsulation, and validations
│
├── Infraestructure/
│   └── NfeXmlReader.cs       # Resilient XML reader and Sefaz mapping
│
├── Files/                    # Folder containing input XML files (.xml)
├── Program.cs                # Flow orchestrator, serialization, and LINQ reports
└── nfe-report.csproj         # Configuration file and automatic asset copying
```

---

## ꕥ Test Data

To validate the robustness and batch-reading capability of the application, sample XML files extracted and adapted from the open-source community reference repository [nfephp-org/nfephp](https://github.com/nfephp-org/nfephp) were used.

---

## ꕥ How to Run the Project

1. Make sure you have the [.NET SDK](https://dotnet.microsoft.com/) installed on your machine.
2. Clone this repository:
   ```bash
   git clone https://github.com/your-username/nfe-reporter-generator.git
   ```
3. Navigate to the project folder:
   ```bash
   cd nfe-reporter-generator
   ```
4. Add your NF-e XML files inside the `Files/` folder.
5. Run the project via terminal:
   ```bash
   dotnet run
   ```
