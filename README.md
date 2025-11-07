# Bankarski sistem

### Opis

Ovo je projekat obavljen za čas objektno-orijentisanog programiranja

Program omogućava korisniku da napravi i da se uloguje u svoj nalog, i da napravi dva računa, Checking i Savings

Checking račun dozvoljava korisniku da prima i šalje novčane transfere sa računa drugih korisnika, dok Savings račun dozvoljava primanje kamate (kamata je trenutno postavljena na 5% po minuti, radi testiranja)

Sve informacije o nalozima i računima se čuvaju u MySql bazi podataka


### .ENV file

Za rad programa i povezivanje s bazom podataka, u glavnom folderu je potreban `.env` file

`.env` treba da bude sačuvan u sledećem formatu:

```dotenv
DB_SERVER="Server na kojem je hostovana BP"
DB_NAME="Ime BP"
DB_USERNAME="Korisničko ime s pristupom k BP"
DB_PASSWORD="Lozinka korisnika s pristupom k BP"
DB_PORT="Port na kojoj je hostovana BP"
```


### Potrebni programi

Za rad programa potreban je dotnet, verzija 7.0 ili 9.0

Ostale potrebštine mogu se automatski instalirati komandom `dotnet build`


### Pokretanje programa

Pokretanje programa se radi komandom `dotnet run`