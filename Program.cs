using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Azure;
using Azure.AI.OpenAI;
using OpenAI.Embeddings;
using Azure.AI.TextAnalytics;
using Azure.AI.Translation.Text;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using Microsoft.Extensions.Configuration;

// Load configuration
var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).
    AddJsonFile("local.settings.json").Build();
var searchEndpoint = configuration["SEARCH_ENDPOINT"] ?? string.Empty;
var searchKey = configuration["SEARCH_ADMIN_KEY"] ?? string.Empty;
var indexName = configuration["SEARCH_INDEX_NAME"] ?? string.Empty;
var aoaiEndpoint = configuration["AOAI_ENDPOINT"] ?? string.Empty;
var aoaiKey = configuration["AOAI_KEY"] ?? string.Empty;
var embeddingDeployment = configuration["AOAI_EMBEDDING_DEPLOYMENT"] ?? string.Empty;
var cognitiveEndpoint = configuration["COGNITIVE_ENDPOINT"] ?? string.Empty;
var cognitiveRegion = configuration["COGNITIVE_REGION"] ?? string.Empty;
var cognitiveKey = configuration["COGNITIVE_KEY"] ?? string.Empty;
Console.WriteLine($"Search: {searchEndpoint}\nAOAI: {aoaiEndpoint}\nCognitive: {cognitiveEndpoint}");

// Read input data for indexing
var inputJson = File.ReadAllText("data/SampleReviewData.json");
var inputDocuments = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(inputJson) ??
    new List<Dictionary<string, object>>();
Console.WriteLine($"Read {inputDocuments.Count} input documents");

// Enrich input documents and create documents for indexing
// Enrichment includes: translation, sentiment, and embeddings
var docsToIndex = await EnrichInputDocuments();
Console.WriteLine($"\nEnriched {docsToIndex.Count} documents");

// Challenge 5: Create the search index

// Challenge 6: Import the documents into the search index

// Function to enrich input documents. It calls other enrichment functions that need to be completed.
async Task<List<SearchDocument>> EnrichInputDocuments()
{
    // Challenge 1: Initialize AI clients
    TextAnalyticsClient textAnalyticsClient = null;
    AzureOpenAIClient aoaiClient = null;
    EmbeddingClient embeddingClient = null;
    TextTranslationClient textTranslationClient = null;

    // Iterate through input documents and enrich them
    List<SearchDocument> documents = new List<SearchDocument>();
    int id = 0;
    foreach(var document in inputDocuments)
    {
        string productName = document["product_name"]?.ToString() ?? string.Empty;
        string productId = document["product_id"]?.ToString() ?? string.Empty;
        string text = document["review_text"]?.ToString() ?? string.Empty;
        string title = document["review_title"]?.ToString() ?? string.Empty;

        // Translate text and title to English
        string translatedText = await TranslateToEnglish(textTranslationClient, text);
        string translatedTitle = await TranslateToEnglish(textTranslationClient, title);
        if(translatedTitle != title)
        {
            Console.WriteLine($"\nOriginal title: {title}");
            Console.WriteLine($"Translated title: {translatedTitle}");
        }
        if(translatedText != text)
        {
            Console.WriteLine($"\nOriginal text: {text}");
            Console.WriteLine($"Translated text: {translatedText}");
        }

        // Analyze sentiment of the translated text
        string sentiment = await AnalyzeSentiment(textAnalyticsClient, translatedText);
        Console.WriteLine($"\nSentiment: {sentiment}");

        // Generate embeddings for the translated text
        float[] embeddings = await CreateEmbeddings(embeddingClient, translatedText);
        Console.WriteLine($"\nEmbedding size: {embeddings.Length}");

        // Create the seach document
        var doc = new SearchDocument
        {
            ["id"] = id++.ToString(),
            ["product_name"] = productName,
            ["product_id"] = productId,
            ["review_title"] = translatedTitle,
            ["review_text"] = translatedText,
            ["sentiment"] = sentiment,
            ["review_vector"] = embeddings
        };
        documents.Add(doc);
    }
    return documents;
}

// Challenge 2: Complete the code to translate text to English
async Task<string> TranslateToEnglish(TextTranslationClient client, string srcText)
{
    var result = srcText;

    // Translation code goes here

    return result;
}

// Challenge 3: Complete the code to analyze sentiment of input text
// Return values: "Positive", "Mixed", or "Negative"
async Task<string> AnalyzeSentiment(TextAnalyticsClient client, string text)
{
    var result = "";

    // Sentiment analysis code goes here

    return result;
}

// Challenge 4: Complete the code to generate embeddings for input text
async Task<float[]> CreateEmbeddings(EmbeddingClient client, string text)
{
    float[] result = new float[0];

    return result;
}
