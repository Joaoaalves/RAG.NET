# RAG.NET - Under Development

RAG.NET is a modular system designed to execute Retrieval-Augmented Generation (RAG) pipelines, allowing configurable workflows for text embedding, retrieval, and ranking tasks.

## 🚀 Technologies Used

- **ASP.NET Core** - Backend API
- **Angular** - Web user interface
- **PostgreSQL** - Relational database
- **QDrant** - Vector database for semantic search
- **Docker** - Containerized deployment and execution

## 💻 How to Run the Project

Make sure you have Docker installed on your machine.

### Run project on Development using:

```bash
docker compose -f "./docker-compose.dev" up --build
```

or

```bash
docker-compose -f "./docker-compose.dev" up --build
```

### Build the project using:

```bash
docker compose up --build
```

or

```bash
docker-compose up --build
```

> **Note:** The correct command depends on your Docker installation. On recent versions, `docker compose` is recommended.

---

## ✅ Development Progress

### Features to Implement

- Front-End
- Context Enrichers
  - SERP
- Query Enhancers
  - Self Querying Retrieval
- Rankers
  - Cohere Reranker
- Tokenizer
  - Tokenizer Service
  - Max tokens on Query retrieval
  - Max tokens on Query Enhancing
  - Max tokens on Embedding
- Adapters
  - Pinecone
  - DeepSeek (Chat completion)
  - Other Embedding and Chat Completion Providers

### Features Completed

- Authentication
- Embedding
  - Embedding Service Factory
- Embedding
  - Callback
    - Queue
- Chunkers
  - Chunker Factory
  - Paragraph Chunker (Default)
  - Semantical Chunker
  - Proposition Chunker
- Context Enrichers
  - ParentChild
- Query Enhancers
  - Hypothetical Document Embedding
  - Auto Query
- Conversation
  - Conversation Service Factory
- Retrieval Pipeline
  - Query Enhancer
  - Parent Child
- Workflows
  - Create Workflow
  - List Workflows
  - Get Workflow by ID
  - API Key Generation
- Filters
  - Relevant Segment Extraction
  - Multiple Score Filter
- Adapters
  - OpenAI (Embedding and Chat Completion)
  - Gemini (Chat Completion, Embedding)
  - Claude (Chat Completion)
  - Voyage (Embedding)
  - QDrant
