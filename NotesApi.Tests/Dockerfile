FROM mcr.microsoft.com/dotnet/sdk:8.0

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

ENV DynamoDb_LocalMode='true'

ARG LBHPACKAGESTOKEN
ENV LBHPACKAGESTOKEN=$LBHPACKAGESTOKEN

ARG SONAR_TOKEN
ENV SONAR_TOKEN=$SONAR_TOKEN

WORKDIR /app

# Install and run sonar cloud scanner
RUN apt-get update && apt-get install -y openjdk-17-jdk && apt-get clean
RUN dotnet tool install --global dotnet-sonarscanner
ENV PATH="$PATH:/root/.dotnet/tools"

RUN dotnet sonarscanner begin /k:"LBHackney-IT_notes-api" /o:"lbhackney-it" /d:sonar.host.url=https://sonarcloud.io /d:sonar.login="${SONAR_TOKEN}"

# Copy csproj and nuget config and restore as distinct layers
COPY ./NotesApi.sln ./
COPY ./NotesApi/NotesApi.csproj ./NotesApi/
COPY ./NotesApi.Tests/NotesApi.Tests.csproj ./NotesApi.Tests/
COPY /NuGet.Config /root/.nuget/NuGet/NuGet.Config

RUN dotnet restore ./NotesApi/NotesApi.csproj
RUN dotnet restore ./NotesApi.Tests/NotesApi.Tests.csproj


# Copy everything else and build
COPY . .

RUN dotnet build -c Release -o out NotesApi/NotesApi.csproj
RUN dotnet build -c debug -o out NotesApi.Tests/NotesApi.Tests.csproj

CMD dotnet test
RUN dotnet sonarscanner end /d:sonar.login="${SONAR_TOKEN}"
