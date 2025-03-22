---

# RAG.NET Documentation

**Warning: This project is under construction.**

---

## Checklist of Features to Implement

- **Authentication**

  - Create User
  - Update User
  - Delete User

- **User Interface**

  - Login Screen
  - Workflows Screen
  - Workflow Configuration Screen
  - User Configuration Screen

- **Workflows**

  - Save Settings
  - Save Structure
  - Generate User API Key

- **Embedding**

  - Embedding Services (e.g., OpenAI Embedding)
  - Integrate QDrant
  - Add support for additional vector databases (Pinecone, Chroma, Milvus, etc.)
  - Chunkers
    - Proposition Chunker
    - Semantic Chunker
  - Stream Response
  - Callback Response

- **Querying Phase**

  - Completion Service (e.g., OpenAI, Claude, Gemini)
  - Query Enhancers
    - Auto Query
    - Hypothetical Document Embedding
    - Self Querying Retrieval
  - Filters
    - Multiple Score Filter
    - Relevant Segment Extraction
  - Rankers
    - Cohere Reranker

---

## 1. Overview

RAG.NET is a modular system designed to perform text embedding and, subsequently, use user queries to retrieve and rank relevant text segments. The architecture is divided into two main phases: **Embedding** and **Querying**. Additionally, business rules and workflow configurations ensure the consistent and configurable execution of the system.

---

## 2. Core Services

### 2.1. EmbeddingService

**Objective:**  
Provide functionality to generate embeddings for texts using external providers (such as OpenAI, etc.).

**Implementation Requirements:**

- **Interface:**

  - Define essential methods for sending text and receiving embeddings.

- **Dependencies:**

  - The service must be injected into Chunkers.
  - Support multiple providers via adapter implementations.

- **Configurations:**

  - API keys, provider URLs, and service-specific configurations should be parameterizable.

- **Example Method:**

  ```csharp
  public interface IEmbeddingService {
      Task<float[]> GenerateEmbedding(string text);
  }
  ```

### 2.2. CompletionService (Replacing LLMService)

**Objective:**  
Provide completions or responses based on prompts, replacing the LLMService interface.

**Implementation Requirements:**

- **Interface:**

  - Define methods for sending prompts and retrieving responses.

- **Dependencies:**

  - Will be used by Evaluators, Query Enhancers, and Filters.
  - Must allow selection among different providers (e.g., OpenAI, Cohere, etc.).

- **Configurations:**

  - API keys, endpoints, and completion parameters should be configurable.

- **Example Method:**

  ```csharp
  public interface ICompletionService {
      Task<string> GetCompletion(string prompt);
  }
  ```

---

## 3. Embedding Phase

### 3.1. Chunkers

**Function:**  
Divide the text into “chunks” and evaluate them to determine which ones will be used.

**Requirements and Dependencies:**

- **Main Method:**

  - `generateChunks(text: string)`: Receives the text, splits it into chunks, and processes each chunk through an evaluation step.

- **Integration with Evaluator:**

  - Each Chunker must include an **Evaluator**.
  - **Chunking Process:**
    1.  Split the text into chunks.
    2.  Evaluate each chunk using the Evaluator.
    3.  Return only the approved chunks.

- **Configuration Parameters:**

  - **EmbeddingService:**
    - Each Chunker should receive its own embedding service (implementation of `IEmbeddingService`).
  - **Evaluator Threshold:**
    - Default value: **0.9**.
  - **MaxChunkSize:**
    - Default value: **600**.
  - **Prompts:**
    - Provide a prompt for the Evaluator and another for the Chunker itself.
    - Some chunkers may not use a prompt for themselves; this should be defined through an interface or specific configuration.

### 3.2. Evaluator (Integrated with Chunkers)

**Function:**  
Assess whether a given chunk meets the desired criteria.

**Requirements and Structure:**

- **Mandatory Methods:**

  - `Evaluate(chunk): boolean`: Returns `true` if the chunk meets the criteria.
  - `IsEnabled: boolean`: Indicates whether the evaluator is enabled.

- **Dependencies and Parameters:**

  - **CompletionService:**
    - Must be injected to perform evaluations based on LLM (using `ICompletionService`).
  - **Configurations:**
    - `IsEnabled`
    - `Threshold` (percentage): Each Evaluator can implement its own evaluation logic and may receive additional parameters as needed.

---

## 4. Querying Phase

### 4.1. Query Enhancers (QE)

**Function:**  
Enhance or generate new queries based on the user’s original query, using prompts and the CompletionService.

**Requirements and Structure:**

- **Configuration Parameters:**

  - Receive a specific prompt for the LLM/CompletionService.
  - Inject an instance of `ICompletionService`.
  - Maximum number of generated queries (default: **2**).

- **Main Method:**

  - `Generate(userQuery: string, maxQueries: int = 2): string[]`
    - Sends the query combined with the prompt to the CompletionService.
    - Returns an array of new queries generated.

### 4.2. Filters

**Function:**  
Filter and extract relevant segments from the chunks based on the user’s query.

**Requirements and Structure:**

- **Dependencies:**

  - Receive an instance of `ICompletionService`.

- **Main Method:**

  - `Filter(chunks: Chunk[], query: string): FilterResult[]`
    - Filters the received chunks for relevant segments based on the query.
    - Retains and returns the scores from the VectorDB.

