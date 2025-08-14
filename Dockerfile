FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG PROJECT_NAME=PoemGenerator.Scalable

WORKDIR /src
COPY ./${PROJECT_NAME}.csproj ./
RUN dotnet restore "./${PROJECT_NAME}.csproj"
COPY . .
RUN dotnet publish "./${PROJECT_NAME}.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "PoemGenerator.Scalable.dll"]
