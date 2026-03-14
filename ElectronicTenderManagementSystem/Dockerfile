# Base stage: Çalışma ortamı
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
# Render varsayılan olarak 8080 portunu bekler
EXPOSE 8080
EXPOSE 8081

# Build stage: SDK ile derleme işlemi
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Proje dosyasını klasör yapısıyla birlikte kopyalıyoruz (Restore işlemi için)
COPY ["ElectronicTenderManagementSystem/ElectronicTenderManagementSystem.csproj", "ElectronicTenderManagementSystem/"]

# Bağımlılıkları geri yüklüyoruz
RUN dotnet restore "./ElectronicTenderManagementSystem/ElectronicTenderManagementSystem.csproj"

# Tüm repo içeriğini kopyalıyoruz
COPY . .

# Klasörün içine girip build alıyoruz
WORKDIR "/src/ElectronicTenderManagementSystem"
RUN dotnet build "./ElectronicTenderManagementSystem.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage: Yayınlanacak dosyaları hazırlama
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ElectronicTenderManagementSystem.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage: Sadece gerekli dosyaları alıp uygulamayı ayağa kaldırma
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ElectronicTenderManagementSystem.dll"]