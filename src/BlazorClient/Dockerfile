# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BlazorClient/BlazorClient.csproj", "BlazorClient/"]
RUN dotnet restore "./BlazorClient/BlazorClient.csproj"
COPY . .
WORKDIR "/src/BlazorClient"
RUN dotnet publish "./BlazorClient.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

 
FROM nginx:alpine
WORKDIR /app
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
COPY --from=build /src/BlazorClient/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80