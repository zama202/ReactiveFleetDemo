FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Web/WebSample/WebSample.csproj", "Web/WebSample/"]
RUN dotnet restore "Web/WebSample/WebSample.csproj"
COPY . .
WORKDIR "/src/Web/WebSample"
RUN dotnet build "WebSample.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WebSample.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebSample.dll"]