FROM mcr.microsoft.com/dotnet/core/sdk:3.1

WORKDIR /app

# Copy csproj and nuget config and restore as distinct layers
COPY *.csproj ./
COPY /NuGet.Config /root/.nuget/NuGet/NuGet.Config

RUN dotnet restore

# Copy everything else and build
COPY . ./

RUN dotnet build -c Release -o out
RUN dotnet publish -c Release -o out

EXPOSE ${PORT:-3000}
CMD ASPNETCORE_URLS=http://+:${PORT:-3000} dotnet ./out/NotesApi.dll
