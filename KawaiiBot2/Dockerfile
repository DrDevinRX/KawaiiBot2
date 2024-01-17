 
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /KawaiiBot2

COPY . ./

RUN dotnet restore
RUN dotnet build -c Release -o out

#build runtime image (?)
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /KawaiiBot2
COPY --from=build-env /KawaiiBot2/out .
ENTRYPOINT ["dotnet", "KawaiiBot2.dll"]