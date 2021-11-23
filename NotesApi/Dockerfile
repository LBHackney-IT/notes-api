FROM mcr.microsoft.com/dotnet/core/sdk:3.1

ENV DynamoDb_LocalMode='true'

ARG LBHPACKAGESTOKEN
ENV LBHPACKAGESTOKEN=$LBHPACKAGESTOKEN

ARG SONAR_PROJECT_KEY=notes-NotesApi
ARG SONAR_ORGANIZATION_KEY=lbhackney-it
ARG SONAR_HOST_URL=https://sonarcloud.io
ARG SONAR_TOKEN

WORKDIR /app

# Install Sonar Scanner, Coverlet and Java (required for Sonar Scanner)
RUN apt-get update && apt-get install -y openjdk-11-jdk
RUN dotnet tool install --global dotnet-sonarscanner
RUN dotnet tool install --global coverlet.console
ENV PATH="$PATH:/root/.dotnet/tools"

# Start Sonar Scanner
RUN dotnet sonarscanner begin \
  /k:"$SONAR_PROJECT_KEY" \
  /o:"$SONAR_OGRANIZAION_KEY" \
  /d:sonar.host.url="$SONAR_HOST_URL" \
  /d:sonar.login="$SONAR_TOKEN" \
  /d:sonar.cs.opencover.reportsPaths=/coverage.opencover.xml

# Copy csproj and nuget config and restore as distinct layers
COPY */NotesApi/NotesApi.csproj ./
COPY /NuGet.Config /root/.nuget/NuGet/NuGet.Config

RUN dotnet restore ./NotesApi.csproj

# Copy everything else and build
COPY . ./

# Build and test the application
RUN dotnet publish --output /out/
RUN dotnet test \
  /p:CollectCoverage=true \
  /p:CoverletOutputFormat=opencover \
  /p:CoverletOutput="/coverage"

RUN dotnet build -c Release -o out ./NotesApi.csproj
RUN dotnet publish -c Release -o out ./NotesApi.csproj

# End Sonar Scanner
RUN dotnet sonarscanner end /d:sonar.login="$SONAR_TOKEN"

EXPOSE ${PORT:-3000}
CMD ASPNETCORE_URLS=http://+:${PORT:-3000} dotnet ./out/NotesApi.dll
