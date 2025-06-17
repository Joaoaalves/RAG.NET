# ---------- Base build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csprojs
COPY ["RAGNet.Domain/RAGNet.Domain.csproj", "RAGNet.Domain/"]
COPY ["RAGNet.Infrastructure/RAGNet.Infrastructure.csproj", "RAGNet.Infrastructure/"]
COPY ["RAGNet.Application/RAGNet.Application.csproj", "RAGNet.Application/"]
COPY ["web/web.csproj", "web/"]

# Restore deps
RUN dotnet restore "web/web.csproj"

# Copy source code
COPY . .

# ---------- Development stage ----------
FROM build AS dev
WORKDIR /src
# Dev watch
CMD ["dotnet", "watch", "run", "--project", "web/" ,"--urls", "http://0.0.0.0:8080"]

# ---------- Production stage ----------
FROM build AS publish
WORKDIR /src/web
RUN dotnet publish "web.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "web.dll"]