#FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
#WORKDIR /app
#
## Copy csproj and restore as distinct layers
#COPY *.csproj ./
#RUN dotnet restore
#
## Copy everything else and build
#COPY . ./
#RUN dotnet publish -c Release -o out
#
## Build runtime image
#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
#WORKDIR /app
#COPY --from=build-env /app/out .
#ENTRYPOINT ["dotnet", "LeadGen.Validator.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LeadGen.Validator.csproj", "../LeadGen.Shared/LeadGen.Core/LeadGen.Core.csproj", "."]
RUN dotnet restore "./LeadGen.Validator.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "LeadGen.Validator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LeadGen.Validator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LeadGen.Validator.dll"]