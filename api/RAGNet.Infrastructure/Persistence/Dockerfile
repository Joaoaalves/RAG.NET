ARG DOTNET_RUNTIME=mcr.microsoft.com/dotnet/aspnet:6.0
ARG DOTNET_SDK=mcr.microsoft.com/dotnet/sdk:6.0

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ENV ASPNETCORE_URLS=http://+:7105
WORKDIR /home/app
EXPOSE 7105

# Base for build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS buildbase
WORKDIR /source

COPY ["RAG.NET.sln", "./"]
COPY ["RAGNet.Domain/RAGNet.Domain.csproj", "RAGNet.Domain/"]
COPY ["RAGNet.Infrastructure/RAGNet.Infrastructure.csproj", "RAGNet.Infrastructure/"]
COPY ["RAGNet.Application/RAGNet.Application.csproj", "RAGNet.Application/"]
COPY ["web/web.csproj", "web/"]
COPY ["tests/tests.csproj", "tests/tests.csproj"]

RUN dotnet restore RAG.NET.sln

COPY . .

## Run migrations
FROM buildbase as migrations
RUN dotnet tool install --version 6.0.9 --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
ENTRYPOINT dotnet-ef database update --project ./RAGNet.Infrastructure --startup-project ./web