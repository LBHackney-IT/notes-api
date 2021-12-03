FROM mcr.microsoft.com/dotnet/core/sdk:3.1

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

ENV DynamoDb_LocalMode='true'

ARG LBHPACKAGESTOKEN
ENV LBHPACKAGESTOKEN=$LBHPACKAGESTOKEN

ARG SONAR_TOKEN
ENV SONAR_TOKEN=$SONAR_TOKEN

WORKDIR /app

# Copy csproj and nuget config and restore as distinct layers
COPY ./NotesApi.sln ./
COPY ./NotesApi/NotesApi.csproj ./NotesApi/
COPY ./NotesApi.Tests/NotesApi.Tests.csproj ./NotesApi.Tests/
COPY /NuGet.Config /root/.nuget/NuGet/NuGet.Config

RUN dotnet restore ./NotesApi/NotesApi.csproj
RUN dotnet restore ./NotesApi.Tests/NotesApi.Tests.csproj


# Copy everything else and build
COPY . .

RUN dotnet tool install --global dotnet-sonarscanner
RUN dotnet sonarscanner begin /k:"LBHackney-IT_notes-api" /o:"lbhackney-it" /d:sonar.login=SONAR_TOKEN
RUN dotnet build -c Release -o out NotesApi/NotesApi.csproj

RUN dotnet sonarscanner end /d:sonar.login=SONAR_TOKEN

RUN dotnet build -c debug -o out NotesApi.Tests/NotesApi.Tests.csproj

CMD dotnet test
