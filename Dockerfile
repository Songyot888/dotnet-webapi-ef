# ---------- Build Stage ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 1) copy csproj แล้ว restore ก่อน เพื่อใช้ layer cache
COPY ./dotnet-webapi-ef.csproj ./
RUN dotnet restore "./dotnet-webapi-ef.csproj"

# 2) copy ที่เหลือ แล้ว publish
COPY . .
RUN dotnet publish "./dotnet-webapi-ef.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ---------- Runtime Stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Render เปิดพอร์ต 8080 -> ให้ Kestrel ฟังที่ 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# (ทางเลือก) ถ้ามี Timezone ไทย
# RUN apt-get update && apt-get install -y tzdata && \
#     ln -snf /usr/share/zoneinfo/Asia/Bangkok /etc/localtime && echo Asia/Bangkok > /etc/timezone

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "dotnet-webapi-ef.dll"]