- **Types of Filters:**

  - **Multiple Score Filter:**
    - Automatically enabled when more than one Query Enhancer is used.
    - Filters/reranks scores when performing searches for each query generated by the Query Enhancers.
  - **Relevant Segment Extraction:**
    - Extracts segments considered most relevant to the query using LLMs.

### 4.3. Rankers

**Function:**  
Reorder the filtered chunks based on their relevance and score.

**Requirements and Structure:**

- **Main Method:**

  - `Rank(chunks: Chunk[], maxOutputs: int): Chunk[]`
    - Receives an array of chunks with their scores and returns the chunks ordered by rank, respecting the maximum number of outputs.

- **Specific Implementations:**

  - **Cohere Reranker:**
    - Implement as an adapter for the external service, used within the Ranker.

---

## 5. Context Enrichers

### 5.1. ParentChild

**Function:**  
Enable the association between chunks and their "parent" context, enriching the data with contextual information.

**Requirements and Structure:**

- **Dependencies:**

  - Must receive an instance of `SQLService` to store related data: vectorId, documentId, text (from the parent), and userId.

- **Main Methods:**

  - `BindToParent(userId, documentId, vectorId, text)`
    - Associates the chunk with a parent context by saving the data in the database.
  - `GetFromVectorId(userId, vectorId): ParentData`
    - Retrieves the parent context based on the vectorId and userId.

---

## 6. Business Rules and Workflow Configurations

**Objective:**  
Ensure that each workflow configured for API usage executes according to defined settings and possesses a unique API key.

**Requirements:**

- **Workflow Configuration:**

  - Each workflow must have a unique API key.
  - Selection and configuration of the Chunker (e.g., **PropositionChunker** or **SemanticChunker**).
  - Configuration of the Parent-Child method:
    - Define whether Parent-Child association will be used.
    - Configure the parameters for `SQLService` to store context data.

- **Query Enhancers:**

  - Ability to enable or disable usage.
  - If enabled, each Query Enhancer must be configured with its specific settings, which should be saved in the database.

- **Rankers:**

  - Enable or disable Rankers.
  - Specific configuration for each Ranker (e.g., Cohere Reranker) must be saved in the database.

- **Persistence:**

  - All configurations regarding Context Enrichers, Chunkers, Query Enhancers, Filters, and Rankers must be stored in the database to ensure execution as configured in the workflow.

- **Streaming Embedding Process:**

  - The system must perform the embedding process via streaming.
  - It should return the percentage of process completion.
  - If callback mode is enabled, a call to the configured URL must be made to notify progress.

---

## 7. Persistence Strategy for Workflow Configurations

In this application, when a user creates a workflow, defines a Chunker and its configuration, enables two Query Enhancers, activates the Relevant Segment Extraction filter, and configures an API key for the Cohere Reranker, it is essential to store this configuration reliably.

### Alternative to JSON Storage

Instead of saving the entire workflow configuration as JSON, a **normalized relational database schema** can be used to improve data integrity, enforce referential constraints, and support complex queries. Below is a sample schema design:

### 7.1. Workflows Table

- **Workflows**  
  Stores the basic information of each workflow.

  **Columns:**

  - `workflow_id` (Primary Key)
  - `name`
  - `user_id` (Foreign Key to Users)
  - `api_key`
  - `created_at`
  - `updated_at`

### 7.2. Chunkers Table

- **Chunkers**  
  Each workflow can be associated with one or more Chunkers.

  **Columns:**

  - `chunker_id` (Primary Key)
  - `workflow_id` (Foreign Key to Workflows)
  - `type` (e.g., "PropositionChunker", "SemanticChunker")
  - `embedding_service_provider` (e.g., OpenAI, etc.)
  - `threshold` (Default: 0.9)
  - `max_chunk_size` (Default: 600)
  - `prompt_evaluator`
  - `prompt_chunker`
  - _Additional configuration fields as needed_

### 7.3. Query Enhancers Table

- **QueryEnhancers**  
  Stores configurations for each Query Enhancer linked to a workflow.

  **Columns:**

  - `query_enhancer_id` (Primary Key)
  - `workflow_id` (Foreign Key to Workflows)
  - `type` (e.g., "AutoQuery", "HypotheticalDocumentEmbedding", "SelfQueryingRetrieval")
  - `prompt`
  - `max_queries` (Default: 2)
  - _Additional configuration fields_

### 7.4. Filters Table

- **Filters**  
  Stores filter configurations (e.g., for Relevant Segment Extraction).

  **Columns:**

  - `filter_id` (Primary Key)
  - `workflow_id` (Foreign Key to Workflows)
  - `type` (e.g., "RelevantSegmentExtraction", "MultipleScoreFilter")
  - _Any specific configuration parameters_

### 7.5. Rankers Table

- **Rankers**  
  Stores configuration details for Rankers such as the Cohere Reranker.

  **Columns:**

  - `ranker_id` (Primary Key)
  - `workflow_id` (Foreign Key to Workflows)
  - `type` (e.g., "CohereReranker")
  - `api_key`
  - _Other configuration parameters as needed_

### Advantages of the Relational Model

- **Data Integrity:** Enforces referential constraints using foreign keys.
- **Query Flexibility:** Supports complex queries that join and filter data across multiple tables.
- **Validation:** Schema constraints help ensure that only valid configurations are stored.
- **Maintainability:** Changes in one part of the workflow configuration can be localized to specific tables.

---
