FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Analyst.WebCore/Analyst.WebCore.csproj", "Analyst.WebCore/"]
RUN dotnet restore "Analyst.WebCore/Analyst.WebCore.csproj"
COPY . .
WORKDIR "/src/Analyst.WebCore"
RUN dotnet build "Analyst.WebCore.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Analyst.WebCore.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Analyst.WebCore.dll"]