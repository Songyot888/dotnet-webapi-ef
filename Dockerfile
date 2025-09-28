# ใช้ SDK สำหรับ build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "./YourProject.csproj"
RUN dotnet publish "./YourProject.csproj" -c Release -o /app/publish

# ใช้ Runtime image สำหรับรัน
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "YourProject.dll"]
