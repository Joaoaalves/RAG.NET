FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["RAGNet.Domain/RAGNet.Domain.csproj", "RAGNet.Domain/"]
COPY ["RAGNet.Infrastructure/RAGNet.Infrastructure.csproj", "RAGNet.Infrastructure/"]
COPY ["RAGNet.Application/RAGNet.Application.csproj", "RAGNet.Application/"]

COPY ["web/web.csproj", "web/"]

RUN dotnet restore "RAGNet.Infrastructure/RAGNet.Infrastructure.csproj"
RUN dotnet restore "web/web.csproj"

COPY . .

WORKDIR "/src/web"

RUN dotnet publish "web.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "web.dll"]
