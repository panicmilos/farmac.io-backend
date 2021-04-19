FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /app

COPY Farmacio_API/Farmacio_API.csproj ./Farmacio_API/
COPY Farmacio_Models/Farmacio_Models.csproj ./Farmacio_Models/
COPY Farmacio_Repositories/Farmacio_Repositories.csproj ./Farmacio_Repositories/
COPY Farmacio_Services/Farmacio_Services.csproj ./Farmacio_Services/
COPY GlobalExceptionHandler/GlobalExceptionHandler.csproj ./GlobalExceptionHandler/
COPY EmailService/EmailService.csproj ./EmailService/
COPY Farmacio_Tests/Farmacio_Tests.csproj ./Farmacio_Tests/

COPY Farmacio.sln .

RUN dotnet restore Farmacio.sln

COPY . .

RUN dotnet publish -c Release -o out Farmacio_API/

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

WORKDIR /app

COPY --from=build /app/out ./

RUN ["apt-get", "update"]
RUN ["apt-get", "-y", "install", "libgdiplus"]
RUN ["apt-get", "-y", "install", "xvfb", "libfontconfig", "wkhtmltopdf"]
RUN ["apt-get", "-y", "install", "libc6-dev"]
RUN ["apt-get", "-y", "install", "openssl"]
RUN ["apt-get", "-y", "install", "libssl1.1"]

CMD [ "dotnet", "Farmacio_API.dll" ]