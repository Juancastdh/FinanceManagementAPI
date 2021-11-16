FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY *.sln .
COPY API/*.csproj ./API/
COPY Core/*.csproj ./Core/
COPY DataAccess/*.csproj ./DataAccess/
COPY Services/*.csproj ./Services/
COPY Tests/*.csproj ./Tests/
RUN dotnet restore

COPY . ./
WORKDIR /app/API
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app/API
COPY --from=build-env /app/API/out .
ENTRYPOINT ["dotnet", "API.dll"]
