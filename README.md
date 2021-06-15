# farmac.io-backend
Projekat za ISA + MRS.

[![Build Status](https://jenkins.bjelicaluka.com/buildStatus/icon?job=farmac.io-backend)](https://jenkins.bjelicaluka.com/job/farmac.io-backend/)

Poslednja verzija projekta se može naći na: <a href="http://farmac-io.bjelicaluka.com/">Ovde</a><br/>
Vreme na live verziji je 2 sata unazad tako da će 16pm biti zapravo 14pm.

## Članovi tima:
<ul>
<li><a href="https://github.com/jojev">Jevtić Jovana</a></li>
<li><a href="https://github.com/bjelicaluka">Bjelica Luka</a></li>
<li><a href="https://github.com/albertmakan">Makan	Albert</a></li>
<li><a href="https://github.com/panicmilos">Panić	Miloš</a></li>
</ul>

## Stvari potrebne za pokretanje projekta

1. Dobra volja
2. `MySQL` ili `MariaDB` server
3. `.NET Core v3.1` ili `Docker v18+` uz opcioni `docker-compose` CLI (koji je nativno podržan u novim verzijama Docker-a kao `docker compose`)

## Koraci za pokretanje projekta pomoću `dotnet` CLI:

1. Definisati okružene promenljive
```bash
export JwtSecret='SUPER_SECRET_SECRET'
export EmailSecret='MEDIUM_SECRET_SECRET'
export RunMigrations='true'
export SeedDatabase='true'
```
<i>SeedDatabase</i> je poželjno pokrenuti samo jednom kako se podaci ne bi bespotrebno duplirali. 

2. Napraviti `appsettings.json` datoteku u `Farmacio/Farmacio_API` direktorijumu sa sledećim sadržajem:
```json
{
	"EmailServiceSettings": {
		"Host": "smtp-server",
		"Port": 587,
		"Username": "thethrottle@goes.max",
		"Password": "pwd"
	},
	"DatabaseSettings": {
		"Server": "localhost",
		"Port": "3306",
		"Database": "farmacio_db",
		"User": "root",
		"Password": "pwd"
	},
	"SwaggerSettings": {
		"JsonRoute": "swagger/{documentName}/swagger.json",
		"Description": "Farmac.io API",
		"UIEndpoint": "v1/swagger.json"
	},
	"Logging": {
		"LogLevel": {
			"Default": "Information",
			"Microsoft": "Warning",
			"Microsoft.Hosting.Lifetime": "Information"
		}
	},
	"AllowedHosts": "*"
}
```
Kao mail snabdevač može se koristiti npr. <a href="https://www.sendinblue.com/">Sendinblue</a> ili <a href="https://www.mailjet.com/">Mailjet</a>

3. `dotnet restore Farmacio/Farmacio.sln`
4. `dotnet build Farmacio/Farmacio.sln`
5. `dotnet run --project Farmacio/Farmacio_API/`

## Koraci za pokretanje projekta pomoću `docker-compose` CLI:

1. `cd DevOps/`
2. Izmeniti okružene promenljive u `.env` datoteci
3. Izmeniti konfiguraciju iz `config/` direktorijuma
4. `docker-compose up -d`

## Koraci za pokretanje projekta pomoću `docker` CLI:

1. Sagraditi Docker kontejner
```bash
docker build -t <image-name> Farmacio/
```
2. Pokrenuti Docker kontejner
```bash
docker run -dp <port>:80 \
  -e JwtSecret='SUPER_SECRET_SECRET' \
  -e EmailSecret='MEDIUM_SECRET_SECRET' \
  -e RunMigrations='true' \
  -e SeedDatabase='true' \
  -v `pwd`/DevOps/config/backend/appsettings.json:/app/appsettings.json \
  <image-name>:<tag>
```

<h1>Korisni linkovi za radno okruženje i biblioteka iz trećih žurki</h1>

| Lib | URL |
| :--- | :--- |
| <b>Docker | https://www.docker.com/get-started |
| <b>.NET Core | https://dotnet.microsoft.com/learn/dotnet/hello-world-tutorial/intro |
| <b>EF Core | https://docs.microsoft.com/en-us/ef/core/ |
