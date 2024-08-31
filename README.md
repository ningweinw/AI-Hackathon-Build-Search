# AI-Hackathon-Build-Search

## Objective
You will use Azure AI Search APIs to build a search index for product reviews, and push the sample review records into the index.

## Requirement
- Non-English reviews should be translated into English.
- The index should support search based on customer sentiment values: Positive, Mixed, Negative.
- The index should support vector search for semantic analysis of product reviews.
- The index should support filtering, sorting, faceting for flexible query.

## Sample Data
`Data\SampleReviewData.json`

## Instruction
1. In your own Azure environment, create Azure AI Search, AI multi-service and Azure OpenAI resources.
2. Deploy an embeddings model.
3. Update local.settings.json file.
4. Build and run the initial code, observe the behavior.
5. Look for comments in `Program.cs` that contain `Challenge X`, complete the code to fulfill the specified function.
6. Demonstrate the search index and the imported data.