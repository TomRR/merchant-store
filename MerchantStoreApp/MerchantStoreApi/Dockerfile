# build image based on aspnet 5.0
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MerchantStoreApi.csproj", "./"]
# RUN dotnet restore brings all nuget packages in we needed
RUN dotnet restore "MerchantStoreApi.csproj"
COPY . .
RUN dotnet build "MerchantStoreApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MerchantStoreApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "MerchantStoreApi.dll"]