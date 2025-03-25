using Microsoft.Extensions.Configuration;
using GenerativeAI;
using GenerativeAI.Types;
using UglyToad.PdfPig;
using System;
using System.IO;
using System.Threading.Tasks;


var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

string apiKey = config["Gemini:ApiKey"];
string modelName = config["Gemini:Model"];

if (string.IsNullOrEmpty(apiKey))
{
    Console.WriteLine("API Key is Missing");
}

string filePath = "C:\\Users\\user\\Documents\\Sem 6\\Intern\\ICL\\financial_stament_review.pdf";
string pdfText = "";

using (var doc =  PdfDocument.Open(filePath))
{
    foreach (var page in doc.GetPages())
    {
        pdfText += page.Text + "\n";
    }
}

var googleai = new GoogleAi(apiKey);
var model = googleai.CreateGenerativeModel(modelName);

var content = new Content
{
    Role = "user",
    Parts =
    {
        new Part
        {
            Text = @"Extract only the **Classified Balance Sheet At December 31, 2009** from the text below, and reproduce it in a **clean two-column markdown format** that matches typical financial statement layouts.
                    Do not use vertical pipes unless needed. Use bold headings for sections.
                    Align the left column for Assets and right column for Liabilities & Shareholder’s Equity."
        },
        new Part
        {
            Text = pdfText
        }
    }
};

var request = new GenerateContentRequest
{
    Contents = { content }
};

// Step 5: Call Gemini
var response = await model.GenerateContentAsync(request);

Console.WriteLine("\n--- Gemini Markdown Response ---\n");
Console.WriteLine(response.Text());