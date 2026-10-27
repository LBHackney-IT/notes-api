FROM mcr.microsoft.com/dotnet/sdk:10.0

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

ENV DynamoDb_LocalMode='true'

ARG LBHPACKAGESTOKEN
ENV LBHPACKAGESTOKEN=$LBHPACKAGESTOKEN

ARG SONAR_TOKEN
ENV SONAR_TOKEN=$SONAR_TOKEN

RUN useradd --create-home --uid 10001 appuser \
    && mkdir -p /app /home/appuser/.nuget/NuGet /home/appuser/.dotnet \
    && chown -R appuser:appuser /app /home/appuser

ENV HOME=/home/appuser
ENV DOTNET_CLI_HOME=/home/appuser
ENV NUGET_PACKAGES=/home/appuser/.nuget/packages
ENV PATH="${PATH}:/home/appuser/.dotnet/tools"

WORKDIR /app

USER appuser

# Scanner 7+ provisions its own JRE from SonarCloud during begin.
RUN dotnet tool install --global dotnet-sonarscanner

RUN dotnet sonarscanner begin /k:"LBHackney-IT_notes-api" /o:"lbhackney-it" /d:sonar.host.url=https://sonarcloud.io /d:sonar.login="${SONAR_TOKEN}"

# Copy csproj and nuget config and restore as distinct layers
COPY --chown=appuser:appuser ./NotesApi.sln ./
COPY --chown=appuser:appuser ./NotesApi/NotesApi.csproj ./NotesApi/
COPY --chown=appuser:appuser ./NotesApi.Tests/NotesApi.Tests.csproj ./NotesApi.Tests/
COPY --chown=appuser:appuser ./NuGet.Config /home/appuser/.nuget/NuGet/NuGet.Config

RUN dotnet restore ./NotesApi/NotesApi.csproj && \
    dotnet restore ./NotesApi.Tests/NotesApi.Tests.csproj

# Copy the projects being built. Sonar flags a full context copy.
COPY --chown=appuser:appuser ./NotesApi/ ./NotesApi/
COPY --chown=appuser:appuser ./NotesApi.Tests/ ./NotesApi.Tests/

RUN dotnet build -c Release -o out NotesApi/NotesApi.csproj && \
    dotnet build -c debug -o out NotesApi.Tests/NotesApi.Tests.csproj

CMD dotnet test
RUN dotnet sonarscanner end /d:sonar.login="${SONAR_TOKEN}"
