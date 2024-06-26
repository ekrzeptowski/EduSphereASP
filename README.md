# EduSphere

Projekt został utworzony przy pomocy [Clean.Architecture.Solution.Template](https://github.com/jasontaylordev/EduSphere)
wersja 8.0.5.

## Budowanie

Uruchom `dotnet build -tl` żeby zbudować projekt.

## Uruchomienie

Żeby uruchomić projekt, użyj `docker-compose up` w głównym katalogu projektu lub uruchom projekt lokalnie
używając `dotnet run` w katalogach głównego serwera i mikroserwisów `.\src\Web\`, `.\src\AuthAPI`, `.\src\EduAPI`.

```bash
docker-compose up
```

lub

```bash
cd .\src\AuthAPI\
dotnet watch run
```

```bash
cd .\src\EduAPI\
dotnet watch run
```

```bash
cd .\src\Web\
dotnet watch run
```

Aplikacja powinna być dostępna pod adresem https://localhost:5001 w przypadku docker-compose lub https://localhost:44447
w przypadku uruchomienia lokalnego.

## Testowanie

Żeby uruchomić testy jednostkowe oraz integracyjne, użyj `dotnet test` w głównym katalogu projektu.

```bash
dotnet test
```

## Swagger

Aby zobaczyć dostępne endpointy, uruchom aplikację i przejdź do https://localhost:5001/api.

## Autor

- [Ewelina Krzeptowska](https://github.com/ekrzeptowski)