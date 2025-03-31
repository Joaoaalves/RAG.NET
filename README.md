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
- Embedding
- Chunker Strategy Factory
- Context Enrichers
  - ParentChild
  - SERP
- Query Enhancers
  - Auto Query
  - Self Querying Retrieval
  - Hypothetical Document Embedding
- Filters
  - Relevant Segment Extraction
  - Multiple Score Filter
- Rankers
  - Cohere Reranker
- Retrieval Pipeline
- Adapters
  - OpenAI (Embedding, Chat Completion)
  - QDrant
  - Pinecone
  - Claude (Chat Completion)
  - Gemini (Chat Completion)
  - Other Embedding and Chat Completion Providers

### Features Completed

- Authentication
- Workflows
  - Create Workflow
  - List Workflows
  - Get Workflow by ID
  - API Key Generation
